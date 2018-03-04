using Core.Services.Assets;
using Core.Services.Factory;
using Core.Services.Levels;
using Core.Services.UI;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

namespace CoreDemo
{
	/// <summary>
	/// Demo level for CoreDemo.
	/// </summary>
	public class CoreDemoLevel : Level
	{
		[SerializeField]
		private Transform spawner; //Place where balls are going to spawn from

		[Inject]
		private FactoryService _factoryService;

		private Pooler<Ball> pooler;

		private float spawningSpeed = 1; //Default spawning speed

		private Ball ballPrefab;

		//Hot observables to be discarded when the object is destroyed
		private CompositeDisposable disposables = new CompositeDisposable();

		//Widget!
		private PoolWidget poolWidget;

		protected override void Awake()
		{
			base.Awake();

			//Get service references and subscribe to window events.
			uiService.OnUIElementClosed.Subscribe(OnUIElementClosed);
			uiService.OnUIElementOpened.Subscribe(OnUIElementOpened);

			//Define bundle to be requested
			BundleRequest bundleNeeded = new BundleRequest(AssetCategoryRoot.Prefabs,
				Constants.Assets.ASSET_BALL, Constants.Assets.ASSET_BALL, _assetService.Configuration);

			//Request ball asset
			_assetService.GetAndLoadAsset<Ball>(bundleNeeded).Subscribe(loadedBall =>
			{
				//Asset ball loaded. Store it and use it as the pool prefab.
				ballPrefab = loadedBall;

				//Initialize pooler, and set the pool to one element
				pooler = _factoryService.CreatePool<Ball>(ballPrefab.gameObject, 1);
			});
		}

		protected override void Start()
		{
			base.Start();

			//Listen to the OnUIElementClosed event.
			uiService.OnUIElementClosed.Subscribe(OnUIElementClosed);

			//Open title screen window window
			uiService.OpenUIElement(Constants.Screens.UI_TITLE_SCREEN_WINDOW)
				.Subscribe(window =>
				{
					Debug.Log(("Window " + window.name + " opened.").Colored(Colors.Fuchsia));
				});

			StartCoroutine(Spawner(1));
		}

		protected void OnUIElementClosed(UIElement element)
		{
			//Close widget after UIShowOffWindowHud closes
			if (element is UIShowOffWindowHud)
			{
				poolWidget.Close().Subscribe();
			}
		}

		protected void OnUIElementOpened(UIElement window)
		{
			//Left panel opened.
			if (window is UIShowOffWindowHud)
			{
				//Listen to OnSpawningSpeedChanged event
				(window as UIShowOffWindowHud).OnSpawningSpeedChanged
					.Subscribe(value =>
					{
						//When the slider changes, stop spawning, and reset the spawner with the selected time
						StopAllCoroutines();
						StartCoroutine(Spawner(value));
					})
					.AddTo(disposables);

				//Listen to OnResetPool event
				(window as UIShowOffWindowHud).OnResetPool
					.Subscribe(OnResetPool)
					.AddTo(disposables);

				//Open widget
				uiService.OpenUIElement(Constants.Screens.UI_POOL_WIDGET).Subscribe(element =>
				{
					poolWidget = element as PoolWidget;
					poolWidget.UpdateWidgetValue(pooler.SizeLimit, pooler.ActiveElements);
				});
			}
		}

		private void OnResetPool(int size)
		{
			//Change pool size. Warning, if the pool size is changed, for example, from 30 to 1
			//the objects that had been created and are in use will stay on the scene until they are pushed back into the pool, at which moment they will be desroyed.
			if (pooler != null)
				pooler.ResizePool(size);
		}

		private IEnumerator Spawner(float time)
		{
			while (enabled)
			{
				//wait time
				yield return new WaitForSeconds(time);

				//get a ball from the pool
				var ball = pooler.Pop();

				//not null?
				if (ball)
				{
					//initialize ball
					ball.Initialize();

					//change position
					ball.transform.position = spawner.position;

					//Subscribe to HasCollided ReactiveProperty
					ball.HasCollided.Subscribe(collided =>
					{
						//if true send it back into the pool
						if (collided)
							pooler.Push(ball);
					});

					if (poolWidget)
						poolWidget.UpdateWidgetValue(pooler.SizeLimit, pooler.ActiveElements);
				}
			}
		}

		protected override void OnDestroy()
		{
			//obliterate pooler
			if (pooler != null)
			{
				pooler.Destroy();
				pooler = null;
			}

			//unload ball
			_assetService.UnloadAsset(ballPrefab.name, true);
			//cleanup events
			disposables.Dispose();
		}
	}
}
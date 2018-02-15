﻿using System.Collections;
using System.Collections.Generic;
using Core.Polling;
using Core.Services;
using Core.Services.Assets;
using Core.Services.Levels;
using Core.Services.UI;
using UniRx;
using UnityEngine;

namespace CoreDemo
{
	/// <summary>
	/// Demo level for CoreDemo. 
	/// </summary>
	public class CoreDemoLevel : Level
	{
		[SerializeField]
		Transform spawner; //Place where balls are going to spawn from

		float spawningSpeed = 1; //Default spawning speed
		Pooler<Ball> pooler;
		IAssetService assetService;
		Ball ballPrefab;

		//Hot observables to be discarded when the object is destroyed
		CompositeDisposable disposables = new CompositeDisposable();

		//Widget!
		PoolWidget poolWidget;

		protected override void Awake()
		{
			base.Awake();

			//Get service references and subscribe to window events.
			assetService = ServiceLocator.GetService<IAssetService>();
			uiService.OnUIElementClosed.Subscribe(OnUIElementClosed);
			uiService.OnUIElementOpened.Subscribe(OnUIElementOpened);

			//Define bundle to be requested
			BundleRequest bundleNeeded = new BundleRequest(AssetCategoryRoot.Prefabs,
				Constants.Assets.ASSET_BALL, Constants.Assets.ASSET_BALL);

			//Request ball asset
			assetService.GetAndLoadAsset<Ball>(bundleNeeded).Subscribe(loadedBall =>
			{
				//Asset ball loaded. Store it and use it as the pool prefab.
				ballPrefab = loadedBall;

				//Initialize pooler, and set the pool to one element
				pooler = new Pooler<Ball>(loadedBall.gameObject, 1);
			});
		}

		protected override void Start()
		{
			base.Start();

			//Listen to the OnUIElementClosed event.
			uiService.OnUIElementClosed.Subscribe(OnUIElementClosed);

			//Open title screen window window
			uiService.OpenUIElement(Constants.Windows.UI_TITLE_SCREEN_WINDOW)
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
				uiService.OpenUIElement(Constants.Windows.UI_POOL_WIDGET).Subscribe(element =>
				{
					poolWidget = element as PoolWidget;
					poolWidget.UpdateWidgetValue(pooler.SizeLimit, pooler.ActiveElements);
				});
			}
		}

		void OnResetPool(int size)
		{
			//Change pool size. Warning, if the pool size is changed, for example, from 30 to 1
			//the objects that had been created and are in use will stay on the scene until they are pushed back into the pool, at which moment they will be desroyed.
			if (pooler != null)
				pooler.ResizePool(size);
		}

		IEnumerator Spawner(float time)
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

					//Subscribe to hasCollided ReactiveProperty
					ball.hasCollided.Subscribe(collided =>
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
			assetService.UnloadAsset(ballPrefab.name, true);
			//cleanup events
			disposables.Dispose();
		}
	}
}
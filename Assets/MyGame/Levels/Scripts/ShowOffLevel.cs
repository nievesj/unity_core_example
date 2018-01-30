using System;
using System.Collections;
using System.Collections.Generic;
using Core.Assets;
using Core.LevelLoaderService;
using Core.Polling;
using Core.Service;
using Core.UI;
using UniRx;
using UnityEngine;

namespace TheAwesomeGame
{
	/// <summary>
	/// Demo level.
	/// </summary>
	public class ShowOffLevel : Level
	{
		[SerializeField]
		Transform spawner; //Place where balls are going to spawn from

		float spawningSpeed = 1; //Default spawning speed
		Pooler<Ball> pooler;
		IAssetService assetService;
		Ball ballPrefab;
		CompositeDisposable disposables = new CompositeDisposable();

		protected override void Awake()
		{
			base.Awake();

			//Get service references and subscribe to window events.
			assetService = ServiceLocator.GetService<IAssetService>();
			uiService.OnWindowClosed.Subscribe(OnWindowClosed);
			uiService.OnWindowOpened.Subscribe(OnWindowOpened);

			//Open 'hud' (for the example the hud is just the left panel window) 
			uiService.OpenWindow(Constants.Windows.UI_SHOW_OFF_WINDOW_HUD).Subscribe();

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

			//Start spawner with a one second delay between spawns.
			StartCoroutine(Spawner(1));
		}

		protected void OnWindowClosed(UIWindow window)
		{
			if (window is UITitleScreenWindow)
			{
				Unload();
			}
		}

		protected void OnWindowOpened(UIWindow window)
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
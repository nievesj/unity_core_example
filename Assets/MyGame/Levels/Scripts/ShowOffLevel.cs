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
	public class ShowOffLevel : Level
	{
		[SerializeField]
		Transform spawner;

		float spawningSpeed = 1;
		Pooler<Ball> poller;
		IAssetService assetService;
		Ball ballPrefab;
		CompositeDisposable disposables = new CompositeDisposable();

		protected override void Awake()
		{
			base.Awake();

			assetService = ServiceLocator.GetService<IAssetService>();
			uiService.OnWindowClosed.Subscribe(OnWindowClosed);
			uiService.OnWindowOpened.Subscribe(OnWindowOpened);
			uiService.OpenWindow(Constants.Windows.UI_SHOW_OFF_WINDOW_HUD).Subscribe();

			BundleNeeded bundleNeeded = new BundleNeeded(AssetCategoryRoot.Prefabs,
				Constants.Assets.ASSET_BALL.ToLower(), Constants.Assets.ASSET_BALL.ToLower());

			assetService.GetAndLoadAsset<Ball>(bundleNeeded).Subscribe(loadedBall =>
			{
				ballPrefab = loadedBall as Ball;
				poller = new Pooler<Ball>(ballPrefab.gameObject, 1);
			});
		}

		protected override void Start()
		{
			base.Start();
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
			if (window is UIShowOffWindowHud)
			{
				(window as UIShowOffWindowHud).OnSpawningSpeedChanged
					.Subscribe(value =>
					{
						StopAllCoroutines();
						StartCoroutine(Spawner(value));
					})
					.AddTo(disposables);

				(window as UIShowOffWindowHud).OnResetPool
					.Subscribe(OnResetPool);
			}
		}

		void OnResetPool(int size)
		{
			if (poller != null)
				poller.ResizePool(size);
		}

		IEnumerator Spawner(float time)
		{
			while (enabled)
			{
				yield return new WaitForSeconds(time);
				var ball = poller.Pop();
				if (ball)
				{
					ball.Initialize();
					ball.transform.position = spawner.position;

					ball.hasCollided.Subscribe(collided =>
					{
						if (collided)
							poller.Push(ball);
					});
				}
			}
		}

		protected override void OnDestroy()
		{
			assetService.UnloadAsset(ballPrefab.name, true);
			disposables.Dispose();

			if (poller != null)
				poller.Destroy();
		}
	}
}
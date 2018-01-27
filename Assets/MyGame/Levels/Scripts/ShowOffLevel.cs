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
		Poller<Ball> poller;
		IAssetService assetService;
		Ball ballPrefab;

		protected override void Awake()
		{
			base.Awake();

			assetService = ServiceLocator.GetService<IAssetService>();
			uiService.OnWindowClosed.Subscribe(OnWindowClosed);
			uiService.OnWindowOpened.Subscribe(OnWindowOpened);

			uiService.OpenWindow(Constants.Windows.UI_SHOW_OFF_WINDOW_HUD)
				.Subscribe(window =>
				{
					//TODO: This observable is not returning a subscription, not sure why... :(
					Debug.Log("Window " + window.name + " opened.");
				});

			BundleNeeded bundleNeeded = new BundleNeeded(AssetCategoryRoot.Windows,
				Constants.Assets.ASSET_BALL.ToLower(), Constants.Assets.ASSET_BALL.ToLower());

			assetService.GetAndLoadAsset<Ball>(bundleNeeded).Subscribe(loadedBall =>
			{
				ballPrefab = loadedBall as Ball;
				poller = new Poller<Ball>(ballPrefab.gameObject, 1);
			});
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
				(window as UIShowOffWindowHud).spawningSpeed
					.Subscribe(value =>
					{
						//TODO: reference is lingering even after being destroyed, workaround until fix is found
						if (this)
						{
							StopAllCoroutines();
							StartCoroutine(Spawner(value));
						}
					});

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
			if (poller != null)
				poller.Destroy();
		}
	}
}
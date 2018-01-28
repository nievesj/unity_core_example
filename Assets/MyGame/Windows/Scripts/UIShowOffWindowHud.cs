using System.Collections;
using System.Collections.Generic;
using Core.LevelLoaderService;
using Core.Service;
using Core.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TheAwesomeGame
{
	public class UIShowOffWindowHud : UIWindow
	{
		[SerializeField]
		Slider poolSizeSlider;
		[SerializeField]
		Slider spawningSpeedSlider;

		ILevelLoaderService levelLoader;

		protected Subject<float> onSpawningSpeedChanged = new Subject<float>();
		public IObservable<float> OnSpawningSpeedChanged { get { return onSpawningSpeedChanged; } }

		protected Subject<int> onResetPool = new Subject<int>();
		public IObservable<int> OnResetPool { get { return onResetPool; } }

		public override void Initialize(IUIService svc)
		{
			base.Initialize(svc);

			levelLoader = ServiceLocator.GetService<ILevelLoaderService>();

		}

		public void BackToTitleOnClick()
		{
			if (uiService.IsWindowOpen(Constants.Windows.UI_RIGHT_WINDOW))
				uiService.GetOpenWindow(Constants.Windows.UI_RIGHT_WINDOW).Close();

			if (uiService.IsWindowOpen(Constants.Windows.UI_TOP_WINDOW))
				uiService.GetOpenWindow(Constants.Windows.UI_TOP_WINDOW).Close();

			if (uiService.IsWindowOpen(Constants.Windows.UI_BOTTOM_WINDOW))
				uiService.GetOpenWindow(Constants.Windows.UI_BOTTOM_WINDOW).Close();

			Close()
				.Subscribe(window =>
				{
					levelLoader.LoadLevel(Constants.Levels.START_SCREEN_LEVEL).Subscribe();
				});
		}

		public void OpenTopWindowOnClick()
		{
			if (!uiService.IsWindowOpen(Constants.Windows.UI_TOP_WINDOW))
			{
				if (uiService.IsWindowOpen(Constants.Windows.UI_RIGHT_WINDOW))
					uiService.GetOpenWindow(Constants.Windows.UI_RIGHT_WINDOW).Close();

				if (uiService.IsWindowOpen(Constants.Windows.UI_BOTTOM_WINDOW))
					uiService.GetOpenWindow(Constants.Windows.UI_BOTTOM_WINDOW).Close();

				uiService.OpenWindow(Constants.Windows.UI_TOP_WINDOW).Subscribe();
			}
		}

		public void OpenBottomWindowOnClick()
		{
			if (!uiService.IsWindowOpen(Constants.Windows.UI_BOTTOM_WINDOW))
			{
				if (uiService.IsWindowOpen(Constants.Windows.UI_RIGHT_WINDOW))
					uiService.GetOpenWindow(Constants.Windows.UI_RIGHT_WINDOW).Close();

				if (uiService.IsWindowOpen(Constants.Windows.UI_TOP_WINDOW))
					uiService.GetOpenWindow(Constants.Windows.UI_TOP_WINDOW).Close();

				uiService.OpenWindow(Constants.Windows.UI_BOTTOM_WINDOW).Subscribe();
			}
		}

		public void OpenRightWindowOnClick()
		{
			if (!uiService.IsWindowOpen(Constants.Windows.UI_RIGHT_WINDOW))
			{

				if (uiService.IsWindowOpen(Constants.Windows.UI_TOP_WINDOW))
					uiService.GetOpenWindow(Constants.Windows.UI_TOP_WINDOW).Close();

				if (uiService.IsWindowOpen(Constants.Windows.UI_BOTTOM_WINDOW))
					uiService.GetOpenWindow(Constants.Windows.UI_BOTTOM_WINDOW).Close();

				uiService.OpenWindow(Constants.Windows.UI_RIGHT_WINDOW).Subscribe();
			}
		}

		public void OnPoolSliderChange(Text text)
		{
			text.text = poolSizeSlider.value.ToString();
		}

		public void OnSpawningSpeedSliderChange(Text text)
		{
			text.text = string.Format("{0:0.00}", spawningSpeedSlider.value);
			onSpawningSpeedChanged.OnNext(spawningSpeedSlider.value);
		}

		public void CreateResetPool()
		{
			onResetPool.OnNext((int)poolSizeSlider.value);
		}

		protected override void OnDestroy()
		{
			onResetPool.Dispose();
		}
	}
}
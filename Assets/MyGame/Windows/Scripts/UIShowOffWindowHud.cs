﻿using System.Collections;
using System.Collections.Generic;
using Core.Services;
using Core.Services.Levels;
using Core.Services.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CoreDemo
{
	/// <summary>
	/// This window is the left panel open during the game demo.
	/// </summary>
	public class UIShowOffWindowHud : UIDialog
	{
		[SerializeField]
		Slider poolSizeSlider;
		[SerializeField]
		Slider spawningSpeedSlider;

		/// <summary>
		/// Signal triggers when the pool speed changes
		/// </summary>
		/// <returns></returns>
		protected Subject<float> onSpawningSpeedChanged = new Subject<float>();
		public IObservable<float> OnSpawningSpeedChanged { get { return onSpawningSpeedChanged; } }

		/// <summary>
		/// Signal triggers when the pool size is reset
		/// </summary>
		/// <returns></returns>
		protected Subject<int> onResetPool = new Subject<int>();
		public IObservable<int> OnResetPool { get { return onResetPool; } }

		public void BackToTitleOnClick()
		{
			if (uiService.IsUIElementOpen(Constants.Windows.UI_RIGHT_WINDOW))
				uiService.GetOpenUIElement(Constants.Windows.UI_RIGHT_WINDOW).Close().Subscribe();

			if (uiService.IsUIElementOpen(Constants.Windows.UI_TOP_WINDOW))
				uiService.GetOpenUIElement(Constants.Windows.UI_TOP_WINDOW).Close().Subscribe();

			if (uiService.IsUIElementOpen(Constants.Windows.UI_BOTTOM_WINDOW))
				uiService.GetOpenUIElement(Constants.Windows.UI_BOTTOM_WINDOW).Close().Subscribe();

			Close()
				.Subscribe(window =>
				{
					uiService.OpenUIElement(Constants.Windows.UI_TITLE_SCREEN_WINDOW).Subscribe();
				});
		}

		public void OpenTopWindowOnClick()
		{
			if (!uiService.IsUIElementOpen(Constants.Windows.UI_TOP_WINDOW))
			{
				if (uiService.IsUIElementOpen(Constants.Windows.UI_RIGHT_WINDOW))
					uiService.GetOpenUIElement(Constants.Windows.UI_RIGHT_WINDOW).Close().Subscribe();

				if (uiService.IsUIElementOpen(Constants.Windows.UI_BOTTOM_WINDOW))
					uiService.GetOpenUIElement(Constants.Windows.UI_BOTTOM_WINDOW).Close().Subscribe();

				uiService.OpenUIElement(Constants.Windows.UI_TOP_WINDOW).Subscribe();
			}
		}

		public void OpenBottomWindowOnClick()
		{
			if (!uiService.IsUIElementOpen(Constants.Windows.UI_BOTTOM_WINDOW))
			{
				if (uiService.IsUIElementOpen(Constants.Windows.UI_RIGHT_WINDOW))
					uiService.GetOpenUIElement(Constants.Windows.UI_RIGHT_WINDOW).Close().Subscribe();

				if (uiService.IsUIElementOpen(Constants.Windows.UI_TOP_WINDOW))
					uiService.GetOpenUIElement(Constants.Windows.UI_TOP_WINDOW).Close().Subscribe();

				uiService.OpenUIElement(Constants.Windows.UI_BOTTOM_WINDOW).Subscribe();
			}
		}

		public void OpenRightWindowOnClick()
		{
			if (!uiService.IsUIElementOpen(Constants.Windows.UI_RIGHT_WINDOW))
			{
				if (uiService.IsUIElementOpen(Constants.Windows.UI_TOP_WINDOW))
					uiService.GetOpenUIElement(Constants.Windows.UI_TOP_WINDOW).Close().Subscribe();

				if (uiService.IsUIElementOpen(Constants.Windows.UI_BOTTOM_WINDOW))
					uiService.GetOpenUIElement(Constants.Windows.UI_BOTTOM_WINDOW).Close().Subscribe();

				uiService.OpenUIElement(Constants.Windows.UI_RIGHT_WINDOW).Subscribe();
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

		protected void OnDestroy()
		{
			onResetPool.Dispose();
		}

		protected override void OnElementShow() {}

		protected override void OnElementHide() {}
	}
}
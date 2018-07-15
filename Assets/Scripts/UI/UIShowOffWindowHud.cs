using System;
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
		private Slider _poolSizeSlider;

		[SerializeField]
		private Slider _spawningSpeedSlider;

		/// <summary>
		/// Signal triggers when the pool speed changes
		/// </summary>
		/// <returns></returns>
		private readonly Subject<float> _onSpawningSpeedChanged = new Subject<float>();
	
		/// <summary>
		/// Signal triggers when the pool size is reset
		/// </summary>
		/// <returns></returns>
		private readonly Subject<int> _onResetPool = new Subject<int>();

		public void BackToTitleOnClick()
		{
			if (UiService.IsUIElementOpen(Constants.UI.RightWindow))
				UiService.GetOpenUIElement(Constants.UI.RightWindow).Close();

			if (UiService.IsUIElementOpen(Constants.UI.TopWindow))
				UiService.GetOpenUIElement(Constants.UI.TopWindow).Close();

			if (UiService.IsUIElementOpen(Constants.UI.BottomWindow))
				UiService.GetOpenUIElement(Constants.UI.BottomWindow).Close();

			Close();
		}

		public void OpenTopWindowOnClick()
		{
			if (!UiService.IsUIElementOpen(Constants.UI.TopWindow))
			{
				if (UiService.IsUIElementOpen(Constants.UI.RightWindow))
					UiService.GetOpenUIElement(Constants.UI.RightWindow).Close();

				if (UiService.IsUIElementOpen(Constants.UI.BottomWindow))
					UiService.GetOpenUIElement(Constants.UI.BottomWindow).Close();

				UiService.OpenUI(Constants.UI.TopWindow).TaskToObservable().Subscribe();
			}
		}

		public void OpenBottomWindowOnClick()
		{
			if (!UiService.IsUIElementOpen(Constants.UI.BottomWindow))
			{
				if (UiService.IsUIElementOpen(Constants.UI.RightWindow))
					UiService.GetOpenUIElement(Constants.UI.RightWindow).Close();

				if (UiService.IsUIElementOpen(Constants.UI.TopWindow))
					UiService.GetOpenUIElement(Constants.UI.TopWindow).Close();

				UiService.OpenUI(Constants.UI.BottomWindow).TaskToObservable().Subscribe();
			}
		}

		public void OpenRightWindowOnClick()
		{
			if (!UiService.IsUIElementOpen(Constants.UI.RightWindow))
			{
				if (UiService.IsUIElementOpen(Constants.UI.TopWindow))
					UiService.GetOpenUIElement(Constants.UI.TopWindow).Close();

				if (UiService.IsUIElementOpen(Constants.UI.BottomWindow))
					UiService.GetOpenUIElement(Constants.UI.BottomWindow).Close();

				UiService.OpenUI(Constants.UI.RightWindow).TaskToObservable().Subscribe();
			}
		}

		public void OnPoolSliderChange(Text text)
		{
			text.text = _poolSizeSlider.value.ToString();
		}

		public void OnSpawningSpeedSliderChange(Text text)
		{
			text.text = $"{_spawningSpeedSlider.value:0.00}";
			_onSpawningSpeedChanged.OnNext(_spawningSpeedSlider.value);
		}

		public IObservable<float> OnSpawningSpeedChanged()
		{
			return _onSpawningSpeedChanged;
		}

		public IObservable<int> OnResetPool()
		{
			return _onResetPool;
		}

		public void CreateResetPool()
		{
			_onResetPool.OnNext((int)_poolSizeSlider.value);
		}

		protected override void OnDestroy()
		{
			_onResetPool.OnCompleted();
			_onSpawningSpeedChanged.OnCompleted();
		}
	}
}
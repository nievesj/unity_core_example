using System.Collections;
using System.Collections.Generic;
using Core.Services;
using Core.Services.Levels;
using Core.Services.UI;
using UniRx;
using UnityEngine;

namespace CoreDemo
{
	/// <summary>
	/// Window that opens when the Title Screen level loads.
	/// </summary>
	public class UITitleScreenWindow : UIDialog
	{
		ILevelLoaderService levelLoader;

		protected override void Awake()
		{
			levelLoader = ServiceLocator.GetService<ILevelLoaderService>();
		}

		/// <summary>
		/// Handles UI OnClick event for the start button.
		/// </summary>
		public void OnStartClick()
		{
			//Trigger Close method. After the window closes load the level SHOW_OFF_LEVEL.
			Close()
				.Subscribe(window =>
				{
					uiService.OpenUIElement(Constants.Windows.UI_SHOW_OFF_WINDOW_HUD).Subscribe();
				});
		}

		protected override void OnElementShow() {}

		protected override void OnElementHide() {}
	}
}
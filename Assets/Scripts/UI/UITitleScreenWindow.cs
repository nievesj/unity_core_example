using System;
using System.Threading.Tasks;
using Core.Services.Levels;
using Core.Services.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace CoreDemo
{
	/// <summary>
	/// Window that opens when the Title Screen level loads.
	/// </summary>
	public class UITitleScreenWindow : UIDialog
	{
		[Inject]
		private LevelLoaderService _levelLoader;

		/// <summary>
		/// Handles UI OnClick event for the start button.
		/// </summary>
		public void OnStartClick()
		{
			//Trigger Close method. After the window closes load the level SHOW_OFF_LEVEL.
			Close()
				.Subscribe(window => { uiService.OpenUIElement(Constants.Screens.UI_SHOW_OFF_WINDOW_HUD).TaskToObservable().Subscribe(); });
		}

		protected override void OnElementShow() { }

		protected override void OnElementHide() { }
	}
}
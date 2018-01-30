using System.Collections;
using System.Collections.Generic;
using Core.LevelLoaderService;
using Core.Service;
using Core.UI;
using UniRx;
using UnityEngine;

namespace TheAwesomeGame
{
	/// <summary>
	/// Window that opens when the Title Screen level loads.
	/// </summary>
	public class UITitleScreenWindow : UIWindow
	{
		LevelLoaderService levelLoader;

		public override void Initialize(IUIService svc)
		{
			base.Initialize(svc);

			//Get level loader service reference
			levelLoader = ServiceLocator.GetService<ILevelLoaderService>()as LevelLoaderService;
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
					levelLoader.LoadLevel(Constants.Levels.SHOW_OFF_LEVEL).Subscribe();
				});
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using Core.LevelLoaderService;
using Core.Service;
using Core.UI;
using UniRx;
using UnityEngine;

namespace TheAwesomeGame
{
	public class UITitleScreenWindow : UIWindow
	{
		LevelLoaderService levelLoader;

		public override void Initialize(IUIService svc)
		{
			base.Initialize(svc);

			levelLoader = ServiceLocator.GetService<ILevelLoaderService>()as LevelLoaderService;
		}

		public void OnStartClick()
		{
			Close()
				.Subscribe(window =>
				{
					levelLoader.LoadLevel(Constants.Levels.SHOW_OFF_LEVEL).Subscribe();
				});
		}
	}
}
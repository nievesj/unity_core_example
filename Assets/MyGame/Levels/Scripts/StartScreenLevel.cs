using System.Collections;
using System.Collections.Generic;
using Core.LevelLoaderService;
using Core.UI;
using UniRx;
using UnityEngine;

namespace TheAwesomeGame
{
	public class StartScreenLevel : Level
	{
		protected override void Start()
		{
			base.Start();

			uiService.OnWindowClosed.Subscribe(OnWindowClosed);
			uiService.OpenWindow(Constants.Windows.UI_TITLE_SCREEN_WINDOW)
				.Subscribe(window =>
				{
					Debug.Log(("Window " + window.name + " opened.").Colored(Colors.fuchsia));
				});
		}

		protected void OnWindowClosed(UIWindow window)
		{
			if (window is UITitleScreenWindow)
			{
				Unload();
			}
		}

	}
}
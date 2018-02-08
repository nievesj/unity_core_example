using Core.Services.Levels;
using Core.Services.UI;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace TheAwesomeGame
{
	public class StartScreenLevel : Level
	{
		protected override void Start()
		{
			base.Start();

			//Listen to the OnWindowClosed event.
			uiService.OnWindowClosed.Subscribe(OnWindowClosed);

			//Open title screen window window
			uiService.OpenWindow(Constants.Windows.UI_TITLE_SCREEN_WINDOW)
				.Subscribe(window =>
				{
					Debug.Log(("Window " + window.name + " opened.").Colored(Colors.fuchsia));
				});
		}

		protected void OnWindowClosed(UIWindow window)
		{
			//When title window closes unload level.
			if (window is UITitleScreenWindow)
			{
				Unload();
			}
		}

	}
}
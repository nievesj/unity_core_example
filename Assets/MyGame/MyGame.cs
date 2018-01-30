using System.Collections;
using System.Collections.Generic;
using Core.Service;
using UniRx;
using UnityEngine;

namespace TheAwesomeGame
{
	public class MyGame : Game
	{
		/// <summary>
		/// Triggers when all services have been created and the Service Locator is ready.
		/// </summary>
		/// <param name="locator"></param>
		protected override void OnGameStart(ServiceLocator locator)
		{
			//Load first level.
			LevelLoader.LoadLevel(Constants.Levels.START_SCREEN_LEVEL)
				.Subscribe(level =>
				{
					Debug.Log(("MyGame Started.").Colored(Colors.fuchsia));
					Debug.Log(("MyGame | Level " + Constants.Levels.START_SCREEN_LEVEL + " loaded.").Colored(Colors.fuchsia));
				});
		}
	}
}
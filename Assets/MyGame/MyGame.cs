using System.Collections;
using System.Collections.Generic;
using Core.Services;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoreDemo
{
	public class MyGame : Game
	{
		/// <summary>
		/// Triggers when all services have been created and the Service Locator is ready.
		/// </summary>
		/// <param name="locator"></param>
		protected override void OnGameStart(ServiceLocator locator)
		{
			base.OnGameStart(locator);
			LevelLoader.LoadLevel(Constants.Levels.CORE_DEMO_LEVEL)
				.Subscribe(level =>
				{
					Debug.Log(("MyGame Started.").Colored(Colors.Fuchsia));

					//Load CoreDemoLevel level
					Debug.Log(("MyGame | Level " + Constants.Levels.CORE_DEMO_LEVEL + " loaded.").Colored(Colors.Fuchsia));
				});
		}
	}
}
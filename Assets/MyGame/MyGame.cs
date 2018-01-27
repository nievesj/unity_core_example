using System.Collections;
using System.Collections.Generic;
using Core.Service;
using UniRx;
using UnityEngine;

namespace TheAwesomeGame
{
	public class MyGame : Game
	{
		protected override void Awake()
		{
			base.Awake();

			Debug.Log(("MyGame Started.").Colored(Colors.fuchsia));
		}

		protected void Start()
		{
			LevelLoader.LoadLevel(Constants.Levels.START_SCREEN_LEVEL)
				.Subscribe(level =>
				{
					Debug.Log(("MyGame | Level " + Constants.Levels.START_SCREEN_LEVEL + " loaded.").Colored(Colors.fuchsia));
				});
		}
	}
}
using Core.Services;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheAwesomeGame
{
	public class SceneTester : Game
	{
		protected override void OnGameStart(ServiceLocator locator)
		{

		}

		public void LoadScene(string scene)
		{
			SceneLoader.LoadScene(scene, LoadSceneMode.Additive).Subscribe();
		}
	}
}
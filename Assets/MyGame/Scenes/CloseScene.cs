using Core.Services;
using Core.Services.Scenes;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace TheAwesomeGame
{
	public class CloseScene : MonoBehaviour
	{
		public void UnloadScene(string scene)
		{
			ServiceLocator.GetService<ISceneLoaderService>().UnLoadScene(scene).Subscribe();
		}
	}
}
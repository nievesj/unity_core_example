using System.Collections;
using System.Collections.Generic;
using Core.Scenes;
using Core.Service;
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
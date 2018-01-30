﻿using System.Collections;
using System.Collections.Generic;
using Core.Audio;
using Core.Service;
using Core.UI;
using UniRx;
using UnityEngine;

namespace Core.LevelLoaderService
{
	public enum LevelState
	{
		Loaded,
		Started,
		InProgress,
		Completed
	}

	/// <summary>
	/// A level has the same purpose of a scene, but we can change them without having to load a scene.
	/// This works well on most plaforms except for WebGL where loading scenes also clears the memory.
	/// </summary>
	public abstract class Level : MonoBehaviour
	{
		public AudioPlayer backgroundMusic;

		protected ILevelLoaderService levelService;
		protected IAudioService audioService;
		protected IUIService uiService;

		protected LevelState levelState;
		public LevelState State { get { return levelState; } }

		protected virtual void Awake()
		{
			Debug.Log(("Level: " + name + " loaded").Colored(Colors.lightblue));

			levelService = ServiceLocator.GetService<ILevelLoaderService>();
			audioService = ServiceLocator.GetService<IAudioService>();
			uiService = ServiceLocator.GetService<IUIService>();

			levelState = LevelState.Loaded;
		}

		protected virtual void Start()
		{
			Debug.Log(("Level: " + name + " started").Colored(Colors.lightblue));

			levelState = LevelState.Started;
			levelState = LevelState.InProgress;

			if (audioService != null && backgroundMusic != null && backgroundMusic.Clip != null)
				audioService.PlayMusic(backgroundMusic);
		}

		public virtual void Unload()
		{
			if (audioService != null && backgroundMusic != null && backgroundMusic.Clip != null)
				audioService.StopClip(backgroundMusic);
		}

		protected virtual void OnDestroy()
		{
			if (audioService != null && backgroundMusic != null && backgroundMusic.Clip != null)
				audioService.StopClip(backgroundMusic);
		}
	}
}
using Core.Services;
using Core.Services.Assets;
using Core.Services.Audio;
using Core.Services.Levels;
using UniRx;
using UnityEngine;
using Zenject;

namespace CoreDemo
{
	public class MyGame : Game
	{
		[Inject]
		private AssetService assetService;

		[Inject]
		private AudioService audioService;

		[Inject]
		private LevelLoaderService _levelLoaderService;

		/// <summary>
		///Global signal emitted when the game starts.
		/// </summary>
		/// <param name = "unit" ></ param >
		protected override void OnGameStart(Unit unit)
		{
			base.OnGameStart(unit);
			_levelLoaderService.LoadLevel(Constants.Levels.CORE_DEMO_LEVEL)
				.Subscribe(level =>
				{
					Debug.Log(("MyGame Started.").Colored(Colors.Fuchsia));

					//Load CoreDemoLevel level
					Debug.Log(("MyGame | Level " + Constants.Levels.CORE_DEMO_LEVEL + " loaded.").Colored(Colors.Fuchsia));
				});
		}
	}
}
using Core.Services;
using Core.Services.Scenes;
using UnityEngine.SceneManagement;
using Zenject;

namespace CoreDemo
{
    public class MyGame : Game
    {
        [Inject]
        private SceneLoaderService _sceneLoaderService;

        /// <inheritdoc />
        /// <summary>
        /// Global signal emitted when the game starts.
        /// </summary>
        protected override async void OnGameStart()
        {
            base.OnGameStart();

            if (!SceneManager.GetSceneByName("UIScene").isLoaded)
            {
                var scene = await _sceneLoaderService.LoadScene("UIScene",LoadSceneMode.Additive);
            }

            // AssetService.LoadAsset<CoreDemoLevel>(AssetCategoryRoot.Levels, Constants.Levels.CoreDemoLevel)
            //     .Run(level =>
            //     {
            //         Debug.Log("MyGame Started.".Colored(Colors.Fuchsia));
            //
            //         //Load CoreDemoLevel level
            //         Debug.Log(("MyGame | Level " + Constants.Levels.CoreDemoLevel + " loaded.").Colored(Colors.Fuchsia));
            //
            //         var demo = FactoryService.Instantiate(level);
            //     });
        }

        protected override void OnGamePaused(bool isPaused)
        {
            // throw new System.NotImplementedException();
        }

        protected override void OnGameFocusChange(bool hasFocus)
        {
            // throw new System.NotImplementedException();
        }

        protected override void OnGameQuit()
        {
            // throw new System.NotImplementedException();
        }
    }
}
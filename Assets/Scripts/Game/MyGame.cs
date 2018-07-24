using Core.Services;
using Core.Services.Assets;
using UnityEngine;

namespace CoreDemo
{
    public class MyGame : Game
    {
        /// <inheritdoc />
        ///  <summary>
        /// Global signal emitted when the game starts.
        ///  </summary>
        protected override void OnGameStart()
        {
            base.OnGameStart();

            AssetService.LoadAsset<CoreDemoLevel>(AssetCategoryRoot.Levels, Constants.Levels.CoreDemoLevel)
                .Run(level =>
                {
                    Debug.Log("MyGame Started.".Colored(Colors.Fuchsia));

                    //Load CoreDemoLevel level
                    Debug.Log(("MyGame | Level " + Constants.Levels.CoreDemoLevel + " loaded.").Colored(Colors.Fuchsia));

                    var demo = FactoryService.Instantiate(level);
                });
        }
    }
}
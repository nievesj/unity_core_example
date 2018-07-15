using Core.Services;
using Core.Services.Assets;
using UniRx;
using UnityEngine;

namespace CoreDemo
{
    public class MyGame : Game
    {
        ///  <summary>
        /// Global signal emitted when the game starts.
        ///  </summary>
        ///  <param name = "unit" />
        protected override void OnGameStart(Unit unit)
        {
            base.OnGameStart(unit);

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
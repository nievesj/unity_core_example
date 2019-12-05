using Core.Factory;
using Core.Services;

public class UISceneInstaller : SceneInstaller
{
    public override void InstallBindings()
    {
        var uiManager = FindObjectOfType<UIManager>();

        Container.BindInstance(uiManager).AsSingle();
        Container.BindInterfacesAndSelfTo<Factory>().AsSingle().WithArguments(Container).NonLazy();
    }
}
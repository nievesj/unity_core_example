using System.Threading;
using System.Threading.Tasks;
using Core.Services.Assets;
using Core.Services.Factory;
using Core.Services.Levels;
using Core.Services.UI;
using UniRx;
using UnityEngine;
using Zenject;

namespace CoreDemo
{
    /// <summary>
    /// Demo level for CoreDemo.
    /// </summary>
    public class CoreDemoLevel : Level
    {
        [SerializeField]
        private Transform _spawner; //Place where balls are going to spawn from

        [Inject]
        private FactoryService _factoryService;

        private Pooler<Ball> _pooler;
        private float _spawningSpeed = 1; //Default spawning speed
        private Ball _ballPrefab;
        private Task _spawnerTask;
        private PoolWidget _poolWidget;
        private CancellationTokenSource _tokenSource;

        //Hot observables to be discarded when the object is destroyed
        private CompositeDisposable _disposables = new CompositeDisposable();


        protected override void Awake()
        {
            base.Awake();

            //Get service references and subscribe to window events.
            uiService.OnUIElementClosed.Subscribe(OnUIElementClosed);
            uiService.OnUIElementOpened.Subscribe(OnUIElementOpened);

            //Define bundle to be requested
            var bundleNeeded = new BundleRequest(AssetCategoryRoot.Prefabs,
                Constants.Assets.ASSET_BALL, Constants.Assets.ASSET_BALL, _assetService.Configuration);

            _assetService.GetAndLoadAsset<Ball>(bundleNeeded)
                .TaskToObservable()
                .Subscribe(ball =>
                {
                    _ballPrefab = ball;
                    //Initialize pooler, and set the pool to one element
                    _pooler = _factoryService.CreatePool<Ball>(_ballPrefab.gameObject, 1);
                });
        }

        protected override void Start()
        {
            base.Start();

            //Listen to the OnUIElementClosed event.
            uiService.OnUIElementClosed.Subscribe(OnUIElementClosed);
            uiService.OpenUIElement(Constants.Screens.UI_TITLE_SCREEN_WINDOW)
                .TaskToObservable()
                .Subscribe(screen =>
                {
                    //Open title screen window window
                    Debug.Log(("Window " + screen.name + " opened.").Colored(Colors.Fuchsia));
                });

            //Used to cancel task
            _tokenSource = new CancellationTokenSource();

            //runs task on main thread
            Spawner(1);
        }

        private void OnUIElementClosed(UIElement element)
        {
            //Close widget after UIShowOffWindowHud closes
            if (element is UIShowOffWindowHud)
            {
                if (_poolWidget)
                    _poolWidget.Close().Subscribe();
            }
        }

        private void OnUIElementOpened(UIElement window)
        {
            //Left panel opened.
            if (window is UIShowOffWindowHud)
            {
                //Listen to OnSpawningSpeedChanged event
                (window as UIShowOffWindowHud).OnSpawningSpeedChanged
                    .Subscribe(value =>
                    {
                        //When the slider changes, stop spawning, and reset the spawner with the selected time
                        _tokenSource.Cancel();
                        Spawner(value);
                    })
                    .AddTo(_disposables);

                //Listen to OnResetPool event
                (window as UIShowOffWindowHud).OnResetPool
                    .Subscribe(OnResetPool)
                    .AddTo(_disposables);

                uiService.OpenUIElement(Constants.Screens.UI_POOL_WIDGET)
                    .TaskToObservable()
                    .Subscribe(asset =>
                    {
                        _poolWidget = asset as PoolWidget;
                        _poolWidget.UpdateWidgetValue(_pooler.SizeLimit, _pooler.ActiveElements);
                    });
            }
        }

        private void OnResetPool(int size)
        {
            //Change pool size. Warning, if the pool size is changed, for example, from 30 to 1
            //the objects that had been created and are in use will stay on the scene until they are pushed back into the pool, at which moment they will be desroyed.
            _pooler?.ResizePool(size);
        }

        private async Task Spawner(float time)
        {
            while (!_tokenSource.Token.IsCancellationRequested)
            {
                _tokenSource.Token.ThrowIfCancellationRequested();
                //wait time
                await new WaitForSeconds(time);

                //get a ball from the pool
                var ball = _pooler.Pop();

                //not null?
                if (ball)
                {
                    //initialize ball
                    ball.Initialize();

                    //change position
                    ball.transform.position = _spawner.position;

                    //Subscribe to HasCollided ReactiveProperty
                    ball.HasCollided.Subscribe(collided =>
                    {
                        //if true send it back into the pool
                        if (collided)
                            _pooler.Push(ball);
                    });

                    if (_poolWidget)
                        _poolWidget.UpdateWidgetValue(_pooler.SizeLimit, _pooler.ActiveElements);
                }
            }

            _tokenSource = new CancellationTokenSource();
        }

        protected override void OnDestroy()
        {
            //obliterate pooler
            if (_pooler != null)
            {
                _pooler.Destroy();
                _pooler = null;
            }

            //unload ball
            _assetService.UnloadAsset(_ballPrefab.name, true);
            //cleanup events
            _disposables.Dispose();
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Services.Assets;
using Core.Services.Factory;
using Core.Services.Levels;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace CoreDemo
{
    /// <summary>
    /// Demo level for CoreDemo.
    /// </summary>
    public class CoreDemoLevel : Level
    {
        [SerializeField]
        private Transform _spawner; //Place where balls are going to spawn from

        private Pooler<Ball> _pooler;
        private float _spawningSpeed = 1; //Default spawning speed
        private Ball _ballPrefab;
        private Task _spawnerTask;
        private PoolWidget _poolWidget;
        private CancellationTokenSource _tokenSource;

        protected override void Awake()
        {
            base.Awake();
            AssetService.LoadAsset<Ball>(AssetCategoryRoot.Prefabs, Constants.Prefabs.Ball)
                .Run(ball =>
                {
                    _ballPrefab = ball;
                    //Initialize pooler, and set the pool to one element
                    _pooler = FactoryService.CreatePool<Ball>(_ballPrefab, 1);
                });
        }

        protected override void Start()
        {
            base.Start();

            //Open UITitleScreenWindow
            OpenTitleScreen();

            //Used to cancel task
            _tokenSource = new CancellationTokenSource();

            //runs task on main thread
            Spawner(1, _tokenSource.Token);
        }

        private void OpenTitleScreen()
        {
            UiService.OpenUI<UITitleScreenWindow>(Constants.UI.UITitleScreenWindow)
                .Run(screen =>
                {
                    //Open title screen window window
                    Debug.Log(("Window " + screen.name + " opened.").Colored(Colors.Fuchsia));

                    screen.OnStartClicked().Subscribe(OnStartClicked).AddTo(this);
                });
        }

        private void OnStartClicked(UITitleScreenWindow start)
        {
            start.Close();
            UiService.OpenUI<UIShowOffWindowHud>(Constants.UI.UIShowOffWindowHud).Run(showOff =>
            {
                showOff.OnClosed().Subscribe(s =>
                {
                    OpenTitleScreen();
                    _poolWidget.Close();
                }).AddTo(this);
                OpenWidget(showOff);
            });
        }

        private void OpenWidget(UIShowOffWindowHud hud)
        {
            //Listen to OnSpawningSpeedChanged event
            hud.OnSpawningSpeedChanged()
                .Subscribe(value =>
                {
                    //When the slider changes, stop spawning, and reset the spawner with the selected time
                    _tokenSource.Cancel();
                    _tokenSource = new CancellationTokenSource();
                    Spawner(value, _tokenSource.Token);
                })
                .AddTo(this);

            //Listen to OnResetPool event
            hud.OnResetPool()
                .Subscribe(OnResetPool)
                .AddTo(this);

            UiService.OpenUI(Constants.UI.PoolWidget)
                .Run(asset =>
                {
                    _poolWidget = asset as PoolWidget;
                    _poolWidget.UpdateWidgetValue(_pooler.SizeLimit, _pooler.ActiveElements);
                });
        }

        private void OnResetPool(int size)
        {
            //Change pool size. Warning, if the pool size is changed, for example, from 30 to 1
            //the objects that had been created and are in use will stay on the scene until they are pushed back into the pool, at which moment they will be desroyed.
            _pooler?.ResizePool(size);
        }

        private async Task Spawner(float time, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                //wait time
                await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);

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
            AssetService.UnloadAsset(_ballPrefab.name, true);
        }
    }
}
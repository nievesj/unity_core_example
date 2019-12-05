using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Factory;
using Core.Services;
using Core.Services.Assets;
using UniRx;
using UniRx.Async;
using UnityEngine;
using Zenject;

namespace CoreDemo
{
    /// <summary>
    /// Demo level for CoreDemo.
    /// </summary>
    public class CoreDemoLevel : CoreBehaviour
    {
        [SerializeField]
        private Transform _spawner; //Place where balls are going to spawn from

        private ComponentPool<Ball> _pooler;
        private float _spawningSpeed = 1; //Default spawning speed
        private Ball _ballPrefab;
        private Task _spawnerTask;
        private CancellationTokenSource _tokenSource;

        [Inject] //This is set up on UISceneInstaller
        private Factory _sceneFactory;

        [Inject] //This is set up on UISceneInstaller
        private UIManager _uiManager;

        private void Awake()
        {
            AssetService.LoadAsset<Ball>(AssetCategoryRoot.Prefabs, Constants.Prefabs.Ball)
                .Run(ball =>
                {
                    _ballPrefab = ball;
                    //Initialize pooler, and set the pool to one element
                    _pooler = _sceneFactory.CreateComponentPool<Ball>(_ballPrefab, 1);
                });
        }

        private void Start()
        {
            //Used to cancel task
            _tokenSource = new CancellationTokenSource();

            //runs task on main thread
            Spawner(1, _tokenSource.Token);

            _uiManager.UIShowOffWindowHud.OnSpawningSpeedChanged().Subscribe(value =>
            {
                //When the slider changes, stop spawning, and reset the spawner with the selected time
                _tokenSource.Cancel();
                _tokenSource = new CancellationTokenSource();
                Spawner(value, _tokenSource.Token);
            });

            _uiManager.UIShowOffWindowHud.OnResetPool()
                .Subscribe(OnResetPool)
                .AddTo(this);
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

                    // Subscribe to HasCollided ReactiveProperty
                    ball.HasCollided.Subscribe(collided =>
                    {
                        //if true send it back into the pool
                        if (collided)
                            _pooler.Push(ball);
                    });

                    if (_uiManager.PoolWidget)
                        _uiManager.PoolWidget.UpdateWidgetValue(_pooler.SizeLimit, _pooler.ActiveElements);
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
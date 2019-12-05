using System;
using Core.Reactive;
using Core.Services.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CoreDemo
{
    /// <summary>
    /// This window is the left panel open during the game demo.
    /// </summary>
    public class UIShowOffWindowHud : UIDialog
    {
        [SerializeField]
        private Slider _poolSizeSlider;

        [SerializeField]
        private Slider _spawningSpeedSlider;

        /// <summary>
        /// Signal triggers when the pool speed changes
        /// </summary>
        /// <returns></returns>
        private readonly Subject<float> _onSpawningSpeedChanged = new Subject<float>();

        /// <summary>
        /// Signal triggers when the pool size is reset
        /// </summary>
        /// <returns></returns>
        private readonly Subject<int> _onResetPool = new Subject<int>();

        public CoreEvent<UIShowOffWindowHud> OnOpenTopWindowOnClicked { get; } = new CoreEvent<UIShowOffWindowHud>();
        public CoreEvent<UIShowOffWindowHud> OnOpenBottomWindowOnClicked { get; } = new CoreEvent<UIShowOffWindowHud>();
        public CoreEvent<UIShowOffWindowHud> OnOpenRightWindowOnClicked { get; } = new CoreEvent<UIShowOffWindowHud>();
        public CoreEvent<UIShowOffWindowHud> OnBackToTitleOnClicked { get; } = new CoreEvent<UIShowOffWindowHud>();

        public void BackToTitleOnClick()
        {
            Close();

            OnBackToTitleOnClicked.Broadcast(this);
        }

        public void OpenTopWindowOnClick()
        {
            OnOpenTopWindowOnClicked.Broadcast(this);
        }

        public void OpenBottomWindowOnClick()
        {
            OnOpenBottomWindowOnClicked.Broadcast(this);
        }

        public void OpenRightWindowOnClick()
        {
            OnOpenRightWindowOnClicked.Broadcast(this);
        }

        public void OnPoolSliderChange(Text text)
        {
            text.text = _poolSizeSlider.value.ToString();
        }

        public void OnSpawningSpeedSliderChange(Text text)
        {
            text.text = $"{_spawningSpeedSlider.value:0.00}";
            _onSpawningSpeedChanged.OnNext(_spawningSpeedSlider.value);
        }

        public IObservable<float> OnSpawningSpeedChanged()
        {
            return _onSpawningSpeedChanged;
        }

        public IObservable<int> OnResetPool()
        {
            return _onResetPool;
        }

        public void CreateResetPool()
        {
            _onResetPool.OnNext((int) _poolSizeSlider.value);
        }

        protected override void OnDestroy()
        {
            _onResetPool.OnCompleted();
            _onSpawningSpeedChanged.OnCompleted();
        }
    }
}
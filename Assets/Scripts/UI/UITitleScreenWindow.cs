using System;
using Core.Services.UI;
using UniRx;

namespace CoreDemo
{
    /// <summary>
    /// Window that opens when the Title Screen level loads.
    /// </summary>
    public class UITitleScreenWindow : UIDialog
    {
        private readonly Subject<UITitleScreenWindow> _onSpawningSpeedChanged = new Subject<UITitleScreenWindow>();

        /// <summary>
        /// Handles UI OnClick event for the start button.
        /// Assigned on editor 
        /// </summary>
        public void OnStartClick()
        {
            _onSpawningSpeedChanged.OnNext(this);
        }

        public IObservable<UITitleScreenWindow> OnStartClicked()
        {
            return _onSpawningSpeedChanged;
        }
    }
}
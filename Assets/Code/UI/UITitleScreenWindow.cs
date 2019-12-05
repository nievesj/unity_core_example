using Core.Reactive;
using Core.Services.UI;

namespace CoreDemo
{
    /// <summary>
    /// Window that opens when the Title Screen level loads.
    /// </summary>
    public class UITitleScreenWindow : UIDialog
    {
        public CoreEvent<UITitleScreenWindow> OnStartClicked = new CoreEvent<UITitleScreenWindow>();

        /// <summary>
        /// Handles UI OnClick event for the start button.
        /// Assigned on editor
        /// </summary>
        public void OnStartClick()
        {
            OnStartClicked.Broadcast(this);
        }
    }
}
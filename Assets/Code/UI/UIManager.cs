using Core.Services;
using Core.Services.UI;
using CoreDemo;
using UniRx;
using UnityEngine;

public class UIManager : CoreBehaviour
{
    [SerializeField]
    private UIElement bottom;

    [SerializeField]
    private UIElement top;

    [SerializeField]
    private UIElement right;

    [SerializeField]
    private UIShowOffWindowHud showOff;

    [SerializeField]
    private UITitleScreenWindow title;

    [SerializeField]
    private PoolWidget poolWidget;

    public UIShowOffWindowHud UIShowOffWindowHud => showOff;
    public PoolWidget PoolWidget => poolWidget;

    private void Awake()
    {
        title.OnStartClicked
            .Subscribe(OnStartClicked);
        showOff.OnBackToTitleOnClicked
            .Subscribe(OnBackToTitleOnClicked);
        showOff.OnOpenBottomWindowOnClicked
            .Subscribe(OnOpenBottomWindowOnClicked);
        showOff.OnOpenRightWindowOnClicked
            .Subscribe(OnOpenRightWindowOnClicked);
        showOff.OnOpenTopWindowOnClicked
            .Subscribe(OnOpenTopWindowOnClicked);
    }

    private void Start()
    {
        title.Show();
    }

    private void OnStartClicked(UITitleScreenWindow wind)
    {
        title.Hide();

        showOff.Show();
        poolWidget.Show();
    }

    private void OnBackToTitleOnClicked(UIShowOffWindowHud wind)
    {
        bottom.Hide();
        top.Hide();
        right.Hide();
        title.Show();
    }

    private void OnOpenBottomWindowOnClicked(UIShowOffWindowHud wind)
    {
        if (bottom.IsVisible) return;

        bottom.Show();
        top.Hide();
        right.Hide();
    }

    private void OnOpenRightWindowOnClicked(UIShowOffWindowHud wind)
    {
        if (right.IsVisible) return;

        bottom.Hide();
        top.Hide();
        right.Show();
    }

    private void OnOpenTopWindowOnClicked(UIShowOffWindowHud wind)
    {
        if (top.IsVisible) return;

        bottom.Hide();
        top.Show();
        right.Hide();
    }
}
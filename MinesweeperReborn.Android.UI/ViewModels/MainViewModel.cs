using CommunityToolkit.Mvvm.ComponentModel;

namespace MinesweeperReborn.Android.UI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Greeting { get; set; } = "Welcome to Avalonia!";
}

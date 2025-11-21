using CommunityToolkit.Mvvm.ComponentModel;

namespace Camping.App.ViewModels
{
    public partial class PlattegrondViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title = "Camping Plattegrond";
    }
}

using Microsoft.Maui.Layouts;
using Camping.App.ViewModels;

namespace Camping.App.Views;

public partial class PlattegrondView : ContentPage
{
    private readonly PlattegrondViewModel _viewModel;
    public PlattegrondView(PlattegrondViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        CampingPlattegrond.SizeChanged += (s, e) => AddButtons();
    }

    private void AddButtons()
    {
        ButtonOverlayLayout.Children.Clear();

        double imgWidth = CampingPlattegrond.Width;
        double imgHeight = CampingPlattegrond.Height;

        foreach (var area in _viewModel.Areas)
        {
            var btn = new Button
            {
                Text = area.Name,
                TextColor = Colors.Black,
                FontAttributes = FontAttributes.Bold,
                FontSize = 22,
                BackgroundColor = Color.FromArgb("#416722"),
                BorderColor = Colors.Black,
                BorderWidth = 3,
                CornerRadius = 15
            };

            btn.Clicked += (s, e) => DisplayAlert($"{area.Name}", $"Het {area.Name.ToLower()} is een prachtige staanplaats voor al uw campeergenot", "OK");

            var pointer = new PointerGestureRecognizer();
            pointer.PointerEntered += (s, e) => btn.BackgroundColor = Color.FromArgb("#369c0b");
            pointer.PointerExited += (s, e) => btn.BackgroundColor = Color.FromArgb("#416722");
            btn.GestureRecognizers.Add(pointer);

            double x = area.XPosition * imgWidth;
            double y = area.YPosition * imgHeight;
            double w = area.Width * imgWidth;
            double h = area.Height * imgHeight;

            AbsoluteLayout.SetLayoutFlags(btn, AbsoluteLayoutFlags.None);
            AbsoluteLayout.SetLayoutBounds(btn, new Rect(x, y, w, h));

            ButtonOverlayLayout.Children.Add(btn);
        }

        var exitBtn = new Button
        {
            Text = "Afsluiten",
            TextColor = Colors.Black,
            FontAttributes = FontAttributes.Bold,
            FontSize = 22,
            BackgroundColor = Color.FromArgb("#701212"),
            BorderColor = Colors.Black,
            BorderWidth = 3,
            CornerRadius = 10
        };

        exitBtn.Clicked += (s, e) => System.Environment.Exit(0);

        var exitPointer = new PointerGestureRecognizer();
        exitPointer.PointerEntered += (s, e) => exitBtn.BackgroundColor = Color.FromArgb("#b30c0c");
        exitPointer.PointerExited += (s, e) => exitBtn.BackgroundColor = Color.FromArgb("#701212");
        exitBtn.GestureRecognizers.Add(exitPointer);

        double exitX = 0.8632 * imgWidth;
        double exitY = 0.89 * imgHeight;
        double exitW = 0.08 * imgWidth;
        double exitH = 0.06 * imgHeight;

        AbsoluteLayout.SetLayoutFlags(exitBtn, AbsoluteLayoutFlags.None);
        AbsoluteLayout.SetLayoutBounds(exitBtn, new Rect(exitX, exitY, exitW, exitH));

        ButtonOverlayLayout.Children.Add(exitBtn);
    }
}

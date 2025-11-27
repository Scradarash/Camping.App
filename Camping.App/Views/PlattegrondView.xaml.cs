using Camping.App.ViewModels;
using Microsoft.Maui.Layouts;

namespace Camping.App.Views;

public partial class PlattegrondView : ContentPage
{
    private readonly PlattegrondViewModel _vm;

    public PlattegrondView(PlattegrondViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;

        _vm.StaanplaatsGeselecteerd += plaats =>
        {
            DisplayAlert(plaats.Name, $"{plaats.Name} is geselecteerd", "OK");
        };

        CampingPlattegrond.Loaded += (s, e) => DrawButtons();
        CampingPlattegrond.SizeChanged += (s, e) => DrawButtons();
    }

    private void DrawButtons()
    {
        if (CampingPlattegrond.Width <= 0 || CampingPlattegrond.Height <= 0)
            return;

        ButtonOverlayLayout.Children.Clear();

        double imgW = CampingPlattegrond.Width;
        double imgH = CampingPlattegrond.Height;

        foreach (var area in _vm.Staanplaatsen)
        {
            var btn = new Button
            {
                Text = area.Name,
                TextColor = Colors.Black,
                FontSize = 22,
                BackgroundColor = Color.FromArgb("#416722"),
                BorderColor = Colors.Black,
                BorderWidth = 3,
                CornerRadius = 15,
                ZIndex = 100
            };

            btn.Command = _vm.SelectCommand;
            btn.CommandParameter = area;

            var pointer = new PointerGestureRecognizer();
            pointer.PointerEntered += (s, e) => btn.BackgroundColor = Color.FromArgb("#369c0b");
            pointer.PointerExited += (s, e) => btn.BackgroundColor = Color.FromArgb("#416722");
            btn.GestureRecognizers.Add(pointer);

            double x = area.XPosition * imgW;
            double y = area.YPosition * imgH;
            double w = area.Width * imgW;
            double h = area.Height * imgH;

            AbsoluteLayout.SetLayoutFlags(btn, AbsoluteLayoutFlags.None);
            AbsoluteLayout.SetLayoutBounds(btn, new Rect(x, y, w, h));

            ButtonOverlayLayout.Children.Add(btn);
        }
    }
}

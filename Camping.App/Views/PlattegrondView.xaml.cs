using Camping.App.ViewModels;
using Camping.Core.Models;
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
                BackgroundColor = Color.FromArgb("#416722"),
                TextColor = Colors.Black,
                FontSize = 18,
                CornerRadius = 10
            };

            btn.Command = _vm.SelectCommand;
            btn.CommandParameter = area;

            double x = area.XPosition * imgW;
            double y = area.YPosition * imgH;
            double w = area.Width * imgW;
            double h = area.Height * imgH;

            AbsoluteLayout.SetLayoutBounds(btn, new Rect(x, y, w, h));
            AbsoluteLayout.SetLayoutFlags(btn, AbsoluteLayoutFlags.None);

            ButtonOverlayLayout.Children.Add(btn);
        }
    }

    private void OpenKalenderClicked(object sender, EventArgs e)
    {
        KalenderPopup.Show();
    }
}

using Microsoft.Maui.Layouts;

namespace Camping.App.Views;

public partial class PlattegrondView : ContentPage
{
    public PlattegrondView()
    {
        InitializeComponent();
        CampingPlattegrond.SizeChanged += (s, e) => AddButtons();
    }

    private void AddButtons()
    {
        ButtonOverlayLayout.Children.Clear();

        double imgWidth = CampingPlattegrond.Width;
        double imgHeight = CampingPlattegrond.Height;

        var areas = new[]
        {
            new { Name="Groepsveld", X=0.08, Y=0.13, Width=0.232, Height=0.213 },
            new { Name="Trekkersveld", X=0.11, Y=0.4457, Width=0.195, Height=0.181 },
            new { Name="Winterveld", X=0.11, Y=0.669, Width=0.195, Height=0.181 },
            new { Name="Staatseveld", X=0.5677, Y=0.576, Width=0.195, Height=0.1952 },
            new { Name="Oranjeveld", X=0.37, Y=0.59, Width=0.145, Height=0.254 }
        };

        foreach (var area in areas)
        {
            var btn = new Button
            {
                Text = area.Name,
                TextColor = Colors.Black,
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

            double x = area.X * imgWidth;
            double y = area.Y * imgHeight;
            double w = area.Width * imgWidth;
            double h = area.Height * imgHeight;

            AbsoluteLayout.SetLayoutFlags(btn, AbsoluteLayoutFlags.None);
            AbsoluteLayout.SetLayoutBounds(btn, new Rect(x, y, w, h));

            ButtonOverlayLayout.Children.Add(btn);
        }
    }
}

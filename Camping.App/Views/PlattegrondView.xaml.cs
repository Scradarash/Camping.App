using Camping.App.ViewModels;
using Microsoft.Maui.Layouts;

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

        foreach (var area in _viewModel.Staanplaatsen)
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

            btn.Clicked += (s, e) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert($"{area.Name}",
                                       $"Het {area.Name.ToLower()} is een prachtige staanplaats voor al uw campeergenot",
                                       "OK");
                });
            };

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
            TextColor = Colors.White,
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

        var kalenderBtn = new Button
        {
            Text = "Kalender",
            TextColor = Colors.White,
            FontAttributes = FontAttributes.Bold,
            FontSize = 22,
            BackgroundColor = Color.FromArgb("#3c7fd6"),
            BorderColor = Colors.Black,
            BorderWidth = 3,
            CornerRadius = 10
        };

        var kalenderPointer = new PointerGestureRecognizer();
        kalenderPointer.PointerEntered += (s, e) => kalenderBtn.BackgroundColor = Color.FromArgb("#0a5dc9");
        kalenderPointer.PointerExited += (s, e) => kalenderBtn.BackgroundColor = Color.FromArgb("#3c7fd6");
        kalenderBtn.GestureRecognizers.Add(kalenderPointer);

        double kalenderX = 0.4 * imgWidth;
        double kalenderY = 0.05 * imgHeight;
        double kalenderW = 0.08 * imgWidth;
        double kalenderH = 0.06 * imgHeight;

        AbsoluteLayout.SetLayoutFlags(kalenderBtn, AbsoluteLayoutFlags.None);
        AbsoluteLayout.SetLayoutBounds(kalenderBtn, new Rect(kalenderX, kalenderY, kalenderW, kalenderH));

        ButtonOverlayLayout.Children.Add(kalenderBtn);

        kalenderBtn.Clicked += async (s, e) =>
        {
            var aankomstPicker = new DatePicker
            {
                MinimumDate = DateTime.Now,
                MaximumDate = DateTime.Now.AddYears(10),
                Date = DateTime.Now
            };

            var vertrekPicker = new DatePicker
            {
                MinimumDate = DateTime.Now,
                MaximumDate = DateTime.Now.AddYears(10),
                Date = DateTime.Now.AddDays(1)
            };

            var closeBtn = new Button
            {
                Text = "Sluiten",
                BackgroundColor = Color.FromArgb("#701212"),
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 10,
                Margin = new Thickness(0, 20, 0, 0)
            };

            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star }
                },
                Padding = 20,
                BackgroundColor = Colors.White,
                WidthRequest = 300,
                HeightRequest = 220,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            grid.Add(new Label { Text = "Aankomst", FontAttributes = FontAttributes.Bold }, 0, 0);
            grid.Add(aankomstPicker, 0, 1);
            grid.Add(new Label { Text = "Vertrek", FontAttributes = FontAttributes.Bold }, 0, 2);
            grid.Add(vertrekPicker, 0, 3);
            grid.Add(closeBtn, 0, 4);

            var popupPage = new ContentPage
            {
                BackgroundColor = Color.FromArgb("#80000000"),
                Content = grid
            };

            closeBtn.Clicked += async (sender, args) =>
            {
                await Navigation.PopModalAsync();
            };

            await Navigation.PushModalAsync(popupPage);
        };
    }
}
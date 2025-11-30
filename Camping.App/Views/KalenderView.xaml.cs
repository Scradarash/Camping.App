namespace Camping.App.Views;

public partial class KalenderView : ContentPage
{
    public KalenderView()
    {
        InitializeComponent();
        AankomstPicker.Date = DateTime.Now;
        VertrekPicker.Date = DateTime.Now.AddDays(1);
    }

    private void OnAankomstDateSelected(object sender, DateChangedEventArgs e)
    {
        VertrekPicker.MinimumDate = e.NewDate.AddDays(1);

        // Vertrekdatum > Aankomstdatum instellen
        if (VertrekPicker.Date <= e.NewDate)
        {
            VertrekPicker.Date = e.NewDate.AddDays(1);
        }

        // Open direct de VertrekPicker kalender nadat de aankomstdatum is geselecteerd
        // (Lange lambda expression ff doorlopen in comments, want het is niet bepaald common sense)
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            // Kleine vertraging om te zorgen dat de UI klaar is met de vorige actie 
            // *Deze kan misschien wel weg met wat aanpassingen, maar hij was moeilijk aan het doen
            await Task.Delay(10);

            // De VetrekPicker is een variabel waarin de MAUI Datepicker wordt aangeroepen.
            // Het probleem is dus, dat op Android en misschien ook op IOS, dit zo'n scroll ding geeft waaruit je datums kan selecteren.
            // Wij gebruiken uitsluitend Windows, en op Windows is dit zo'n mooi kalendertje (nice!).
            // Maar omdat de DatePicker van MAUI gemaakt is voor alle platforms, 
            // herkent hij Windows specifieke opdrachten niet, hij vertaald de DatePicker naar whatever platform er wordt gebruikt
            // Maar met wat reflectie kunnen we toch die Windows specifieke eigenschappen gebruiken. 

            var platformView = VertrekPicker.Handler?.PlatformView;

            // *Dit breekt de boel niet op andere platforms. De view bestaat daar wel, maar de 'GetProperty' hieronder vangt het veilig op.
            if (platformView != null)
            {
                // We zoeken naar de eigenschap "IsCalendarOpen" en zetten die op 'true'
                // Dit werkt op Windows, maar niet op Android/IOS (daar werkt deze eigenschap niet, property blijft dan leeg/null en er gebeurt niks)
                var property = platformView.GetType().GetProperty("IsCalendarOpen");
                property?.SetValue(platformView, true);
            }
        });
    }

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
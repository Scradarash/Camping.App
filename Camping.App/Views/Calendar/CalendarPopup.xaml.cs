using System;
using Microsoft.Maui.Controls;

namespace Camping.App.Views.Calendar;

public partial class CalendarPopup : ContentView
{
    private DateTime _currentMonth = DateTime.Today;

    public CalendarPopup()
    {
        InitializeComponent();
        RenderCalendar();
    }

    public void Show()
    {
        IsVisible = true;
        InputTransparent = false;
    }

    public void Hide()
    {
        IsVisible = false;
        InputTransparent = true;
    }

    private void RenderCalendar()
    {
        CalendarGrid.Children.Clear();

        var first = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
        int startCol = ((int)first.DayOfWeek + 6) % 7;
        int daysInMonth = DateTime.DaysInMonth(_currentMonth.Year, _currentMonth.Month);

        LblMonth.Text = _currentMonth.ToString("MMMM yyyy");

        int row = 0;
        int col = startCol;

        for (int day = 1; day <= daysInMonth; day++)
        {
            var btn = new Button
            {
                Text = day.ToString(),
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Black,
                CornerRadius = 20,
                Margin = new Thickness(4)
            };

            CalendarGrid.Add(btn, col, row);

            col++;
            if (col >= 7)
            {
                col = 0;
                row++;
            }
        }
    }

    private void NextMonthClicked(object sender, EventArgs e)
    {
        _currentMonth = _currentMonth.AddMonths(1);
        RenderCalendar();
    }

    private void PreviousMonthClicked(object sender, EventArgs e)
    {
        _currentMonth = _currentMonth.AddMonths(-1);
        RenderCalendar();
    }
}

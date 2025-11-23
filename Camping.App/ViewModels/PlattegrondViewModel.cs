using System.Collections.ObjectModel;
using Camping.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Camping.App.ViewModels;

public partial class PlattegrondViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ClickArea> areas = new();

    public PlattegrondViewModel()
    {
        LoadAreas();
    }

    private void LoadAreas()
    {
        Areas.Add(new ClickArea
        {
            Name = "Groepsveld",
            XPosition = 0.08,
            YPosition = 0.13,
            Width = 0.232,
            Height = 0.213
        });

        Areas.Add(new ClickArea
        {
            Name = "Trekkersveld",
            XPosition = 0.11,
            YPosition = 0.4457,
            Width = 0.195,
            Height = 0.181
        });

        Areas.Add(new ClickArea
        {
            Name = "Winterveld",
            XPosition = 0.11,
            YPosition = 0.669,
            Width = 0.195,
            Height = 0.181
        });

        Areas.Add(new ClickArea
        {
            Name = "Staatseveld",
            XPosition = 0.5677,
            YPosition = 0.576,
            Width = 0.195,
            Height = 0.1952
        });

        Areas.Add(new ClickArea
        {
            Name = "Oranjeveld",
            XPosition = 0.37,
            YPosition = 0.59,
            Width = 0.145,
            Height = 0.254
        });
    }
}

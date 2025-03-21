using System;
using System.Collections.ObjectModel;
using LightingApp.Models;
using ReactiveUI;

namespace LightingApp.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public ObservableCollection<DeviceViewModel> Devices { get; }

        public MainWindowViewModel()
        {
            var random = new Random();
            Devices = new ObservableCollection<DeviceViewModel>
            {
                new DeviceViewModel(new Flashlight(random)),
                new DeviceViewModel(new DeskLamp(random)),
                new DeviceViewModel(new Chandelier(random)),
                new DeviceViewModel(new FloorLamp(random))
            };
        }
    }
}
using System;
using System.Windows.Input;
using LightingApp.Models;
using ReactiveUI;

namespace LightingApp.ViewModels
{
    public class DeviceViewModel : ReactiveObject
    {
        private readonly LightingDevice device;
        private string lastAction;

        public DeviceViewModel(LightingDevice lightingDevice)
        {
            device = lightingDevice ?? throw new ArgumentNullException(nameof(lightingDevice));
            TurnOnCommand = ReactiveCommand.Create(ExecuteTurnOn);
            TurnOffCommand = ReactiveCommand.Create(ExecuteTurnOff);
            PlugInCommand = ReactiveCommand.Create(ExecutePlugIn, canExecute: this.WhenAnyValue(x => x.CanPlugIn));
            UnplugCommand = ReactiveCommand.Create(ExecuteUnplug, canExecute: this.WhenAnyValue(x => x.CanUnplug));
            lastAction = "Инициализировано";

            device.Broken += (s, e) =>
            {
                LastAction = "Устройство сломалось";
                this.RaisePropertyChanged(nameof(IsBroken));
                this.RaisePropertyChanged(nameof(IsOperational));
                this.RaisePropertyChanged(nameof(Status));
                this.RaisePropertyChanged(nameof(ConnectionStatus));
            };
        }

        public string Name => device.GetType().Name;
        public bool IsOn => device.IsOn;
        public bool IsBroken => device.IsBroken;
        public bool IsOperational => !device.IsBroken;
        public bool CanPlugIn => !device.IsBroken && (device is DeskLamp || device is FloorLamp) && !((device as DeskLamp)?.IsPluggedIn ?? (device as FloorLamp)?.IsPluggedIn ?? true);
        public bool CanUnplug => (device is DeskLamp || device is FloorLamp) && ((device as DeskLamp)?.IsPluggedIn ?? (device as FloorLamp)?.IsPluggedIn ?? false);
        public bool HasPlug => device is DeskLamp || device is FloorLamp;

        public string Status => device.IsBroken ? "Сломано" : device switch
        {
            Chandelier chandelier => chandelier.GetCurrentMode(),
            _ => IsOn ? "Включено" : "Выключено"
        };

        public string ConnectionStatus => device switch
        {
            DeskLamp deskLamp => deskLamp.IsPluggedIn ? "Подключено к сети" : "Нет подключения",
            FloorLamp floorLamp => floorLamp.IsPluggedIn ? "Подключено к сети" : "Нет подключения",
            _ => string.Empty
        };

        public string LastAction
        {
            get => lastAction;
            private set => this.RaiseAndSetIfChanged(ref lastAction, value);
        }

        public ICommand TurnOnCommand { get; }
        public ICommand TurnOffCommand { get; }
        public ICommand PlugInCommand { get; }
        public ICommand UnplugCommand { get; }

        private void ExecuteTurnOn()
        {
            if (!IsOperational) return;
            if ((device is DeskLamp deskLamp && !deskLamp.IsPluggedIn) || (device is FloorLamp floorLamp && !floorLamp.IsPluggedIn))
            {
                LastAction = "Требуется подключение к сети";
                return;
            }
            device.TurnOn();
            LastAction = IsBroken ? "Устройство сломалось" : device is Chandelier ? "Переключён режим" : "Включено";
            this.RaisePropertyChanged(nameof(IsOn));
            this.RaisePropertyChanged(nameof(Status));
        }

        private void ExecuteTurnOff()
        {
            if (!IsOperational) return;
            device.TurnOff();
            LastAction = device is Chandelier ? "Переключён режим" : "Выключено";
            this.RaisePropertyChanged(nameof(IsOn));
            this.RaisePropertyChanged(nameof(Status));
        }

        private void ExecutePlugIn()
        {
            if (device is DeskLamp deskLamp)
                deskLamp.PlugIn();
            else if (device is FloorLamp floorLamp)
                floorLamp.PlugIn();
            LastAction = "Подключено к сети";
            this.RaisePropertyChanged(nameof(CanPlugIn));
            this.RaisePropertyChanged(nameof(CanUnplug));
            this.RaisePropertyChanged(nameof(ConnectionStatus));
        }

        private void ExecuteUnplug()
        {
            if (device is DeskLamp deskLamp)
                deskLamp.Unplug();
            else if (device is FloorLamp floorLamp)
                floorLamp.Unplug();
            LastAction = "Отключено от сети";
            this.RaisePropertyChanged(nameof(CanPlugIn));
            this.RaisePropertyChanged(nameof(CanUnplug));
            this.RaisePropertyChanged(nameof(IsOn));
            this.RaisePropertyChanged(nameof(Status));
            this.RaisePropertyChanged(nameof(ConnectionStatus));
        }
    }
}
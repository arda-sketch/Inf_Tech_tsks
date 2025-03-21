using System;

namespace LightingApp.Models
{
    public abstract class LightingDevice
    {
        public bool IsOn { get; protected set; }
        public bool IsBroken { get; protected set; }

        public event EventHandler? Broken; // Исправлено: допускает null

        protected LightingDevice()
        {
            IsOn = false;
            IsBroken = false;
        }

        protected void TriggerBrokenEvent()
        {
            IsBroken = true;
            IsOn = false;
            Broken?.Invoke(this, EventArgs.Empty);
        }

        public abstract void TurnOn();
        public abstract void TurnOff();
    }
}
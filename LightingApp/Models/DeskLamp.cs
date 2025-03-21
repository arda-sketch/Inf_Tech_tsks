using System;

namespace LightingApp.Models
{
    public class DeskLamp : LightingDevice
    {
        public bool IsPluggedIn { get; private set; }
        private readonly Random random;

        public DeskLamp(Random randomGenerator)
        {
            random = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));
            IsPluggedIn = false;
        }

        public override void TurnOn()
        {
            if (IsBroken || !IsPluggedIn) return;
            if (random.NextDouble() < 0.05)
            {
                TriggerBrokenEvent();
            }
            else
            {
                IsOn = true;
            }
        }

        public override void TurnOff()
        {
            if (!IsBroken)
            {
                IsOn = false;
            }
        }

        public void PlugIn()
        {
            if (!IsBroken)
            {
                IsPluggedIn = true;
            }
        }

        public void Unplug()
        {
            IsPluggedIn = false;
            if (IsOn)
            {
                IsOn = false;
            }
        }
    }
}
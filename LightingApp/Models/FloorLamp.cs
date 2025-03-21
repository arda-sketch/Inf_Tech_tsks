using System;

namespace LightingApp.Models
{
    public class FloorLamp : LightingDevice
    {
        public bool IsPluggedIn { get; private set; }
        private readonly Random random;

        public FloorLamp(Random randomGenerator)
        {
            random = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));
            IsPluggedIn = false;
        }

        public override void TurnOn()
        {
            if (IsBroken || !IsPluggedIn) return; // Не включается без подключения
            if (random.NextDouble() < 0.1)
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
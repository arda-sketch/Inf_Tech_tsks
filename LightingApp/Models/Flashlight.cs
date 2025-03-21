using System;

namespace LightingApp.Models
{
    public class Flashlight : LightingDevice
    {
        private readonly Random random;

        public Flashlight(Random randomGenerator)
        {
            random = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));
        }

        public override void TurnOn()
        {
            if (IsBroken) return;
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
    }
}
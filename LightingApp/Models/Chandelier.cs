using System;

namespace LightingApp.Models
{
    public class Chandelier : LightingDevice
    {
        private int mode; // 0 - выкл, 1 - часть 1, 2 - часть 2, 3 - обе части
        private readonly Random random;

        public Chandelier(Random randomGenerator)
        {
            random = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));
            mode = 0;
        }

        public override void TurnOn()
        {
            if (IsBroken) return;
            if (random.NextDouble() < 0.15)
            {
                TriggerBrokenEvent();
            }
            else if (mode < 3)
            {
                mode++;
                IsOn = true;
            }
        }

        public override void TurnOff()
        {
            if (IsBroken || mode == 0) return;
            mode--; // Поэтапное уменьшение: 3 -> 2 -> 1 -> 0
            IsOn = mode > 0;
        }

        public string GetCurrentMode()
        {
            return mode switch
            {
                0 => "Выключено",
                1 => "Часть 1",
                2 => "Часть 2",
                3 => "Обе части",
                _ => "Неизвестно"
            };
        }
    }
}
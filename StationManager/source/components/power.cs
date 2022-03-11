using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers
{
    /// <summary> 
    /// Energetika 
    /// </summary> 
    class Power
    {
        /// <summary>
        /// Instance
        /// </summary>
        private readonly Group current = null;

        /// <summary>
        /// Objekt svetla
        /// </summary>
        private Light light = null;

        /// <summary>
        /// Reference mnozstvi uranu
        /// </summary>
        private double reference = 0;

        /// <summary>
        /// Status
        /// </summary>
        private readonly List<string> status = new List<string>();

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="group">Blok</param>
        public Power(string group)
        {
            current = new Group(group);
        }

        /// <summary>
        /// Nastaveni referencni hodnoty uranu
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Power SetUraniuReference(double amount)
        {
            reference = amount;
            return this;
        }

        /// <summary> 
        /// Status energetickych zdroju
        /// </summary> 
        /// <returns></returns> 
        public List<string> GetStatus()
        {
            // ocista
            status.Clear();
            //prochazeni bloku 
            foreach (var block in current)
            {
                if (block.Is<IMyBatteryBlock>())
                {
                    ReadBattery(block);
                }
                else if (block.Is<IMyPowerProducer>())
                {
                    ReadProducer(block);
                } else
                {
                    throw new Exception("E-PW-01: Block '" + block.Name + "' is neither PowerProducer or Battery");
                }
            }
            // vraceni
            return status;
        }

        /// <summary>
        /// Pridani svetelne signalizace
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Power AddFailsafeLight(string name)
        {
            light = new Light(name);
            return this;
        }

        /// <summary> 
        /// Baterkovy failsafe 
        /// </summary> 
        /// <returns>Power</returns> 
        public Power EnableBatteryFailsafe()
        {
            // definice
            int producerFailed = 0;
            int producerCount = 0;
            //overeni provozu producentu energie
            foreach (var block in current.Only<IMyPowerProducer>())
            {
                producerCount++;
                if (!block.IsWorking)
                {
                    producerFailed++;
                }
            }
            //pokud reaktor selhal pripnou se baterky a spusti poplach jinak se vse vypne 
            if (producerFailed == producerCount)
            {
                // prepnuti baterek na vybijeni
                current.SetValue<IMyBatteryBlock, long>("ChargeMode", 2);
                // vystrazne svetlo
                if (light != null)
                {
                    light
                       .Blink(1)
                       .Red()
                       .Intensity(5)
                       .On();
                }
            }
            else
            {
                // prepnuti baterek na nabijeni
                current.SetValue<IMyBatteryBlock, long>("ChargeMode", 1);
                // vypnuti vystrazneho svetla
                if (light != null)
                {
                    light.Off();
                }
            }
            //vraceni 
            return this;
        }

        /// <summary>
        /// Nacteni stavu baterky
        /// </summary>
        /// <param name="block"></param>
        private void ReadBattery(Block block)
        {
            // definice
            var battery = block.As<IMyBatteryBlock>();
            // hodnoty 
            float input = battery.CurrentInput;
            float output = battery.CurrentOutput;
            float stored = battery.CurrentStoredPower;
            float maximum = battery.MaxStoredPower;
            // overeni funkcnosti
            if (block.IsWorking)
            {
                if (stored == maximum)
                {
                    status.Add(Formater.BarsWithPercent(Formater.Status(block, Status.FullyCharged), stored, maximum));
                }
                else if (input > output)
                {
                    status.Add(Formater.BarsWithPercent(Formater.Status(block, Status.Recharging), stored, maximum));
                }
                else
                {
                    status.Add(Formater.BarsWithPercent(Formater.Status(block, Status.Discharging), stored, maximum));
                }
            }
            else
            {
                status.Add(Formater.BarsWithPercent(Formater.Status(block, Status.NotWorking), 0, maximum));
            }
        }

        /// <summary>
        /// Nacteni stavu generatoru
        /// </summary>
        /// <param name="block"></param>
        private void ReadProducer(Block block)
        {
            // generator
            var producer = block.As<IMyPowerProducer>();
            // hodnoty 
            float actual = producer.CurrentOutput;
            float maximum = producer.MaxOutput;
            // reaktor (kontrola uranu
            if (block.Is<IMyReactor>())
            {
                // definice
                var items = Tools.GetInventoryInfo(block.As<IMyReactor>().GetInventory(0));
                var uranium = items.GetValueOrDefault("Uranium", (0, -1)).Amount;
                // kontrola mnozstvi uranu
                if (uranium > 0)
                {
                    status.Add(Formater.BarsWithPercent(Formater.Status(block, uranium < reference ? Status.UraniumIsLow : Status.Working), actual, maximum));
                }
                else
                {
                    status.Add(Formater.BarsWithPercent(Formater.Status(block, Status.UraniumIsEmpty), 0, maximum));
                }
            }
            else
            {
                status.Add(Formater.BarsWithPercent(Formater.Status(block, Status.Working), actual, maximum));
            }
        }
    }
}
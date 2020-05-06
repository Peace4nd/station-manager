

using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using VRage;

namespace SpaceEngineers
{
    /// <summary> 
    /// Energetika 
    /// </summary> 
    class Power
    {
        /// <summary> 
        /// Referencni hodnota uranu v reaktoru 
        /// </summary> 
        private int UraniumReference = 0;

        /// <summary>
        /// Instance
        /// </summary>
        private readonly Block Instance = null;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="block">Blok</param>
        public Power(string block) {
            Instance = new Block(block);
        }

        /// <summary> 
        /// Nastaveni referencni hodnoty uranu v reaktoru 
        /// </summary> 
        /// <param name="reference">Referencni hodnota</param> 
        /// <returns>Power</returns> 
        public Power SetUraniuReference(int reference)
        {
            UraniumReference = reference;
            return this;
        }

        /// <summary> 
        /// Status generatoru 
        /// </summary> 
        /// <returns></returns> 
        public List<string> GetReactorStatus()
        {
            // definice
            List<string> status = new List<string>();
            //prochazeni bloku 
            foreach (var block in Instance.GetByType<IMyReactor>())
            {
                //hodnoty
                IMyReactor reactor = block.Value as IMyReactor;
                float actual = reactor.CurrentOutput;
                float maximum = reactor.MaxOutput;
                float uranium = 0;
                //overeni funkcnosti 
                if (reactor.IsFunctional && reactor.IsWorking)
                {
                    //overeni mnozstvi uranu 
                    if (UraniumReference > 0)
                    {
                        // polozky inventare
                        List<MyInventoryItem> items = new List<MyInventoryItem>();
                        reactor.GetInventory(0).GetItems(items);
                        // vypocet mnozstvi uranu 
                        if (items.Count > 0)
                        {
                            for (int j = 0; j < items.Count; j++)
                            {
                                if (items[j].Type.SubtypeId == "Uranium")
                                {
                                    uranium += (float)items[j].Amount;
                                }
                            }
                        }
                        // doplneni statusu do vystupu
                        status.Add(Formater.BarsWithPercent(Block.GetStatus(block.Value, uranium <= UraniumReference ? Status.UraniumIsLow : Status.Working), actual, maximum));
                    }
                    else
                    {
                        status.Add(Formater.BarsWithPercent(Block.GetStatus(block.Value, Status.UraniumIsEmpty), 0, maximum));
                    }
                }
                else
                {
                    status.Add(Formater.BarsWithPercent(Block.GetStatus(block.Value, Status.NotWorking), 0, maximum));
                }
            }
            // vraceni 
            return status;
        }

        /// <summary> 
        /// Baterkovy failsafe 
        /// </summary> 
        /// <returns>Power</returns> 
        public Power EnableBatteryFailsafe()
        {
            //overeni provoyu reaktoru 
            int reactorFailed = 0;
            int reactorCount = 0;
            foreach (var block in Instance.GetByType<IMyReactor>())
            {
                reactorCount++;
                if (!block.Value.IsFunctional || !block.Value.IsWorking)
                {
                    reactorFailed++;
                }
            }
            //pokud reaktor selhal pripnou se baterky a spusti poplach jinak se vse vypne 
            if (reactorFailed == reactorCount)
            {
                // prepnuti baterek na vybijeni
                Instance.SetValue<IMyBatteryBlock, long>("ChargeMode", 2);
                // vystrazne svetlo
                if (Instance.Exist<IMyInteriorLight>())
                {
                    Light.Create(Instance)
                       .Blink(1)
                       .Red()
                       .Intensity(5)
                       .On();
                }
                // vystrazny zvuk
                if (Instance.Exist<IMySoundBlock>())
                {
                    Sound.Create(Instance)
                        .Play();
                }
            }
            else
            {
                // prepnuti baterek na nabijeni
                Instance.SetValue<IMyBatteryBlock, long>("ChargeMode", 1);
                // zastaveni zvuku
                Instance.Action<IMySoundBlock>("StopSound");
                // vypnuti vystrazneho svetla
                Instance.Action<IMyInteriorLight>("OnOff_Off");
            }
            //vraceni 
            return this;
        }

        /// <summary> 
        /// Status baterek 
        /// </summary> 
        /// <returns></returns> 
        public List<string> GetBatteryStatus()
        {
            // definice
            List<string> status = new List<string>();
            //prochazeni bloku 
            foreach (var block in Instance.GetByType<IMyBatteryBlock>())
            {
                // hodnoty 
                float input = block.Value.CurrentInput;
                float output = block.Value.CurrentOutput;
                float stored = block.Value.CurrentStoredPower;
                float maximum = block.Value.MaxStoredPower;
                // overeni funkcnosti
                if (block.Value.IsFunctional && block.Value.IsWorking)
                {
                    if (stored == maximum)
                    {
                        status.Add(Formater.BarsWithPercent(Block.GetStatus(block.Value, Status.FullyCharged), stored, maximum));
                    }
                    else if (input > output)
                    {
                        status.Add(Formater.BarsWithPercent(Block.GetStatus(block.Value, Status.Recharging), stored, maximum));
                    }
                    else
                    {
                        status.Add(Formater.BarsWithPercent(Block.GetStatus(block.Value, Status.Discharging), stored, maximum));
                    }
                }
                else
                {
                    status.Add(Formater.BarsWithPercent(Block.GetStatus(block.Value, Status.NotWorking), 0, maximum));
                }
            }
            // zadne baterky
            if (status.Count == 0)
            {
                status.Add(Status.NoBatteries);
            }
            // vraceni
            return status;
        }

        /// <summary> 
        /// Status solarnich panelu 
        /// </summary> 
        /// <param name="groupedName">Nazev pro seskupeni</param> 
        /// <returns></returns> 
        public List<string> GetSolarStatus()
        {
            // definice
            List<string> status = new List<string>();
            //prochazeni bloku 
            foreach (var block in Instance.GetByType<IMySolarPanel>())
            {
                // hodnoty 
                float actual = block.Value.CurrentOutput;
                float maximum = block.Value.MaxOutput;
                // overeni funkcnosti
                status.Add(Formater.BarsWithPercent(Block.GetStatus(block.Value), actual, maximum));
            }
            // zadne panely
            if (status.Count == 0)
            {
                status.Add(Status.NoSolarPanels);
            }
            // vraceni
            return status;
        }
    }
}
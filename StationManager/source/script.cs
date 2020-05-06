using Sandbox.ModAPI.Ingame;

namespace SpaceEngineers
{
    public sealed class Program : MyGridProgram
    {
        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        public void Save()
        {
            // ulozeni stavu
        }

        public void Main()
        {
            // debugger
            Debugger.SetEcho(Echo);
            Debugger.Enable();
            // terminal
            Terminal.Init(GridTerminalSystem);


            // napajeni a provozni plyny
            Power pwr = new Power("Power")
                .SetUraniuReference(50)
                .EnableBatteryFailsafe();
            Gases gas = new Gases("Gases")
                .SetIceReference(1500);
            // status napajeni a plynu
            Display.Create("Panel_Basic")
                .Clear()
                .Small()
                .White()
                .Line("Energy")
                .Ruler()
                .Lines(pwr.GetReactorStatus())
                .Lines(pwr.GetBatteryStatus())
                .Lines(pwr.GetSolarStatus())
                .NewLine()
                .Line("H2/O2")
                .Ruler()
                .Lines(gas.GetIceStatus())
                .Lines(gas.GetTankStatus());

            // nakladovy prostor
            Cargo crg = new Cargo("Cargo")
                .EnableSorter()
                .EnableRefineryControl()
                .EnableCache()
                .EnableAssemblerControl();
            // status skladu
            Display.Create("Panel_Cargo")
                .Clear()
                .Small()
                .White()
                .Line("Component")
                .Ruler()
                .Lines(crg.GetComponentList())
                .NewLine()
                .Line("Ingot")
                .Ruler()
                .Lines(crg.GetIngotList())
                .NewLine()
                .Line("Ore")
                .Ruler()
                .Lines(crg.GetOreList());
            // rafinacni status
            Display.Create("Panel_Refinery")
                .Clear()
                .Small()
                .Line("Refinery status")
                .Ruler()
                .Table(crg.GetRefineryOverview());
            // vyrobni status
            Display.Create("Panel_Assembler")
                .Clear()
                .Small()
                .Line("Assembler status")
                .Ruler()
                .Table(crg.GetAssemblesOverview());
        }
    }
}

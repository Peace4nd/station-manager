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
                .Text()
                .Clear()
                .Small()
                .White()
                .Line("Energy")
                .Ruler()
                .Lines(pwr.GetReactorStatus())
                //.Lines(pwr.GetBatteryStatus())
                //.Lines(pwr.GetSolarStatus())
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

            // chybejici rudy
            Display.Create("Panel_Missing", true)
                .Text()
                .Clear()
                .Medium()
                .Line("Missing ingots")
                .Ruler()
                .Table(crg.GetMissing());

            // status skladu
            Display.Create("Panel_Cargo")
                .Text()
                .Clear()
                .Small()
                .White()
                .Line("Component")
                .Ruler()
                .Lines(crg.GetComponentList())
                .NewLine()
                .NewLine()
                .Line("Ingot")
                .Ruler()
                .Lines(crg.GetIngotList())
                .NewLine()
                .NewLine()
                .Line("Ore")
                .Ruler()
                .Lines(crg.GetOreList());

            // rafinacni status
            Display.Create("Panel_Refinery")
                .Text()
                .Clear()
                .Small()
                .Line("Refinery status")
                .Ruler()
                .Table(crg.GetRefineryOverview());

            // vyrobni status
            bool stucked;
            TableData table = crg.GetAssemblesOverview(out stucked);
            Display.Create("Panel_Assembler")
                .Text()
                .Clear()
                .Small()
                .Line("Assembler status")
                .Ruler()
                .Table(table)
                .Alert(stucked);

            // kokpit
            // Cargo crg2 = new Cargo("Cargo");
            // Debugger.Log("ore", crg2.GetOreList());
            // Display.Create("Panel_Drill").Cocpit().Clear().Large().Lines(crg2.GetOreList(true));



            // ReferenceEquals na rudy200k
        }
    }
}

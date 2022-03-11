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

            /*
            // prechodove komory
            Airlock arl = new Airlock()
                .AddDoor("Airlock_Coridor_Entrance", "Airlock_Coridor_Exit")
                .AddDoor("Airlock_Basement_Entrance", "Airlock_Basement_Exit")
                .AddDoor("Airlock_Main_Entrance", "Airlock_Main_Exit")
                .AddDoor("Airlock_Hangar_Entrance", "Airlock_Hangar_Exit")
                .Watch();
            */

            // napajeni plyny
            Power pwr = new Power("Power")
                .SetUraniuReference(50)
                .EnableBatteryFailsafe();

            // provozni plyny
            Gases gas = new Gases("Gases")
                .SetIceReference(2500);

            // status napajeni a plynu
            new Display("Panel_Basic")
                .Text()
                .Clear()
                .Small()
                .White()
                .Line("Energy")
                .Ruler()
                .Lines(pwr.GetStatus())
                .NewLine()
                .Line("H2/O2")
                .Ruler()
                .Lines(gas.GetStatus());

            // nakladovy prostor
            Cargo crg = new Cargo()
                .AddStorage("Cargo_Component", StorageType.Component)
                .AddStorage("Cargo_Ingot", StorageType.Ingot)
                .AddStorage("Cargo_Ore", StorageType.Ore)
                .AddSorter("Cargo_Sorter")
                .AddCache("Cargo_Cache")
                .AddThrower("Cargo_Thrower", new string[] { "Stone" })
                .AddAssembler("Cargo_Assembler", true)
                .AddRefinery("Cargo_Refinery", true)
                .Watch();

            // chybejici rudy
            new Display("Panel_Missing")
                .Text()
                .Clear()
                .Medium()
                .Line("Missing ingots")
                .Ruler()
                .Table(crg.GetMissing());

            // status skladu
            new Display("Panel_Cargo")
                .Text()
                .Clear()
                .Small()
                .White()
                .Line("Component")
                .Ruler()
                .Lines(crg.GetComponentOverview())
                .NewLine()
                .NewLine()
                .Line("Ingot")
                .Ruler()
                .Lines(crg.GetIngotOverview())
                .NewLine()
                .NewLine()
                .Line("Ore")
                .Ruler()
                .Lines(crg.GetOreOverview());

            // rafinacni status
            new Display("Panel_Refinery")
                .Text()
                .Clear()
                .Small()
                .Line("Refinery status")
                .Ruler()
                .Table(crg.GetRefineryOverview());

            // vyrobni status
            new Display("Panel_Assembler")
                .Text()
                .Clear()
                .Small()
                .Line("Assembler status")
                .Ruler()
                .Table(crg.GetAssemblesOverview())
                .Alert(crg.GetAssemblesStucked());

            // kokpit
            // Cargo crg2 = new Cargo("Cargo");
            // Debugger.Log("ore", crg2.GetOreList());
            // Display.Create("Panel_Drill").Cocpit().Clear().Large().Lines(crg2.GetOreList(true));
        }
    }
}

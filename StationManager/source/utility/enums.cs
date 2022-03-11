namespace SpaceEngineers
{
    /// <summary> 
    /// Status
    /// </summary> 
    struct Status
    {
        public const string UraniumIsLow = "Low";
        public const string UraniumIsEmpty = "Depleted";
        public const string Stuck = "Stuck";
        public const string NotWorking = "Stopped";
        public const string Working = "Working";
        public const string Recharging = "Charge";
        public const string Discharging = "Discharge";
        public const string FullyCharged = "Charged";
        public const string Idle = "Idle";
        public const string NoBatteries = "No batteries detected";
        public const string NoSolarPanels = "No solar panels detected";
    }

    /// <summary>
    /// Typ ulozneho prostoru
    /// </summary>
    enum StorageType
    {
        Component,
        Ore,
        Ingot,
        Ice
    }

    /// <summary>
    /// Typ inventare
    /// </summary>
    enum InventoryType
    {
        Input,
        Output
    }
}

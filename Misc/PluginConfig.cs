using BepInEx.Configuration;

namespace CleaningCompany.Misc
{
    public class PluginConfig
    {
        readonly ConfigFile configFile;

        // active bodies
        public bool SPIDER {  get; set; }
        public bool THUMPER {  get; set; }
        public bool NUTCRACKER {  get; set; }
        public bool CENTIPEDE {  get; set; }
        public bool HOARDER {  get; set; }
        public bool BRACKEN {  get; set; }
        public bool MOUTHDOG {  get; set; }
        public bool BABOON {  get; set; }

        // bodyspawn values
        public int SPIDER_MIN {  get; set; }
        public int THUMPER_MIN {  get; set; }
        public int NUTCRACKER_MIN {  get; set; }
        public int CENTIPEDE_MIN {  get; set; }
        public int HOARDER_MIN {  get; set; }
        public int BRACKEN_MIN {  get; set; }
        public int MOUTHDOG_MIN {  get; set; }
        public int BABOON_MIN {  get; set; }
        public int NUTCRACKER_MAX {  get; set; }
        public int SPIDER_MAX {  get; set; }
        public int THUMPER_MAX {  get; set; }
        public int CENTIPEDE_MAX {  get; set; }
        public int HOARDER_MAX {  get; set; }
        public int BRACKEN_MAX {  get; set; }
        public int MOUTHDOG_MAX {  get; set; }
        public int BABOON_MAX {  get; set; }

        // bodystpawn weights
        public float NUTCRACKER_WEIGHT {  get; set; }
        public float SPIDER_WEIGHT {  get; set; }
        public float THUMPER_WEIGHT {  get; set; }
        public float CENTIPEDE_WEIGHT {  get; set; }
        public float HOARDER_WEIGHT {  get; set; }
        public float BRACKEN_WEIGHT {  get; set; }
        public float MOUTHDOG_WEIGHT {  get; set; }
        public float BABOON_WEIGHT {  get; set; }


        public PluginConfig(ConfigFile cfg)
        {
            configFile = cfg;
        }

        private T ConfigEntry<T>(string section, string key, T defaultVal, string description)
        {
            return configFile.Bind(section, key, defaultVal, description).Value;
        }

        public void InitBindings()
        {
            CENTIPEDE_MIN = ConfigEntry("Body Values", "Min price of Centipede Bodies", 45, "");
            CENTIPEDE_MAX = ConfigEntry("Body Values", "Max price of Centipede Bodies", 70, "");
            HOARDER_MIN = ConfigEntry("Body Values", "Min price of Hoarding Bug Bodies", 55, "");
            HOARDER_MAX = ConfigEntry("Body Values", "Max price of Hoarding Bug Bodies", 88, "");
            SPIDER_MIN = ConfigEntry("Body Values", "Min price of Spider Bodies", 70, "");
            SPIDER_MAX = ConfigEntry("Body Values", "Max price of Spider Bodies", 110, "");
            THUMPER_MIN = ConfigEntry("Body Values", "Min price of Thumper Bodies", 120, "");
            THUMPER_MAX = ConfigEntry("Body Values", "Max price of Thumper Bodies", 160, "");
            NUTCRACKER_MIN = ConfigEntry("Body Values", "Min price of Nutcracker Bodies", 125, "");
            NUTCRACKER_MAX = ConfigEntry("Body Values", "Max price of Nutcracker Bodies", 150, "");
            BRACKEN_MAX = ConfigEntry("Body Values", "Max price of Bracken Bodies", 140, "");
            BRACKEN_MIN = ConfigEntry("Body Values", "Min price of Bracken Bodies", 100, "");
            BRACKEN_MAX = ConfigEntry("Body Values", "Max price of Baboon Hawk Bodies", 155, "");
            BABOON_MIN = ConfigEntry("Body Values", "Min price of Baboon Hawk Bodies", 105, "");
            MOUTHDOG_MAX = ConfigEntry("Body Values", "Max price of Eyeless Dog Bodies", 200, "");
            MOUTHDOG_MIN = ConfigEntry("Body Values", "Min price of Eyeless Dog Bodies", 175, "");

            CENTIPEDE = ConfigEntry("Body Weights", "Enable selling of centipede bodies", true, "");
            HOARDER = ConfigEntry("Body Weights", "Enable selling of hoarder bodies", true, "");
            SPIDER = ConfigEntry("Body Weights", "Enable selling of spider bodies", true, "");
            THUMPER = ConfigEntry("Body Weights", "Enable selling of crawler / half / thumper bodies", true, "");
            NUTCRACKER = ConfigEntry("Body Weights", "Enable selling of nutcracker bodies", true, "");
            MOUTHDOG = ConfigEntry("Body Weights", "Enable selling of eyeless dog bodies", true, "");
            BABOON = ConfigEntry("Body Weights", "Enable selling of baboon hawk bodies", true, "");
            BRACKEN = ConfigEntry("Body Weights", "Enable selling of bracken bodies", true, "");

            CENTIPEDE_WEIGHT = ConfigEntry("Body Weights", "Weight of Centipede Bodies", 1.65f, "");
            HOARDER_WEIGHT = ConfigEntry("Body Weights", "Weight of Hoarding Bug Bodies", 1.6f, "");
            SPIDER_WEIGHT = ConfigEntry("Body Weights", "Weight of Spider Bodies", 2.3f, "");
            THUMPER_WEIGHT = ConfigEntry("Body Weights", "Weight of Thumper Bodies", 2.9f, "");
            NUTCRACKER_WEIGHT = ConfigEntry("Body Weights", "Weight of Nutcracker Bodies", 2.9f, "");
            MOUTHDOG_WEIGHT = ConfigEntry("Body Weights", "Weight of Eyeless Dog Bodies", 3.0f, "");
            BABOON_WEIGHT = ConfigEntry("Body Weights", "Weight of Baboon Hawk Bodies", 2.5f, "");
            BRACKEN_WEIGHT = ConfigEntry("Body Weights", "Weight of Bracken Bodies", 1.9f, "");
        }
    }
}

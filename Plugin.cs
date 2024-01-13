using BepInEx;
using BepInEx.Configuration;
using CleaningCompany.Misc;
using CleaningCompany.Monos;
using HarmonyLib;
using LethalLib.Extras;
using LethalLib.Modules;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace CleaningCompany
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency("evaisa.lethallib", "0.10.0")]
    public class Plugin : BaseUnityPlugin
    {
        readonly Harmony harmony = new Harmony(GUID);
        const string GUID = "malco.sell_bodies";
        const string NAME = "Sell Bodies";
        const string VERSION = "1.0.0";

        static string root = "Assets/CleaningAssets/";


        Dictionary<string, int> minBodyValues;

        Dictionary<string, bool> BodiesToDrop;

        Dictionary<string, int> maxBodyValues;
        Dictionary<string, float> bodyWeights;

        Dictionary<string, string> pathToName = new Dictionary<string, string>()
        {
            { root+"HoarderItem.asset", "Hoarding bug" },
            { root+"SpiderItem.asset", "Bunker Spider" },
            { root+"ThumperItem.asset", "Crawler" },
            { root+"CentipedeItem.asset", "Centipede"},
            { root+"NutcrackerItem.asset", "Nutcracker"},
            { root+"BrackenBodyItem.asset", "Flowerman"},
            { root+"BaboonItem.asset", "Baboon hawk"},
            { root+"MouthDogItem.asset", "MouthDog"},
        };

        public Dictionary<string, Item> BodySpawns = new Dictionary<string, Item>();
        public List<GameObject> tools = new List<GameObject>();

        AssetBundle bundle;
        public static Plugin instance;

        public static PluginConfig cfg { get; private set; }

        void Awake()
        {
            cfg = new PluginConfig(base.Config);
            cfg.InitBindings();


            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }

            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "sellbodies");
            bundle = AssetBundle.LoadFromFile(assetDir);

            instance = this;

            ApplyConfig();
            SetupScrap();

            harmony.PatchAll();
            Logger.LogInfo($"Sell Bodies is patched!");
        }

        void ApplyConfig()
        {
            bodyWeights = new Dictionary<string, float>()
            {
                { root+"HoarderItem.asset", cfg.HOARDER_WEIGHT },
                { root+"SpiderItem.asset", cfg.SPIDER_WEIGHT },
                { root+"ThumperItem.asset", cfg.THUMPER_WEIGHT },
                { root+"NutcrackerItem.asset", cfg.NUTCRACKER_WEIGHT },
                { root+"CentipedeItem.asset", cfg.CENTIPEDE_WEIGHT },
                { root+"BrackenBodyItem.asset", cfg.BRACKEN_WEIGHT},
                { root+"BaboonItem.asset", cfg.BABOON_WEIGHT},
                { root+"MouthDogItem.asset", cfg.MOUTHDOG_WEIGHT},
            };

            maxBodyValues = new Dictionary<string, int>()
            {
                { root+"HoarderItem.asset", cfg.HOARDER_MAX },
                { root+"SpiderItem.asset", cfg.SPIDER_MAX },
                { root+"ThumperItem.asset", cfg.THUMPER_MAX },
                { root+"NutcrackerItem.asset", cfg.NUTCRACKER_MAX },
                { root+"CentipedeItem.asset", cfg.CENTIPEDE_MAX },
                { root+"BrackenBodyItem.asset", cfg.BRACKEN_MAX},
                { root+"BaboonItem.asset", cfg.BABOON_MAX},
                { root+"MouthDogItem.asset", cfg.MOUTHDOG_MAX},
            };

            minBodyValues = new Dictionary<string, int>()
            {
                { root+"HoarderItem.asset", cfg.HOARDER_MIN },
                { root+"SpiderItem.asset", cfg.SPIDER_MIN },
                { root+"ThumperItem.asset", cfg.THUMPER_MIN },
                { root+"NutcrackerItem.asset", cfg.NUTCRACKER_MIN },
                { root+"CentipedeItem.asset", cfg.CENTIPEDE_MIN },
                { root+"BrackenBodyItem.asset", cfg.BRACKEN_MIN},
                { root+"BaboonItem.asset", cfg.BABOON_MIN},
                { root+"MouthDogItem.asset", cfg.MOUTHDOG_MIN},
            };
            BodiesToDrop = new Dictionary<string, bool>()
            {
                { root+"HoarderItem.asset", cfg.HOARDER },
                { root+"SpiderItem.asset", cfg.SPIDER },
                { root+"ThumperItem.asset", cfg.THUMPER },
                { root+"NutcrackerItem.asset", cfg.NUTCRACKER },
                { root+"CentipedeItem.asset", cfg.CENTIPEDE },
                { root+"BrackenBodyItem.asset", cfg.BRACKEN},
                { root+"BaboonItem.asset", cfg.BABOON},
                { root+"MouthDogItem.asset", cfg.MOUTHDOG},
            };
        }

        void SetupScrap()
        {
            foreach (KeyValuePair<string, string> pair in pathToName)
            {
                Item body = bundle.LoadAsset<Item>(pair.Key);
                Utilities.FixMixerGroups(body.spawnPrefab);
                body.twoHanded = true;
                body.spawnPrefab.AddComponent<BodySyncer>();
                body.maxValue = maxBodyValues[pair.Key];
                body.minValue = minBodyValues[pair.Key];
                body.weight = bodyWeights[pair.Key];
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(body.spawnPrefab);
                Items.RegisterItem(body);

                if (BodiesToDrop[pair.Key])
                {
                    Logger.LogInfo($"Set {pair.Value} to drop {body.itemName}");
                    BodySpawns.Add(pair.Value, body);
                }
                else
                {
                    Logger.LogInfo($"Disregarding {body.itemName} - disabled in config");
                }
            }
        }
    }
}
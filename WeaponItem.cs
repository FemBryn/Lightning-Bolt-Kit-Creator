using System.Collections.Generic;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Lightning_Bolt_Kit_Creator.Models
{
    public class WeaponItem
    {

        public string Name { get; set; } = string.Empty;
        public Bitmap? Icon { get; set; }
        public Bitmap? IconTop { get; set; }
        public string Class { get; set; } = string.Empty;
        public string CodeName { get; set; } = string.Empty;
        public int Id { get; set; }
        public static Boolean MakeChargerScopeAndUnscope = false;
        //Static method to create all your weapons easily
        public static List<WeaponItem> GetAllMainClasses()
        {
            return new List<WeaponItem>
            {
                new WeaponItem { Name = "Shooter", Icon = LoadBitmap("Icon/Weapon/ClassIcons/Shooter.jpg"), Class = "Main", CodeName = "Shooter" },
                new WeaponItem { Name = "Blaster", Icon = LoadBitmap("Icon/Weapon/ClassIcons/Blaster.jpg"), Class = "Main", CodeName = "Blaster" },
                new WeaponItem { Name = "Roller", Icon = LoadBitmap("Icon/Weapon/ClassIcons/Roller.jpg"), Class = "Main", CodeName = "Roller" },
                new WeaponItem { Name = "Charger", Icon = LoadBitmap("Icon/Weapon/ClassIcons/Charger.jpg"), Class = "Main", CodeName = "Charger" },
                new WeaponItem { Name = "Slosher", Icon = LoadBitmap("Icon/Weapon/ClassIcons/Slosher.jpg"), Class = "Main", CodeName = "Slosher" },
                new WeaponItem { Name = "Splatling", Icon = LoadBitmap("Icon/Weapon/ClassIcons/Splatling.jpg"), Class = "Main", CodeName = "Spinner" },
                new WeaponItem { Name = "Dualies", Icon = LoadBitmap("Icon/Weapon/ClassIcons/Dualies.jpg"), Class = "Main", CodeName = "Maneuver" },
                new WeaponItem { Name = "Brella", Icon = LoadBitmap("Icon/Weapon/ClassIcons/Brella.jpg"), Class = "Main", CodeName = "Shelter" },
                new WeaponItem { Name = "Brush", Icon = LoadBitmap("Icon/Weapon/ClassIcons/Brush.jpg"), Class = "Main", CodeName = "Brush" },
                new WeaponItem { Name = "Stringer", Icon = LoadBitmap("Icon/Weapon/ClassIcons/Stringer.jpg"), Class = "Main", CodeName = "Stringer" },
                new WeaponItem { Name = "Splatana", Icon = LoadBitmap("Icon/Weapon/ClassIcons/Splatana.jpg"), Class = "Main", CodeName = "Saber" }
            };
        }
        public static List<WeaponItem> GetAllSubs()
        {
            return new List<WeaponItem>
            {
                new WeaponItem { Name = "Squid Beakon",
                Icon = LoadBitmap("Icon/Sub/Wsb_Beacon00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_Beacon01.png"),
                Class = "Main", CodeName = "Beacon", Id=8 },

                new WeaponItem { Name = "Curling Bomb",
                Icon = LoadBitmap("Icon/Sub/Wsb_Bomb_Curling00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_Bomb_Curling01.png"),
                Class = "Sub", CodeName = "Bomb_Curling" , Id=6 },

                new WeaponItem { Name = "Fizzy Bomb",
                Icon = LoadBitmap("Icon/Sub/Wsb_Bomb_Fizzy00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_Bomb_Fizzy01.png"),
                Class = "Sub", CodeName = "Bomb_Fizzy" , Id=5 },

                new WeaponItem { Name = "Burst Bomb",
                Icon = LoadBitmap("Icon/Sub/Wsb_Bomb_Quick00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_Bomb_Quick01.png"),
                Class = "Sub", CodeName = "Bomb_Quick" , Id=2 },

                new WeaponItem { Name = "Autobomb",
                Icon = LoadBitmap("Icon/Sub/Wsb_Bomb_Robot00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_Bomb_Robot01.png"),
                Class = "Sub", CodeName = "Bomb_Robot" , Id=7 },

                new WeaponItem { Name = "Splat Bomb",
                Icon = LoadBitmap("Icon/Sub/Wsb_Bomb_Splash00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_Bomb_Splash01.png"),
                Class = "Sub", CodeName = "Bomb_Splash" , Id=0 },

                new WeaponItem { Name = "Suction Bomb",
                Icon = LoadBitmap("Icon/Sub/Wsb_Bomb_Suction00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_Bomb_Suction01.png"),
                Class = "Sub", CodeName = "Bomb_Suction" , Id=1 },

                new WeaponItem { Name = "Torpedo",
                Icon = LoadBitmap("Icon/Sub/Wsb_Bomb_Torpedo00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_Bomb_Torpedo01.png"),
                Class = "Sub", CodeName = "Bomb_Torpedo" , Id=13 },

                new WeaponItem { Name = "Line Marker",
                Icon = LoadBitmap("Icon/Sub/Wsb_LineMarker00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_LineMarker01.png"),
                Class = "Sub", CodeName = "LineMarker" , Id=12 },

                new WeaponItem { Name = "Point Sensor",
                Icon = LoadBitmap("Icon/Sub/Wsb_PointSensor00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_PointSensor01.png"),
                Class = "Sub", CodeName = "PointSensor" , Id=9 },

                new WeaponItem { Name = "Toxic Mist",
                Icon = LoadBitmap("Icon/Sub/Wsb_PoisonMist00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_PoisonMist01.png"),
                Class = "Sub", CodeName = "PoisonMist" , Id=11 },

                new WeaponItem { Name = "Smallfry",
                Icon = LoadBitmap("Icon/Sub/Wsb_SalmonBuddy00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_SalmonBuddy01.png"),
                Class = "Sub", CodeName = "SalmonBuddy" , Id=10100 },

                new WeaponItem { Name = "Splash Wall",
                Icon = LoadBitmap("Icon/Sub/Wsb_Shield00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_Shield01.png"),
                Class = "Sub", CodeName = "Shield" , Id= 4},

                new WeaponItem { Name = "Sprinkler",
                Icon = LoadBitmap("Icon/Sub/Wsb_Sprinkler00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_Sprinkler01.png"),
                Class = "Sub", CodeName = "Sprinkler" , Id= 3},

                new WeaponItem { Name = "Ink Mind",
                Icon = LoadBitmap("Icon/Sub/Wsb_Trap00.png"),
                IconTop = LoadBitmap("Icon/Sub/Wsb_Trap01.png"),
                Class = "Sub", CodeName = "Trap" , Id=10 }
            };
        }


        public static List<WeaponItem> GetAllSpecials()
        {
            return new List<WeaponItem>
            {
                CreateSpecial("Ink Vac","SpBlower",8),
                CreateSpecial("Kraken Royale","SpCastle",17),
                CreateSpecial("Crab Tank","SpChariot",12),
                CreateSpecial("Splattercolor Screen","SpChimney",19),
                CreateSpecial("Tacticooler","SpEnergyStand",15),
                CreateSpecial("Super Chump","SpFirework",16),
                CreateSpecial("Big Bubbler","SpGreatBarrier",2),
                CreateSpecial("Ink Storm","SpInkStorm",5),
                CreateSpecial("Inkjet","SpJetpack",10),
                CreateSpecial("Killer Wail 5.1","SpMicroLaser",9),
                CreateSpecial("Tenta Missiles","SpMultiMissile",4),
                CreateSpecial("Booyah Bomb","SpNiceBall",6),
                CreateSpecial("Triple Splashdown","SpPogo",18),
                CreateSpecial("Wave Breaker","SpShockSonar",7),
                CreateSpecial("Reefslider","SpSkewer",13),
                CreateSpecial("Zipcaster","SpSuperHook",3),
                CreateSpecial("Triple Inkstrike","SpTripleTornado",14),
                CreateSpecial("Trizooka","SpUltraShot",1),
                CreateSpecial("Ultra Stamp","SpUltraStamp",11),
                CreateSpecial("Splashdown","SpSuperLanding",101),
                CreateSpecial("Rainmaker (????)","SpGachihoko",20)
            };
        }

        private static Bitmap? LoadBitmap(string path)
        {
            try
            {
                var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                var uri = new Uri($"avares://{assemblyName}/{path}");
                using var stream = AssetLoader.Open(uri);
                return new Bitmap(stream);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return null;
            }
        }
        // Name, Codename, Class, Id
        public static List<WeaponItem> GetMainWeaponsOfClass(String WeaponClass)
        {
            List<WeaponItem> ret = WeaponClass switch
            {
                "Shooter" => new List<WeaponItem>
            {
                CreateMain("Aerospray","Blaze","Shooter", 30),
                CreateMain("Splattershot Pro","Expert","Shooter", 70),
                CreateMain("Splattershot Jr.","First","Shooter", 10),
                CreateMain("Squeezer","Flash","Shooter", 400),
                CreateMain(".52 Gal","Gravity","Shooter", 50),
                CreateMain(".96 Gal","Heavy","Shooter", 80),
                CreateMain("Jet Squelcher","Long","Shooter", 90),
                CreateMain("Splattershot","Normal","Shooter", 40),
                CreateMain("Splash-o-matic","Precision","Shooter", 20),
                CreateMain("Splattershot Nova","QuickLong","Shooter", 100),
                CreateMain("N-Zap","QuickMiddle","Shooter", 60),
                CreateMain("Sploosh-o-matic","Short","Shooter", 0),
                CreateMain("H-3 Nozzlenose","TripleMiddle","Shooter", 310),
                CreateMain("L-3 Nozzlenose","TripleQuick","Shooter", 300)
            },
                "Blaster" => new List<WeaponItem>
            {
                CreateMain("Rapid Blaster Pro","LightLong","Blaster", 250),
                CreateMain("Clash Blaster","LightShort","Blaster", 230),
                CreateMain("Rapid Blaster","Light","Blaster", 240),
                CreateMain("Ranger Blaster","Long","Blaster", 220),
                CreateMain("Blaster","Middle","Blaster", 210),
                CreateMain("S-Blast","Precision","Blaster", 260),
                CreateMain("Luna Blaster","Short","Blaster", 200)
            },
                "Roller" => new List<WeaponItem>
            {
                CreateMain("Carbon Roller","Compact","Roller", 1000),
                CreateMain("Dynamo Roller","Heavy","Roller", 1020),
                CreateMain("Flingza Roller","Hunter","Roller", 1030),
                CreateMain("Splat Roller","Normal","Roller", 1010),
                CreateMain("Big Swig Roller","Wide","Roller", 1040),
            },
                "Charger" => new List<WeaponItem>
            {
                CreateMain("SnipeWriter","Pencil","Charger", 2070),
                CreateMain("Goo Tuber","Keeper","Charger", 2060),
                CreateMain("Bamboozler 14","Light","Charger", 2050),
                CreateMain("Squiffer","Quick","Charger", 2000)
            },
                "Slosher" => new List<WeaponItem>
            {
                CreateMain("Bloblobber","Bathtub","Slosher", 3030),
                CreateMain("Tri-Slosher","Diffusion","Slosher", 3010),
                CreateMain("Dread Wringer","Double","Slosher", 3050),
                CreateMain("Sloshing Machine","Launcher","Slosher", 3020),
                CreateMain("Slosher","Strong","Slosher", 3000),
                CreateMain("Explosher","Washtub","Slosher", 3040),
            },
                "Spinner" => new List<WeaponItem>
            {
                CreateMain("Ballpoint Splatling","Downpour","Spinner", 4030),
                CreateMain("Hydra Splatling","Hyper","Spinner", 4020),
                CreateMain("Heavy Edit Splatling","HyperShort","Spinner", 4050),
                CreateMain("Mini Splatling","Quick","Spinner", 4000),
                CreateMain("Heavy Splatling","Standard","Spinner", 4010),
                CreateMain("Nautilus","Serein","Spinner", 4040)
            },
                "Maneuver" => new List<WeaponItem>
            {
                CreateMain("Dualie Squelchers","Dual","Maneuver", 5030),
                CreateMain("Glooga Dualies","Gallon","Maneuver", 5020),
                CreateMain("Douser Dualies","Long","Maneuver", 5050),
                CreateMain("Splat Dualies","Normal","Maneuver", 5010),
                CreateMain("Dapple Dualies","Short","Maneuver", 5000),
                CreateMain("Tetra Dualies","Stepper","Maneuver", 5040)
            },
                "Shelter" => new List<WeaponItem>
            {
                CreateMain("Undercover Brella","Compact","Shelter", 6020),
                CreateMain("Recycled Brella 24","Focus","Shelter", 6030),
                CreateMain("Splat Brella","Normal","Shelter", 6000),
                CreateMain("Tenta Brella","Wide","Shelter", 6010)
            },
                "Brush" => new List<WeaponItem>
            {
                CreateMain("Painbrush","Heavy","Brush", 1120),
                CreateMain("Inkbrush","Mini","Brush", 1100),
                CreateMain("Octobrush","Normal","Brush", 1110)
            },
                "Stringer" => new List<WeaponItem>
            {
                CreateMain("Explosion","Explosion","Stringer", 7030),
                CreateMain("Tri-Stringer","Normal","Stringer", 7010),
                CreateMain("REEF-LUX 450","Short","Stringer", 7020)
            },
                "Saber" => new List<WeaponItem>
            {
                CreateMain("Decavitator","Heavy","Saber", 8020),
                CreateMain("Splatana Wiper","Lite","Saber", 8010),
                CreateMain("Splatana Stamper","Normal","Saber", 8000)
            },
                _ => new List<WeaponItem> { }
            };
            if (WeaponClass.Equals("Charger"))
            {
                if (MakeChargerScopeAndUnscope)
                {
                    ret.Add(CreateMain("E-Liter 4k", "Long", "Charger", 2030));
                    ret.Add(CreateMain("Splat Charger", "Normal", "Charger", 2010));
                }
                else
                {
                    ret.Add(CreateMain("E-Liter 4k Scope", "LongScope", "Charger", 2040));
                    ret.Add(CreateMain("E-Liter 4k", "Long", "Charger", 2030));
                    ret.Add(CreateMain("SplatterScope", "NormalScope", "Charger", 2020));
                    ret.Add(CreateMain("Splat Charger", "Normal", "Charger", 2010));
                }
            }
            return ret;

        }
        public static WeaponItem CreateMain(String Name, String CodeName, String Class, int Id)
        {
            return new WeaponItem { Name = Name, Icon = LoadBitmap($"Icon/Weapon/{Class}/Path_Wst_{Class}_{CodeName}_00.jpg"), Class = Class, CodeName = CodeName, Id = Id };
        }
        public static WeaponItem CreateSpecial(String Name, String CodeName, int Id)
        {
            return new WeaponItem {
                Name = Name,
                Icon = LoadBitmap($"Icon/Special/Wsp_{CodeName}00.png"),
                IconTop = LoadBitmap($"Icon/Special/Wsp_{CodeName}01.png"),
                Class = "Special",
                CodeName = CodeName,
                Id = Id };
        }
    }
}
using BymlLibrary;
using ZstdSharp;
using SarcLibrary;
namespace Lightning_Bolt_Kit_Creator;

using Syroot.NintenTools.NSW.Bfres;

using Revrs;
using Syroot.NintenTools.NSW.Bntx;

public class FileProcessing
{
    //Paths
    public String RomfsPath, WeaponFolder, WeaponFolderRoot;
    //WeaponInfoMain and other similar Details
    public String WeaponName, WeaponSuffix, OriginalCodename, Sub, Special, SpecialPoints, ID;
    public bool isShelter = false;

    //Variables we start with
    public FileProcessing() { }

    public void Empty(String path)
    {
        if (Directory.Exists(path)) { Directory.Delete(path, true); }
        Directory.CreateDirectory(path);
    }
    public void onClose()
    {
        try
        {
            Directory.Delete(WeaponFolderRoot + "\\__tempTT", true);
            
        }
        catch (Exception ex)
        {
            Debug.Write(ex.ToString());
        }
        
    }
    public void WriteWeaponInfo()
    {
        try
        {
            WeaponFolder = WeaponFolderRoot + "\\" + WeaponName;
            String TextContents = "[NAME]\n" + WeaponName + "\n";
            TextContents += "[Suffix]\n" + WeaponSuffix + "\n";
            TextContents += "[ORIGINAL]\n" + OriginalCodename + "\n";
            TextContents += "[SUB]\n" + Sub + "\n";
            TextContents += "[Special]\n" + Special + "\n";
            TextContents += "[POINTS]\n" + SpecialPoints + "\n";
            TextContents += "[ID]\n" + ID + "\n";

            Empty(WeaponFolderRoot + "\\__tempTT");
            Empty(WeaponFolder);

            File.WriteAllText(WeaponFolder + "\\info.txt", TextContents);
            CreateNewActor("S");
            CreateNewActor("G");

            Debug.Write("All Done");


        }
        catch (Exception ex)
        {
            Debug.Write(ex.ToString());
        }
        EditVisualFiles();

    }
    public void EditVisualFiles()
    {
        try
        {
            File.Copy(RomfsPath + "UI\\Icon\\Wpn\\Wst_" + OriginalCodename + "_00.bntx.zs", WeaponFolder + "\\Wst_" + WeaponName + ".bntx.zs");
            File.Copy(RomfsPath + "UI\\Icon\\WpnPath\\Path_Wst_" + OriginalCodename + "_00.bntx.zs", WeaponFolder + "\\Path_Wst_" + WeaponName + ".bntx.zs");
            File.Copy(RomfsPath + "Model\\Wmn_" + (OriginalCodename switch
            {
                "Shooter_Precision" => "Shooter_Short",
                "Spinner_Quick" => "Spinner_QuickT",
                "Shooter_TripleQuick" => "Shooter_Triple",
                "Charger_NormalScope" => "Charger_NormalT",
                "Charger_Normal" => "Charger_NormalT",
                "Charger_LongScope" => "Charger_Long",
                "Saber_Lite" => "Saber_Light",
                "Shooter_Normal" => "Shooter_NormalT",
                "Blaster_LightLong" => "Blaster_Light",
                "Maneuver_Normal" => "Maneuver_NormalT",
                "Roller_Normal" => "Roller_NormalT",
                "Slosher_Strong" => "Slosher_StrongT",
                "Spinner_Standard" => "Spinner_StandardT",
                _ => OriginalCodename
            }) + ".bfres.zs", WeaponFolder + "\\Wmn_" + OriginalCodename + "_" + WeaponSuffix + ".bfres.zs");
            

        }
        catch (Exception ex)
        {
            Debug.Write(ex.ToString());
        }
        //Wpn Icon Name Editing
        BntxFile wpn = new BntxFile(new MemoryStream(Zstd.Decompress(File.ReadAllBytes(WeaponFolder + "\\Wst_" + WeaponName + ".bntx.zs"), File.ReadAllBytes(WeaponFolder + "\\Wst_" + WeaponName + ".bntx.zs").Length *6)));
        wpn.Name = "Wst_" + WeaponName;
        wpn.Textures[0].Name = "Wst_" + WeaponName;
        MemoryStream writeStream = new MemoryStream();
        wpn.Save(writeStream);
        File.WriteAllBytes(WeaponFolder + "\\Wst_" + WeaponName + ".bntx.zs", Zstd.Compress(writeStream.ToArray(), 3));

        //WpnPath Icon Name Editing
        BntxFile wpnPath = new BntxFile(new MemoryStream(Zstd.Decompress( File.ReadAllBytes(WeaponFolder + "\\Path_Wst_" + WeaponName + ".bntx.zs"), File.ReadAllBytes(WeaponFolder + "\\Path_Wst_" + WeaponName + ".bntx.zs").Length *6)));
        wpnPath.Name = "Path_Wst_" + WeaponName;
        wpnPath.Textures[0].Name = "Path_Wst_" + WeaponName;
        writeStream = new MemoryStream();
        wpnPath.Save(writeStream);
        File.WriteAllBytes(WeaponFolder + "\\Path_Wst_" + WeaponName + ".bntx.zs", Zstd.Compress(writeStream.ToArray(), 3));
        editModelInfo();
    }

    public async void editModelInfo()
    {
        await Task.Delay(100);
        
        String ModelName = "Wmn_" + OriginalCodename + "_" + WeaponSuffix;
        ResFile Model = new ResFile(new MemoryStream(Zstd.Decompress(File.ReadAllBytes(WeaponFolder + "\\Wmn_" + OriginalCodename + "_" + WeaponSuffix + ".bfres.zs"), File.ReadAllBytes(WeaponFolder + "\\Wmn_" + OriginalCodename + "_" + WeaponSuffix + ".bfres.zs").Length * 4)));
        Model.Name = ModelName;
        Model.Models[0].Name = ModelName;
        MemoryStream writeStream = new MemoryStream();
        Model.Save(writeStream);
        File.WriteAllBytes( WeaponFolder + "\\Wmn_" + OriginalCodename + "_" + WeaponSuffix + ".bfres.zs", Zstd.Compress(writeStream.ToArray(),3));
        Debug.Write("Model Name: " + ModelName);
        Debug.Write("Model Info: " + Model.Models[0].Name);
        
        Debug.Write("Model Info Edited!");
    }

    public async void CreateNewActor(String prefix)
    {
        isShelter = false;
        String asset;
        String tempPath = WeaponFolderRoot + "__tempTT\\" + prefix + "\\";
        //check if its a fucking brella
        if (OriginalCodename.Contains("Shelter"))
        {
            isShelter = true;
            //Use second Kit actor path
            asset = RomfsPath + "Pack\\Actor\\Wmn" + prefix + "_" + OriginalCodename + "_01.pack.zs";
        }
        else
        {
            //original actor path
            asset = RomfsPath + "Pack\\Actor\\Wmn" + prefix + "_" + OriginalCodename + "_00.pack.zs";
        }
        //Decompress
        Debug.Write("Asset " + asset);
        Byte[] assetDecompressed = Zstd.Decompress(File.ReadAllBytes(asset), File.ReadAllBytes(asset).Length * 4);
        //Extract Sarc
        await SarcFile.FromBinary(assetDecompressed).ExtractToDirectory(tempPath);
        Debug.Write(tempPath);
        //loop all files
        Directory.CreateDirectory(tempPath + "\\");
        String Text = "";
        foreach (String i in Directory.GetFiles(tempPath, "*.*", SearchOption.AllDirectories))
        {
            //check for brella exclusive wmng 
            Text += i + "\n";
            if (isShelter && prefix.Equals("G"))
            {
                //if ActorReservation
                if (i.Contains("ActorReservation\\WeaponShelter") && i.Contains("_Cstm"))
                {
                    Debug.Write("Found Actor Reservation data :3");
                    BymlEditing.ShelterActorReservation(i);

                    var resdataInfoData = Byml.FromBinary(File.ReadAllBytes(i));

                    // Navigate to RequestList[3]["Actor"] and modify it
                    resdataInfoData.GetMap()["RequestList"].GetArray()[3].GetMap()["Actor"] = Byml.FromText(resdataInfoData.GetMap()["RequestList"].GetArray()[3].GetMap()["Actor"].GetString().Replace("_Cstm", "_" + WeaponSuffix));
                    // Write the modified data back to new file

                    File.WriteAllBytes(i.Replace("_Cstm", "_" + WeaponSuffix), resdataInfoData.ToBinary(Revrs.Endianness.Little));

                    File.Delete(i);
                }
                else if (i.Contains("Actor\\WeaponShelter") && i.Contains("_Cstm"))
                {
                    Debug.Write("Found Shelter actor data :3");
                    BymlEditing.bymlPrint(i);

                    var ShelterInfoData = Byml.FromBinary(File.ReadAllBytes(i));
                    Byml Components = ShelterInfoData.GetMap()["Components"];
                    // Navigate to RequestList[3]["Actor"] and modify it
                    Components.GetMap()["ModelInfoRef"] = Byml.FromText(Components.GetMap()["ModelInfoRef"].GetString().ToString().Replace("_Cstm", "_" + WeaponSuffix));
                    Components.GetMap()["ActorReservation"] = Byml.FromText(Components.GetMap()["ActorReservation"].GetString().Replace("_Cstm", "_" + WeaponSuffix));
                    ShelterInfoData.GetMap()["Components"] = Components;
                    // Write the modified data back to new file
                    var modifiedData = ShelterInfoData.ToBinary(Endianness.Little);
                    File.WriteAllBytes(i.Replace("_Cstm", "_" + WeaponSuffix), modifiedData.ToArray());

                    File.Delete(i);
                }
            }

            // check if file is actor data
            if (i.Contains("Actor\\Wmn"))
            {
                Debug.Write("Found actor data!");
                Byml actordataInfoData = Byml.FromBinary(File.ReadAllBytes(i));
                //If brella
                if (isShelter)
                {
                    if (prefix.Equals("G"))
                    {
                        actordataInfoData.GetMap()["$parent"] = Byml.FromText(actordataInfoData.GetMap()["$parent"].GetString().Replace("Cstm", WeaponSuffix));

                    }
                    actordataInfoData.GetMap()["Components"].GetMap()["ModelInfoRef"] = Byml.FromText(actordataInfoData.GetMap()["Components"].GetMap()["ModelInfoRef"].GetString().Replace(OriginalCodename + "_01", OriginalCodename + "_" + WeaponSuffix));
                }
                else
                {
                    Debug.Write("Actor Data:");
                    BymlEditing.recursiveBymlContentPrint(actordataInfoData, " - ");
                    try
                    {
                        actordataInfoData.GetMap()["Components"].GetMap()["ModelInfoRef"] = Byml.FromText(actordataInfoData.GetMap()["Components"].GetMap()["ModelInfoRef"].GetString().Replace(OriginalCodename + "_00", OriginalCodename + "_" + WeaponSuffix));
                    }
                    catch (Exception ex) { }

                    try
                    {
                        actordataInfoData.GetMap()["Components"].GetMap()["MirrorModel"] = Byml.FromText(actordataInfoData.GetMap()["Components"].GetMap()["MirrorModel"].GetString().Replace(OriginalCodename + "_00", OriginalCodename + "_" + WeaponSuffix));
                    }
                    catch (Exception ex) { }
                }

                if (isShelter)
                {
                    File.WriteAllBytes(i.Replace("_01", "_" + WeaponSuffix), actordataInfoData.ToBinary(Endianness.Little));
                }
                else
                {
                    File.WriteAllBytes(i.Replace("_00", "_" + WeaponSuffix), actordataInfoData.ToBinary(Endianness.Little));
                }

                File.Delete(i);

            }

            // Model data 
            if (i.Contains("ModelInfo\\Wmn_") || i.Contains("MirrorModel\\Wmn"))
            {
                Debug.Write("Found model info :3");

                Byml modelinfoData = Byml.FromBinary(File.ReadAllBytes(i));
                //Remove T after name if it exists, and replace name with name + suffix
                if (i.Contains("ModelInfo\\Wmn_"))
                {
                    modelinfoData.GetMap()["Fmdb"] = Byml.FromText(modelinfoData.GetMap()["Fmdb"].GetString().Replace(OriginalCodename + "T", OriginalCodename).Replace(OriginalCodename, OriginalCodename + "_" + WeaponSuffix));
                    modelinfoData.GetMap()["Fmdb"] = Byml.FromText(modelinfoData.GetMap()["Fmdb"].GetString().Replace((OriginalCodename switch
                    {
                        "Shooter_Precision" => "Shooter_Short",
                        "Shooter_TripleQuick" => "Shooter_Triple/",
                        "Charger_NormalScope" => "Charger_Normal/",
                        "Charger_LongScope" => "Charger_Long/",
                        "Saber_Lite" => "Saber_Light",
                        "Blaster_LightLong" => "Blaster_Light",
                        _ => "@=*^"
                    }), OriginalCodename + "_" + WeaponSuffix + (OriginalCodename.Contains("Scope") || OriginalCodename.Equals("Shooter_TripleQuick") ? "/" : "")));
                    if (isShelter) { modelinfoData.GetMap()["Fmdb"] = Byml.FromText(modelinfoData.GetMap()["Fmdb"].GetString().Replace("_Cstm01", "")); }
                }
                else if (i.Contains("MirrorModel\\Wmn"))
                {
                    modelinfoData.GetMap()["RightFmdb"] = Byml.FromText(modelinfoData.GetMap()["RightFmdb"].GetString().Replace(OriginalCodename + "T", OriginalCodename).Replace(OriginalCodename, OriginalCodename + "_" + WeaponSuffix));
                    if (OriginalCodename.Equals("Maneuver_Dual"))
                    {
                        modelinfoData.GetMap()["Fmdb"] = Byml.FromText(modelinfoData.GetMap()["Fmdb"].GetString().Replace(OriginalCodename, OriginalCodename + "_" + WeaponSuffix));
                    }

                }
                if (isShelter)
                {
                    File.WriteAllBytes(i.Replace("_01", "_" + WeaponSuffix), modelinfoData.ToBinary(Endianness.Little));
                }
                else
                {
                    File.WriteAllBytes(i.Replace("_00", "_" + WeaponSuffix), modelinfoData.ToBinary(Endianness.Little));
                }

                File.Delete(i);
            }
        }
        //File.WriteAllText(".\\Debug.txt", Text);
        //Exit the Loop
        SarcFile NewSarc = SarcFile.LoadFromDirectory(tempPath, searchPattern: "*.*", searchOption: SearchOption.AllDirectories);
        File.WriteAllBytes(WeaponFolder + "\\" + "Wmn" + prefix + "_" + OriginalCodename + "_" + WeaponSuffix + ".pack.zs", Zstd.Compress(NewSarc.ToBinary()));
        Debug.Write("New Actor Made!!!! :3");
        if (isShelter && prefix.Equals("G"))
        {
            Debug.Write("Brella canopy stuff '`'");
            tempPath = WeaponFolderRoot + "\\__tempTT\\filecanopy";
            Directory.CreateDirectory(tempPath);
            assetDecompressed = ZstdSharp.Zstd.Decompress(File.ReadAllBytes(asset), File.ReadAllBytes(asset).Length * 4);
            //Extract Sarc
            await SarcFile.FromBinary(assetDecompressed).ExtractToDirectory(tempPath);
            //oh no... 
            foreach (String i in Directory.GetFiles(tempPath, ".", SearchOption.AllDirectories))
            {
                if (i.Contains("Actor/") && i.Contains("_Cstm"))
                {
                    Byml actordataInfoData = Byml.FromBinary(File.ReadAllBytes(i));
                    actordataInfoData.GetMap()["Components"].GetMap()["ModelInfoRef"] = Byml.FromText(actordataInfoData.GetMap()["Components"].GetMap()["ModelInfoRef"].GetString().Replace("_Cstm", "_" + WeaponSuffix));
                    File.WriteAllBytes(i.Replace("_Cstm", "_" + WeaponSuffix), actordataInfoData.ToBinary(Endianness.Little));
                    File.Delete(i);
                }
                if (i.Contains("ModelInfo"))
                {
                    Debug.Write("Found model info :3");

                    Byml modelinfoData = Byml.FromBinary(File.ReadAllBytes(i));
                    //Remove T after name if it exists, and replace name with name + suffix
                    if (i.Contains("ModelInfo\\Wmn_"))
                    {
                        Debug.Write("Canopy Info Found");
                        modelinfoData.GetMap()["Fmdb"] = Byml.FromText(modelinfoData.GetMap()["Fmdb"].GetString().Replace("_Cstm01", "_" + WeaponSuffix));
                        File.WriteAllBytes(i.Replace("_Cstm", "_" + WeaponSuffix), modelinfoData.ToBinary(Endianness.Little));
                        File.Delete(i);
                    }
                }
            }
            //Exit loop
            NewSarc = SarcFile.LoadFromDirectory(tempPath, searchPattern: "*.*", searchOption: SearchOption.AllDirectories);

            File.WriteAllBytes(WeaponFolder + "\\BulletShelterCanopy" + (OriginalCodename.Equals("Shelter_Normal") ? "Base" : OriginalCodename.Replace("Shelter_", "")) + "_" + WeaponSuffix + ".pack.zs", Zstd.Compress(NewSarc.ToBinary()));
            Debug.Write("New Canopy Actor Made!!!! :3");
        }
    }
}
import os
import stat
import pathlib
import shutil

try:
    import sarclib
    import byml
    import zstandard
except ImportError:
    print("It looks like one of your packages failed to be imported!\nTry downloading the requirements by doing 'pip install requirements.txt' in the command line!")

assetName = ""
assetWeapon = ""
assetSuffix = ""
assetInternalID = ""
wpnPath_ORIGINAL = ""
wpnPath = ""
gameVersion = ""
assetSub = ""
assetSpecial= ""
assetSpPoint = ""


## FUNCTIONS
  
# create WmnG or WmnS actor, depending on prefix  
def createNewActor(prefix):
    isShelter = False
    # make temp filepath
    filePath = "__tempTT\\file" + prefix + "\\"

    # check if its a fucking brella
    if "Shelter" in assetWeapon:
        isShelter = True
        asset = open("filepath.txt", "r").read() + "\\Pack\\Actor\\Wmn" + prefix + "_" + assetWeapon + "_01" + ".pack.zs" # create and rename copy of second kits' actor (brella)
    else:        
        
        asset = open("filepath.txt", "r").read() + "\\Pack\\Actor\\Wmn" + prefix + "_" + assetWeapon + "_00" + ".pack.zs" # create and rename copy of original weapon's actor
    
    # decompress new actor
    assetDecompressed = zstandard.decompress(open(asset, "rb").read())
    
    # get extracted list of files within the actor
    fileList = sarclib.extract(assetDecompressed, filePath)

    # trace through fileList to find things that need to be edited
    for i in fileList:
        # print scanned file
        print(i)
        
        
        # check for brella exclusive files in WmnG
        if isShelter and prefix == "G": # if brella WmnG
            
            if "ActorReservation/WeaponShelter" in i and "_Cstm" in i: # if actorreservation
                
                # notify user
                print(" - Found actor reservation data!")
                # open file into resdataInfo
                resdataInfo = open(filePath + i, "rb").read()
                # parse the data into resdataInfoData
                resdataInfoData = byml.Byml(resdataInfo).parse()
            
                # change requestlist data
                resdataInfoData["RequestList"][3]["Actor"] = resdataInfoData["RequestList"][3]["Actor"].replace("_Cstm", "_" + assetSuffix)    
            
                # create resdataInfoWriter to write data back into file
                resdataInfoWriter = byml.Writer(resdataInfoData)
                
                with open(filePath + i, "wb") as w:
                    # write data to shelter file
                    resdataInfoWriter.write(w)
                
                # rename the res file with the appropriate suffix (original is Cstm for brellas)
                os.rename(filePath + i, filePath + i.replace("_Cstm", "_" + assetSuffix))
                # Done to here ???
            elif "Actor/WeaponShelter" in i and "_Cstm" in i: # if WeaponShelter
                # notify user
                print(" - Found shelter actor data!")
                # open file into shelterdataInfo
                shelterdataInfo = open(filePath + i, "rb").read()
                # parse the data into shelterdataInfoData
                shelterdataInfoData = byml.Byml(shelterdataInfo).parse()
            
                # change data
                shelterdataInfoData["Components"]["ModelInfoRef"] = shelterdataInfoData["Components"]["ModelInfoRef"].replace("Cstm", assetSuffix)
                shelterdataInfoData["Components"]["ActorReservation"] = shelterdataInfoData["Components"]["ActorReservation"].replace("Cstm", assetSuffix)      
            
                # create shelterdataInfoWriter to write data back into file
                shelterdataInfoWriter = byml.Writer(shelterdataInfoData)
                
                with open(filePath + i, "wb") as w:
                    # write data to shelter file
                    shelterdataInfoWriter.write(w)
                
                # rename the actor file with the appropriate suffix (original is Cstm for brellas)
                os.rename(filePath + i, filePath + i.replace("_Cstm", "_" + assetSuffix))
                
        
        
        # if if file is actor data
        if "Actor/Wmn" in i:
            # notify user
            print(" - Found actor data!")
            # open file into actordataInfo
            actordataInfo = open(filePath + i, "rb").read()
            # parse the data into actordataInfoData
            actordataInfoData = byml.Byml(actordataInfo).parse()
            
            # check if brella
            if isShelter:
                if prefix == "G":
                    # replace parent (Cstm), change is only present in WmnG for some godforsaken reason
                    actordataInfoData["$parent"] = actordataInfoData["$parent"].replace("Cstm", assetSuffix)
                
                actordataInfoData["Components"]["ModelInfoRef"] = actordataInfoData["Components"]["ModelInfoRef"].replace(assetWeapon + "_01", assetWeapon + "_" + assetSuffix)
            else: # if not brella
                # replace "ModelInfoRef" or "MirrorModel" in parsed data
                if (actordataInfoData.get("Components", {}).get("ModelInfoRef", "NA") != "NA"): actordataInfoData["Components"]["ModelInfoRef"] = actordataInfoData["Components"]["ModelInfoRef"].replace(assetWeapon + "_00", assetWeapon + "_" + assetSuffix) #non-dualies
                if (actordataInfoData.get("Components", {}).get("MirrorModel", "NA") != "NA"): actordataInfoData["Components"]["MirrorModel"] = actordataInfoData["Components"]["MirrorModel"].replace(assetWeapon + "_00", assetWeapon + "_" + assetSuffix) #dualies
                    
            # create actordataInfoWriter to write data back into file
            actordataInfoWriter = byml.Writer(actordataInfoData)
                
            with open(filePath + i, "wb") as w:
                # write data to actor file
                actordataInfoWriter.write(w)
                
            # rename the actor file with the appropriate suffix
            if isShelter:
                os.rename(filePath + i, filePath + i.replace("_01", "_" + assetSuffix))
            else:
                os.rename(filePath + i, filePath + i.replace("_00", "_" + assetSuffix))
            
        # if file is model data
        if ("ModelInfo/Wmn_" in i) or ("MirrorModel/Wmn_" in i):
            # notify user
            print(" - Found model info!")
            # open file into modelInfo
            modelInfo = open(filePath + i, "rb").read()
            # parse the data into modelInfoData
            modelInfoData = byml.Byml(modelInfo).parse()
            

            

            if ("ModelInfo/Wmn_" in i): # non-dualies
                # get rid of T for weapons that have it
                modelInfoData["Fmdb"] = modelInfoData["Fmdb"].replace(assetWeapon + "T", assetWeapon)
                
                # replace "Fmdb" value in parsed data
                modelInfoData["Fmdb"] = modelInfoData["Fmdb"].replace(assetWeapon, assetWeapon + "_" + assetSuffix) #non-dualies
                # replace unique names for models with different names
                if assetWeapon == "Shooter_Precision":
                    modelInfoData["Fmdb"] = modelInfoData["Fmdb"].replace("Shooter_Short", assetWeapon + "_" + assetSuffix)
                if assetWeapon == "Shooter_TripleQuick":
                    modelInfoData["Fmdb"] = modelInfoData["Fmdb"].replace("Shooter_Triple/", assetWeapon + "_" + assetSuffix + "/")
                elif assetWeapon == "Charger_NormalScope":
                    modelInfoData["Fmdb"] = modelInfoData["Fmdb"].replace("Charger_Normal/", assetWeapon + "_" + assetSuffix + "/")
                elif assetWeapon == "Charger_LongScope":
                    modelInfoData["Fmdb"] = modelInfoData["Fmdb"].replace("Charger_Long/", assetWeapon + "_" + assetSuffix + "/")
                elif assetWeapon == "Saber_Lite":
                    modelInfoData["Fmdb"] = modelInfoData["Fmdb"].replace("Saber_Light", assetWeapon + "_" + assetSuffix)
                elif assetWeapon == "Blaster_LightLong":
                    modelInfoData["Fmdb"] = modelInfoData["Fmdb"].replace("Blaster_Light", assetWeapon + "_" + assetSuffix)
                elif isShelter:
                    modelInfoData["Fmdb"] = modelInfoData["Fmdb"].replace("_Cstm01", "") # brella just needs Cstm01 removed because it uses second kit
            elif ("MirrorModel/Wmn_" in i): # dualies
                modelInfoData["RightFmdb"] = modelInfoData["RightFmdb"].replace(assetWeapon + "T", assetWeapon)
                modelInfoData["RightFmdb"] = modelInfoData["RightFmdb"].replace(assetWeapon, assetWeapon + "_" + assetSuffix)
                if (assetWeapon == "Maneuver_Dual"): #dualie squelchers
                    # replace "Fmdb" value in parsed data
                    modelInfoData["Fmdb"] = modelInfoData["Fmdb"].replace(assetWeapon, assetWeapon + "_" + assetSuffix)
            
            # create modelInfoWriter to write data back into file
            modelInfoWriter = byml.Writer(modelInfoData)
            
            with open(filePath + i, "wb") as w:
                # write data to model file
                modelInfoWriter.write(w)
            
            # rename the ModelInfo file with the appropriate suffix
            #os.rename(filePath + i, filePath + i.replace("_00", "_" + assetSuffix))
            if isShelter:
                new_filename = i.replace("_01", "_" + assetSuffix)
            else:
                new_filename = i.replace("_00", "_" + assetSuffix)
            if not os.path.exists(filePath + new_filename):
                os.rename(filePath + i, filePath + new_filename)
            else:
                print(f"File {new_filename} already exists. Skipping rename.")

    # pack newly written data
    assetPacked = sarclib.pack(filePath, '>', -1)
    # compress for final actor
    assetRecompressed = zstandard.compress(assetPacked)
       
       
    # write new actor to system
    with open("output\\" + assetName + "\\Wmn" + prefix + "_" + wpnPath + ".pack.zs", "wb") as new:
        new.write(assetRecompressed)

    #notify user
    print("\nCreated new actor: Wmn" + prefix + "_" + wpnPath + ".pack.zs")
    
    
    
    
    # check if a brella bullet actor needs to be made (do in wmnG only)
    if isShelter and prefix == "G":
        print("doing canopy actor stuff...")
        # clear variables
        asset = None
        assetDecompressed = None
        assetPacked = None
        assetRecompressed = None
        actordataInfo = None
        fileList = None
        actordataInfoData = None
        actordataInfoWriter = None
        actordataInfo = None
        modelInfo = None
        modelInfoData = None
        modelInfoWriter = None
        filePath = "__tempTT\\filecanopy\\"
        if assetWeapon == "Shelter_Normal":
            asset = open("filepath.txt", "r").read() + "\\Pack\\Actor\\BulletShelterCanopyBase_Cstm.pack.zs" # create and rename copy of second kits' actor (brella)
        else:
            wpnSuffix = assetWeapon.replace("Shelter_", "")
            asset = open("filepath.txt", "r").read() + "\\Pack\\Actor\\BulletShelterCanopy" + wpnSuffix + "_Cstm.pack.zs" # create and rename copy of second kits' actor (brella)
        
        
        # decompress new actor
        assetDecompressed = zstandard.decompress(open(asset, "rb").read())
    
        # get extracted list of files within the actor
        fileList = sarclib.extract(assetDecompressed, filePath)

        # trace through fileList to find things that need to be edited
        for i in fileList:
            # print scanned file
            print(i)
        
            # if file is actor data
            if "Actor/" in i and "_Cstm" in i:
                # notify user
                print(" - Found canopy actor data!")
                # open file into actordataInfo
                actordataInfo = open(filePath + i, "rb").read()
                # parse the data into actordataInfoData
                actordataInfoData = byml.Byml(actordataInfo).parse()
                
                # edit data accordingly 
                actordataInfoData["Components"]["ModelInfoRef"] = actordataInfoData["Components"]["ModelInfoRef"].replace("_Cstm", "_" + assetSuffix)
                

                # create actordataInfoWriter to write data back into file
                actordataInfoWriter = byml.Writer(actordataInfoData)
                    
                with open(filePath + i, "wb") as w:
                    # write data to actor file
                    actordataInfoWriter.write(w)
                
                # rename the actor file with the appropriate suffix
                os.rename(filePath + i, filePath + i.replace("_Cstm", "_" + assetSuffix))
        

            # if file is model data
            if "ModelInfo" in i:
                # notify user
                print(" - Found canopy model info!")
                # open file into modelInfo
                modelInfo = open(filePath + i, "rb").read()
                # parse the data into modelInfoData
                modelInfoData = byml.Byml(modelInfo).parse()
            
                # replace "Fmdb" value in parsed data
                modelInfoData["Fmdb"] = modelInfoData["Fmdb"].replace("_Cstm01", "_" + assetSuffix)
  
                # create modelInfoWriter to write data back into file
                modelInfoWriter = byml.Writer(modelInfoData)
            
                with open(filePath + i, "wb") as w:
                    # write data to model file
                    modelInfoWriter.write(w)
            
                # rename the ModelInfo file with the appropriate suffix
                #os.rename(filePath + i, filePath + i.replace("_00", "_" + assetSuffix))
                new_filename = i.replace("_Cstm", "_" + assetSuffix)
                if not os.path.exists(filePath + new_filename):
                    os.rename(filePath + i, filePath + new_filename)
                else:
                    print(f"File {new_filename} already exists. Skipping rename.")
    
        # pack newly written data
        assetPacked = sarclib.pack(filePath, '>', -1)
        # compress for final actor
        assetRecompressed = zstandard.compress(assetPacked)
       
       
        # if vbrella
        if assetWeapon == "Shelter_Normal":
            # write new actor to system
            with open("output\\" + assetName + "\\BulletShelterCanopyBase_" + assetSuffix + ".pack.zs", "wb") as new:
                new.write(assetRecompressed)

            #notify user
            print("\nCreated new canopy actor!")
        else: # if not vbrella
            # write new actor to system
            with open("output\\" + assetName + "\\BulletShelterCanopy" + wpnSuffix + "_" + assetSuffix + ".pack.zs", "wb") as new:
                new.write(assetRecompressed)

            #notify user
            print("\nCreated new canopy actor!")
    
    
## EXECUTION 
while True: 

    ## USER INPUT 
    print("\nWhat is the weapon you'd like to make a kit for? (ex. Spinner_Serein, Shooter_QuickLong, Roller_Heavy)")
    assetWeapon = input(">> ")
    
    try: os.makedirs("__tempTT")
    except FileExistsError: pass
    
    print("\nEnter desired weapon display name:")
    assetName = input(">> ")
    
    # attempt to make directory for asset
    try: os.makedirs("output\\" + assetName)
    except FileExistsError: pass

    print("\nEnter desired kit suffix: (alphanumeric characters only, will not work if 00, 01, O, or if conflicting with another mod)")
    assetSuffix = input(">> ")

    print("\nEnter desired internal ID: (0-2147483647, will not work if conflicting with another weapon's ID)")
    assetInternalID = input(">> ")
    
    print("\nEnter desired sub weapon: (ex. Bomb_Splash, Beacon, PoisonMist")
    assetSub = input(">> ")
    
    print("\nEnter desired special weapon: (ex. SpUltraShot, SpSuperHook, SpPogo")
    assetSpecial= input(">> ")
    
    print("\nEnter desired points for special:")
    assetSpPoint = input(">> ")

    wpnPath_ORIGINAL = assetWeapon + "_00"
    wpnPath = assetWeapon + "_" + assetSuffix
    
    

    # create WmnG Actor
    createNewActor("G")
    # create WmnS Actor
    createNewActor("S")
    # copy model
    # deal with the stupid ass naming inconsistencies
    if assetWeapon == "Shooter_Precision":
        shutil.copyfile((open("filepath.txt", "r").read() + "\\Model\\Wmn_Shooter_Short.bfres.zs"), ("output\\" + assetName + "\\Wmn_" + wpnPath + ".bfres.zs"))
    elif assetWeapon == "Spinner_Quick":
        shutil.copyfile((open("filepath.txt", "r").read() + "\\Model\\Wmn_Spinner_QuickT.bfres.zs"), ("output\\" + assetName + "\\Wmn_" + wpnPath + ".bfres.zs"))
    elif assetWeapon == "Shooter_TripleQuick":
        shutil.copyfile((open("filepath.txt", "r").read() + "\\Model\\Wmn_Shooter_Triple.bfres.zs"), ("output\\" + assetName + "\\Wmn_" + wpnPath + ".bfres.zs"))
    elif assetWeapon == "Charger_NormalScope":
        shutil.copyfile((open("filepath.txt", "r").read() + "\\Model\\Wmn_Charger_NormalT.bfres.zs"), ("output\\" + assetName + "\\Wmn_" + wpnPath  + ".bfres.zs"))
    elif assetWeapon == "Charger_Normal":
        shutil.copyfile((open("filepath.txt", "r").read() + "\\Model\\Wmn_Charger_NormalT.bfres.zs"), ("output\\" + assetName + "\\Wmn_" + wpnPath  + ".bfres.zs"))
    elif assetWeapon == "Charger_LongScope":
        shutil.copyfile((open("filepath.txt", "r").read() + "\\Model\\Wmn_Charger_Long.bfres.zs"), ("output\\" + assetName + "\\Wmn_" + wpnPath  + ".bfres.zs"))
    elif assetWeapon == "Saber_Lite":
        shutil.copyfile((open("filepath.txt", "r").read() + "\\Model\\Wmn_Saber_Light.bfres.zs"), ("output\\" + assetName + "\\Wmn_" + wpnPath  + ".bfres.zs"))
    elif assetWeapon == "Shooter_Normal":
        shutil.copyfile((open("filepath.txt", "r").read() + "\\Model\\Wmn_Shooter_NormalT.bfres.zs"), ("output\\" + assetName + "\\Wmn_" + wpnPath  + ".bfres.zs"))
    elif assetWeapon == "Blaster_LightLong":
        shutil.copyfile((open("filepath.txt", "r").read() + "\\Model\\Wmn_Blaster_Light.bfres.zs"), ("output\\" + assetName + "\\Wmn_" + wpnPath  + ".bfres.zs"))
    elif assetWeapon == "Maneuver_Normal":
        shutil.copyfile((open("filepath.txt", "r").read() + "\\Model\\Wmn_Maneuver_NormalT.bfres.zs"), ("output\\" + assetName + "\\Wmn_" + wpnPath  + ".bfres.zs"))
    elif assetWeapon == "Roller_Normal":
        shutil.copyfile((open("filepath.txt", "r").read() + "\\Model\\Wmn_Roller_NormalT.bfres.zs"), ("output\\" + assetName + "\\Wmn_" + wpnPath  + ".bfres.zs"))
    elif assetWeapon == "Slosher_Strong":
        shutil.copyfile((open("filepath.txt", "r").read() + "\\Model\\Wmn_Slosher_StrongT.bfres.zs"), ("output\\" + assetName + "\\Wmn_" + wpnPath  + ".bfres.zs"))
    elif assetWeapon == "Spinner_Standard":
        shutil.copyfile((open("filepath.txt", "r").read() + "\\Model\\Wmn_Spinner_StandardT.bfres.zs"), ("output\\" + assetName + "\\Wmn_" + wpnPath  + ".bfres.zs"))
    else:
        shutil.copyfile((open("filepath.txt", "r").read() + "\\Model\\Wmn_" + assetWeapon + ".bfres.zs"), ("output\\" + assetName + "\\Wmn_" + wpnPath  + ".bfres.zs"))
    # copy wpn ui
    shutil.copyfile((open("filepath.txt", "r").read() + "\\UI\\Icon\\Wpn\\Wst_" + wpnPath_ORIGINAL + ".bntx.zs"), ("output\\" + assetName + "\\Wst_" + assetName  + ".bntx.zs"))
    # copy wpnpath ui
    shutil.copyfile((open("filepath.txt", "r").read() + "\\UI\\Icon\\WpnPath\\Path_Wst_" + wpnPath_ORIGINAL + ".bntx.zs"), ("output\\" + assetName + "\\Path_Wst_" + assetName  + ".bntx.zs"))
    
    # create info.txt
    create = open("output\\" + assetName + "\\info.txt", "w")
    create.close()
    info = open("output\\" + assetName + "\\info.txt", "w")
    
    info.write(str("[NAME]\n" + assetName + "\n"))
    info.write("[SUFFIX]\n" + assetSuffix + "\n")
    info.write("[ORIGINAL]\n" + assetWeapon + "\n")
    info.write("[SUB]\n" + assetSub + "\n")
    info.write("[SPECIAL]\n" + assetSpecial + "\n")
    info.write("[POINTS]\n" + assetSpPoint + "\n")
    info.write("[ID]\n" + assetInternalID + "\n")

    info.close()
    # delete temp folder
    os.chmod("__tempTT", stat.S_IRUSR | stat.S_IWUSR | stat.S_IXUSR)
    shutil.rmtree("__tempTT", ignore_errors=False)
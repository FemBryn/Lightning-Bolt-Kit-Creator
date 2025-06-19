using BymlLibrary;
using ZstdSharp;
namespace Lightning_Bolt_Kit_Creator;

public class BymlEditing
{
    public static void ShelterActorReservation(String filePath)
    {
        Byte[] filedata = File.ReadAllBytes(filePath);
        Byml byml = Byml.FromBinary(filedata);
    }
    public static void bymlPrint(String filePath)
    {
        Byte[] filedata = File.ReadAllBytes(filePath);
        Byml byml = Byml.FromBinary(filedata);

        recursiveBymlContentPrint(byml, Path.GetFileNameWithoutExtension(filePath));
    }

    public static void recursiveBymlContentPrint(Byml byml, String Prefix)
    {
        Debug.Write(Prefix+"  : " + byml.Type);
    }
}
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveHangarData(ShuttleManager shuttleManager, ShuttleColorManager colorManager, ShuttleTailManager tailManager, HangarManager hangarManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/hangar.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        HangarData data = new HangarData(shuttleManager, colorManager, tailManager, hangarManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static HangarData LoadHangarData()
    {
        string path = Application.persistentDataPath + "/hangar.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            HangarData data = formatter.Deserialize(stream) as HangarData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }

    /* Things to save:
     * 
     * - didTutorial = false
     * - receiveInstallReward = false;
     * 
     * - Selected Shuttle = 0
     * - Bought Shuttles = 1
     * 
     * - Selected Color = 0
     * - Bought Colors = 1
     * 
     * - Selected Tail = 0
     * - Bought Tails = 1
     * 
     * - Coins = 0
     * - Shields = 0
     * - Highscore = 0
     * 
     * - Music Toggle Value = true
     * - Soundeffects Toggle Value = true
     * - Vibration Toggle Value = false
     * - Sensivity Slider Value = 50
     * 
     */

}

using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

public static class SaveManager {
    public static string directory = "/SaveData/";
    public static string fileName = "data.json";
    static bool IsSaving = false;

    public static bool Check {
        get {
            string fullPath = Application.persistentDataPath + directory + fileName;
            return File.Exists(fullPath);
        }
    }

    public static void CreateNewGame() {
        InGameData data = new InGameData();
        Save(data);
    }

    public static void Save(InGameData data) {
        if(!IsSaving) {
            IsSaving = true;
            string dir = Application.persistentDataPath + directory;
            if(!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            FileStream writer = new FileStream(dir + fileName, FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(InGameData));
            ser.WriteObject(writer, data);
            writer.Close();
            IsSaving = false;
        }
    }

    public static InGameData Load() {
        InGameData result = null;
        string fullPath = Application.persistentDataPath + directory + fileName;

        if(File.Exists(fullPath)) {
            FileStream fs = new FileStream(fullPath, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            DataContractSerializer ser = new DataContractSerializer(typeof(InGameData));

            // Deserialize the data and read it from the instance.
            result = (InGameData)ser.ReadObject(reader, true);
            reader.Close();
            fs.Close();
        } 
        else {
            Debug.Log("Save file does not exist");
        }

        return result;
    }

    public static void Delete() {
        string fullPath = Application.persistentDataPath + directory + fileName;
        if(File.Exists(fullPath)) {
            File.Delete(fullPath);
        }
    }
}

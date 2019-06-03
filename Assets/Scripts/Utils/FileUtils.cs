using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

public class FileUtils {
    public static void Save(string path, object data) {
        using (FileStream fs = File.Create(Application.persistentDataPath + "/" + path)) {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, data);
            fs.Close();
        }
    }  

    public static T Load<T>(string path) {
        var p = Application.persistentDataPath + "/" + path;
        if (!File.Exists(p))
            return default(T);

        using (FileStream fs = File.OpenRead(p)) {
            BinaryFormatter bf = new BinaryFormatter();
            T x;
            try {
                x = (T)bf.Deserialize(fs);
            } catch {
                x = default(T); ;
            }
            
            fs.Close();
            return x;
        }
    }

    public static bool FileExists(string path) {
        return File.Exists(Application.persistentDataPath + "/" + path);
    }

    public static void Delete(string path) {
        File.Delete(Application.persistentDataPath + "/" + path);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Managers
{
    public class JsonFileManager
    {
        public void SaveJson(string fileName,string json)
        {
            string path = Path.Combine(Application.streamingAssetsPath,fileName+"json");
            File.WriteAllText(path,json);
        }
        public string LoadData<T>(string fileName) where T : class
        {
            string path = Path.Combine(Application.streamingAssetsPath,fileName+"json");
            string json;
            if(!File.Exists(path))
            {
                Debug.LogError("File not found: "+path);
            }
            json = File.ReadAllText(path);
            return json;
        }
    }
}

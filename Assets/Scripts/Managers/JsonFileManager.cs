using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Managers
{
    /*
     * Žg‚í‚È‚¢
     */
    public static class JsonFileManager
    {
        const string json_extension = ".json";
        public static void SaveJson(string fileName,string json)
        {
            string path = Path.Combine(Application.streamingAssetsPath,fileName+json_extension);
            File.WriteAllText(path,json);
        }
        public static string LoadJson<T>(string fileName) where T : class
        {
            string path = Path.Combine(Application.streamingAssetsPath,fileName+json_extension);
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

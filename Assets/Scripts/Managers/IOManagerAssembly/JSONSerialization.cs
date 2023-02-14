using IOManagerAssembly;
using UnityEditor;
using UnityEngine;
using System.Text.Json;
using System;
using System.Runtime.Serialization;

namespace IOManagerAssembly
{
    public class JSONSerialization<T> : IDataSerialization<T>
    {
        private const string extension = ".json";
        public string Extension=>extension;
        public string Serialize(T value, bool formattingEnabled = false)
        {
            try
            {
                string json = JsonSerializer.Serialize(value);
                return json;
            }
            catch (ArgumentException e)
            {
                Debug.LogError("無効な引数" + value.ToString()
                    + "ErrorMessage: " + e.Message);
                return string.Empty;
            }
        }
        public T Deserialize(string serialData)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(serialData);
            }
            catch (JsonException e)
            {
                Debug.LogError("無効なJSONデータErrorMessage: " + e.Message);
                return default(T);
            }
            catch (ArgumentNullException e)
            {
                Debug.LogError("JSONがNull。ErrorMessage: " + e.Message);
                return default(T);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.Json;

namespace Characters
{
    public class CharacterDataBase : MonoBehaviour
    {
        #region プロパティ
        public List<CharacterData> Characters { get; set; } = new List<CharacterData>();
        #endregion

        #region メソッド
        public void AddCharacter(params CharacterData[] _characters)
        {
            foreach(var character in _characters)
            {
                if(!Characters.Any(c=>c.Name==character.Name))
                {
                    Characters.Add(character);
                }
            }
        }
        public void RemoveCharacter(CharacterData _character)
        {
            Characters.Remove(_character);
        }
        public void Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(Characters);
                if(string.IsNullOrEmpty(json.Trim('[',']')))
                {
                    Debug.LogError("Error: json file is empty.");
                }
                PlayerPrefs.SetString("CharacterData",json);
                if(!PlayerPrefs.HasKey("CharacterData"))
                {
                    Debug.LogError("Key does not exist.");
                }
            }
            catch(System.ArgumentNullException e)
            {
                Debug.LogError("Error: Characters list is null. Exception message: "+e.Message);
            }
            catch(System.Text.Json.JsonException e)
            {
                Debug.LogError("Error: Failed to Serialize Object. Exception Message: "+e.Message);
            }
        }

        public List<CharacterData> Load()
        {
            string json = PlayerPrefs.GetString("CharacterData");
            return JsonSerializer.Deserialize<List<CharacterData>>(json);
        }
        #endregion

        // JSONセーブデータを読み込むためのクラス
        [System.Serializable]
        public class CharacterData
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int HP { get; set; }
            public int MP { get; set; }
            public int Strength { get; set; }
            public int Defense { get; set; }
            public int Speed { get; set; }
        }
    }
}
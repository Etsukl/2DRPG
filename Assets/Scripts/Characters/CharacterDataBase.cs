using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.Json;
using IOManagerAssembly;

namespace Characters
{


    public class CharacterDataBase
    {
        private IDataIO dataIO;
        private IDataSerialization<List<CharacterData>> serialization;
        public CharacterDataBase(IDataIO dataIO, IDataSerialization<List<CharacterData>> serialization)
        {
            this.dataIO = dataIO;
            this.serialization = serialization;
        }


        #region プロパティ
        public List<CharacterData> Characters { get; set; } = new List<CharacterData>();
        #endregion

        #region メソッド
        public void AddCharacter(CharacterData _character)
        {
            if(!Characters.Contains(_character))Characters.Add(_character);
        }
        public void AddCharacter(CharacterData _character1,CharacterData _character2)
        {
            this.AddCharacter(_character1);
            this.AddCharacter(_character2);
        }

        public void AddCharacter(CharacterData _character1,CharacterData _character2,
            CharacterData _character3,CharacterData _character4)
        {
            this.AddCharacter(_character1,_character2);
            this.AddCharacter(_character3,_character4);
        }
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

        /*　セーブ/ロードがデータベースに必要だろうか。
         *　A.多分必要？各データのセーブ/ロードを行う際は、ここを参照してすべてのデータを書き込む。
         *　ただし、変更があったファイルだけを書き換えたい。*/
        public void Save()
        {
            string serialData = serialization.Serialize(Characters);
            dataIO.WriteData(serialData);
        }

        public List<CharacterData> Load()
        {
            string serialData = dataIO.ReadData();
            return serialization.Deserialize(serialData);
        }
        
        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using Characters;
using IOManagerAssembly;
using System.Text.Json;
using System.IO;

namespace Editor
{
    [TestFixture]
    public class CharacterDataBaseTests
    {
        private string path;
        private IDataIO dataIO;
        private string filename = "CharacterData";
        private IDataSerialization<List<CharacterData>> dataSerialization;
        private CharacterDataBase characterDataBase;
        private CharacterData character1;
        private CharacterData character2;
        private CharacterData character3;
        private CharacterData character4;
        private CharacterData character5;
        [SetUp]
        public void SetUp()
        {
            path = Application.streamingAssetsPath;
            dataIO = new FileIO(filename,".json");
            dataSerialization = new JSONSerialization<List<CharacterData>>();

            characterDataBase = new CharacterDataBase(dataIO, dataSerialization);

            character1 = new CharacterData
            {
                Name = "勇者",
                Description = "世界を救うため旅する勇者",
                HP = 100,
                MP = 50,
                Strength = 10,
                Defense = 5,
                Speed = 8
            };

            character2 = new CharacterData
            {
                Name = "魔法使い",
                Description = "魔法を使い魔物を倒す魔法使い",
                HP = 80,
                MP = 80,
                Strength = 5,
                Defense = 3,
                Speed = 6
            };
            character3 = new CharacterData
            {
            Name = "サムライ",
            Description = "剣の使い手であり忠誠心があるサムライ",
            HP = 90,
            MP = 40,
            Strength = 9,
            Defense = 7,
            Speed = 7
            };

            character4 = new CharacterData
            {
            Name = "忍者",
            Description = "影から敵を倒す忍者",
            HP = 75,
            MP = 60,
            Strength = 6,
            Defense = 8,
            Speed = 10
            };

            character5 = new CharacterData
            {
            Name = "武者",
            Description = "格闘技で敵を倒す武者",
            HP = 85,
            MP = 30,
            Strength = 8,
            Defense = 6,
            Speed = 9
            };

        }
        

        /// <summary>
        /// CharacterDataBase.AddCharacter()が正常に動作しているかのテスト。
        /// １人追加したときに１人追加される。
        /// </summary>
        [Test]
        public void AddCharacter_AddingOneCharacter_ShouldAddOneCharacterToList()
        {
            characterDataBase.AddCharacter(character1);
            Assert.AreEqual(1,characterDataBase.Characters.Count);
        }
        /// <summary>
        /// CharacterDataBase.AddCharacter()が正常に動作しているかのテスト。
        /// 複数のキャラクターをまとめて追加出来るかどうか。
        /// </summary>
        [Test]
        public void AddCharacter_AddingMultipleCharacters_ShoudAddMultipleCharactersToList()
        {
            characterDataBase.AddCharacter(character1,character2);
            Assert.AreEqual(2,characterDataBase.Characters.Count);
        }
        /// <summary>
        /// CharacterDataBase.AddCharacter()が正常に動作しているかのテスト。
        /// 重複を許さずに、データベースにキャラクターを追加できるかどうか。
        /// </summary>
        [Test]
        public void TestAddUniqueCharacter_AddingDupulicateCharacter_ShouldAddOnlyCharacterToList()
        {
            characterDataBase.AddCharacter(character1);
            characterDataBase.AddCharacter(character1);
            Assert.AreEqual(1,characterDataBase.Characters.Count);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Save_Called_ShouldSaveCharacter()
        {
            characterDataBase.AddCharacter(character1);
            characterDataBase.Save();

            string savedJson = dataIO.ReadData();

            Assert.IsFalse(string.IsNullOrEmpty(savedJson));

            var savedCharacters = JsonSerializer.Deserialize<List<CharacterData>>(savedJson);

            Assert.AreEqual(characterDataBase.Characters.Count, savedCharacters.Count);
            Assert.AreEqual(characterDataBase.Characters[0].Name,savedCharacters[0].Name);            
        }
        [Test]
        public void Save_Called_ShouldSaveAllCharactersToPlayerPrefs()
        {
            characterDataBase.AddCharacter(character1,character2,character3,character4,character5);
            characterDataBase.Save();

            string savedJson = dataIO.ReadData();
            var savedCharacters = JsonSerializer.Deserialize<List<CharacterData>>(savedJson);

            Assert.AreEqual(characterDataBase.Characters.Count, savedCharacters.Count);
            for(int i=0;i<savedCharacters.Count;i++)
            {
                Assert.AreEqual(characterDataBase.Characters[i].Name,savedCharacters[i].Name);
            }
        }
        [Test]
        public void Load_WhenCalled_ShouldReturnCharactersFromPlayerPrefs()
        {
            characterDataBase.AddCharacter(character2);
            characterDataBase.Save();

            var loadedCharacters = characterDataBase.Load();

            Assert.AreEqual(characterDataBase.Characters.Count, loadedCharacters.Count);
            Assert.AreEqual(characterDataBase.Characters[0].Name,loadedCharacters[0].Name);
        }
        [Test]
        public void Save_SavesCharacterDataCorrectly_AfterFixed()
        {
            var expectedCharacterName = "After Fixed.";
            characterDataBase.AddCharacter(character1,character2,character3,character4,character5);
            characterDataBase.Characters[0].Name = expectedCharacterName;
            characterDataBase.Save();

            string savedJson = dataIO.ReadData();

            var savedCharacters = JsonSerializer.Deserialize<List<CharacterData>>(savedJson);

            Assert.AreEqual(expectedCharacterName, savedCharacters[0].Name);
        }
    }
}

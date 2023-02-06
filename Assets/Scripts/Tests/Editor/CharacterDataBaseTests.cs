using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using Characters;
using System.Text.Json;
namespace Editor
{
    [TestFixture]
    public class CharacterDataBaseTests
    {
        private GameObject mock;
        private CharacterDataBase characterDataBase;
        private CharacterDataBase.CharacterData character1;
        private CharacterDataBase.CharacterData character2;
        private CharacterDataBase.CharacterData character3;
        private CharacterDataBase.CharacterData character4;
        private CharacterDataBase.CharacterData character5;
        [SetUp]
        public void SetUp()
        {
            mock = new GameObject();
            characterDataBase = mock.AddComponent<CharacterDataBase>();

            character1 = new CharacterDataBase.CharacterData
            {
                Name = "勇者",
                Description = "世界を救うため旅する勇者",
                HP = 100,
                MP = 50,
                Strength = 10,
                Defense = 5,
                Speed = 8
            };

            character2 = new CharacterDataBase.CharacterData
            {
                Name = "魔法使い",
                Description = "魔法を使い魔物を倒す魔法使い",
                HP = 80,
                MP = 80,
                Strength = 5,
                Defense = 3,
                Speed = 6
            };
            character3 = new CharacterDataBase.CharacterData
            {
            Name = "サムライ",
            Description = "剣の使い手であり忠誠心があるサムライ",
            HP = 90,
            MP = 40,
            Strength = 9,
            Defense = 7,
            Speed = 7
            };

            character4 = new CharacterDataBase.CharacterData
            {
            Name = "忍者",
            Description = "影から敵を倒す忍者",
            HP = 75,
            MP = 60,
            Strength = 6,
            Defense = 8,
            Speed = 10
            };

            character5 = new CharacterDataBase.CharacterData
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
        

        [Test]
        public void AddCharacter_AddingOneCharacter_ShouldAddOneCharacterToList()
        {
            characterDataBase.AddCharacter(character1);
            Assert.AreEqual(1,characterDataBase.Characters.Count);
        }
        [Test]
        public void AddCharacter_AddingMultipleCharacters_ShoudAddMultipleCharactersToList()
        {
            characterDataBase.AddCharacter(character1,character2);
            Assert.AreEqual(2,characterDataBase.Characters.Count);
        }
        [Test]
        public void TestAddUniqueCharacter_AddingDupulicateCharacter_ShouldAddOnlyCharacterToList()
        {
            characterDataBase.AddCharacter(character1);
            characterDataBase.AddCharacter(character1);
            Assert.AreEqual(1,characterDataBase.Characters.Count);
        }
        [Test]
        public void Save_Called_ShouldSaveCharacterToPlayerPrefs()
        {
            characterDataBase.AddCharacter(character1);
            characterDataBase.Save();

            var savedJson = PlayerPrefs.GetString("CharacterData");
            Assert.IsFalse(string.IsNullOrEmpty(savedJson));

            var savedCharacters = JsonSerializer.Deserialize<List<CharacterDataBase.CharacterData>>(savedJson);

            Assert.AreEqual(characterDataBase.Characters.Count, savedCharacters.Count);
            Assert.AreEqual(characterDataBase.Characters[0].Name,savedCharacters[0].Name);            
        }
        [Test]
        public void Save_Called_ShouldSaveAllCharactersToPlayerPrefs()
        {
            characterDataBase.AddCharacter(character1,character2,character3,character4,character5);
            characterDataBase.Save();

            var savedJson = PlayerPrefs.GetString("CharacterData");
            var savedCharacters = JsonSerializer.Deserialize<List<CharacterDataBase.CharacterData>>(savedJson);

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

            var savedJson = PlayerPrefs.GetString("CharacterData");
            var savedCharacters = JsonSerializer.Deserialize<List<CharacterDataBase.CharacterData>>(savedJson);

            Assert.AreEqual(expectedCharacterName, savedCharacters[0].Name);
        }
    }
}

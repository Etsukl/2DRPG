using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class CharacterClass : MonoBehaviour
    {
        #region プロパティ
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int Strength  { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public List<ActionClass> Actions => _actions;
        #endregion

        private List<ActionClass> _actions;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Characters
{
    [Serializable]
    public class ActionClass
    {
        #region プロパティ
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Damage { get; set; }
        public int Effect { get; set; }
        public int Cost { get; set; }
        #endregion
    }
}
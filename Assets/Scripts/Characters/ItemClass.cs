using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class ItemClass : MonoBehaviour
    {
        #region プロパティ
        public string Name{ get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public Sprite Icon { get; set; }
        #endregion

        #region メソッド
        public void UseItem()
        {
            //アイテムを使用する処理
        }
        #endregion
    }
}
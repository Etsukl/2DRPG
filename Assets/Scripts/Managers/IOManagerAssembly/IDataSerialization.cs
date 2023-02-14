using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;

namespace IOManagerAssembly
{
    /// <summary>
    /// 各種データをシリアライズ化することが出来るクラス
    /// </summary>

    public interface IDataSerialization<T>
    {
        public string Extension { get; }
        public string Serialize(T value, bool formattingEnabled = false);

        public T Deserialize(string serialData);
    }
}

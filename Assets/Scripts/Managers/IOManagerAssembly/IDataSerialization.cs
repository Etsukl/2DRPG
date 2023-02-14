using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;

namespace IOManagerAssembly
{
    /// <summary>
    /// �e��f�[�^���V���A���C�Y�����邱�Ƃ��o����N���X
    /// </summary>

    public interface IDataSerialization<T>
    {
        public string Extension { get; }
        public string Serialize(T value, bool formattingEnabled = false);

        public T Deserialize(string serialData);
    }
}

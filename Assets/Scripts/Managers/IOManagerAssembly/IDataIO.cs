using UnityEditor;
using UnityEngine;

namespace IOManagerAssembly
{
    public interface IDataIO
    {
        public string ReadData();
        public void WriteData(string data);
    }
}
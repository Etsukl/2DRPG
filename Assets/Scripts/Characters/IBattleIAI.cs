using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public interface IBattleAI
    {
        public string SelectCommand()
        {
            return "Attack";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JeuDeThud.Util
{
    public class DebugLogComponent : MonoBehaviour
    {

        public bool canDebugMessage = true;

        public void DebugMessage(string message, bool isActive)
        {
            if (canDebugMessage && isActive)
            {
                Debug.Log(message);
            }
        }
    }
}



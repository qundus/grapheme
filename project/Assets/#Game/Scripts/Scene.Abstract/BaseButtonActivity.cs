using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Scene
{
    [Serializable]
    public class BaseButtonActivity
    {
        public Button Button;
        public string Method;
        public int MethodIndex = 0;
    }
}
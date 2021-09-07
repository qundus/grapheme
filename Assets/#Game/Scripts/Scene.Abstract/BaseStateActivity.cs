using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Scene
{
    [Serializable]
    public class BaseStateActivity
    {
        public string StateName;
        public int PlayOnSceneState;
        public float Duration = 1;
    }
}
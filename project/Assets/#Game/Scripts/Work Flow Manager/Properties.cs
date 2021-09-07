using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace WorkFlowManager
{
    public class EventHandlerProperties
    {
        public struct InternalButton
        {
            public Button button { set; get; }
            public int currentState { set; get; }

            public InternalButton(Button obj = null)
            {
                button = obj;
                currentState = 0;
            }
        }

        public GameObject obj { set; get; }
        public Animator anim { set; get; }
        public List<string> states { set; get; }
        public List<InternalButton> buttonsList { set; get; }

        public EventHandlerProperties(GameObject obj = null)
        {
            this.obj = obj;
            anim = null;
            buttonsList = new List<InternalButton>();
            states = new List<string>();
        }

        public void AddButton(Button obj)
        {
            buttonsList.Add(new InternalButton(obj));
        }

    }
}
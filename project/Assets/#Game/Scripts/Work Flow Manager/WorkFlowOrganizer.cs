using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace WorkFlowManager
{
    public class WorkFlowOrganizer : MonoBehaviour
    {
        public List<EventHandlerProperties> eventObjects { set; get; }

        public WorkFlowOrganizer()
        {
            eventObjects = new List<EventHandlerProperties>();
        }

        void Awake()
        {
            //DontDestroyOnLoad(this);
        }

        void FixedUpdate()
        {
            print(eventObjects[0].obj.name);
        }
    }
}
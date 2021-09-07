using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WorkFlowManager
{
    public class EditorProperties
    {
        public float yIndentation { set; get; }
        public Vector2 scrollPos { set; get; }
        public int selectedObject { set; get; }
        //public int selectedButton { set; get; }
        public int selectedObjectEdit { set; get; }
        public List<bool> selectedButtonFoldOut { set; get; }
        public float scrollHeight { set; get; }

        public EditorProperties()
        {
            yIndentation = 0;
            scrollPos = Vector2.zero;
            selectedObject = -1;
            //selectedButton = 0;
            selectedObjectEdit = 0;
            selectedButtonFoldOut = new List<bool>();
            scrollHeight = 100;
        }

        public string[] GetObjectsNames(List<EventHandlerProperties> objects)
        {
            List<string> names = new List<string>();

            try
            {
                //names.Add("None");

                foreach (EventHandlerProperties prop in objects)
                {
                    names.Add(prop.obj.name);
                }

                return names.ToArray();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return new string[] { "FAIL!!" };
            }


        }
    }
}
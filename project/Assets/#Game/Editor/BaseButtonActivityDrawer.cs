using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Scene
{
    // IngredientDrawer
    [CustomPropertyDrawer(typeof(BaseButtonActivity))]
    public class BaseButtonActivityDrawer : PropertyDrawer
    {
        // Variables
        List<string> methodsCollection = new List<string>();
        string activeScene = string.Empty;

        void RefreshMethods()
        {

            MethodInfo[] methodInfoCollection = null;
            methodsCollection.Clear();

            if (activeScene == GameProperties.GameScenes.Intro.ToString()) ;// Empty - No Buttons.
            else
            if (activeScene == GameProperties.GameScenes.MainMenu.ToString()) 
                methodInfoCollection = ScriptableObject.CreateInstance<MainMenu.MainMenuAnimationsMapper>().GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            else
            if (activeScene == GameProperties.GameScenes.Gameplay.ToString())
                methodInfoCollection = ScriptableObject.CreateInstance<Gameplay.GameplayAnimationsMapper>().GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);


            if (methodInfoCollection != null)
            {
                foreach (MethodInfo mi in methodInfoCollection)
                    if (mi.Name.StartsWith("_"))
                        methodsCollection.Add(mi.Name);

            }
            else
                MonoBehaviour.print("Methods Info Is Null In Scene " + activeScene);

        }
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            if (activeScene == string.Empty)
            {
                BaseMonoBehaviour go = (BaseMonoBehaviour)property.serializedObject.targetObject;
                activeScene = go.gameObject.scene.name;
            }
            else
                RefreshMethods();
            
            EditorGUI.BeginProperty(position, label, property);

            //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            
            //EditorGUI.DrawRect(new Rect(0, position.y + 5, position.width, position.height), Color.black);

            var buttonRect = new Rect(position.x + 50, position.y + 5, 250, position.height);
            SerializedProperty sButton = property.FindPropertyRelative("Button");
            SerializedProperty sMethod = property.FindPropertyRelative("Method");
            SerializedProperty sMethodIndex = property.FindPropertyRelative("MethodIndex");
            
            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(buttonRect, sButton, GUIContent.none);
            
            if (sButton.objectReferenceValue != null)
            {
                var labelRect = new Rect(buttonRect.x + buttonRect.size.x, position.y + 2, 80, position.height);
                EditorGUI.DropShadowLabel(labelRect, "OnCLick()");
                
                if (methodsCollection.Count > 0)
                {
                    var methodRect = new Rect(labelRect.x + labelRect.size.x, position.y + 5, 150, position.height);
                    sMethodIndex.intValue = EditorGUI.Popup(methodRect, sMethodIndex.intValue, methodsCollection.ToArray());

                    if (methodsCollection[sMethodIndex.intValue] != sMethod.stringValue)
                        sMethod.stringValue = methodsCollection[sMethodIndex.intValue];
                }
            }

            EditorGUI.EndProperty();
            position.size = new Vector2(position.size.x, position.size.y + 100);

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.EndProperty();

        }
    }
}
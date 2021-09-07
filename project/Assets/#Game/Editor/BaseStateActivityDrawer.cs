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
    [CustomPropertyDrawer(typeof(BaseStateActivity))]
    public class BaseStateActivityDrawer : PropertyDrawer
    {
        // Variables
        int ControlHeight = 5;

        // so that the next control goes bellow of our second line of previous control
        public override float GetPropertyHeight(SerializedProperty prop,GUIContent label)
        {
            return base.GetPropertyHeight(prop, label) + ControlHeight;
        }

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float xIndent = position.x;
            BaseAnimationsMapper go = property.serializedObject.targetObject as BaseAnimationsMapper;

            EditorGUI.BeginProperty(position, label, property);

            var labelRect = new Rect(xIndent, position.y , 100, position.height);
            EditorGUI.DropShadowLabel(labelRect, "Play State");
            xIndent += labelRect.size.x;

            var stateNameRect = new Rect(xIndent, position.y + 5, 176, position.height);
            SerializedProperty sStateName = property.FindPropertyRelative("StateName");
            sStateName.stringValue = EditorGUI.TextField(stateNameRect, sStateName.stringValue);
            xIndent += stateNameRect.size.x;


            
            labelRect = new Rect(xIndent, position.y, 30, position.height);
            EditorGUI.DropShadowLabel(labelRect, "On");
            xIndent += labelRect.size.x;

            

            var playOnSceneStateRect = new Rect( xIndent , position.y + 10, 150, position.height);
            SerializedProperty sPlayOnState = property.FindPropertyRelative("PlayOnSceneState");
            switch (go.sceneReference)
            {
                case GameProperties.GameScenes.Intro:
                    sPlayOnState.intValue = EditorGUI.Popup(playOnSceneStateRect, sPlayOnState.intValue, Enum.GetNames(typeof(Scene.Intro.States)));
                    break;

                case GameProperties.GameScenes.MainMenu:
                    sPlayOnState.intValue = EditorGUI.Popup(playOnSceneStateRect, sPlayOnState.intValue, Enum.GetNames(typeof(Scene.MainMenu.States)) );
                    break;

                case GameProperties.GameScenes.Gameplay:
                    sPlayOnState.intValue = EditorGUI.Popup(playOnSceneStateRect, sPlayOnState.intValue, Enum.GetNames(typeof(Scene.Gameplay.States)) );
                    break;
            }
            xIndent += playOnSceneStateRect.size.x;


            labelRect = new Rect(xIndent, position.y, 50, position.height);
            EditorGUI.DropShadowLabel(labelRect, "Time");
            xIndent += labelRect.size.x;

            var durationRect = new Rect(xIndent, position.y + 5, 50, position.height);
            SerializedProperty sDuration = property.FindPropertyRelative("Duration");
            sDuration.floatValue = EditorGUI.FloatField(durationRect, sDuration.floatValue);
            xIndent += durationRect.size.x;


            EditorGUI.EndProperty();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;
using System.Collections.Generic;


// The goal of building this script is to help me with maintaining and keeping track of UI Buttons and 
// their functionalities all in one place.


// Development of the EditorScript.cs has stopped due to the difficulity in passing editor filled 
// properties/objects to the target (base script's variables) during runtime where it's always 'NULL'
// despite the various attempts to fill them up, from using "EditorUtility.SetDirty()" and  "EditorSceneManager.MarkAllScenesDirty()"
// to setting the "WorkFlowOrganizerProperties.cs" to [Serialized] with all variables to [SerializedProperty] but with zero benefecial 
// outcome.

namespace WorkFlowManager
{
    [CustomEditor(typeof(WorkFlowOrganizer))]
    [CanEditMultipleObjects]
    public class WorkFlowOrganizerEditor : Editor
    {
        /////////////////////////////////////////////////////////////////////////////////////////
        // Add menu item named "SPEC : Work Flow Spectator" to the Window menu
        //[MenuItem("Window/SPEC : Work Flow Spectator")]
        //public static void ShowWindow()
        //{
        //Show existing window instance. If one doesn't exist, make one.
        //GetWindow(typeof(WorkFlowSpectator));
        //}
        /////////////////////////////////////////////////////////////////////////////////////////

        // Variables
        EditorProperties layoutProp { set; get; }
        List<GameObject> tempObjectsList { set; get; }
        List<Button> tempButtonsList { set; get; }
        string tempStatesString { set; get; }
        public static WorkFlowOrganizer eventHandler { set; get; }

        // Machine STates


        // Initialize Everything
        void Init()
        {

            if (tempObjectsList == null)
                tempObjectsList = new List<GameObject>();

            if (tempButtonsList == null)
                tempButtonsList = new List<Button>();

            if (layoutProp == null)
                layoutProp = new EditorProperties();


            EditorStyles.boldLabel.alignment = TextAnchor.MiddleCenter;
            EditorStyles.boldLabel.fontSize = 17;
            EditorStyles.boldLabel.fontStyle = FontStyle.Bold;
            EditorStyles.boldLabel.wordWrap = false;
            EditorStyles.boldLabel.padding = new RectOffset(0, 0, 0, 0);
            GUILayout.Label("Event Handler v1.0", EditorStyles.boldLabel);
        }

        void OnEnable()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorSceneManager.MarkAllScenesDirty();
                eventHandler = (WorkFlowOrganizer)target;
            }

            Debug.Log("Scenes Marked Dirty");
        }

        public override void OnInspectorGUI()
        {
            Init();


            using (var scroll = new EditorGUILayout.ScrollViewScope(layoutProp.scrollPos, GUILayout.Height(layoutProp.scrollHeight), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
            {
                layoutProp.scrollPos = scroll.scrollPosition;

                layoutProp.yIndentation = 10;
                //////////////////////// Buttons \\\\\\\\\\\\\\\\\\\\\\
                if (GUI.Button(new Rect(17, layoutProp.yIndentation, 75, 30), "Add Object"))
                {

                    if (tempObjectsList.Count < 4)
                        tempObjectsList.Add(null);
                }

                //////////////////////// Buttons END \\\\\\\\\\\\\\\\\\\\\\


                //////////////////////// Adding Objects To Be Controlled \\\\\\\\\\\\\\\\\\\\\\
                if (tempObjectsList.Count > 0)
                {
                    //layoutProp.scrollPos = EditorGUILayout.BeginScrollView(layoutProp.scrollPos, true, false , GUILayout.MinHeight(1), GUILayout.Width(450), GUILayout.MaxHeight(layoutProp.scrollRect.height), GUILayout.ExpandHeight(true));

                    layoutProp.yIndentation += 15;
                    if (GUI.Button(new Rect(350, layoutProp.yIndentation, 100, 40), "Confirm", EditorStyles.boldLabel))
                    {
                        foreach (GameObject go in tempObjectsList)
                        {
                            if (go != null)
                                eventHandler.eventObjects.Add(new EventHandlerProperties(go));
                        }

                        tempObjectsList.Clear();
                    }

                    //layoutProp.yIndentation += 35;
                    for (int i = 0; i < tempObjectsList.Count; i++)
                    {
                        tempObjectsList[i] = (GameObject)EditorGUI.ObjectField(new Rect(25, layoutProp.yIndentation, 335, 20), "Object", tempObjectsList[i], typeof(GameObject), true);

                        layoutProp.yIndentation += 22;
                    }
                }
                //////////////////////// Adding Objects To Be Controlled END \\\\\\\\\\\\\\\\\\\\\\

                // Giving User Options To Choose From.
                layoutProp.yIndentation += 35;
                //EditorGUI.DrawRect(new Rect(0, layoutProp.yIndentation, focusedWindow.position.width, 30), Color.white);
                layoutProp.yIndentation += 10;
                layoutProp.selectedObject = EditorGUI.Popup(new Rect(10, layoutProp.yIndentation, 400, 30), "Select An Object", layoutProp.selectedObject, layoutProp.GetObjectsNames(eventHandler.eventObjects));


                // If The User Choose An Object From The List.
                if (layoutProp.selectedObject >= 0)
                {
                    //////////////////////// (EDIT) Controlling Objects \\\\\\\\\\\\\\\\\\\\\\

                    layoutProp.yIndentation += 28;
                    EditorGUI.DrawRect(new Rect(0, layoutProp.yIndentation, EditorWindow.focusedWindow.position.width, 30), Color.white);
                    layoutProp.yIndentation += 10;
                    layoutProp.selectedObjectEdit = EditorGUI.Popup(new Rect(10, layoutProp.yIndentation, 400, 30), "Select An Order", layoutProp.selectedObjectEdit, new string[] { "None", "Add A Button", "Add Animation States" });

                    layoutProp.yIndentation += 30;

                    switch (layoutProp.selectedObjectEdit)
                    {
                        case 0:

                            break;

                        case 1:
                            // User Wants TO Add Buttons.
                            if (tempButtonsList.Count < 1)
                                tempButtonsList.Add(null);

                            if (GUI.Button(new Rect(375, layoutProp.yIndentation, 75, 40), "Confirm", EditorStyles.boldLabel))
                            {
                                foreach (Button butt in tempButtonsList)
                                {
                                    if (butt != null)
                                    {
                                        eventHandler.eventObjects[layoutProp.selectedObject].AddButton(butt);
                                        layoutProp.selectedButtonFoldOut.Add(false);
                                    }
                                }

                                tempButtonsList.Clear();
                                layoutProp.selectedObjectEdit = 0;
                            }

                            for (int i = 0; i < tempButtonsList.Count; i++)
                            {

                                tempButtonsList[i] = (Button)EditorGUI.ObjectField(new Rect(25, layoutProp.yIndentation, 335, 20), "Button", tempButtonsList[i], typeof(Button), true);
                            }

                            break;
                        case 2:
                            // Adding Animation States
                            //layoutProp.yIndentation += 17;

                            if (eventHandler.eventObjects[layoutProp.selectedObject].obj.GetComponent<Animator>())
                            {
                                if (string.IsNullOrEmpty(tempStatesString))
                                    tempStatesString = "Seperated By '$$'";

                                tempStatesString = EditorGUI.TextField(new Rect(10, layoutProp.yIndentation, 350, 30), "Type Animation States", tempStatesString);

                                if (GUI.Button(new Rect(375, layoutProp.yIndentation, 75, 40), "Confirm", EditorStyles.boldLabel))
                                {

                                    foreach (string W in tempStatesString.Split(new char[] { '$' }))
                                        if (!string.IsNullOrEmpty(W))
                                            eventHandler.eventObjects[layoutProp.selectedObject].states.Add(W);

                                    tempStatesString = null;
                                    layoutProp.selectedObjectEdit = 0;

                                }

                            }
                            else
                                GUI.Label(new Rect(100, layoutProp.yIndentation, 400, 40), "!! Game Object Doesn't Consist Of An Animator Component");

                            break;

                    }

                    //////////////////////// (EDIT) Controlling Objects END \\\\\\\\\\\\\\\\\\\\\\



                    //////////////////////// (EDIT) Controlling Objects' Buttons \\\\\\\\\\\\\\\\\\\\\\
                    layoutProp.yIndentation += 30;
                    for (int i = 0; i < eventHandler.eventObjects[layoutProp.selectedObject].buttonsList.Count; i++)
                    {
                        layoutProp.yIndentation += 15;
                        layoutProp.selectedButtonFoldOut[i] = EditorGUI.Foldout(new Rect(10, layoutProp.yIndentation, 350, 30), layoutProp.selectedButtonFoldOut[i], eventHandler.eventObjects[layoutProp.selectedObject].buttonsList[i].button.name);

                        if (layoutProp.selectedButtonFoldOut[i])
                        {
                            // Finish How The states will be assigned to the button....
                        }
                    }



                    //////////////////////// (EDIT) Controlling Objects' Buttons END \\\\\\\\\\\\\\\\\\\\\\
                }




                Refresh();

                //if (GUI.Button(new Rect(200,0,50,30),"Save"))
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(eventHandler);
                }
            }
        }

        void Refresh()
        {
            layoutProp.scrollHeight = 100 + layoutProp.yIndentation;
            Repaint();
        }
    }


}
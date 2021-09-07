using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scene.MainMenu
{
    // Enums
    public enum States
    {
        DoNothing,

        Menu,

        Levels,

        Settings,

        RefBook,

        HideSettings,

        HideRefBook,

        LoadGame
    }

    public class MainMenuManager : BaseMonoBehaviour
    {

        // Variables
        [Space(25)]
        public GameObject samplePanelContentShape;
        
        
        // Scene Entry Point
        protected override void Awake()
        {
            base.Awake();

            currentSceneState = (int)States.DoNothing;

            Screen.orientation = ScreenOrientation.Portrait;

            LoadRefBookImagesToUI(samplePanelContentShape);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            switch (currentSceneState)
            {
                case (int)States.DoNothing:
                    break;

                case (int)States.Menu:
                    currentSceneState = (int)States.DoNothing;
                    break;

                case (int)States.Levels:
                    currentSceneState = (int)States.DoNothing;
                    break;

                case (int)States.Settings:
                    currentSceneState = (int)States.DoNothing;
                    break;

                case (int)States.RefBook:
                    currentSceneState = (int)States.DoNothing;
                    break;

                case (int)States.HideSettings:
                    currentSceneState = (int)States.DoNothing;
                    break;

                case (int)States.HideRefBook:
                    currentSceneState = (int)States.DoNothing;
                    break;

                case (int)States.LoadGame:
                    StartCoroutine( DelayGameStart() );
                    break;
            }
        }

        public void SetCurrentSceneState(States state)
        {
            currentSceneState = (int)state;
        }

        IEnumerator DelayGameStart()
        {
            yield return new WaitForSeconds(2.85f);

            LoadScene(GameProperties.GameScenes.Gameplay);
        }

        // Clickable \\
        

        public void _OnDifficulityChange(Dropdown dropdown)
        {
            userData.GameDifficulity = (GameProperties.DifficulityChoices)dropdown.value;

            string medium = "You Can Always Use The Reference Book To " + 
                            "Know Which Shape Represents Which Letter, And It's Set One Time.";
            string hard   = "The Connections Between Shapes And Letters " + 
                            "Is Randomised In Every Turn Of The Game.";

            if (userData.GameDifficulity == GameProperties.DifficulityChoices.Medium)
                dropdown.transform.parent.Find("DifficulityRef").GetComponent<Text>().text = medium;
            else
                dropdown.transform.parent.Find("DifficulityRef").GetComponent<Text>().text = hard;
        }
    }
}

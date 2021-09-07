using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Scene.MainMenu
{
    public class MainMenuAnimationsMapper : BaseAnimationsMapper
    {
        // Variables
        private MainMenuManager sceneManager { set; get; }
        private bool showSettings {set;get;}
        private bool showRefBook {set;get; }

        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (initialize)
            {
                base.OnStateEnter(animator, stateInfo, layerIndex);

                showSettings = false;

                showRefBook = false;

                sceneManager = FindObjectOfType<MainMenuManager>();

                foreach (var bba in sceneManager.ButtonsCollection)
                {
                    if (bba.Button != null && GetType().GetMethod(bba.Method) != null)
                    {
                        if (bba.Method == "_LevelChoice")
                        {
                            UnityAction<string> action = Delegate.CreateDelegate(typeof(UnityAction<string>), this, bba.Method) as UnityAction<string>;
                            string captureButton = bba.Button.name;
                            bba.Button.onClick.AddListener(() => action(captureButton));
                        }
                        else
                        {
                            UnityAction action = Delegate.CreateDelegate(typeof(UnityAction), this, bba.Method) as UnityAction;
                            bba.Button.onClick.AddListener(action);
                        }
                    }
                }


                sceneManager.ButtonsCollection.Clear();

                //MonoBehaviour.print("Main Menu Scene Animations Mapper is >> " + animator.gameObject.name);
            }
        }

        public void _Menu()
        {
            BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Button_Click, 1, 1.25f);

            sceneManager.SetCurrentSceneState(States.Menu);
        }

        public void _Levels()
        {
            BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Button_Click, 1, 1.25f);

            sceneManager.SetCurrentSceneState(States.Levels);
        }

        public void _Settings()
        {
            BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Button_Click, 1, 1.25f);

            showSettings = !showSettings;


            if (showSettings)
                sceneManager.SetCurrentSceneState(States.Settings);
            else
                sceneManager.SetCurrentSceneState(States.HideSettings);
        }

        public void _RefBook()
        {
            BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Button_Click, 1, 1.25f);

            showRefBook = !showRefBook;


            if (showRefBook)
                sceneManager.SetCurrentSceneState(States.RefBook);
            else
                sceneManager.SetCurrentSceneState(States.HideRefBook);
        }

        public void _LevelChoice(string lvlName)
        {
            BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Button_Click, 1, 1.25f);

            if (lvlName == GameProperties.GameLevels.Frog.ToString())
                sceneManager.GetUserData().ChosenLevel = GameProperties.GameLevels.Frog;
            else
            if (lvlName == GameProperties.GameLevels.Ant.ToString())
                sceneManager.GetUserData().ChosenLevel = GameProperties.GameLevels.Ant;
            else
            if (lvlName == GameProperties.GameLevels.Pigeon.ToString())
                sceneManager.GetUserData().ChosenLevel = GameProperties.GameLevels.Pigeon;
            else
                MonoBehaviour.print("Level Doesn't Exist" + lvlName);

            sceneManager.SetCurrentSceneState(States.LoadGame);
        }

        public void _ExitGame()
        {
            Application.Quit();
        }
    }
}
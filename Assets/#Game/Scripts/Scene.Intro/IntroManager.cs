using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Scene.Intro
{
     // Enums
     public enum States
    {
        DoNothing,

        Intro,
    }

    public class IntroManager : BaseMonoBehaviour
    {

        // Scene Entry Point
        protected override void Awake()
        {
            base.Awake();

            Screen.orientation = ScreenOrientation.Portrait;

            currentSceneState = (int)States.Intro;


            //SaveDataToJSON(ref userData, GameProperties.JSONFiles.UserData.ToString() + ".json", GameProperties.Folders.UserData.ToString());
        }

        protected override void Start()
        {
            audioMan.PlayActivityClip(AudioManager.Tracks.Intro, 0.75f);
        }

        public void LoadMainMenu()
        {
            //print("Scene Ended");

            LoadScene(GameProperties.GameScenes.MainMenu);
        }
    }
}
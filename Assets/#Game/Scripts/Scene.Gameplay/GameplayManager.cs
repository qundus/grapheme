using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;
using System.Reflection;
using UnityEngine.EventSystems;


// Determine 4 points to represent the borders in which inside of the shapes will be distributed.
// shapes will be modifid seperately and the GameManager.cs will control them.
// each shape will have its own Touchable/ShapeBehaviour.cs to bring the shape's menu up.
// Canvas will be propotional to each shape's center.
// Move the canvas functionalities to an independant class.
// Complete the swap functionality.

namespace Scene.Gameplay
{
    // Enum
    public enum States
    {
        DoNothing,

        PlayfieldFound,

        PlayfieldNotFound,

        StartWave,

        CountDown,

        PlayGame,

        WrongSolution,

        CorrectSolution,

        HideScoreBoard,

        EndWave
    }
    

    public class GameplayManager : BaseMonoBehaviour
    {

        // Variables
        [Space(25)]
        public GameObject template;
        public GameObject playfield;
        public GameObject waveWordPanel;
        public GameObject GameCanvas;
        //public GameObject WaveStartPanel { set; get; } 
        //public GameObject samplePanelContentShape;
        public Text timer;
        public Text debugText;


        // Classes Instances
        public ShapesAndLettersHandler SALH { private set; get; }

        // Machine States
        public bool playerIsInControl { private set; get; }


        // Scene Entry Point
        protected override void Awake()
        {
            base.Awake();

            Screen.orientation = ScreenOrientation.Landscape;

            currentSceneState = (int)States.DoNothing;

            template.SetActive(false);

            GameCanvas.SetActive(false);

            waveWordPanel.transform.GetChild(0).gameObject.SetActive(false);

            //WaveStartPanel = GameCanvas.transform.GetChild(0).gameObject;

            playerIsInControl = false;

            SALH = new ShapesAndLettersHandler();
            SALH.Initialize(playfield, template, ref userData, ref refBook, ButtonsCollection.Find(x => x.Button.name == "Release").Button);

            //LoadRefBookImagesToUI(samplePanelContentShape);

            //debugText.text = "Started";
        }

        // Game Loop
        protected override void Update()
        {
            base.Update();

            SALH.Run(playerIsInControl);
            
            if (SALH.DoAction == ShapesAndLettersHandler.Commands.DoNothig)
            {
                playerIsInControl = (currentSceneState == (int)States.DoNothing)? true: false;

                switch (currentSceneState)
                {
                    case (int)States.DoNothing :
                        if (!playfield.GetComponent<MeshCollider>().enabled)
                        {
                            currentSceneState = (int)States.PlayfieldNotFound;
                            GameCanvas.SetActive(false);
                        }
                        break;

                    case (int)States.PlayfieldFound:
                        currentSceneState = (int)States.DoNothing;
                        break;

                    case (int)States.PlayfieldNotFound: // Controlled by vuforia library's image target detection.
                        if (playfield.GetComponent<MeshCollider>().enabled)
                        {
                            currentSceneState = (int)States.PlayfieldFound;

                            if (SALH.GetActiveShapes().Count <= 0 )
                                GameCanvas.SetActive(true);
                            else
                                GameCanvas.SetActive(false);
                        }
                        break;

                    case (int)States.StartWave: //Controlled by clickable() & SALH
                        InitiateWave();
                        break;

                    case (int)States.CountDown:
                        DoCount();
                        break;

                    case (int)States.PlayGame:
                        GameOn();
                        break;

                    case (int)States.WrongSolution:
                        TryAgain();
                        break;

                    case (int)States.CorrectSolution:
                        // Show Score Animation Is Played By GameplayAnimationsMapper
                        //Congrats();
                        break;

                    case (int)States.HideScoreBoard:
                        currentSceneState = (int)States.PlayfieldFound;
                        break;

                    case (int)States.EndWave:
                        EndWaveNecessities();
                        break;
                }
            }

            
        }

        public static void SetCurrentSceneState(States state)
        {
            currentSceneState = (int)state;
        }        
        
        // StartWave ########
        void InitiateWave()
        {
            timer.text = string.Empty;

            

            currentSceneState = (int)States.CountDown;
        }

        // CountDown ########
        void DoCount()
        {

            if (timer.text == string.Empty)
            {
                // Ready Sound
                BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Ready);

                switch (userData.GameDifficulity)
                {
                    case GameProperties.DifficulityChoices.Medium:
                        //StartCoroutine(MediumCD(SALH.GetWaveWord().Length, 2));
                        StartCoroutine(MediumCD(3, 2));
                        break;

                    case GameProperties.DifficulityChoices.Hard:
                        StartCoroutine(HardCD(SALH.GetWaveWord().Length, 2));
                        break;
                }
            }
        }

        IEnumerator MediumCD(int seconds, float readyFlag = 1)
        {
            timer.text = seconds.ToString();

            yield return new WaitForSecondsRealtime(1.5f * readyFlag);

            seconds--;

            if (seconds > 0)
            {
                // Count Down Sound
                audioMan.PlayActivityClip(AudioManager.Tracks.Count_Down_Beep);

                StartCoroutine(MediumCD(seconds));
            }
            else
            {
                // Final Count Down
                audioMan.PlayActivityClip(AudioManager.Tracks.Count_Down_Final);

                currentSceneState = (int)States.PlayGame;
            }
        }

        IEnumerator HardCD(int seconds, float readyFlag = 1)
        {
            timer.text = seconds.ToString();

            yield return new WaitForSecondsRealtime(1.5f * readyFlag);

            seconds--;

            if (seconds > 0)
            {
                // Count Down Sound
                audioMan.PlayActivityClip(AudioManager.Tracks.Count_Down_Beep);

                StartCoroutine(HardCD(seconds));
            }
            else
            {
                // Final Count Down
                audioMan.PlayActivityClip(AudioManager.Tracks.Count_Down_Final);

                if (userData.GameDifficulity == GameProperties.DifficulityChoices.Hard)
                    foreach (GameObject go in SALH.GetActiveShapes())
                        go.transform.SetParent(playfield.transform);

                currentSceneState = (int)States.PlayGame;
            }

        }

        // PlayGame ########
        void GameOn()
        {
            GameObject sampleLetter =  waveWordPanel.transform.GetChild(0).gameObject;

            StartCoroutine( ShowWordLetters(sampleLetter) );

            currentSceneState = (int)States.DoNothing;
        }

        IEnumerator ShowWordLetters(GameObject sample, int i = 0)
        {
            yield return new WaitForSecondsRealtime(0.5f);

            if (i < SALH.GetWaveWord().Length)
            {
                GameObject go = Instantiate(sample, sample.transform.parent.transform) as GameObject;

                go.name = SALH.GetActiveRefBook()[i].Name;

                switch (userData.GameDifficulity)
                {
                    case GameProperties.DifficulityChoices.Medium:
                        go.GetComponentInChildren<Text>().text = SALH.GetActiveRefBook()[i].FixedLetter.ToString();
                        break;

                    case GameProperties.DifficulityChoices.Hard:
                        go.GetComponentInChildren<Text>().text = SALH.GetActiveRefBook()[i].RandomeLetter.ToString();
                        break;
                }

                go.SetActive(true);

                i++;

                StartCoroutine(ShowWordLetters(sample, i));
            }
        }

        // WrongSolution ########
        void TryAgain()
        {
            // Wrong Answer Sound
            audioMan.PlayActivityClip(AudioManager.Tracks.Wrong_Answer);

            debugText.text = "TryAgain";

            currentSceneState = (int)States.DoNothing;
        }

        // CorrectSolution ########
        // No Special Cases

        // EndWave #######
        void EndWaveNecessities()
        {
            for (int i = 1; i < waveWordPanel.transform.childCount; i++)
                Destroy(waveWordPanel.transform.GetChild(i).gameObject);

            GameCanvas.SetActive(true);

            //WaveStartPanel.SetActive(true);

            if (SALH.waveWordsolved)
                currentSceneState = (int)States.HideScoreBoard;
            else
                currentSceneState = (int)States.PlayfieldFound;


            timer.text = "0";
        }
    }
}
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace Scene.Gameplay
{
    /////////////// Scene Specific Animations Mapper \\\\\\\\\\\\
    public class GameplayAnimationsMapper : BaseAnimationsMapper
    {
        // Variables
        private GameplayManager sceneManager { set; get; }
        private GameObject ScorePanel { set; get; }
        private int waveNumber { set; get; }

        // Machine States
        private bool showMenu { set; get; }
        private bool showScore { set; get; }


        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (initialize)
            {
                base.OnStateEnter(animator, stateInfo, layerIndex);

                waveNumber = 0;

                showMenu = true;

                showScore = true;

                sceneManager = FindObjectOfType<GameplayManager>();

                ScorePanel = animator.gameObject.transform.Find("ScoreP").gameObject;

                foreach (var bba in sceneManager.ButtonsCollection)
                    if (bba.Button != null && GetType().GetMethod(bba.Method) != null)
                    {
                        if (bba.Method == "_ShowHideMenu")
                        {
                            UnityAction<Animator> action = Delegate.CreateDelegate(typeof(UnityAction<Animator>), this, bba.Method) as UnityAction<Animator>;

                            Animator captureAnimator = animator;

                            bba.Button.onClick.AddListener(() => action(captureAnimator));
                        }
                        else
                        {
                            UnityAction action = Delegate.CreateDelegate(typeof(UnityAction), this, bba.Method) as UnityAction;
                            bba.Button.onClick.AddListener(action);
                        }
                    }

                sceneManager.ButtonsCollection.Clear();

                //MonoBehaviour.print("Gameplay Scene Animations Mapper is >> " + animator.gameObject.name);
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            ScoreMenu();
        }

        void ScoreMenu()
        {
            // Correct Answer Scenario
            if (BaseMonoBehaviour.GetCurrentSceneState() == (int)Gameplay.States.CorrectSolution && showScore)
            {
                BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Correct_Answer);

                // Show Wave Number First
                waveNumber++;
                ScorePanel.GetComponentsInChildren<Text>()[0].text = "Wave  " + waveNumber;

                // Show How Many Words Have Been Solved Second
                int count = sceneManager.GetUserData().SolvedWords.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
                ScorePanel.GetComponentsInChildren<Text>()[1].text = "Solved Words   " + count;

                // Show Star Level Last (Random For Now)
                count = UnityEngine.Random.Range(1, 3);
                GameObject temp = ScorePanel.transform.Find("Stars").gameObject;

                // Hide All Stars
                for (int i = 0; i < 3; i++)
                    temp.transform.GetChild(i).gameObject.SetActive(false);

                // Show Some Stars
                for (int i = 0; i < count; i++)
                    temp.transform.GetChild(i).gameObject.SetActive(true);

                showScore = false;
            }

            
        }

        public void _ShowHideMenu(Animator anim)
        {
            BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Button_Click, 1, 1.25f);

            if (sceneManager.SALH.releaseObjectButton.activeSelf)
            {
                if (!showMenu)
                {
                    anim.Play("Hide Menu");

                    showMenu = !showMenu;
                }
            }
            else
            {
                if (showMenu)
                    anim.Play("Show Menu");
                else
                    anim.Play("Hide Menu");

                showMenu = !showMenu;
            }
        }

        public void _StartWave()
        {
            BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Button_Click, 1, 1.25f);

            if (sceneManager.playerIsInControl && sceneManager.SALH.GetActiveShapes().Count <= 0)
                sceneManager.SALH.DoAction = ShapesAndLettersHandler.Commands.BuildWave;
        }

        public void _ShowHint()
        {

            BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Button_Click, 1, 1.25f);

            if (sceneManager.playerIsInControl && !sceneManager.SALH.releaseObjectButton.activeSelf)
                sceneManager.SALH.DoAction = ShapesAndLettersHandler.Commands.Hint;
        }

        public void _SolveWaveWord()
        {
            BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Button_Click, 1, 1.25f);

            if (sceneManager.playerIsInControl && !sceneManager.SALH.releaseObjectButton.activeSelf)
                sceneManager.SALH.DoAction = ShapesAndLettersHandler.Commands.CheckUserAnswer;
                //sceneManager.SALH.DoAction = ShapesAndLettersHandler.Commands.Solved;
        }

        public void _SkipWave()
        {
            if (sceneManager.playerIsInControl && !sceneManager.SALH.releaseObjectButton.activeSelf)
            {
                // Play Fial Beat Count For Skipping The Word
                BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Count_Down_Final);
                //BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Button_Click, 1, 1.25f);

                sceneManager.SALH.DoAction = ShapesAndLettersHandler.Commands.Skipped;
            }
        }

        public void _MainMenu()
        {
            BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Button_Click, 1, 1.25f);

            sceneManager.LoadScene(GameProperties.GameScenes.MainMenu);
        }

        public void _Music()
        {
            BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Button_Click, 1, 1.25f);

            //if (sceneManager.playerIsInControl)
               // ;
        }

        public void _HideScore()
        {
            BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Button_Click, 1, 1.25f);

            if (sceneManager.playerIsInControl)
            {
                sceneManager.SALH.DoAction = ShapesAndLettersHandler.Commands.EndWave;
                showScore = true;
            }
        }

        public void _ReleaseObjectFromControl()
        {
            BaseMonoBehaviour.audioMan.PlayActivityClip(AudioManager.Tracks.Button_Click, 1, 1.25f);

            sceneManager.SALH.DoAction = ShapesAndLettersHandler.Commands.ReleaseObjectUnderControl;
        }
    }
}
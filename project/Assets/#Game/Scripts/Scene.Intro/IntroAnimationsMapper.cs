using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace Scene.Intro
{
    public class IntroAnimationsMapper : BaseAnimationsMapper
    {
        // Variables
        private IntroManager sceneManager { set; get; }
        public float delay;
        public BaseStateActivity introState;


        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (initialize)
            {
                base.OnStateEnter(animator,stateInfo, layerIndex);

                BaseMonoBehaviour.coroutines.Add(UpdateAnimatorParameter(delay, animator));

                sceneManager = FindObjectOfType<IntroManager>();
            }
        }

        protected IEnumerator UpdateAnimatorParameter(float animationDuration, Animator animator)
        {
            yield return new WaitForSeconds(animationDuration);

            if (introState.StateName != string.Empty)
            {
                animator.Play(introState.StateName);

                introState.StateName = string.Empty;

                BaseMonoBehaviour.coroutines.Add(UpdateAnimatorParameter(introState.Duration, animator));
            }
            else
                sceneManager.LoadMainMenu();
        }
    }
}
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
using Scene.Gameplay;
using Scene.MainMenu;


namespace Scene
{
    [ExecuteInEditMode]
    //[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class BaseAnimationsMapper : StateMachineBehaviour
    {
        // Variables
        private List<int> StatesMonitor { set; get; }
        public GameProperties.GameScenes sceneReference;
        [Tooltip("To Be Used Globally By Any Animator That Needs Scene.States ")]
        [SerializeField]
        private List<BaseStateActivity> StateCollection;


        // Machine State
        protected bool initialize = true;
        private bool animationInPlay { set; get; }


        // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (initialize)
            {
                StatesMonitor = new List<int>();

                animationInPlay = false;

                initialize = false;
            }
        }

        // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            if (BaseMonoBehaviour.GetCurrentSceneState() != 0)
            {

                if (StatesMonitor.Count <= 0)
                    StatesMonitor.Add(BaseMonoBehaviour.GetCurrentSceneState());
                else
                if (StatesMonitor[StatesMonitor.Count - 1] != BaseMonoBehaviour.GetCurrentSceneState())
                {
                    // first we remove the old sceneState from queue..
                    StatesMonitor.RemoveAt(0);

                    StatesMonitor.Add(BaseMonoBehaviour.GetCurrentSceneState());

                    animationInPlay = false;
                }

                //MonoBehaviour.print("Scene State is --> " + StatesMonitor[0] + " Count >> " + StatesMonitor.Count);
            }

            // When the lock is released
            if (!animationInPlay && StatesMonitor.Count > 0)
            {
                BaseStateActivity temp = null;

                // For the one state that played while in the previous game state StatesMonitor[0]..
                foreach (var sa in StateCollection.Where(sa => ((int)sa.PlayOnSceneState == StatesMonitor[0])))
                    temp = sa;

                // If it exists.
                if (temp != null)
                    BaseMonoBehaviour.coroutines.Add(UpdateAnimatorParameter(temp, animator));

            }
        }

        protected virtual IEnumerator UpdateAnimatorParameter(BaseStateActivity state, Animator animator)
        {
            animationInPlay = true;

            animator.Play(state.StateName);

            yield return new WaitForSeconds(Mathf.Abs(state.Duration));

        }

    }
}
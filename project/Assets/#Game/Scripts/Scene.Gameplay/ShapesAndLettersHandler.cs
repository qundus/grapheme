using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace Scene.Gameplay
{
    // Class's job is to provide all functionalities to mix shapes and letters together along with how 
    // they're represented, not to be mixed with Gameplay.cs which is the scene manager that controlls turn
    // of events and validates correct answers.
    public class ShapesAndLettersHandler
    {
        // Enums
        public enum Commands
        {
            DoNothig,

            BuildWave,

            ReleaseObjectUnderControl,

            Hint,

            CheckUserAnswer,

            Solved,

            Skipped,
            
            EndWave
        }
        
        // Variables
        // The Vuforia ImageTarget after adding a MeshCollider to it.
        private GameObject playfield { set; get; }
        // The Template prefab that has all 3DModels.
        private GameObject template { set; get; }
        public GameObject releaseObjectButton { set; get; }
        // Gets filled with objects that are chosen to be on the playField.
        private List<GameObject> activeShapesList { set; get; }
        private List<ShapeData> activeRefBookList { set; get; }

        private Vector3 touchStartPoint { set; get; }
        private Vector3 touchEndPoint { set; get; }
        private Bounds playfieldBounds { set; get; }

        private string waveWord { set; get; }
        private int currentSceneState { set; get;}
        


        // Classes Instances
        private UserData userdata { set; get; }
        private ShapeBehaviour objectGettingControlled { set; get; }
        private List<ShapeData> refBook { set; get; }


        // Machine States
        public Commands DoAction { set; get; }
        public bool userIsControllingObject { set; get; }
        public bool waveWordsolved { private set; get; }


        /// <param name="Playfield"></param>
        /// <param name="Template"></param>
        /// <param name="Userdata"></param>
        /// <param name="RefBook"></param>
        public void Initialize(GameObject Playfield, GameObject Template, ref UserData Userdata, ref List<ShapeData> RefBook, Button ReleaseButton)
        {
            playfield = Playfield;

            template = Template;

            userdata = Userdata;

            refBook = RefBook;

            activeShapesList = new List<GameObject>();

            activeRefBookList= new List<ShapeData>();

            userIsControllingObject = false;

            waveWordsolved = false;

            playfieldBounds = playfield.GetComponentsInChildren<MeshCollider>()[1].bounds;

            releaseObjectButton = ReleaseButton.gameObject;

            releaseObjectButton.SetActive(false);

            DoAction = Commands.DoNothig;
        }

        public void Run(bool PlayerIsInControl)
        {
            
            switch (DoAction)
            {
                case Commands.DoNothig:
                    if (PlayerIsInControl)
                    {
                        DebugHandleShapesMovement();
                        HandleShapesMovement();
                    }
                    break;

                case Commands.BuildWave:
                    BuildWaveNecessities();
                    GameplayManager.SetCurrentSceneState(States.StartWave);// We presume that solution provided is wrong first.
                    break;

               
                case Commands.CheckUserAnswer:
                    GameplayManager.SetCurrentSceneState(States.WrongSolution);// We presume that solution provided is wrong first.
                    CheckShapesSpelling();
                    break;

                case Commands.ReleaseObjectUnderControl:
                    ReleaseObjectUnderControl();
                    break;

                case Commands.Hint:
                    ChooseAnObjectForAHint();
                    break;

                case Commands.Solved:
                    Solved();
                    GameplayManager.SetCurrentSceneState(States.CorrectSolution);
                    break;

                case Commands.Skipped:
                    Skipped();
                    //GameplayManager.SetCurrentSceneState(States.ChangeWord);
                    break;

                case Commands.EndWave:
                    EndWaveNecessities();
                    GameplayManager.SetCurrentSceneState(States.EndWave);
                    break;
            }

        }

        public string GetWaveWord()
        {
            return waveWord;
        }

        public List<ShapeData> GetActiveRefBook()
        {
            return activeRefBookList;
        }

        public List<GameObject> GetActiveShapes()
        {
            return activeShapesList;
        }

        void BuildWaveNecessities()
        {
            PickWaveWord();


            PickWaveShapes();


            DoAction = Commands.DoNothig;
        }

        void PickWaveShapes()
        {
            switch (userdata.GameDifficulity)
            {
                case GameProperties.DifficulityChoices.Medium:
                    ActivateMediumDifficultyShapes();
                    break;

                case GameProperties.DifficulityChoices.Hard:
                    ActivateHardDifficultyShapes();
                    break;
            }
        }

        void ActivateMediumDifficultyShapes()
        {
            foreach (char letter in waveWord)
                foreach (ShapeData sd in refBook)
                    if (sd.FixedLetter.Equals(letter))
                    {
                        activeRefBookList.Add(sd);

                        activeShapesList.Add(template.transform.Find(sd.Name).gameObject);

                        template.transform.Find(sd.Name).SetParent(playfield.transform);

                        //MonoBehaviour.print(sd.Name);

                        break;
                    }
        }

        void ActivateHardDifficultyShapes()
        {
            int rand;

            foreach (char letter in waveWord)
            {
                rand = UnityEngine.Random.Range(0, template.transform.childCount);
                foreach (ShapeData sd in refBook)
                {
                    if (sd.Name.Equals(template.transform.GetChild(rand).name))
                    {
                        template.transform.GetChild(rand).GetComponentInChildren<Text>().text = letter.ToString();

                        sd.RandomeLetter = letter;

                        activeRefBookList.Add(sd);

                        activeShapesList.Add(template.transform.Find(sd.Name).gameObject);

                        template.transform.Find(sd.Name).SetParent(playfield.GetComponentInChildren<Canvas>().transform);

                        break;
                    }
                }
            }
        }

        void PickWaveWord()
        {
            TextAsset binarydata = null;
            string path = "Database/Words/";

            switch (userdata.ChosenLevel)
            {

                case GameProperties.GameLevels.Frog:
                    binarydata = Resources.Load<TextAsset>(path + "EasyWords");
                    break;

                case GameProperties.GameLevels.Ant:
                    binarydata = Resources.Load<TextAsset>(path + "MediumWords");
                    break;

                case GameProperties.GameLevels.Pigeon:
                    binarydata = Resources.Load<TextAsset>(path + "HardWords");
                    break;
            }

            string[] words = binarydata.text.Split(new char[] { '\r', '\n', ' ', '.', '-' }, StringSplitOptions.RemoveEmptyEntries);

            waveWord = words[UnityEngine.Random.Range(0, words.Length)].ToUpper();

            if (Input.touchCount > 1)
                waveWord = "CAT";

            if (!string.IsNullOrEmpty(userdata.SolvedWords) && userdata.SolvedWords.Contains(waveWord))
                PickWaveWord();

            //BaseMonoBehaviour.print(waveWord);
        }

        void HandleShapesMovement()
        {
            //MonoBehaviour.print("User Is Moving Objects");

            if (Input.touchCount > 0)
            {
                touchStartPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                touchEndPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 1000));

                // Setting A New Object TO Be Controlled Only If User Is Free Of Control.
                if (!userIsControllingObject)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(touchStartPoint, touchEndPoint, out hit, 1000, 1 << LayerMask.NameToLayer("RefBook")))
                    {
                        if (hit.collider != null)
                        {
                            // We found an object to control.
                            foreach (GameObject go in activeShapesList)
                                if (hit.collider.name.Equals(go.name))
                                {
                                    objectGettingControlled = go.AddComponent<ShapeBehaviour>();

                                    objectGettingControlled.Initialize(playfield.GetComponentsInChildren<MeshCollider>()[1]);

                                    releaseObjectButton.SetActive(true);

                                    userIsControllingObject = true;

                                    //MonoBehaviour.print("Started Moving");
                                }
                        }
                    }
                }
            }
        }

        void DebugHandleShapesMovement()
        {
#if UNITY_STANDALONE
            MonoBehaviour.print("Debugging");

            touchStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchEndPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + ( Vector3.forward * 1000) );

            // Setting A New Object TO Be Controlled Only If User Is Free Of Control.
            if (!userIsControllingObject)
            {
                RaycastHit hit;
                if (Physics.Raycast(touchStartPoint, touchEndPoint, out hit))
                {
                    if (hit.collider != null)
                    {
                         foreach (GameObject go in activeShapesList)
                            if (hit.collider.name.Equals(go.name))// We found an object to control.
                            //if (hit.collider.name.Equals("Boat"))// We found an object to control.
                            {
                                //GameObject go = hit.collider.gameObject;

                                objectGettingControlled = go.AddComponent<ShapeBehaviour>();

                                objectGettingControlled.Initialize(playfield.GetComponentsInChildren<MeshCollider>()[1]);

                                releaseObjectButton.SetActive(true);

                                userIsControllingObject = true;

                                MonoBehaviour.print("Started Moving");
                        }
                    }
                }
            }
#endif
        }

        void CheckShapesSpelling()
        {
            //playfieldBounds = playfield.GetComponent<MeshCollider>().bounds;
            Vector3 start = Vector3.zero;
            RaycastHit hit;
            int letterPosition = 0;


            for (float z = playfieldBounds.max.z; z > playfieldBounds.min.z && letterPosition >= 0; z -= 5)
            {
                for (float x = playfieldBounds.min.x; x < playfieldBounds.max.x && letterPosition >= 0; x += 5)
                {
                    start = new Vector3(x, playfieldBounds.center.y - 100, z);
                    //UnityEngine.Gizmos.DrawRay(start, Vector3.up);

                    if (Physics.Raycast(start, Vector3.up, out hit, 300, 1 << LayerMask.NameToLayer("RefBook")))
                    {
                        if (hit.collider != null)
                        {
                            //MonoBehaviour.print(hit.collider.name);
                            foreach (ShapeData sd in activeRefBookList)
                            { 
                                if (sd.Name.Equals(hit.collider.name))
                                {
                                    switch (userdata.GameDifficulity)
                                    {
                                        case GameProperties.DifficulityChoices.Medium:
                                            if (sd.FixedLetter.Equals(waveWord[letterPosition]))
                                                activeShapesList[letterPosition].GetComponent<BoxCollider>().enabled = false;
                                            break;

                                        case GameProperties.DifficulityChoices.Hard:
                                            if (sd.RandomeLetter.Equals(letterPosition))
                                                activeShapesList[letterPosition].GetComponent<BoxCollider>().enabled = false;
                                            break;
                                    }

                                    if (activeShapesList[letterPosition].GetComponent<BoxCollider>().enabled)
                                        letterPosition = -1;
                                    else
                                        letterPosition++;

                                    break;
                                }
                            }
                        }
                    }
                }
            }


            foreach (GameObject go in activeShapesList)
                go.GetComponent<BoxCollider>().enabled = true;


            // Solved
            if (letterPosition >= activeShapesList.Count && activeShapesList.Count > 0)
                DoAction = Commands.Solved;
            else
                DoAction = Commands.DoNothig;

            //MonoBehaviour.print("Command Is = " + DoAction.ToString());
        }

        void Solved()
        {
            if (waveWordsolved == false)
            {
                userdata.SolvedWords += waveWord + " ";

                waveWordsolved = true;
            }

            // End Wave Command Is Initiated By A Button
        }

        void Skipped()
        {
            DoAction = Commands.EndWave;
        }

        void ReleaseObjectUnderControl()
        {
            userIsControllingObject = false;

            releaseObjectButton.SetActive(false);

            UnityEngine.Object.Destroy(objectGettingControlled);

            DoAction = Commands.DoNothig;
        }

        void ChooseAnObjectForAHint()
        {
            
            int rand = UnityEngine.Random.Range(0, activeShapesList.Count);

            switch (userdata.GameDifficulity)
            {
                case GameProperties.DifficulityChoices.Medium:
                    BaseMonoBehaviour.coroutines.Add(LauncHintObject(activeShapesList[rand], activeRefBookList[rand].FixedLetter));
                    break;

                case GameProperties.DifficulityChoices.Hard:
                    BaseMonoBehaviour.coroutines.Add(LauncHintObject(activeShapesList[rand], activeRefBookList[rand].RandomeLetter));
                    break;
            }
            

            DoAction = Commands.DoNothig;
        }

        IEnumerator LauncHintObject(GameObject Shape, char Letter)
        {
            Shape.GetComponentInChildren<Text>().text = Letter.ToString();

            Shape.transform.SetParent(playfield.GetComponentInChildren<Canvas>().transform);

            yield return new WaitForSeconds(3.5f);

            Shape.transform.SetParent(playfield.GetComponentInChildren<Canvas>().transform.parent);
        }

        void EndWaveNecessities()
        {
            foreach (GameObject go in activeShapesList)
                go.transform.SetParent(template.transform);

            activeShapesList.Clear();
            activeRefBookList.Clear();
            waveWord = string.Empty;

            // Determining if user has finished the level
            int wordsCount = userdata.SolvedWords.Split(new char[] { ' ' , '\r' , '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

            if (waveWordsolved)
                switch (userdata.ChosenLevel)
                {
                    case GameProperties.GameLevels.Frog:
                        if (wordsCount >= (int)GameProperties.WavesToFinishLevels.Frog)
                            userdata.FinishedLevels.Add(userdata.ChosenLevel);
                        break;

                    case GameProperties.GameLevels.Ant:
                        if (wordsCount >= (int)GameProperties.WavesToFinishLevels.Ant)
                            userdata.FinishedLevels.Add(userdata.ChosenLevel);
                        break;

                    case GameProperties.GameLevels.Pigeon:
                        if (wordsCount >= (int)GameProperties.WavesToFinishLevels.Pigeon)
                            userdata.FinishedLevels.Add(userdata.ChosenLevel);
                        break;
                }

            DoAction = Commands.DoNothig;
        }
    }
}
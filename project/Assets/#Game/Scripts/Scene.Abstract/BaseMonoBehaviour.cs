using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using UnityEngine.UI;

namespace Scene
{
    public class BaseMonoBehaviour : MonoBehaviour
    {
        
        // STATICS
        public static List<IEnumerator> coroutines { set; get; }

        // Variables
        [HideInInspector]
        protected static int currentSceneState { set; get; }
        protected string path { set; get; }
        public List<BaseButtonActivity> ButtonsCollection;
        public static AudioManager audioMan { set; get; }


        // Classes Instances
        protected UserData userData;
        protected List<ShapeData> refBook;

        protected virtual void Awake()
        {
            // Use JSON file to store all you need.

            LoadDataFromJSON(ref userData, GameProperties.JSONFiles.UserData.ToString() + ".json", GameProperties.Folders.UserData.ToString());

            LoadDataFromJSON(ref refBook, GameProperties.JSONFiles.RefBook.ToString() + ".json", GameProperties.Folders.RefBook.ToString(), true);

            currentSceneState = 0;

            coroutines = new List<IEnumerator>();

            ButtonsCollection.RemoveAll(x => x.Button == null);
        }

        protected virtual void Start()
        {
            CheckCoroutineRequests();
        }

        protected virtual void Update()
        {
            CheckCoroutineRequests();
        }

        protected virtual void FixedUpdate()
        {
            CheckCoroutineRequests();
        }

        public static int GetCurrentSceneState()
        {
            return currentSceneState;
        }

        public UserData GetUserData()
        {
            return userData;
        }

        protected void SaveDataToJSON<T>(ref T obj, string fileName, string dir = "")
        {
            try
            {
                dir = "/" + dir;
                path = Application.persistentDataPath + dir;

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path += "/";

                if (!File.Exists(path + fileName))
                    File.Create(path + fileName).Dispose();

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;
                settings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
                settings.TypeNameHandling = TypeNameHandling.All;
                settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;

                string data = JsonConvert.SerializeObject(obj, settings);

                File.WriteAllText(path + fileName, data, System.Text.Encoding.ASCII);
            }
            catch (Exception e)
            {
                print("Exception " + e.Message);
            }
        }

        protected void LoadDataFromJSON<T>(ref T obj, string fileName, string dir, bool loadFromDatabaseFolder = false) where T : new()
        {
            try
            {
                dir = "/" + dir;

                if (loadFromDatabaseFolder)
                {
                    path = "Database" + dir + "/";

                    if (fileName.Contains("."))
                        fileName = fileName.Substring(0, fileName.IndexOf('.'));

                    TextAsset binaryData = Resources.Load<TextAsset>(path + fileName);

                    if (binaryData != null)
                        obj = JsonConvert.DeserializeObject<T>(binaryData.text);
                }
                else
                {
                    path = Application.persistentDataPath + dir + "/";

                    if (File.Exists(path + fileName))
                        obj = JsonConvert.DeserializeObject<T>(File.ReadAllText(path + fileName));
                }

                if (obj == null)
                    obj = new T();
            }
            catch (Exception e)
            {
                print("Exception " + e.Message);
            }
        }

        protected void LoadRefBookImages(string folderInDatabase)
        {
            path = "Database/" + folderInDatabase + "/";

            TextAsset binaryData;

            foreach (ShapeData sd in refBook)
            {
                binaryData = Resources.Load<TextAsset>(path + sd.Name);

                if (binaryData != null)
                    sd.ImageBytes = binaryData.bytes;
            }
        }

        protected void LoadRefBookImagesToUI(GameObject samplePanelShape)
        {
            Texture2D tex;

            LoadRefBookImages(GameProperties.Folders.RefBook.ToString() + "/" + GameProperties.Folders.FL_Imagebytes.ToString());

            foreach (ShapeData sd in refBook)
            {
                GameObject go = (GameObject)Instantiate(samplePanelShape, samplePanelShape.transform.parent.transform);

                go.name = sd.Name;

                tex = new Texture2D(100, 100); //NOW INSIDE THE FOR LOOP

                tex.LoadImage(sd.ImageBytes);

                tex.name = sd.Name;

                go.GetComponent<RawImage>().texture = tex;

                //go.GetComponent<Renderer>().material.mainTexture = tempTex;

                go.GetComponentInChildren<Text>().text = sd.FixedLetter.ToString();
            }

            samplePanelShape.SetActive(false);
        }

        protected void CheckCoroutineRequests()
        {
            for (int i = 0; i < coroutines.Count; i++)
            {
                StartCoroutine(coroutines[i]);
                coroutines.RemoveAt(i);
            }
        }

        public void LoadScene(GameProperties.GameScenes gs)
        {
            SaveDataToJSON(ref userData, GameProperties.JSONFiles.UserData.ToString() + ".json", GameProperties.Folders.UserData.ToString());

            SceneManager.LoadScene((int)gs);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net;
using Newtonsoft.Json;


namespace Scene
{
    //Used one time to generate reference book.
    public class GenerateReferenceBook : BaseMonoBehaviour
    {
        // Enums
        public enum Commands
        {
            FullGeneration,
            Only_FL_ImagesBytes,
            Only_RL_ImagesBytes
        }

        public enum BytesEncoding
        {
            JPEG,
            PNG
        }

        // Varibles
        public Commands action;
        public BytesEncoding encoding;
        public List<Texture2D> imagesList;

        protected override void Awake()
        {
            base.Awake();

            if (refBook.Count >= 0)
                print("Ref Book Already Exists in Database Foder");

            switch (action)
            {
                case Commands.FullGeneration:
                    Generate();
                    GenerateShapesImages(GameProperties.Folders.RefBook.ToString() + "/" + GameProperties.Folders.FL_Imagebytes.ToString());
                    break;

                case Commands.Only_FL_ImagesBytes:
                    GenerateShapesImages(GameProperties.Folders.RefBook.ToString() + "/" + GameProperties.Folders.FL_Imagebytes.ToString());
                    break;

                case Commands.Only_RL_ImagesBytes:
                    GenerateShapesImages(GameProperties.Folders.RefBook.ToString() + "/" + GameProperties.Folders.RL_Imagebytes.ToString());
                    break;
            }
        }

        void Generate()
        {
            refBook = new List<ShapeData>();
            string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            List<char> lLetters = Letters.ToList();
            GameObject go;

            //foreach (GameObject go in base.GetComponentsInChildren<GameObject>()) // DOesn't work..


            for (int i = 0; i < transform.childCount; i++)
            {
                go = transform.GetChild(i).gameObject;
                ShapeData sd = new ShapeData();

                // Get Name.
                sd.Name = go.name;

                // Get FixedLetter.
                int r = UnityEngine.Random.Range(0, lLetters.Count);
                sd.FixedLetter = lLetters[r];
                sd.RandomeLetter = lLetters[r];
                lLetters.RemoveAt(r);

                // Add Object To List.
                refBook.Add(sd);
            }

            print("RefBook Is Generated");

            SaveDataToJSON(ref refBook, GameProperties.JSONFiles.RefBook.ToString() + ".json", GameProperties.Folders.RefBook.ToString());
        }

        void GenerateShapesImages(string folderOfImageBytes)
        {
            path = Application.persistentDataPath + "/" + folderOfImageBytes + "/";



            if (imagesList.Count <= 0)
                print("No Images To Populate Bytes Files From");
            {
                string fileName;

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                foreach (Texture2D i in imagesList)
                {
                    fileName = i.name + ".bytes";

                    if (!File.Exists(path + fileName))
                        File.Create(path + fileName).Dispose();

                    if (encoding == BytesEncoding.JPEG)
                        File.WriteAllBytes(path + fileName, i.EncodeToJPG());

                    if (encoding == BytesEncoding.PNG)
                        File.WriteAllBytes(path + fileName, i.EncodeToPNG());
                }

                print("Image/s Bytes Generated");

                imagesList.Clear();
            }

        }
    }

}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Scene
{
    public static class GenerateWords
    {
        /// <summary>
        /// Stand alone function to generate words files for the game from file "Resources/Database/Words/Engmix.txt".
        /// determine where the files are going to be saved and filtered.
        /// <param name="minLetters"> what it says</param>
        /// <param name="maxLetters"> what it says</param>
        /// </summary>
        public static void Work(string path, int minLetters, int maxLetters, int letterRedundancy = 1)
        {
            TextAsset bd = Resources.Load<TextAsset>("Database/Words/Engmix");

            string[] words = bd.text.Split(new char[] { '\r', '\n', ' ', '.', '-' }, System.StringSplitOptions.RemoveEmptyEntries);

            List<string> extractedWords = words.Where(w => (w.Length >= minLetters && w.Length <= maxLetters)).ToList();

            List<string> filteredWords = new List<string>();

            int count;

            foreach (string word in extractedWords)
            {
                bool addWord = true;

                for (char c = 'a'; c <= 'z'; c++)
                {
                    count = word.Count(l => (l == c));

                    // Delete The Word If Redundant Letters Are count.
                    if (count > letterRedundancy)
                        addWord = false;
                }

                if (addWord)
                    filteredWords.Add(word);
            }

            path += "/Words " + minLetters + " - " + maxLetters + ".txt";

            if (!File.Exists(path))
                File.Create(path).Dispose();

            File.WriteAllLines(path, filteredWords.ToArray());
        }
    }
}
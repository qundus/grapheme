using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameProperties
{
    public enum DifficulityChoices
    {
        Medium,
        Hard
    }

    public enum GameScenes
    {
        Intro = 0,
        MainMenu = 1,
        Gameplay = 2
    }

    public enum GameLevels
    {
        Frog,
        Ant,
        Pigeon
    }

    public enum JSONFiles
    {
        UserData,
        RefBook
    }

    public enum Folders
    {
        RefBook,
        UserData,
        FL_Imagebytes,
        RL_Imagebytes,
        Words
    }

    public enum WavesToFinishLevels
    {
        Frog = 20,
        Ant = 30,
        Pigeon = 40
    }
}


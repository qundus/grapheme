using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

// For Now User Name And Personal Info Will Be Omited Until The Game Has Multiplayer.
[Serializable]
public class UserData
{
    public GameProperties.DifficulityChoices GameDifficulity;

    public GameProperties.GameLevels ChosenLevel;

    public List<GameProperties.GameLevels> FinishedLevels = new List<GameProperties.GameLevels>();

    public string SolvedWords = "";
}


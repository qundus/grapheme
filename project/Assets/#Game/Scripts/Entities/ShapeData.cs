using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

[Serializable]
public class ShapeData
{
    public string Name;

    public char FixedLetter;

    public char RandomeLetter;

    [JsonIgnore]
    public byte[] ImageBytes { set; get; }
    
}


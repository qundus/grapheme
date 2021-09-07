using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public InputField input;
    public List<GameObject> letters;
    int times = 0;

	// Use this for initialization
	void Start ()
    {
        //letters = new List<GameObject>();
        RearrangeList(0);

        for (int i = 0; i < letters.Count; i++)
            Debug.Log(letters[i].transform.position.x);

        Debug.Log(times);
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void _ValidateText()
    {
        print(input.text);
        RearrangeList(0);
        for (int i = 0; i < input.text.Length && i < letters.Count; i++)
        {
            if (input.text.ToLower().ToCharArray()[i] == letters[i].name.ToLower().ToCharArray()[0]) 
                continue;
            else
            {
                input.text = "FAIL";
                break;
            }
        }
        if (input.text == "FAIL") ;
        else
            input.text = "CORRECT";
    }

    void RearrangeList(int i)
    {
        times += 1;
        GameObject go;

        if (i + 1 < letters.Count)
            if (letters[i].transform.position.x > letters[i + 1].transform.position.x)
            {
                go = letters[i];
                letters[i] = letters[i + 1];
                letters[i + 1] = go;
            }

        if (i - 1 >= 0)
            if (letters[i].transform.position.x < letters[i - 1].transform.position.x)
            {
                go = letters[i];
                letters[i] = letters[i - 1];
                letters[i - 1] = go;
                RearrangeList(i - 1);
            }


        if (i + 1 < letters.Count)
            RearrangeList(i + 1);

        //if (i - 1 >= 0) RearrangeList(i - 1);
    }
}

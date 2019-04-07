using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorfulText : MonoBehaviour
{
    [Header("Set In Inspector")]
    public Text text;

    int _index = 0;
    static Color[] colors = { Color.red, new Color(1f, .6471f, 0f), Color.yellow, Color.green, Color.cyan, Color.blue, new Color(0.75f, 0, 0.75f) }; //color array for looping through colors


    void Start()
    {
        _index = 0;
        Invoke("ColorChange", 0.5f);
    }

    void ColorChange() //loop through colors 
    {
        text.color = colors[_index++];
        if (_index == colors.Length)
            _index = 0;
        Invoke("ColorChange", 0.5f);
    }
}

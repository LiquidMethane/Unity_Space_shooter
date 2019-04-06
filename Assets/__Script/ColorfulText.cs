using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorfulText : MonoBehaviour
{
    [Header("Set In Inspector")]
    public Text text;

    int _index = 0;
    Color[] colors = { Color.red, Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta };


    void Start()
    {
        _index = 0;
        Invoke("ColorChange", 0.5f);
    }

    void ColorChange()
    {
        text.color = colors[_index++];
        if (_index == colors.Length)
            _index = 0;
        Invoke("ColorChange", 0.5f);
    }
}

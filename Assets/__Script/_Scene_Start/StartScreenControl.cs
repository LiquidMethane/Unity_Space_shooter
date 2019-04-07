using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenControl : MonoBehaviour
{
    [Header("Set In Inspector")]
    public Button playButton;
    public Text titleText;

    private void Update()
    {
        titleText.transform.position += Vector3.up * Mathf.Sin(Time.time * 5) * 0.1f; //make title jump
        playButton.GetComponent<Image>().color = new Color(playButton.GetComponent<Image>().color.r, //make play buttom breath
            playButton.GetComponent<Image>().color.g, playButton.GetComponent<Image>().color.b,
            Mathf.Sin(Time.time * 2));
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("_Scene_0");
    }

}

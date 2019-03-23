using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    static public ScoreManager Singleton; //scoremanager singleton

    [Header("Set in Inspector")]
    public Text scoreText;
    public Text highScoreText;
    [SerializeField]
    private bool eraseHighScoreOnStart = false; 

    [Header("Set Dynamically")]
    public int HIGH_SCORE = 0;
    [SerializeField]
    private int _score;

    public int Score
    {
        get { return _score; }
        set { _score = value; }
    }

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Debug.LogError("ERROR: ScoreManager.Awake(): S is already set!");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (eraseHighScoreOnStart) //reset highscore on start if true
            PlayerPrefs.DeleteKey("HighestScore");
        LoadHighScore();
        DisplayScore();
    }

    public void LoadHighScore() //loads highscore from playerprefs
    {
        if (PlayerPrefs.HasKey("HighestScore"))
            HIGH_SCORE = PlayerPrefs.GetInt("HighestScore");
    }

    public void SaveHighScore() //save highscore into playerprefs
    {
            PlayerPrefs.SetInt("HighestScore", HIGH_SCORE);
    }

    public void DisplayScore()
    {
        if (_score > HIGH_SCORE)
        {
            scoreText.color = Color.yellow;
            highScoreText.color = Color.yellow;
            HIGH_SCORE = _score;
        }
        scoreText.text = _score.ToString();
        highScoreText.text = "Highest Score: " + HIGH_SCORE.ToString();
    }
}

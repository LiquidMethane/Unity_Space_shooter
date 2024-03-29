﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    static public ScoreManager Singleton; //scoremanager singleton

    [Header("Set in Inspector")]
    public Text scoreText;
    public Text highScoreText;
    public Text victoryText;

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
        LoadHighScore();
        DisplayScore();
    }

    void Update()
    {
        if (Hero.Ship.Win)
        {
            victoryText.text = "VICTORY";
        }
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
        scoreText.color = Color.white;
        highScoreText.color = Color.white;
        if (_score > HIGH_SCORE) //if current score exceeds high score, save new high score and make texts yellow
        {
            HIGH_SCORE = _score;
            SaveHighScore();
            scoreText.color = Color.yellow;
            highScoreText.color = Color.yellow;
        }
        scoreText.text = _score.ToString();
        highScoreText.text = "High Score: " + HIGH_SCORE;
    }
}

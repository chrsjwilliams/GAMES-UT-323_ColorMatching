using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static event Action IncreaseBoardSize;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI recordText;

    public int score { get; private set; }
    public int record { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if(LoadScore())
        {
            record = PlayerPrefs.GetInt("RECORD");
        }
        else
        {
            PlayerPrefs.SetInt("RECORD", 0);
            record = 0;
        }

        scoreText.text = 0 + "";
        recordText.text = record + "";
    }

    public void SaveScore()
    {
        if(score > record)
        {
            PlayerPrefs.SetInt("RECORD", score);
            PlayerPrefs.Save();
        }
    }

    bool LoadScore() { return PlayerPrefs.HasKey("RECORD"); }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = score + "";
    }

    public void IncrementScore()
    {
        score += 1;
        scoreText.text = score + "";
        if (score > record)
        {
            record = score;
            recordText.text = record + "";
            SaveScore();
        }

        if(score == 4)
        {
            IncreaseBoardSize?.Invoke();
        }
    }
}

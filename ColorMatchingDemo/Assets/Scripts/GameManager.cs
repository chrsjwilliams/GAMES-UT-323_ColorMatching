using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum ColorMode { HUE, SAT, VAL}
    public enum PlayMode { TITLE, GAME, REPLAY}

    [SerializeField] Timer timer;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] TextMeshProUGUI playQuitButtonText;

    [SerializeField] GridLayoutGroup colorButtonLayoutGroup;

    [SerializeField] MonoTweener showGameTweener;
    [SerializeField] MonoTweener hideGameTweener;

    [SerializeField] PlayMode playMode = PlayMode.TITLE;
    [SerializeField] ColorMode changeMode;

    [SerializeField] Color targetColor;
    [SerializeField] Color offColor;

    [SerializeField] float initalStartTime = 10;

    int numButtonsToSpawn = 4;

    // ~TODO:   -   create class for buttons to tap
    //          -   when correct button is tapped
    //          -   increment score
    //          -   tweens for bringing in buttons
    //          -   game stars with 4 buttons but after
    //              4 correct selections increases to 9


    private void OnEnable()
    {
        ScoreManager.IncreaseBoardSize += IncreaseBoardSize;
    }

    private void OnDisable()
    {
        ScoreManager.IncreaseBoardSize -= IncreaseBoardSize;
    }

    // Start is called before the first frame update
    void Start()
    {
        playQuitButtonText.text = "play";
        GenerateColor();
    }


    public void TogglePlayMode()
    {
        switch (playMode)
        {
            case PlayMode.TITLE:
                // go into game mode
                playMode = PlayMode.GAME;
                playQuitButtonText.text = "quit";

                break;
            case PlayMode.GAME:
                // go into title mode
                playMode = PlayMode.TITLE;
                playQuitButtonText.text = "play";
                timer.StopTimer();

                break;
            case PlayMode.REPLAY:
                // go into game mode
                playMode = PlayMode.GAME;
                playQuitButtonText.text = "replay";

                break;
        }

        if(playMode == PlayMode.GAME)
        {
            timer.SetTime(initalStartTime);
            StartGame();
        }
        else
        {
            scoreManager.SaveScore();
        }
    }

    public void StartGame()
    {
        timer.StartTimer();
        colorButtonLayoutGroup.constraintCount = 2;
        numButtonsToSpawn = 4;
    }

    public void OnTimerFinished()
    {
        scoreManager.SaveScore();
        playMode = PlayMode.REPLAY;
        playQuitButtonText.text = "replay";
    }

    public void GenerateColor()
    {
        targetColor = Random.ColorHSV();

        float offset = 0.3f / (float)Mathf.Max(scoreManager.score, 1);

        changeMode = (ColorMode)Random.Range(0, 2);
        float hue, sat, val;
        Color.RGBToHSV(targetColor, out hue, out sat, out val);
        switch (changeMode)
        {
            case ColorMode.HUE:
                offColor = Color.HSVToRGB(  GenerateValue(hue, offset),
                                            sat,
                                            val);
                break;
            case ColorMode.SAT:
                offColor = Color.HSVToRGB(  hue ,
                                            GenerateValue(sat, offset),
                                            val);
                break;
            case ColorMode.VAL:
                offColor = Color.HSVToRGB(  hue,
                                            sat,
                                            GenerateValue(val, offset));
                break;
        }    
    }

    float GenerateValue(float value, float offset)
    {
        float newVal = float.MinValue;
        int rand = Random.Range(0, 10);
        if(rand % 2 == 0 )
        {
            newVal = value + offset;
            if(newVal > 1.0f)
            {
                newVal = value - offset;
            }
        }
        else
        {
            newVal = value - offset;
            if (newVal < 0.0f)
            {
                newVal = value + offset;
            }
        }

        return newVal;
    }

    void IncreaseBoardSize()
    {
        colorButtonLayoutGroup.constraintCount = 3;
        numButtonsToSpawn = 9;

    }


    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GenerateColor();
        }
    }
}

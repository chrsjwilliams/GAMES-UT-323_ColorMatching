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

    [SerializeField] CanvasGroup gameCanvasGroup;

    [SerializeField] MonoTweener showGameTweener;
    [SerializeField] MonoTweener hideGameTweener;

    [SerializeField] PlayMode playMode = PlayMode.TITLE;
    [SerializeField] ColorMode changeMode;

    [SerializeField] Color targetColor;
    [SerializeField] Color offColor;

    [SerializeField] float initalStartTime = 10;

    [SerializeField] SelectionButton selectionButtonPrefab;
    List<SelectionButton> selectionButtons = new List<SelectionButton>();

    int numButtonsToSpawn = 4;
    int offButtonIndex = -1;

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
        gameCanvasGroup.alpha = 0;

    }

    public void RestGame()
    {
        if (playMode == PlayMode.GAME) return;
        scoreManager.ResetRecord();
    }


    public void TogglePlayMode()
    {
        List<SelectionButton> toDelete = new List<SelectionButton>();
        switch (playMode)
        {
            case PlayMode.TITLE:
                // go into game mode
                playMode = PlayMode.GAME;
                playQuitButtonText.text = "quit";
                gameCanvasGroup.alpha = 1;
                break;
            case PlayMode.GAME:
                // go into title mode
                playMode = PlayMode.TITLE;
                playQuitButtonText.text = "play";
                timer.StopTimer();
                gameCanvasGroup.alpha = 0;
                foreach (SelectionButton btn in selectionButtons)
                {
                    btn.OnPressed -= NewColor;
                    toDelete.Add(btn);
                }
                foreach (SelectionButton btn in toDelete)
                {
                    Destroy(btn.gameObject);
                }
                toDelete.Clear();
                selectionButtons.Clear();
                break;
            case PlayMode.REPLAY:
                

                foreach (SelectionButton btn in selectionButtons)
                {
                    btn.OnPressed -= NewColor;
                    toDelete.Add(btn);
                }
                foreach (SelectionButton btn in toDelete)
                {
                    Destroy(btn.gameObject);
                }

                toDelete.Clear();
                selectionButtons.Clear();
                // go into game mode
                playMode = PlayMode.GAME;
                playQuitButtonText.text = "replay";

                break;
        }

        if(playMode == PlayMode.GAME)
        {
            scoreManager.ResetScore();
            timer.ResetTimer(initalStartTime);
            GenerateColor();

            numButtonsToSpawn = 4;
            offButtonIndex = Random.Range(0, numButtonsToSpawn - 1);
            for(int i = 0; i < numButtonsToSpawn; i++)
            {
                var newButton = Instantiate(selectionButtonPrefab, colorButtonLayoutGroup.transform);
                if (i == offButtonIndex)
                {
                    newButton.SetButton(offColor, true);
                }
                else
                {
                    newButton.SetButton(targetColor, false);
                }


                newButton.OnPressed += NewColor;
                selectionButtons.Add(newButton);
            }

            StartGame();
        }
        else
        {
            scoreManager.SaveScore();
        }
    }

    public void NewColor(bool foundColor)
    {
        if(!foundColor)
        {
            timer.SetTime(0);
            for (int i = 0; i < selectionButtons.Count; i++)
            {
                if (i == offButtonIndex)
                {
                    selectionButtons[i].ShowIndicator(buttonPress: false);
                }

            }
        }
        else
        {
            scoreManager.IncrementScore();
            timer.AddTime(3f);
            GenerateColor();
            SetBoard();
        }
    }

    public void StartGame()
    {
        timer.StartTimer();
        colorButtonLayoutGroup.constraintCount = 2;
    }

    public void OnTimerFinished()
    {
        scoreManager.SaveScore();
        playMode = PlayMode.REPLAY;
        playQuitButtonText.text = "replay";

        timer.SetTime(0);
        for (int i = 0; i < selectionButtons.Count; i++)
        {

            if (i == offButtonIndex)
            {
                selectionButtons[i].ShowIndicator(buttonPress: false);
            }
            selectionButtons[i].OnPressed -= NewColor;


        }
    }

    public void SetBoard()
    {
        offButtonIndex = Random.Range(0, numButtonsToSpawn - 1);
        for (int i = 0; i < selectionButtons.Count; i++)
        {
            if (i == offButtonIndex)
            {
                selectionButtons[i].SetButton(offColor, true);
            }
            else
            {
                selectionButtons[i].SetButton(targetColor, false);
            }
        }
    }

    public void GenerateColor()
    {
        targetColor = Random.ColorHSV();
        int difficulty = 0;

        if (scoreManager.score % 4 == 0)
            difficulty += 1;

        float offset = 0.3f / (float)Mathf.Max(difficulty, 1);
        changeMode = (ColorMode)Random.Range(0, 3);
        float hue, sat, val;
        Color.RGBToHSV(targetColor, out hue, out sat, out val);
        switch (changeMode)
        {
            case ColorMode.HUE:
                offColor = Color.HSVToRGB(  GenerateValue(hue, offset),
                                            sat,
                                            val, true);
                break;
            case ColorMode.SAT:
                offColor = Color.HSVToRGB(  hue ,
                                            GenerateValue(sat, offset),
                                            val,true);
                break;
            case ColorMode.VAL:
                offColor = Color.HSVToRGB(  hue,
                                            sat,
                                            GenerateValue(val, offset), true);
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
        int leftToSpawn = numButtonsToSpawn - selectionButtons.Count;

        for (int i = 0; i < leftToSpawn; i++)
        {
            var newButton = Instantiate(selectionButtonPrefab, colorButtonLayoutGroup.transform);
            if (i == offButtonIndex)
            {
                newButton.SetButton(offColor, true);
            }
            else
            {
                newButton.SetButton(targetColor, false);
            }


            newButton.OnPressed += NewColor;
            selectionButtons.Add(newButton);
        }

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

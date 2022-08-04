using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    // we can use enums as a way to assign names to numbers
    // they help us make our code more readable
    public enum DisplayType { Text, Image, TextAndImage}

    [SerializeField] DisplayType displayType;
    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] float elapsedTime = 0;
    [SerializeField] float startTime = 10;
    [SerializeField] Image timerImage;
    [SerializeField] UnityEvent OnTimerOver;

    bool timeOver = false;
    bool canStartTimer = false;

    [SerializeField] Color almostDone;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = startTime;
        string time = elapsedTime.ToString("0");
        timerText.text = time;
    }

    public void StartTimer()
    {
        canStartTimer = true;
    }

    public void StopTimer()
    {
        canStartTimer = false;
    }

    public void ResetTimer(float t)
    {
        startTime = t;
        elapsedTime = t;
        timeOver = false;
    }

    public void AddTime(float t)
    {
        elapsedTime = elapsedTime + t;
        string time = elapsedTime.ToString("0");
        timerText.text = time;
    }

    public void SetTime(float t)
    {
        elapsedTime = t;
        string time = elapsedTime.ToString("0");
        timerText.text = time;
    }

    public void RemoveTime(float t)
    {
        float temp = elapsedTime - t;
        if(temp < 0)
        {
            elapsedTime = 0;
        }
        else
        {
            elapsedTime = temp;
        }
        string time = elapsedTime.ToString("0");
        timerText.text = time;
    }

    public void ToggleTimer()
    {
        canStartTimer = !canStartTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canStartTimer) return;

        if (elapsedTime > 0)
        {
            elapsedTime -= 1 * Time.deltaTime;

            // changes text color based on percentage of the time
            // that has been elapsed
            if(elapsedTime < (startTime * 0.3f))
            {
                timerText.color = almostDone;
            }
            else
            {
                timerText.color = Color.white;
            }

            // format the string to only display values before the decimal point
            string time = elapsedTime.ToString("0");

            // switch statement is a way to branch our code based on the value of
            // a variable
            switch (displayType)
            {
                case DisplayType.Text:
                    timerText.text = time;

                    // hide the image by setting scale to 0
                    timerImage.transform.localScale = Vector3.zero;
                    break;
                case DisplayType.Image:
                    // hide the text
                    timerText.text = "";

                    // show image in case we leave a sate where the image was hidden
                    timerImage.transform.localScale = Vector3.one;
                    // update fill amount
                    timerImage.fillAmount = elapsedTime / startTime;
                    break;
                case DisplayType.TextAndImage:
                    timerText.text = time;
                    // show image in case we leave a sate where the image was hidden
                    timerImage.transform.localScale = Vector3.one;
                    // update fill amount
                    timerImage.fillAmount = elapsedTime / startTime;
                    break;
            }

        }
        else if(elapsedTime <= 0 && !timeOver)
        {
            timeOver = true;
            // calling our unity event
            if (OnTimerOver != null)
            {
                OnTimerOver.Invoke();
            }
        }
    }
}

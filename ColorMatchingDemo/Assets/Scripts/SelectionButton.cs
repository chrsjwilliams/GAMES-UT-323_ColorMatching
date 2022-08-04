using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectionButton : MonoBehaviour
{
    public event Action<bool> OnPressed;

    [SerializeField] Sprite correctImage;
    [SerializeField] Sprite incorrectImage;
    [SerializeField] Image answerIndicator;

    [Space(25)]
    [SerializeField] Image circleImage;

    private bool _isOffButton;
    public bool isOffButton { get { return _isOffButton; } }

    public void SetButton(Color col, bool isOddColor)
    {
        answerIndicator.transform.DOScale(0, 0.33f).SetUpdate(true).SetEase(Ease.InElastic).SetDelay(0.33f);
        circleImage.color = col;
        _isOffButton = isOddColor;
    }

    public void ShowIndicator(bool buttonPress)
    {
        answerIndicator.sprite = isOffButton ? correctImage : incorrectImage;
        answerIndicator.transform.DOScale(1, 0.33f)
                                 .SetUpdate(true)
                                 .SetEase(Ease.OutElastic)
                                 .OnComplete(
                                    ()=>
                                    {
                                        if (buttonPress) ButtonPressed();
                                    }
                                    );
        
    }

    public void ButtonPressed()
    {
        OnPressed?.Invoke(isOffButton);
    }


}

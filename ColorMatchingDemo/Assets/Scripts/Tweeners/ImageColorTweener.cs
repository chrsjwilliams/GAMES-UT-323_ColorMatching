using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageColorTweener : MonoTweener
{
    [SerializeField] Image ImageToUse;
    [SerializeField] ColorMode colorMode;
    [SerializeField, DrawIf("colorMode", ColorMode.ALPHA, ComparisonType.Equals)] float alpha;
    [SerializeField, DrawIf("colorMode", ColorMode.RGB, ComparisonType.Equals)] Color TargetColor;
    [DrawIf("colorMode", ColorMode.HSV,ComparisonType.Equals), Header("X: Change Hue by, Y: Change Saturation by, Z: Change Value by")]
    [SerializeField, DrawIf("colorMode", ColorMode.HSV, ComparisonType.Equals)] Vector3 HSVDelta;
    [SerializeField, DrawIf("colorMode", ColorMode.HSV, ComparisonType.Equals)] bool hdr;


    Color _originalColor;
    protected override Tweener LocalPlay()
    {
        _originalColor = ImageToUse.color;
        if (colorMode == ColorMode.HSV)
        {
            float h, s, v;
            Color imgColor = ImageToUse.color;
            Color.RGBToHSV(new Color(imgColor.r, imgColor.g, imgColor.b), out h, out s, out v);
            TargetColor = Color.HSVToRGB(h + HSVDelta.x, s + HSVDelta.y, v + HSVDelta.z, hdr);

        }
        else if (colorMode == ColorMode.ALPHA)
        {
            Color imgColor = ImageToUse.color;
            TargetColor = new Color(imgColor.r, imgColor.g, imgColor.b, alpha);
        }

        return ImageToUse.DOColor(TargetColor, duration).SetEase(easing);

    }

    public void SetTargetToColor(string hexCode)
    {
        char[] hexCheck = hexCode.ToCharArray();
        if (hexCheck[0] != '#')
        {
            hexCode = "#" + hexCode;
        }
        Color newColor;
        bool canUseColor = ColorUtility.TryParseHtmlString(hexCode, out newColor);
        if (canUseColor)
        {
            ImageToUse.color = newColor;
        }
        else
        {
            Debug.LogError("Cannot create Color using hexcode: " + hexCode);
        }
    }

    public void SetToOriginalColor()
    {
        ImageToUse.color = _originalColor;
    }

    public void SetToTargetColor()
    {
        ImageToUse.color = TargetColor;
    }

    public enum ColorMode
    {
        RGB, HSV, ALPHA
    }
}

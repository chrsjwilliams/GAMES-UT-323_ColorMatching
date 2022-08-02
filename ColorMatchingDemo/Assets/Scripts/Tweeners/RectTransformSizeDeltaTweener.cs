using UnityEngine;
using DG.Tweening;

public class RectTransformSizeDeltaTweener : RectTransformTweener
{
    enum AdjustBy
    {
        SetSize,
        Offset,
    }
    [SerializeField] AdjustBy tweenBy;
    [Tooltip("Tween to this size exactly")]
    [SerializeField, DrawIf("tweenBy", AdjustBy.SetSize, ComparisonType.Equals)]
    Vector2 size;

    [Tooltip("Tween to the offset relative to the position it started from")]
    [SerializeField, DrawIf("tweenBy", AdjustBy.Offset, ComparisonType.Equals)]
    Vector2 offset;

    Vector2 cachedSize = new Vector2();

    protected override Tweener LocalPlay()
    {
        cachedSize = target.sizeDelta;
        Vector2 newSize = size;
        var targetSize = tweenBy == AdjustBy.Offset ?
                                target.anchoredPosition + offset
                                : newSize;

        return target.DOSizeDelta(targetSize, duration).SetEase(easing).SetDelay(delay);
    }

    public void ResetSize()
    {
        target.sizeDelta = cachedSize;
    }
}

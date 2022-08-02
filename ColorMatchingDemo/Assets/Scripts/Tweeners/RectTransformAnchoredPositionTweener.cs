using UnityEngine;
using DG.Tweening;

public class RectTransformAnchoredPositionTweener : RectTransformTweener
{

    enum AdjustBy
    {
        Offset,
        Position,
    }

    [SerializeField] AdjustBy tweenBy;
    [Tooltip("Tween to this position exactly")]
    [SerializeField, DrawIf("tweenBy", AdjustBy.Position, ComparisonType.Equals)]
    Vector2 position;


    [Tooltip("Tween to the offset relative to the position it started from")]
    [SerializeField, DrawIf("tweenBy", AdjustBy.Offset, ComparisonType.Equals)]
    Vector2 offset;

    protected override Tweener LocalPlay()
    {
        Vector2 newPos = position;
        var targetPosition = tweenBy == AdjustBy.Offset ?
                                target.anchoredPosition + offset
                                : newPos;

        return target.DOAnchorPos(targetPosition, duration).SetEase(easing).SetDelay(delay);
    }

    public void SetToTargetPosition()
    {
        if (tweenBy == AdjustBy.Position)
        {
            target.anchoredPosition = position;
        }
        else
        {
            target.anchoredPosition = new Vector2(  target.anchoredPosition.x + offset.x,
                                                    target.anchoredPosition.y + offset.y);
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public abstract class MonoTweener : MonoBehaviour
{
    [SerializeField,TextArea] string description;   

    [SerializeField] protected bool completeIfKilled;
    [SerializeField] protected bool randomDuration;
    [Header("Duration")]
    [SerializeField, DrawIf("randomDuration", false, ComparisonType.Equals)] protected float duration;

    [SerializeField, DrawIf("randomDuration", true, ComparisonType.Equals)] protected float minDurationRange;
    [SerializeField, DrawIf("randomDuration", true, ComparisonType.Equals)] protected float maxDurationRange;


    [SerializeField] protected bool randomDelay;
    [SerializeField, DrawIf("randomDelay", false, ComparisonType.Equals)] protected float delay;

    [SerializeField, DrawIf("randomDelay", true, ComparisonType.Equals)] protected float minDelayRange;
    [SerializeField, DrawIf("randomDelay", true, ComparisonType.Equals)] protected float maxDelayRange;


    [SerializeField] protected bool playWhilePaused;
    [SerializeField] protected bool loop;
    [Header("Setting loops to -1 will make the tween loop infinitely.")]
    [SerializeField, DrawIf("loop", true, ComparisonType.Equals)] protected int loops;

    [SerializeField, DrawIf("loop", true, ComparisonType.Equals)] protected LoopType loopType;

    [SerializeField] UnityEvent OnBegin;

    [SerializeField] UnityEvent OnComplete;


    [SerializeField] UnityEvent OnKill;
    [SerializeField] protected Ease easing;

    public float length { get { return duration + delay; } }

    private Tweener tweener;

    private bool isActive, complete;

    public void Play()
    {
        Play(() => { });
    }

    public void Kill()
    {
        if (tweener != null)
        {
            tweener.Kill(completeIfKilled);
        }
    }

    public void Play(Action callback)
    {
        if (isActive)
        {
            return;
        }
        complete = false;
        isActive = true;
        tweener = SetupPlay();
        tweener.onComplete += () =>
        {
            complete = true;
            OnComplete?.Invoke();
            callback();
            Cleanup();
        };
        tweener.onKill += () =>
        {
            if (!complete && !completeIfKilled)
            {
                OnKill?.Invoke();
                callback();
                Cleanup();
            }
        };

    }

    void Cleanup()
    {
        isActive = false;
        tweener = null;
    }

    Tweener SetupPlay()
    {
        OnBegin?.Invoke();
        tweener = LocalPlay();
        if (delay > 0f)
        {
            tweener.SetDelay(delay);
        }
        if (loop)
        {
            tweener.SetLoops(loops, loopType);
        }
        tweener.SetUpdate(playWhilePaused);
        return tweener;
    }

    void OnDestroy()
    {
        Kill();
        Cleanup();
    }

    protected abstract DG.Tweening.Tweener LocalPlay();

}

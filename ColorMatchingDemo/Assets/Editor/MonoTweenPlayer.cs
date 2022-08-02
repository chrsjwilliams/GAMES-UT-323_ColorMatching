using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonoTweener), true)]
public class MonoTweenPlayer : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MonoTweener tween = (MonoTweener)target;
        if (GUILayout.Button("Play"))
        {
            tween.Play();
        }
    }
}

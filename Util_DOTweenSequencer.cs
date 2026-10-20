using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Util_DOTweenSequencer : MonoBehaviour
{
    [SerializeField] DOTweenKeyframe[] keyframes;
    [SerializeField] bool playOnStart;
    [SerializeField] bool saveDefaultValue;
    [SerializeField] bool deleteOnAnimationEnd;

    [Header("Debug")]
    [SerializeField] bool previewAnimation;

    [Header("Initial Values")]
    [SerializeField] Vector3 ogPos;
    [SerializeField] Quaternion ogRot;
    [SerializeField] Vector3 ogScale;
    [SerializeField] Vector4 ogColor;

    private Coroutine _sequenceCoroutine;

    private void Update()
    {
        if (previewAnimation)
        {
            PlayKeyframes();
            previewAnimation = false;
        }
    }

    public void Start()
    {
        if (saveDefaultValue)
        {
            ogPos = transform.position;
            ogRot = transform.rotation;
            ogScale = transform.localScale;
            ogColor = GetColor(gameObject);
        }

        if (playOnStart) PlayKeyframes();
    }

    public void PlayKeyframes()
    {
        if (_sequenceCoroutine != null) StopCoroutine(_sequenceCoroutine);
        DOTween.Kill(gameObject);

        if (saveDefaultValue)
        {
            transform.SetPositionAndRotation(ogPos, ogRot);
            transform.localScale = ogScale;
            SetColor(gameObject, ogColor);
        }

        _sequenceCoroutine = StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        var groups = new List<List<DOTweenKeyframe>>();
        var currentGroup = new List<DOTweenKeyframe>();

        foreach (var key in keyframes)
        {
            currentGroup.Add(key);
            if (!key.isLayer)
            {
                groups.Add(currentGroup);
                currentGroup = new List<DOTweenKeyframe>();
            }
        }

        // Leftover layers with no non-layer anchor — play them as a final group
        if (currentGroup.Count > 0)
            groups.Add(currentGroup);

        foreach (var group in groups)
        {
            bool groupDone = false;

            // The last keyframe in the group is the "anchor" (isLayer = false)
            // All others are layers (run concurrently)
            for (int i = 0; i < group.Count; i++)
            {
                DOTweenKeyframe key = group[i];
                bool isAnchor = i == group.Count - 1;

                PlayKeyframe(key, isAnchor ? () => groupDone = true : null);
            }

            yield return new WaitUntil(() => groupDone);
        }

        _sequenceCoroutine = null;
        if (deleteOnAnimationEnd)
        {
            Destroy(gameObject);
            if (gameObject.transform.parent.TryGetComponent<DOTweenSequenceGroup>(out DOTweenSequenceGroup group))
            {
                if (group == null) yield break;
                Destroy(group.gameObject);
            }
        }
    }

    private void PlayKeyframe(DOTweenKeyframe key, Action onComplete)
    {
        switch (key.transType)
        {
            case TransformType.Position:
                transform.DOMove((Vector3)key.vectors, key.dur)
                    .SetDelay(key.delay)
                    .SetEase(key.easeType)
                    .OnComplete(() => onComplete?.Invoke())
                    .SetLink(gameObject);
                break;

            case TransformType.Rotation:
                transform.DORotate((Vector3)key.vectors, key.dur)
                    .SetDelay(key.delay)
                    .SetEase(key.easeType)
                    .OnComplete(() => onComplete?.Invoke())
                    .SetLink(gameObject);
                break;

            case TransformType.Scale:
                transform.DOScale((Vector3)key.vectors, key.dur)
                    .SetDelay(key.delay)
                    .SetEase(key.easeType)
                    .OnComplete(() => onComplete?.Invoke())
                    .SetLink(gameObject);
                break;

            case TransformType.Color:
                TweenColor(gameObject, key.vectors, key.dur, key.delay, key.easeType, onComplete);
                break;
        }
    }

    private void TweenColor(GameObject obj, Color targetColor, float dur, float delay, Ease ease, Action onComplete)
    {
        Tween tween = null;

        if (obj.TryGetComponent<Graphic>(out var graphic))
            tween = graphic.DOColor(targetColor, dur);
        else if (obj.TryGetComponent<SpriteRenderer>(out var sr))
            tween = sr.DOColor(targetColor, dur);
        else if (obj.TryGetComponent<MeshRenderer>(out var mr))
            tween = mr.material.DOColor(targetColor, dur);
        else
        {
            Debug.LogWarning($"[DOTweenSequencer] No colorable component found on {obj.name}");
            onComplete?.Invoke();
            return;
        }

        tween.SetDelay(delay)
             .SetEase(ease)
             .OnComplete(() => onComplete?.Invoke())
             .SetLink(gameObject);
    }

    public static Color GetColor(GameObject obj)
    {
        if (obj.TryGetComponent<Graphic>(out var graphic)) return graphic.color;
        if (obj.TryGetComponent<SpriteRenderer>(out var sr)) return sr.color;
        if (obj.TryGetComponent<MeshRenderer>(out var mr)) return mr.material.color;

        Debug.LogWarning($"[DOTweenSequencer] No colorable component found on {obj.name}");
        return Color.white;
    }

    public static void SetColor(GameObject obj, Color color)
    {
        if (obj.TryGetComponent<Graphic>(out var graphic)) { graphic.color = color; return; }
        if (obj.TryGetComponent<SpriteRenderer>(out var sr)) { sr.color = color; return; }
        if (obj.TryGetComponent<MeshRenderer>(out var mr)) { mr.material.color = color; return; }

        Debug.LogWarning($"[DOTweenSequencer] No colorable component found on {obj.name}");
    }
}

[Serializable]
public class DOTweenKeyframe
{
    public TransformType transType;
    public Ease easeType;           // DG.Tweening.Ease instead of LeanTweenType
    public float dur;
    public float delay;
    public Vector4 vectors;
    public bool isLayer;
}

public enum TransformType
{
    Position,
    Rotation,
    Scale,
    Color
}

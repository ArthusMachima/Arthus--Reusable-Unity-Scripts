using UnityEngine;

public class Util__DOTweenSequenceGroup : MonoBehaviour
{
    [SerializeField] DOTweenSequencer[] Sequences;
    [Header("Debug")]
    [SerializeField] bool PreviewSequence;

    private void Update()
    {
        if (PreviewSequence)
        {
            PlaySequences();
            PreviewSequence = false;
        }
    }

    public void PlaySequences()
    {
        foreach (var sequence in Sequences)
        {
            sequence.PlayKeyframes();
        }
    }
}

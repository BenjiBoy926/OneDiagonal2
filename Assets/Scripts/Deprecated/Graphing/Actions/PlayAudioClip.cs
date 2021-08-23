using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayAudioClip : MonoBehaviour
{
    public Input<AudioSource> source;
    public Input<AudioClip> clip;

    public UnityEvent soundStart;
    public UnityEvent soundEnd;

    public void Invoke()
    {
        source.value.clip = clip.value;
        source.value.Play();

        soundStart.Invoke();

        StopAllCoroutines();
        StartCoroutine("CheckClipFinishedPlaying");
    }

    private IEnumerator CheckClipFinishedPlaying()
    {
        yield return new WaitUntil(() => !source.value.isPlaying);
        soundEnd.Invoke();
    }
}

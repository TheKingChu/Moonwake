using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    //audio stuff
    public AudioSource track1, track2, track3, track4; //1=day, 2=day healed, 3=night, 4=monster night
    bool isPlaying;
    private float fadeTime = 2f;

    //day and night stuff
    private DayCycleManager cycleManager;

    private void Start()
    {
        cycleManager = GameObject.FindGameObjectWithTag("DayNightManager").GetComponent<DayCycleManager>();
    }

    private void Update()
    {
        //need to add the healed version and the one where monsters are close!
        if (cycleManager.TimeOfDay >= 0.33f) //night for some reason
        {
            /*StartCoroutine(FadeIn(track3, fadeTime));
            StartCoroutine(FadeOut(track3, fadeTime));*/
            track3.Play();
        }
        else //day
        {
            /*StartCoroutine(FadeIn(track1, fadeTime));
            StartCoroutine(FadeOut(track1, fadeTime));*/

            track1.Play();
        }

    }

    private static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
    }

    private static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        audioSource.Play();
        audioSource.volume = 0f;
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }
}

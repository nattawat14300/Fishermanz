using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip introMusic;
    public AudioClip afterOrcaMusic;

    [Header("Fade Settings")]
    public float fadeDuration = 1.5f;

    void Start()
    {
        PlayIntroImmediate();   // ✅ เริ่มเพลงแบบไม่ Fade
    }

    // 🎬 Intro เล่นปกติ
    public void PlayIntroImmediate()
    {
        if (audioSource == null || introMusic == null)
        {
            Debug.LogError("MusicManager: AudioSource or IntroMusic missing");
            return;
        }

        StopAllCoroutines();
        audioSource.clip = introMusic;
        audioSource.volume = 1f;          // ✅ เล่นเต็ม ไม่ Fade
        audioSource.loop = true;
        audioSource.Play();
    }

    // 🐋 Orca ขึ้น → FadeOut Intro
    public void FadeOutIntro()
    {
        if (audioSource == null || !audioSource.isPlaying) return;

        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    // ▶️ หลัง Orca Next → เล่น AfterOrca + FadeIn
    public void PlayAfterOrca()
    {
        if (audioSource == null || afterOrcaMusic == null)
        {
            Debug.LogError("MusicManager: AudioSource or AfterOrcaMusic missing");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(SwitchMusic(afterOrcaMusic));
    }

    // 🛑 Win / Lose
    public void StopMusic()
    {
        StopAllCoroutines();
        if (audioSource != null)
            audioSource.Stop();
    }

    // -------------------- FADE SYSTEM --------------------

    IEnumerator SwitchMusic(AudioClip nextClip)
    {
        yield return StartCoroutine(FadeOut());

        audioSource.clip = nextClip;
        audioSource.Play();

        yield return StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 1;
    }

    IEnumerator FadeOut()
    {
        float t = 0;
        float startVolume = audioSource.volume;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }
}

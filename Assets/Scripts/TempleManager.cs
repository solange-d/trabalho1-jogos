using System.Collections;
using SunTemple;
using UnityEngine;

public class TempleManager : MonoBehaviour
{
    [Header("Portas")]
    public Door[] portasCentrais;
    public Door portaLateralBoss;
    public Door portaLateralExtra;

    [Header("Inimigos")]
    public InimigoComum[] inimigosSala;
    public Boss boss;

    [Header("Áudio")]
    public AudioClip musicaAmbiente;
    public AudioClip musicaBoss;
    private AudioSource audioSource;

    private int inimigosMortos = 0;
    private bool lateralAberta = false;
    private bool normaisAbertas = false;

    private void OnEnable() => InimigoComum.OnInimigoMorreu += VerificarInimigos;
    private void OnDisable() => InimigoComum.OnInimigoMorreu -= VerificarInimigos;

    private void Start()
    {
        foreach (Door d in portasCentrais) d.IsLocked = true;
        if (portaLateralBoss != null) portaLateralBoss.IsLocked = true;
        if (portaLateralExtra != null) portaLateralExtra.IsLocked = true;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 1f;

        TrocarMusica(musicaAmbiente);
    }

    private void VerificarInimigos(InimigoComum inimigo)
    {
        foreach (InimigoComum i in inimigosSala)
            if (i == inimigo) inimigosMortos++;

        if (!lateralAberta && inimigosMortos >= inimigosSala.Length)
        {
            lateralAberta = true;
            if (portaLateralBoss != null) portaLateralBoss.UnlockWithFeedback();

            if (GameManager.Instance != null)
                GameManager.Instance.MostrarMensagemEspecial("Você irritou o Chefão! Prepare-se!");

            TrocarMusica(musicaBoss);
        }

        if (!normaisAbertas && inimigo == boss)
        {
            normaisAbertas = true;
            foreach (Door d in portasCentrais) d.UnlockWithFeedback();
            if (portaLateralExtra != null) portaLateralExtra.UnlockWithFeedback();

            TrocarMusica(musicaAmbiente);
        }
    }

    private void TrocarMusica(AudioClip novaMusica, float fadeDuration = 2f)
    {
        if (audioSource.clip == novaMusica) return;
        StartCoroutine(FadeMusica(novaMusica, fadeDuration));
    }

    private IEnumerator FadeMusica(AudioClip novaMusica, float fadeDuration)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        audioSource.Stop();
        audioSource.clip = novaMusica;
        audioSource.Play();
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        audioSource.volume = startVolume;
    }
}

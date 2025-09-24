using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum Dificuldade { Facil, Medio, Dificil }
    public Dificuldade dificuldadeAtual = Dificuldade.Medio;

    [Header("HUD")]
    public TMP_Text textoInimigosMortos;
    private int inimigosMortos = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Procura automaticamente o Text na nova cena
        textoInimigosMortos = GameObject.Find("TextoInimigos")?.GetComponent<TMP_Text>();
        AtualizarTextoInimigos();
    }

    public void DefinirDificuldade(string nivel)
    {
        switch (nivel)
        {
            case "Facil": dificuldadeAtual = Dificuldade.Facil; break;
            case "Medio": dificuldadeAtual = Dificuldade.Medio; break;
            case "Dificil": dificuldadeAtual = Dificuldade.Dificil; break;
        }

        Debug.Log("Dificuldade escolhida: " + dificuldadeAtual);
    }

    public void RegistrarMorte()
    {
        inimigosMortos++;
        AtualizarTextoInimigos();
    }

    private void AtualizarTextoInimigos()
    {
        if (textoInimigosMortos != null)
            textoInimigosMortos.text = "Mutantes Mortos: " + inimigosMortos.ToString();
    }

    public void ResetarInimigos()
    {
        inimigosMortos = 0;
        AtualizarTextoInimigos();
    }
}

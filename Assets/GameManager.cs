using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum Dificuldade { Facil, Medio, Dificil }
    public Dificuldade dificuldadeAtual = Dificuldade.Medio;

    [Header("HUD")]
    public TMP_Text textoInimigosMortos;
    public TMP_Text textoPontuacao;
    public TMP_Text mensagemEspecial;

    [Header("Configuração")]
    public float duracaoMensagemEspecial = 3f; // duração que a mensagem aparece

    private int inimigosMortos = 0;
    private int pontuacao = 0;
    private bool bossPontuado = false;

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
            return;
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
        // Reconecta os elementos do HUD
        textoInimigosMortos = GameObject.Find("TextoInimigos")?.GetComponent<TMP_Text>();
        textoPontuacao = GameObject.Find("TextoPontuacao")?.GetComponent<TMP_Text>();
        mensagemEspecial = GameObject.Find("MensagemEspecial")?.GetComponent<TMP_Text>();

        AtualizarHUD();
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

    // Registrar morte de inimigo comum
    public void RegistrarMorte()
    {
        inimigosMortos++;
        pontuacao += 10;
        AtualizarHUD();
    }

    // Registrar morte do boss
    public void RegistrarMorteBoss()
    {
        if (bossPontuado) return;
        inimigosMortos++;
        pontuacao += 30;
        AtualizarHUD();
        bossPontuado = true;
    }

    // Mostrar mensagem especial
    public void MostrarMensagemEspecial(string mensagem)
    {
        if (mensagemEspecial == null) return;
        StopAllCoroutines();
        StartCoroutine(ExibirMensagemCoroutine(mensagem));
    }

    private System.Collections.IEnumerator ExibirMensagemCoroutine(string mensagem)
    {
        mensagemEspecial.text = mensagem;
        mensagemEspecial.enabled = true;
        yield return new WaitForSeconds(duracaoMensagemEspecial);
        mensagemEspecial.text = "";
        mensagemEspecial.enabled = false;
    }

    // Atualiza HUD de inimigos e pontuação
    private void AtualizarHUD()
    {
        if (textoInimigosMortos != null)
            textoInimigosMortos.text = "Mutantes Mortos: " + inimigosMortos;

        if (textoPontuacao != null)
            textoPontuacao.text = "Pontuação: " + pontuacao;
    }

    // Reset total para novo jogo
    public void ResetarProgresso()
    {
        inimigosMortos = 0;
        pontuacao = 0;
        bossPontuado = false;
        AtualizarHUD();

        if (mensagemEspecial != null)
        {
            mensagemEspecial.text = "";
            mensagemEspecial.enabled = false;
        }
    }
}

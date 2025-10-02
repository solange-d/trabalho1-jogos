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
    public float duracaoMensagemEspecial = 3f;

    private int inimigosMortos = 0;
    private int pontuacao = 0;
    private bool bossPontuado = false;

    private int pontuacaoFinal = 0;
    private string mensagemFinal = "";
    private bool jogoFinalizado = false;


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
        if (scene.buildIndex == 1)
        {
            textoInimigosMortos = GameObject.Find("TextoInimigos")?.GetComponent<TMP_Text>();
            textoPontuacao = GameObject.Find("TextoPontuacao")?.GetComponent<TMP_Text>();
            mensagemEspecial = GameObject.Find("MensagemEspecial")?.GetComponent<TMP_Text>();
            AtualizarHUD();
        }
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
        pontuacao += 10;
        AtualizarHUD();
    }

    public void RegistrarMorteBoss()
    {
        if (bossPontuado) return;
        inimigosMortos++;
        pontuacao += 30;
        AtualizarHUD();
        bossPontuado = true;
    }

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

    private void AtualizarHUD()
    {
        if (textoInimigosMortos != null)
            textoInimigosMortos.text = "Mutantes Mortos: " + inimigosMortos;

        if (textoPontuacao != null)
            textoPontuacao.text = "Pontuação: " + pontuacao;
    }

    public void ResetarProgresso()
    {
        inimigosMortos = 0;
        pontuacao = 0;
        bossPontuado = false;

        jogoFinalizado = false;
        mensagemFinal = "";
        pontuacaoFinal = 0;

        if (mensagemEspecial != null)
        {
            mensagemEspecial.text = "";
            mensagemEspecial.enabled = false;
        }
    }

    public void FinalizarJogo(bool vitoria)
    {
        jogoFinalizado = true;
        pontuacaoFinal = pontuacao;

        if (vitoria)
        {
            mensagemFinal = "Vitória!";
        }
        else
        {
            mensagemFinal = "Você Morreu!";
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

    public bool JogoFoiFinalizado() { return jogoFinalizado; }
    public string GetMensagemFinal() { return mensagemFinal; }
    public int GetPontuacaoFinal() { return pontuacaoFinal; }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartGame : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Options;

    [Header("Grupo de Fim de Jogo")]
    public GameObject grupoResultado;
    public TMP_Text textoResultado;
    public TMP_Text textoPontuacaoFinal;

    void Start()
    {
        Menu.SetActive(true);
        Options.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (grupoResultado == null)
        {;
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.JogoFoiFinalizado())
        {
            grupoResultado.SetActive(true);
            textoResultado.text = GameManager.Instance.GetMensagemFinal();
            textoPontuacaoFinal.text = "Pontuação Final: " + GameManager.Instance.GetPontuacaoFinal();
        }
        else
        {
            grupoResultado.SetActive(false);
        }
    }

    public void ReiniciarJogo()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetarProgresso();
        }

        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void FecharPainelResultado()
    {
        if (grupoResultado != null)
        {
            grupoResultado.SetActive(false);
        }
        
        Menu.SetActive(true);
    }

    public void SairJogo() { Application.Quit(); }
    public void AbrirOpcoes() { Menu.SetActive(false); Options.SetActive(true); }
    public void VoltarMenu()
    {
        Options.SetActive(false);
        Menu.SetActive(true);
    }
    public void DefinirDificuldade(string dificuldade) { GameManager.Instance.DefinirDificuldade(dificuldade); }
}
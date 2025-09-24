using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Options;
    public void ReiniciarJogo()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void SairJogo()
    {
        Application.Quit();
    }

    public void AbrirOpcoes()
    {
        Menu.SetActive(false);
        Options.SetActive(true);
    }

    public void VoltarMenu()
    {
        Options.SetActive(false);
        Menu.SetActive(true);
    }

    public void DefinirDificuldade(string dificuldade)
    {
        GameManager.Instance.DefinirDificuldade(dificuldade);
    }
}

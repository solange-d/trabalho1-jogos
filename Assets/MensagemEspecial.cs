// MensagemEspecial.cs
using TMPro;
using UnityEngine;
using System.Collections; // Adicione este using

public class MensagemEspecial : MonoBehaviour
{
    public static MensagemEspecial Instance;

    [Header("Configurações")]
    public TMP_Text textoMensagem;
    public float duracao = 3f;

    private Coroutine coroutineAtual; // Para gerenciar a corrotina

    private void Awake()
    {
        {
            // Se você decidiu manter o Singleton para outros usos:
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            if (textoMensagem == null)
            {
                textoMensagem = GetComponent<TMP_Text>();
            }

            if (textoMensagem != null)
                textoMensagem.text = "";
            textoMensagem.gameObject.SetActive(false); // Garante que comece desativado
        }
    }

    public void MostrarMensagem(string mensagem)
    {
        if (textoMensagem == null)
        {
            Debug.LogError("MensagemEspecial: nenhum TMP_Text atribuído ou encontrado!");
            return;
        }

        // Parar a corrotina anterior se houver uma para evitar sobreposição
        if (coroutineAtual != null)
        {
            StopCoroutine(coroutineAtual);
        }

        coroutineAtual = StartCoroutine(ExibirMensagem(mensagem));
    }

    private IEnumerator ExibirMensagem(string mensagem)
    {
        textoMensagem.text = mensagem;
        textoMensagem.gameObject.SetActive(true); // Ativa o objeto de texto

        yield return new WaitForSeconds(duracao);

        textoMensagem.text = "";
        textoMensagem.gameObject.SetActive(false); // Desativa o objeto de texto
    }
}
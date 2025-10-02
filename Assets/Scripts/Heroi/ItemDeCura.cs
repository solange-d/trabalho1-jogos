using UnityEngine;

public class ItemDeCura : MonoBehaviour, IPegavel
{
    [Header("Configuração da Cura")]
    [Tooltip("A porcentagem de vida perdida que este item restaura. 0.5 = 50%, 1.0 = 100%")]
    [Range(0.1f, 1.0f)]
    public float porcentagemDeCura = 0.5f;

    public void Pegar()
    {
        MovimentarPersonagem jogador = FindAnyObjectByType<MovimentarPersonagem>();

        if (jogador != null)
        {
            jogador.CurarPorcentagem(porcentagemDeCura);
        }
        else
        {
            Debug.LogError("Não foi possível encontrar o script MovimentarPersonagem na cena!");
        }
    }
}
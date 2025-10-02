using UnityEngine;

public class ZonaDeVitoria : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jogador alcan�ou a zona de vit�ria!");
            GameManager.Instance.FinalizarJogo(true);
        }
    }
}
using UnityEngine;

public class ZonaDeVitoria : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jogador alcançou a zona de vitória!");
            GameManager.Instance.FinalizarJogo(true);
        }
    }
}
using UnityEngine;
using TMPro;

public class EntrarNoCaminhao : MonoBehaviour
{
    public GameObject player;
    public Camera cameraCaminhao;
    public Camera cameraPlayer;
    public Car scriptCarro;
    public TMP_Text mensagemUI;

    [Header("Configuração de Saída")]
    public Transform pontoDeSaida;

    private bool dentroDoCaminhao = false;
    private bool podeEntrar = false;

    void Start()
    {
        if (mensagemUI != null)
            mensagemUI.gameObject.SetActive(false);

        scriptCarro.enabled = false;
        cameraCaminhao.enabled = false;
    }

    void Update()
    {
        if (podeEntrar && !dentroDoCaminhao && Input.GetKeyDown(KeyCode.E))
        {
            Entrar();
        }
        else if (dentroDoCaminhao && Input.GetKeyDown(KeyCode.E))
        {
            Sair();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            podeEntrar = true;
            if (mensagemUI != null)
            {
                mensagemUI.text = "Pressione [E] para entrar no caminhão";
                mensagemUI.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            podeEntrar = false;
            if (mensagemUI != null)
                mensagemUI.gameObject.SetActive(false);
        }
    }

    void Entrar()
    {
        dentroDoCaminhao = true;
        if (mensagemUI != null)
            mensagemUI.gameObject.SetActive(false);

        player.SetActive(false);
        cameraPlayer.enabled = false;
        cameraCaminhao.enabled = true;
        scriptCarro.enabled = true;

        scriptCarro.LigarMotor();
    }

    void Sair()
    {
        dentroDoCaminhao = false;

        player.transform.position = pontoDeSaida.position;
        player.transform.rotation = pontoDeSaida.rotation;

        player.SetActive(true); 
        cameraPlayer.enabled = true;
        cameraCaminhao.enabled = false;
        scriptCarro.enabled = false;

        scriptCarro.PararCompleto();

        scriptCarro.DesligarMotor();
        
        scriptCarro.enabled = false;

        podeEntrar = false;
    }
}
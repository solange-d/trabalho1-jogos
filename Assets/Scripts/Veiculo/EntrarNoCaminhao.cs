using UnityEngine;
using TMPro;

public class EntrarNoCaminhao : MonoBehaviour
{
    [Header("Referências Principais")]
    public GameObject player;
    public Camera cameraCaminhao;
    public Camera cameraPlayer;
    public Car scriptCarro;
    public ArmaCaminhao armaDoCaminhao;

    [Header("UI")]
    public TMP_Text mensagemUI;
    public GameObject miraUI;

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
        if (armaDoCaminhao != null)
            armaDoCaminhao.enabled = false;
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
        if (miraUI != null)
            miraUI.SetActive(false);

        cameraCaminhao.enabled = true;
        scriptCarro.enabled = true;

        if (armaDoCaminhao != null)
        {
            armaDoCaminhao.enabled = true;
            armaDoCaminhao.cameraDaArma = this.cameraCaminhao;
        }

        scriptCarro.LigarMotor();
    }

    void Sair()
    {
        dentroDoCaminhao = false;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null) { rb.velocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }

        player.transform.position = pontoDeSaida.position;
        player.transform.rotation = pontoDeSaida.rotation;

        player.SetActive(true);
        if (cc != null) cc.enabled = true;

        cameraPlayer.enabled = true;
        if (miraUI != null)
            miraUI.SetActive(true);

        cameraCaminhao.enabled = false;
        if (armaDoCaminhao != null)
            armaDoCaminhao.enabled = false;

        scriptCarro.PararCompleto();
        scriptCarro.DesligarMotor();
        scriptCarro.enabled = false;

        podeEntrar = false;
    }
}
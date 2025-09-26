using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovimentarPersonagem : MonoBehaviour
{
    public CharacterController controle;
    public float velocidade = 6f;
    public float alturaPulo = 2f;
    public float gravidade = -20f;
    private int vida = 100;
    public Slider sliderVida;

    public AudioClip somPulo;
    public AudioClip somPasso;
    private AudioSource audioSrc;

    public Transform checaChao;
    public float raioEsfera = 0.4f;
    public LayerMask chaoMask;
    public bool estaNoChao;

    private Transform cameraTransform;
    private bool estahAbaixado = false;
    private bool levantarBloqueado;
    public float alturaLevantado, alturaAbaixado, posicaoCameraEmPe, posicaoCameraAbaixado;

    Vector3 velocidadeCai;

    void Start()
    {
        controle = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        audioSrc = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (vida<=0)
        {
            FimdeJogo();
            return;
        }
        estaNoChao = Physics.CheckSphere(checaChao.position, raioEsfera, chaoMask);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 mover = transform.right * x + transform.forward * z;
        controle.Move(mover * velocidade * Time.deltaTime);

        ChecarBloqueioAbaixado();

        if (!levantarBloqueado && estaNoChao && Input.GetButtonDown("Jump"))
        {
            velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
            audioSrc.PlayOneShot(somPulo);
        }

        if (!estaNoChao)
        {
            velocidadeCai.y += gravidade * Time.deltaTime;
        }

        controle.Move(velocidadeCai * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            AgacharLevantar();
        }

       
        if (estaNoChao && mover.magnitude > 0.1f) 
        {
            if (!audioSrc.isPlaying)
            {
                audioSrc.clip = somPasso;
                audioSrc.loop = true;
                audioSrc.Play();
            }
        }
        else
        {
            if (audioSrc.isPlaying && audioSrc.clip == somPasso)
            {
                audioSrc.Stop();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(checaChao.position, raioEsfera);
    }

    private void AgacharLevantar()
    {
        if (levantarBloqueado || estaNoChao == false)
        {
            return;
        }
        estahAbaixado = !estahAbaixado;
        if (estahAbaixado)
        {
            controle.height = alturaAbaixado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraAbaixado, 0);
        }
        else
        {
            controle.height = alturaLevantado;
            cameraTransform.localPosition = new Vector3(0, posicaoCameraEmPe, 0);
        }
    }

    private void ChecarBloqueioAbaixado()
    {
        RaycastHit hit;
        levantarBloqueado = Physics.Raycast(cameraTransform.position, Vector3.up, out hit, 1.1f);
    }

    public void AtualizarVida(int novaVida)
    {
        vida = Mathf.CeilToInt(Mathf.Clamp(vida + novaVida, 0, 100));
        sliderVida.value = vida;
    }

    private void FimdeJogo()
    {
        //desativa varios componentes
        // Time.timeScale = 0; // vai de 0 a 1 ... 1 a velocidade normal... 0  parado
        // entre 0 e 1 possivel configurar camera lenta
        // Camera.main.GetComponent<AudioListener>().enabled = false;
        if (GameManager.Instance != null)
            GameManager.Instance.ResetarProgresso();
        // GetComponentInChildren<M1911>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(0);
    }

}

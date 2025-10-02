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

    public int vidaMaxima = 100;
    private int vida;
    public Slider sliderVida;
    private bool jaMorreu = false;

    public AudioClip somPulo;
    public AudioClip somPasso;
    public AudioClip somCura;
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

        vida = vidaMaxima;
        sliderVida.maxValue = vidaMaxima;
        sliderVida.value = vida;
    }


    void Update()
    { 
        if (vida <= 0 && !jaMorreu)
        {
            FimdeJogo();
            return;
        }
        if (jaMorreu) return;
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

    public void AtualizarVida(int valor)
    {
        vida += valor;
        vida = Mathf.Clamp(vida, 0, vidaMaxima);
        sliderVida.value = vida;
    }

    public void CurarPorcentagem(float porcentagem)
    {
        if (vida >= vidaMaxima) return;

        int vidaPerdida = vidaMaxima - vida;

        int curaParaAplicar = Mathf.CeilToInt(vidaPerdida * porcentagem);

        AtualizarVida(curaParaAplicar);

        if (somCura != null)
        {
            audioSrc.PlayOneShot(somCura);
        }
    }

    private void FimdeJogo()
    {
        jaMorreu = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.FinalizarJogo(false);
        }

        if (controle != null) controle.enabled = false;
    }

}

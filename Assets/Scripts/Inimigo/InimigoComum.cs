using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Video;

public class InimigoComum : MonoBehaviour, ILevarDano
{
    private NavMeshAgent agente;
    private GameObject player;
    private Animator anim;
    private AudioSource audioSource;

    public float distanciaDoAtaque = 2.0f;
    public int vida = 50;
    public AudioClip somMorte;
    public AudioClip somPasso;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        VaiAtrasJogador();
        OlharParaJogador();
        if (vida <= 0)
        {
            Morrer();
        }
    }

    private void VaiAtrasJogador()
    {
        float distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanciaDoPlayer < distanciaDoAtaque)
        {
            agente.isStopped = true;
            Debug.Log("Ataque");
            anim.SetTrigger("ataque");
            anim.SetBool("podeAndar", false);
            anim.SetBool("pararAtaque", false);
            CorrigirRigiEntrar();
        }

        // player se afastou
        if (distanciaDoPlayer >= 3)
        {
            anim.SetBool("pararAtaque", true);
            CorrigirRigiSair();
        }

        if (anim.GetBool("podeAndar"))
        {
            agente.isStopped = false;
            agente.SetDestination(player.transform.position);
            anim.ResetTrigger("ataque");
        }
    }

    private void OlharParaJogador()
    {
        Vector3 direcaoOlhar = player.transform.position - transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlhar);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300);
    }

    private void CorrigirRigiEntrar()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void CorrigirRigiSair()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void LevarDano(int dano)
    {
        vida -= dano;
        agente.isStopped = true;
        anim.SetTrigger("levouTiro");
        anim.SetBool("podeAndar", false);
    }

    public void Morrer()
    {
        audioSource.clip = somMorte;
        audioSource.Play();

        agente.isStopped = true;
        anim.SetBool("podeAndar", false);
        anim.SetBool("pararAtaque", true);

        anim.SetBool("morreu", true);
        this.enabled = false;
    }

    public void DarDano()
    {
        player.GetComponent<MovimentarPersonagem>().AtualizarVida(-10);
    }

    public void Passo()
    {
        //ideal para sons repetitivos
        audioSource.PlayOneShot(somPasso, 0.05f);
    }
}

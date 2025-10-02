using System;
using UnityEngine;
using UnityEngine.AI;

public class InimigoComum : MonoBehaviour, ILevarDano
{
    protected NavMeshAgent agente;
    protected GameObject player;
    protected Animator anim;
    protected AudioSource audioSource;
    protected FieldOfView fov;
    protected PatrulharAleatorio pal;

    [Header("Atributos Base")]
    public float distanciaDoAtaque = 2.0f;
    public int vida = 50;
    public int danoBase = 10;

    [Header("Sons")]
    public AudioClip somMorte;
    public AudioClip somPasso;

    protected bool jaMorreu = false;
    public static event Action<InimigoComum> OnInimigoMorreu;

    protected virtual void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        fov = GetComponent<FieldOfView>();
        pal = GetComponent<PatrulharAleatorio>();
        AplicarDificuldade();
    }

    private void AplicarDificuldade()
    {
        if (GameManager.Instance == null) return;

        switch (GameManager.Instance.dificuldadeAtual)
        {
            case GameManager.Dificuldade.Facil:
                vida = 30;
                agente.speed = 2f;
                danoBase = 5;
                break;

            case GameManager.Dificuldade.Medio:
                vida = 50;
                agente.speed = 3.5f;
                danoBase = 10;
                break;

            case GameManager.Dificuldade.Dificil:
                vida = 70;
                agente.speed = 5f;
                danoBase = 15;
                break;
        }
    }

    protected virtual void Update()
    {
        if (vida <= 0)
        {
            Morrer();
            return;
        }

        if (fov.podeVerPlayer)
        {
            VaiAtrasJogador();
        }
        else
        {
            anim.SetBool("pararAtaque", true);
            agente.isStopped = false;
            pal.Andar();
        }
    }

    protected virtual void VaiAtrasJogador()
    {
        float distanciaDoPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanciaDoPlayer < distanciaDoAtaque)
        {
            agente.isStopped = true;
            anim.SetTrigger("ataque");
            anim.SetBool("podeAndar", false);
            anim.SetBool("pararAtaque", false);
        }

        if (distanciaDoPlayer >= 3)
        {
            anim.SetBool("pararAtaque", true);
        }

        if (anim.GetBool("podeAndar"))
        {
            agente.isStopped = false;
            agente.SetDestination(player.transform.position);
            anim.ResetTrigger("ataque");
        }
    }


    public virtual void LevarDano(int dano)
    {
        if (jaMorreu) return;

        vida -= dano;

        if (vida <= 0)
        {
            Morrer();
            return;
        }

        agente.isStopped = true;
        anim.SetTrigger("levouTiro");
        anim.SetBool("podeAndar", false);
    }

    public virtual void Morrer()
    {
        if (jaMorreu) return;

        jaMorreu = true;
        OnInimigoMorreu?.Invoke(this);

        if (GameManager.Instance != null && !(this is Boss))
        {
            GameManager.Instance.RegistrarMorte();
        }

        if (audioSource != null && somMorte != null)
            audioSource.PlayOneShot(somMorte);

        if (agente != null)
        {
            agente.isStopped = true;
            agente.enabled = false;
        }

        if (pal != null) pal.enabled = false;
        if (fov != null) fov.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }

        if (anim != null)
        {
            anim.SetBool("podeAndar", false);
            anim.SetBool("pararAtaque", true);
            anim.SetBool("morreu", true);
        }
    }

    public virtual void DarDano()
    {
        if (jaMorreu) return;

        player.GetComponent<MovimentarPersonagem>().AtualizarVida(-danoBase);
    }

    public void Passo()
    {
        if (audioSource != null && somPasso != null)
            audioSource.PlayOneShot(somPasso, 0.05f);
    }
}

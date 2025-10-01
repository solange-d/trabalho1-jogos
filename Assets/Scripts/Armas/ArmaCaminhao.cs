using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmaCaminhao : MonoBehaviour
{
    [Header("Referências")]
    public Text textoMunicao;
    [HideInInspector] public Camera cameraDaArma;
    public Transform pontoDeMira;

    [Header("Configuração de Tiro")]
    public int dano = 50;
    public LayerMask camadasParaIgnorar;
    public float distanciaDeMiraPadrao = 200f;
    public float larguraDoTiro = 0.5f;
    
    [Tooltip("Tempo em segundos entre cada disparo. Valores mais altos = tiro mais lento.")]
    public float cadenciaDeTiro = 1.5f;

    [Header("Efeitos Visuais")]
    public GameObject posEfeitoTiro;
    public GameObject efeitoTiro;
    public GameObject faisca;
    public GameObject efeitoExplosao;
    public float raioDaExplosao = 10f;

    [Header("Munição")]
    private int municaoTotal = 10;

    [Header("Áudio")]
    private AudioSource somDaArma;
    public AudioClip somDisparo;
    public AudioClip somSemBala;

    private bool estahAtirando;
    private RaycastHit hit;

    void Start()
    {
        estahAtirando = false;
        somDaArma = GetComponent<AudioSource>();
        AtualizarTextoMunicao();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && !estahAtirando)
        {
           
            if (municaoTotal > 0)
            {
                estahAtirando = true;
                municaoTotal--;
                somDaArma.PlayOneShot(somDisparo);
                StartCoroutine(Atirar());
            }
            else
            { 
                somDaArma.PlayOneShot(somSemBala);
            }
        }
        AtualizarTextoMunicao();
    }

    IEnumerator Atirar()
    {
        if (pontoDeMira == null)
        {
            yield break;
        }
        Ray ray = new Ray(pontoDeMira.position, pontoDeMira.forward);

        Vector3 pontoDeImpacto;
        int mascaraFinal = ~camadasParaIgnorar.value;
        float distanciaMaxima = 1000f;

        if (Physics.SphereCast(ray, larguraDoTiro, out hit, distanciaMaxima, mascaraFinal))
        {
            pontoDeImpacto = hit.point;
            Instantiate(faisca, pontoDeImpacto, Quaternion.FromToRotation(Vector3.up, hit.normal));
        }
        else
        {
            pontoDeImpacto = ray.GetPoint(distanciaDeMiraPadrao);
        }

        if (efeitoExplosao != null)
        {
            Instantiate(efeitoExplosao, pontoDeImpacto, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(pontoDeImpacto, raioDaExplosao);
        foreach (Collider colisorProximo in colliders)
        {
            if (colisorProximo.CompareTag("LevarDano"))
            {
                ILevarDano levarDano = colisorProximo.GetComponent<ILevarDano>();
                if (levarDano != null)
                {
                    levarDano.LevarDano(dano);
                }
            }
        }

        GameObject efeitoTiroObj = Instantiate(efeitoTiro, posEfeitoTiro.transform.position, posEfeitoTiro.transform.rotation);
        efeitoTiroObj.transform.parent = posEfeitoTiro.transform;

        yield return new WaitForSeconds(cadenciaDeTiro);

        Destroy(efeitoTiroObj);
        estahAtirando = false;
    }

    private void AtualizarTextoMunicao()
    {
        if (textoMunicao != null)
        {
            textoMunicao.text = municaoTotal.ToString();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M1911 : MonoBehaviour
{
    public Text textoMunicao;

    private Animator anim;
    private bool estahAtirando;
    private RaycastHit hit;

    [Header("Arma")]
    public GameObject posEfeitoTiro;
    public GameObject efeitoTiro;
    public GameObject faisca;
    public GameObject imgCursor;

    [Header("Munição")]
    private int carregador = 3;  
    private int municao = 17;

    [Header("Áudio")]
    private AudioSource somTiro;
    public AudioClip somDisparo; 
    public AudioClip somRecarregar; 
    public AudioClip somSemBala;    

    void Start()
    {
        estahAtirando = false;
        anim = GetComponent<Animator>();
        somTiro = GetComponent<AudioSource>();
        AtualizarTextoMunicao();
    }

    void Update()
    {
        if (anim.GetBool("acaoOcorrendo")) return;

        // tiro
        if (Input.GetButtonDown("Fire1") && !estahAtirando)
        {
            if (municao > 0)
            {
                estahAtirando = true;
                municao--;

                somTiro.PlayOneShot(somDisparo);
                StartCoroutine(Atirando());
            }
            else if (carregador > 0)
            {
                Recarregar();
            }
            else
            {
                somTiro.PlayOneShot(somSemBala);
            }
        }

        // recarregar
        if (Input.GetButtonDown("Recarregar"))
        {
            if (carregador > 0 && municao < 17)
            {
                Recarregar();
            }
            else
            {
                somTiro.PlayOneShot(somSemBala);
            }
        }
        AtualizarTextoMunicao();
        if (Input.GetButton("Fire2"))
        {
            anim.SetBool("mirar", true);
            imgCursor.SetActive(false);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 45, Time.deltaTime * 10);
        }
        else
        {
            anim.SetBool("mirar", false);
            imgCursor.SetActive(true);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime * 10);
        }
    }

    IEnumerator Atirando()
    {
        // centro da tela
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY, 0));

        anim.Play("AtirarM1911");

        // efeito visual
        GameObject efeitoTiroObj = Instantiate(efeitoTiro, posEfeitoTiro.transform.position, posEfeitoTiro.transform.rotation);
        efeitoTiroObj.transform.parent = posEfeitoTiro.transform;

        GameObject faiscaObj = null;

        // colisão
        if (Physics.SphereCast(ray, 0.1f, out hit))
        {
            faiscaObj = Instantiate(faisca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            if (hit.transform.tag == "Arrastar")
            {
                Vector3 direcaoBala = ray.direction;
                hit.rigidbody.AddForceAtPosition(direcaoBala * 500, hit.point);
            }
            else
            {
                if(hit.transform.tag == "LevarDano")
                {
                    ILevarDano levarDano = hit.transform.GetComponent<ILevarDano>();
                    levarDano.LevarDano(5);
                }
            }
        }

        yield return new WaitForSeconds(0.9f);
        Destroy(efeitoTiroObj);
        if (faiscaObj != null) Destroy(faiscaObj);

        estahAtirando = false;
    }

    private void Recarregar()
    {
        anim.Play("RecarregarM1911");
        somTiro.PlayOneShot(somRecarregar);

        municao = 17;
        carregador--;
    }

    private void AtualizarTextoMunicao()
    {
        textoMunicao.text = municao.ToString() + "/" + carregador.ToString();
    }

    public void AddCarregador()
    {
        carregador++;
        AtualizarTextoMunicao();
    }
}

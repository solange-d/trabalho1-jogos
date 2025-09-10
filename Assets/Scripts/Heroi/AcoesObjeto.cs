using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcoesObjeto : MonoBehaviour
{
    private IdentificarObjeto idObjeto;
    private bool pegou = false;
    public AudioClip somPegar;
    private AudioSource audioSrc;

    void Start()
    {
        idObjeto = GetComponent<IdentificarObjeto>();
        audioSrc = GetComponent<AudioSource>();
    }

   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && idObjeto.GetObjPegar() != null)
        {
            Pegar();
        }

        if (Input.GetKeyDown(KeyCode.F) && idObjeto.GetObjArrastar() != null)
        {
            if (!pegou)
            {
                Arrastar();
            }
            else
            {
                Soltar();
            }
            pegou = !pegou;
        }
    }

    private void Pegar()
    {
        IPegavel obj = idObjeto.GetObjPegar().GetComponent<IPegavel>();
        obj.Pegar();
        Destroy(idObjeto.GetObjPegar());

        if (somPegar != null && audioSrc != null)
        {
            audioSrc.PlayOneShot(somPegar);
        }

        idObjeto.EsconderTexto();
    }

    private void Arrastar()
    {
        GameObject obj = idObjeto.GetObjArrastar();
        obj.AddComponent<DragDrop>();
        obj.GetComponent<DragDrop>().Ativar();
        idObjeto.enabled = false;
    }

    private void Soltar()
    {
        GameObject obj = idObjeto.GetObjArrastar();
        Destroy(obj.GetComponent<DragDrop>());
        idObjeto.enabled = true;
    }
}

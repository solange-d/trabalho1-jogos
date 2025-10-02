using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdentificarObjeto : MonoBehaviour
{
    private float distanciaAlvo;
    private GameObject objArrastar, objPegar, objAlvo;
    public Text textoTecla, textoMsg;

    void Update()
    {
        if (Time.frameCount % 5 == 0)
        {
            RaycastHit hit;
            int ignorarLayer = 1 << 7;
            ignorarLayer = ~ignorarLayer;

            if (Physics.SphereCast(transform.position, 0.1f, transform.TransformDirection(Vector3.forward), out hit, 5, ignorarLayer))
            {
                GameObject objetoAtingido = hit.transform.gameObject;

                if (objAlvo != null && objetoAtingido != objAlvo)
                {
                    Outline outlineAntigo = objAlvo.GetComponent<Outline>();
                    if (outlineAntigo != null) outlineAntigo.OutlineWidth = 0f;
                }

                objAlvo = objetoAtingido;

                Outline outlineAtual = objAlvo.GetComponent<Outline>();

                if (objAlvo.CompareTag("Arrastar") || objAlvo.CompareTag("Pegar"))
                {
                    if (outlineAtual != null)
                    {
                        outlineAtual.OutlineWidth = 5f;
                    }

                    if (objAlvo.CompareTag("Arrastar"))
                    {
                        objArrastar = objAlvo;
                        objPegar = null;
                        textoTecla.color = new Color(248 / 255f, 248 / 255f, 13 / 255f);
                        textoMsg.color = textoTecla.color;
                        textoTecla.text = "[F]";
                        textoMsg.text = "Arrastar/Soltar";
                    }
                    else 
                    {
                        objPegar = objAlvo;
                        objArrastar = null;
                        textoTecla.color = new Color(51 / 255f, 1, 0);
                        textoMsg.color = textoTecla.color;
                        textoTecla.text = "[F]";

                        if (objAlvo.GetComponent<ItemDeCura>() != null)
                        {
                            textoMsg.text = "Pegar Kit de Cura";
                        }
                        else
                        {
                            textoMsg.text = "Pegar";
                        }
                    }
                }
                else
                {
                    if (outlineAtual != null) outlineAtual.OutlineWidth = 0f;
                    EsconderTexto();
                    objPegar = null;
                    objArrastar = null;
                }
            }
            else
            {
                if (objAlvo != null)
                {
                    Outline outlineAntigo = objAlvo.GetComponent<Outline>();
                    if (outlineAntigo != null) outlineAntigo.OutlineWidth = 0f;
                    objAlvo = null;
                    EsconderTexto();
                    objPegar = null;
                    objArrastar = null;
                }
            }
        }
    }

    public float GetDistanciaAlvo() { return distanciaAlvo; }
    public GameObject GetObjArrastar() { return objArrastar; }
    public GameObject GetObjPegar() { return objPegar; }
    public void EsconderTexto() { textoTecla.text = ""; textoMsg.text = ""; }
}
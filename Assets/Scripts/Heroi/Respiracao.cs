using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respiracao : MonoBehaviour
{
    private bool estahInspirando = true;
    public float minAltura = -0.035f;
    public float maxAltura = 0.035f;

    [Range(0f, 5f)]
    public float forcaResp = 1f;

    private float movimento;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (estahInspirando)
        {
            movimento = Mathf.Lerp(movimento, maxAltura, Time.deltaTime * forcaResp);
            transform.localPosition = new Vector3(transform.localPosition.x, movimento, transform.localPosition.z);

            if(movimento >= maxAltura - 0.01f)
            {
                estahInspirando = false;
            }
        }
        else
        {
            movimento = Mathf.Lerp(movimento, minAltura, Time.deltaTime * forcaResp);
            transform.localPosition = new Vector3(transform.localPosition.x, movimento, transform.localPosition.z);

            if(movimento <= minAltura + 0.01f)
            {
                estahInspirando = true;
            }
        }

        if(forcaResp > 1)
        {
            // serve para descer a for�a de respira��o para 1. No futuro vamos adicionar um c�digo para que o her�i, ao correr, tenha uma respira��o ofegante.
            forcaResp = Mathf.Lerp(forcaResp, 1f, Time.deltaTime * 0.2f);
        }
        
    }
}

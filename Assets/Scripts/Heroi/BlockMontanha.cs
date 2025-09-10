using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMontanha : MonoBehaviour
{
    public GameObject texto;

    void OnTriggerEnter(Collider other)
    {
        texto.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        texto.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

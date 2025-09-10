using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineM1911 : MonoBehaviour, IPegavel
{
    public void Pegar()
    {
        M1911 m = GameObject.FindWithTag("Arma").GetComponent<M1911>();
        m.AddCarregador();
    }
}

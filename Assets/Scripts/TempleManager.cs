using System.Collections.Generic;
using SunTemple;
using UnityEngine;

public class TempleManager : MonoBehaviour
{
    [Header("Portas")]
    public Door[] portasNormais;      // 3 portas principais
    public Door portaLateral;         // Porta da sala do boss

    [Header("Inimigos")]
    public InimigoComum[] inimigosSala; // inimigos comuns
    public InimigoComum boss;           // boss

    private int inimigosMortos = 0;
    private bool lateralAberta = false;
    private bool normaisAbertas = false;

    private void OnEnable()
    {
        InimigoComum.OnInimigoMorreu += VerificarInimigos;
    }

    private void OnDisable()
    {
        InimigoComum.OnInimigoMorreu -= VerificarInimigos;
    }

    private void Start()
    {
        foreach (Door d in portasNormais)
            d.IsLocked = true;

        if (portaLateral != null)
            portaLateral.IsLocked = true;
    }

    private void VerificarInimigos(InimigoComum inimigo)
    {
        // Se é inimigo comum
        foreach (InimigoComum i in inimigosSala)
        {
            if (i == inimigo)
            {
                inimigosMortos++;
                break;
            }
        }

        // Abre porta lateral quando todos morrerem
        if (!lateralAberta && inimigosMortos >= inimigosSala.Length)
        {
            lateralAberta = true;
            if (portaLateral != null)
            {
                portaLateral.UnlockWithFeedback();
                Debug.Log("Porta lateral desbloqueada!");
            }
        }

        // Abre portas principais quando boss morrer
        if (!normaisAbertas && inimigo == boss)
        {
            normaisAbertas = true;
            foreach (Door d in portasNormais)
                d.UnlockWithFeedback();

            Debug.Log("Portas principais desbloqueadas!");
        }
    }
}

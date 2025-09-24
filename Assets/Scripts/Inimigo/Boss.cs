using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : InimigoComum
{
    protected override void Start()
    {
        base.Start();
        AplicarDificuldadeBoss();
    }

    private void AplicarDificuldadeBoss()
    {
        if (GameManager.Instance == null) return;

        switch (GameManager.Instance.dificuldadeAtual)
        {
            case GameManager.Dificuldade.Facil:
                vida = 100;
                agente.speed = 5f;
                danoBase = 15;
                distanciaDoAtaque = 3f;
                break;

            case GameManager.Dificuldade.Medio:
                vida = 150;
                agente.speed = 7f;
                danoBase = 20;
                distanciaDoAtaque = 4f;
                break;

            case GameManager.Dificuldade.Dificil:
                vida = 200;
                agente.speed = 9f;
                danoBase = 40;
                distanciaDoAtaque = 5f;
                break;
        }
 
    }

    public override void LevarDano(int dano)
    {
        if (jaMorreu) return;
        base.LevarDano(dano);
    }

}

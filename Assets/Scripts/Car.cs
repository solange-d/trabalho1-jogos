using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float motorForce, streetForce, brakeForce;
    public WheelCollider WF_L, WF_R, WR_L, WR_R;

    public Transform WheelMesh_FR_L, WheelMesh_FR_R, WheelMesh_RE_L, WheelMesh_RE_R;

    void Update()
    {
        // Entrada do jogador
        float vertical = -Input.GetAxis("Vertical") * motorForce;
        float horizontal = Input.GetAxis("Horizontal") * streetForce;

        // Torque nas rodas traseiras (motrizes)
        WR_L.motorTorque = vertical;
        WR_R.motorTorque = vertical;

        // Direção nas rodas dianteiras
        WF_L.steerAngle = horizontal;
        WF_R.steerAngle = horizontal;

        // Freio manual (Barra de espaço)
        if (Input.GetKey(KeyCode.Space))
        {
            WR_L.brakeTorque = brakeForce;
            WR_R.brakeTorque = brakeForce;
        }
        else
        {
            WR_L.brakeTorque = 0;
            WR_R.brakeTorque = 0;
        }

        // Atualizar posição + rotação das rodas
        UpdateWheelPose(WF_L, WheelMesh_FR_L);
        UpdateWheelPose(WF_R, WheelMesh_FR_R);
        UpdateWheelPose(WR_L, WheelMesh_RE_L);
        UpdateWheelPose(WR_R, WheelMesh_RE_R);
    }

    void UpdateWheelPose(WheelCollider col, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);

        trans.position = pos;   // <- Faltava isso no seu código
        trans.rotation = rot;
    }
}

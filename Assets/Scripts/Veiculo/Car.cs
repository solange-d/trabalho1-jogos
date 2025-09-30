using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // Garante que o caminhão sempre tenha um Rigidbody
public class Car : MonoBehaviour
{
    public float motorForce, streetForce, brakeForce;
    public WheelCollider WF_L, WF_R, WR_L, WR_R;
    public Transform WheelMesh_FR_L, WheelMesh_FR_R, WheelMesh_RE_L, WheelMesh_RE_R;

    [Header("Áudio do Motor")]
    public AudioSource audioSourceMotor;
    public AudioClip somLigar;
    public AudioClip somMotorAcelerando;

    [Header("Configuração do Pitch")]
    public float pitchMinimo = 0.5f;
    public float pitchMaximo = 2.0f;
    private float velocidadeAtualParaPitch;

    private Rigidbody rb;

    void Start()
    {
        
        rb = GetComponent<Rigidbody>();

        if (audioSourceMotor == null)
        {
            audioSourceMotor = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float motorInput = -vertical * motorForce;
        float horizontal = Input.GetAxis("Horizontal") * streetForce;

        WR_L.motorTorque = motorInput;
        WR_R.motorTorque = motorInput;

        WF_L.steerAngle = horizontal;
        WF_R.steerAngle = horizontal;

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

        UpdateWheelPose(WF_L, WheelMesh_FR_L);
        UpdateWheelPose(WF_R, WheelMesh_FR_R);
        UpdateWheelPose(WR_L, WheelMesh_RE_L);
        UpdateWheelPose(WR_R, WheelMesh_RE_R);

        AtualizarSomMotor(vertical);
    }

    public void PararCompleto()
    {
        WR_L.motorTorque = 0;
        WR_R.motorTorque = 0;
        
        WF_L.steerAngle = 0;
        WF_R.steerAngle = 0;

        WR_L.brakeTorque = brakeForce;
        WR_R.brakeTorque = brakeForce;
        WF_L.brakeTorque = brakeForce;
        WF_R.brakeTorque = brakeForce;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void UpdateWheelPose(WheelCollider col, Transform mesh)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        mesh.position = pos;
        mesh.rotation = rot;
    }

    private void AtualizarSomMotor(float inputVertical)
    {
        if (audioSourceMotor.isPlaying)
        {
            velocidadeAtualParaPitch = Mathf.Abs(WR_L.rpm) / 100f;
            float novoPitch = Mathf.Lerp(pitchMinimo, pitchMaximo, velocidadeAtualParaPitch);
            audioSourceMotor.pitch = Mathf.Clamp(novoPitch, pitchMinimo, pitchMaximo);
        }
    }

    public void LigarMotor()
    {
        WR_L.brakeTorque = 0;
        WR_R.brakeTorque = 0;
        WF_L.brakeTorque = 0;
        WF_R.brakeTorque = 0;
        audioSourceMotor.PlayOneShot(somLigar);
        audioSourceMotor.clip = somMotorAcelerando;
        audioSourceMotor.loop = true;
        audioSourceMotor.PlayDelayed(somLigar.length);
    }

    public void DesligarMotor()
    {
        audioSourceMotor.Stop();
    }
}
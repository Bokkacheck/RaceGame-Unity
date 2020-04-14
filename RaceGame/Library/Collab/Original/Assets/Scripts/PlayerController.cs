using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    WheelCollider wcLF;
    [SerializeField]
    WheelCollider wcLB;
    [SerializeField]
    WheelCollider wcRF;
    [SerializeField]
    WheelCollider wcRB;
    [SerializeField]
    TrailRenderer tRR;
    [SerializeField]
    TrailRenderer tRL;
    [SerializeField]
    GameObject[] tires;
    [SerializeField]
    ParticleSystem psFire;
    [SerializeField]
    float torque = 1000.0f;
    [SerializeField]
    float steer = 40.0f;

    [SerializeField]
    Camera prim;
    [SerializeField]
    Camera sec;

    int trackRPM = 1;
    int currentGear = 1;

    Speedometer spdMtr;
    RpmMeter rpmMeter;
    Rigidbody rb;
    bool primCam = true;
    bool direction = true;
    float forward;
    private float rpm;

    [SerializeField]
    Text LapTimer;
    public float time;
    public float lapTime;
    bool startstop = false;
    private TimeSpan vreme;

    [SerializeField]
    GameObject[] Checkpoints;
    public int cpCount = 0;
    Vector3 savedPos;
    Quaternion SavedRot;
    Vector3 spawnPos;
    Quaternion spawnRot;




    //sound
    public float[] MinRpmTable = { 500, 750, 1120, 1669, 2224, 2783, 3335, 3882, 4355, 4833, 5384, 5943, 6436, 6928, 7419, 7900 };
    public float[] NormalRpmTable = { 720, 930, 1559, 2028, 2670, 3145, 3774, 4239, 4721, 5194, 5823, 6313, 6808, 7294, 7788, 8261 };
    public float[] MaxRpmTable = { 920, 1360, 1829, 2474, 2943, 3575, 4036, 4525, 4993, 5625, 6123, 6616, 7088, 7589, 8060, 10000 };
    public float[] PitchingTable = { 0.12f, 0.12f, 0.12f, 0.12f, 0.11f, 0.10f, 0.09f, 0.08f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f };
    public float RangeDivider = 4f;
    public AudioSource[] Audio;
    public AudioSource AudioStart;
    public AudioSource AudioBrake;
    void Start()
    {
        AudioStart.Play();
        tRL.enabled = false;
        tRR.enabled = false;
        spdMtr = GameObject.Find("Speedometer").GetComponent<Speedometer>();
        rpmMeter = GameObject.Find("RpmMeter").GetComponent<RpmMeter>();
        LapTimer = GameObject.Find("LapTimer").GetComponent<Text>();
        Checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        savedPos = spawnPos = transform.position;
        SavedRot = spawnRot = transform.rotation;
        rb = GetComponent<Rigidbody>();
        prim.gameObject.SetActive(true);
        sec.gameObject.SetActive(false);
        foreach(AudioSource asrc in Audio)
        {
            asrc.Play();
        }
        
    }
    void FixedUpdate()
    {
        CountTime();
        Movement();
        Brake();
        AnimateWheel();
        RpmParticles();
        sound();
        spdMtr.Speed = rb.velocity.magnitude*3.6f;  //"m/s" -> "km/h"
    }
    void LateUpdate()
    {
        CameraSwitch();
        VratiNaPoint();
    }
    private void Movement()
    {
        forward = Input.GetAxis("Vertical");
        float steering = Input.GetAxis("Horizontal");
        if(forward<0 && (rb.velocity.magnitude*3.6) > 38)
        {
            forward = 0f;
        }
        wcLB.motorTorque = forward * torque;
        wcRB.motorTorque = forward * torque;
        if (steering != 0)
        {
            wcLF.steerAngle = steering * steer;
            wcRF.steerAngle = steering *steer;
        }
    }
    private void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            AudioBrake.loop=true;
            if(!AudioBrake.isPlaying)
                AudioBrake.Play();
            wcRB.brakeTorque = 3000;
            wcLB.brakeTorque = 3000;
            if (wcRB.isGrounded)
            {
                tRR.enabled = true;
            }
            else
            {
                tRR.enabled = false;
            }
            if (wcLB.isGrounded)
            {
                tRL.enabled = true;
            }
            else
            {
                tRL.enabled = false;
            }
        }
        else
        {
            AudioBrake.loop = false;
            if (AudioBrake.isPlaying)
                AudioBrake.Stop();
            wcRB.brakeTorque = 0;
            wcLB.brakeTorque = 0;
            tRL.enabled = false;
            tRR.enabled = false;
            tRL.Clear();
            tRR.Clear();
        }
    }
    private void AnimateWheel()
    {
        Quaternion quat;
        Quaternion quat2;
        Vector3 position;
        Vector3 position2;
        wcLF.GetWorldPose(out position, out quat);
        wcLB.GetWorldPose(out position2, out quat2);
        tires[0].transform.rotation = quat;
        tires[1].transform.rotation = quat2;
        tires[2].transform.rotation = quat;
        tires[3].transform.rotation = quat2;
    }
    private void RpmParticles()
    {
        rpm = 800;
        string gear = "";
        float speed = rb.velocity.magnitude * 3.6f;
        if (speed < 50)
        {
            gear = "1";
            rpm += speed * 6500 / 50.0f;
        }
        else if (speed < 90)
        {
            gear = "2";
            rpm += speed * 6500 / 90.0f;
        }
        else if (speed < 130)
        {
            gear = "3";
            rpm += speed * 6500 / 130.0f;
        }
        else if (speed < 170)
        {
            gear = "4";
            rpm += speed * 6500 / 170.0f;
        }
        else
        {
            gear = "5";
            rpm += speed * 7000 / 240.0f;
        }
        if (forward< 0 && speed < 5)
        {
            direction = false;

        }else if(forward > 0 &&  speed < 5)
        {
            direction = true;
        }
        if (!direction)
        {
            gear = "R";
        }
        if(speed<1)
        {
            rpm = 800;
            gear = "N";
        }
        rpmMeter.Speed = rpm;
        rpmMeter.Gear = gear;
        if (rpm > 7200)
        {
            if (!psFire.isPlaying)
            {
                psFire.Play();
            }
        }
    }

    private void CameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (primCam)
            {
                prim.gameObject.SetActive(false);
                sec.gameObject.SetActive(true);
                primCam = false;
            }
            else
            {
                prim.gameObject.SetActive(true);
                sec.gameObject.SetActive(false);
                primCam = true;
            }
        }
    }

    private void sound()
    {
        float rpm = this.rpm * 1.25f;
        for (int i = 0; i < 16; i++)
        {
            if (i == 0)
            {
                //Set Audio[1
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 1)
            {
                //Set Audio[2
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 2)
            {
                //Set Audio[3
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 3)
            {
                //Set Audio[4
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 4)
            {
                //Set Audio[5
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 5)
            {
                //Set Audio[6
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 6)
            {
                //Set Audio[7
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 7)
            {
                //Set Audio[8
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 8)
            {
                //Set Audio[9
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 9)
            {
                //Set Audio[10
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 10)
            {
                //Set Audio[11
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ((ReducedRPM / Range) * 2f) - 1f;
                    //Audio[i].volume = 0.0f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 11)
            {
                //Set Audio[12
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 12)
            {
                //Set Audio[13
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 13)
            {
                //Set Audio[14
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 14)
            {
                //Set Audio[15
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
                else if (rpm > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = rpm - MaxRpmTable[i];
                    Audio[i].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //Audio[i].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 15)
            {
                //Set Audio[16
                if (rpm < MinRpmTable[i])
                {
                    Audio[i].volume = 0.0f;
                }
                else if (rpm >= MinRpmTable[i] && rpm < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = rpm - MinRpmTable[i];
                    Audio[i].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    Audio[i].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (rpm >= NormalRpmTable[i] && rpm <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = rpm - NormalRpmTable[i];
                    Audio[i].volume = 1f;
                    float PitchMath = (ReducedRPM * (PitchingTable[i] + 0.1f)) / Range;
                    Audio[i].pitch = 1f + PitchMath;
                }
            }
        }
    }
    //laptajmer i bice checkpointi
   
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NewLap") && !startstop)
        {
            lapTime = Time.time;
            startstop = true;
        }
        if (other.CompareTag("CheckPoint"))
        {
            other.enabled = false;
            savedPos = other.transform.position;
            SavedRot = other.transform.rotation;
            cpCount++;
        }
        if(other.CompareTag("NewLap") && Checkpoints.Length==cpCount)
        {
            //mozda pokrenuti neku novu scenu kad se zavrsi trka a ovaj deo zanemariti u tom slucaju
            GameObject c = GameObject.Find("Canvas");
            AddTextToCanvas("Your time is "+vreme.ToString("mm\\:ss\\:ff")+"!", c);
            startstop = false;
            cpCount = 0;
            SavedRot = other.transform.rotation;
            savedPos = other.transform.position;
            foreach(GameObject cp in Checkpoints)
            {
                cp.GetComponent<Collider>().enabled = true;
            }

        }
    }

    private void CountTime()
    {
        if (startstop)
        {
            time = Time.time - lapTime;
            vreme = TimeSpan.FromSeconds(time);
            LapTimer.text = vreme.ToString("mm\\:ss\\:ff");
        }

    }

    void VratiNaPoint()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rb.velocity = Vector3.zero;
            transform.rotation = SavedRot;
            transform.position = savedPos + new Vector3(0,2,0);

        }
    }

    public static Text AddTextToCanvas(string textString, GameObject canvasGameObject)
    {
        Text text = canvasGameObject.AddComponent<Text>();
        text.text = textString;

        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        text.font = ArialFont;
        text.fontSize = 18;
        text.material = ArialFont.material;

        return text;
    }
}

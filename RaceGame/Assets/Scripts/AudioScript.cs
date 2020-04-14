using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AudioScript : NetworkBehaviour
{
    public float[] MinRpmTable = { 500, 750, 1120, 1669, 2224, 2783, 3335, 3882, 4355, 4833, 5384, 5943, 6436, 6928, 7419, 7900 };
    public float[] NormalRpmTable = { 720, 930, 1559, 2028, 2670, 3145, 3774, 4239, 4721, 5194, 5823, 6313, 6808, 7294, 7788, 8261 };
    public float[] MaxRpmTable = { 920, 1360, 1829, 2474, 2943, 3575, 4036, 4525, 4993, 5625, 6123, 6616, 7088, 7589, 8060, 10000 };
    public float[] PitchingTable = { 0.12f, 0.12f, 0.12f, 0.12f, 0.11f, 0.10f, 0.09f, 0.08f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f };
    public float RangeDivider = 4f;
    public AudioSource[] Audio;
    public AudioSource AudioStart;
    public AudioSource AudioBrake;
    private PlayerController plc;

    private Rigidbody rb;

    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        AudioStart.Play();
        AudioBrake.Play();
        foreach (AudioSource asrc in Audio)
        {
            asrc.Play();
        }
        plc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }
    void LateUpdate()
    {
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey("joystick button 0")) && rb.velocity.magnitude > 0.5)
        {
            AudioBrake.loop = true;
            if (!AudioBrake.isPlaying)
            {
                AudioBrake.Play();
            }
        }
        else
        {
            AudioBrake.loop = false;
            if (AudioBrake.isPlaying)
            {
                AudioBrake.Stop();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Sound();
    }
    private void Sound()
    {
        float rpm = plc.rpm * 1.25f;
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
}

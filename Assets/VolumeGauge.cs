using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class VolumeGauge : MonoBehaviour
{
    private Slider slider;
    private Manager manager;
    private MicAudioSource AS;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        AS = GameObject.Find("MyAudioSource").GetComponent<MicAudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.GetState() == State.Speaking)
        {
            // 声の音量でゲージをチャージする
            float db = AS.now_dB;
            slider.value += Math.Max((db + 80 - 50) * 0.6f, 0);
            slider.value -= 0.5f;
        }
        else if (manager.GetState() == State.Prepare)
        {
            slider.value = 0f;
        }
    }
}

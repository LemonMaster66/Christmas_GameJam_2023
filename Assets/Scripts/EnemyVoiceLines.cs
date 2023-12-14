using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVoiceLines : MonoBehaviour
{
    [Header("SFX")]
    public AudioSource audioSource;
    public AudioClip[] VoiceLine;

    private float VoiceLineCooldown;


    private void VoiceLineCooldownFunction()
    {
        VoiceLineCooldown -= Time.deltaTime;
        if(VoiceLineCooldown <= 0f)
        {
            VoiceLineCooldown = 30;
            audioSource.clip = VoiceLine[Random.Range(1,6)];
        }
        return;
    }

    void Update()
    {
        if(VoiceLineCooldown > 0) VoiceLineCooldownFunction();
    }
}
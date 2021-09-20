using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPitch : MonoBehaviour
{
    private AudioSource m_Source;
    public AudioClip m_Clip;

    public float m_Min = 0.5f;
    public float m_Max = 0.9f;

    [SerializeField] private float m_AudioCooldown = 0.3f;
    private float timer = 0;

    private void Start()
    {
        m_Source = GetComponent<AudioSource>();
        m_Source.clip = m_Clip;
    }

    public void Play()
    {
        m_Source.pitch = Random.Range(m_Min, m_Max);
        timer += Time.deltaTime;
        if (timer >= m_AudioCooldown)
        {
            m_Source.PlayOneShot(m_Source.clip);
            timer = 0f;
        }
    }    
}

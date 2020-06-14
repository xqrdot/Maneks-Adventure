﻿#pragma warning disable 0649
using UnityEngine;

public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.5f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    public float randomPitch = 0.1f;

    AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
    }
    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }
}

public class ManagerSFX : MonoBehaviour
{
    public static ManagerSFX instance;
    [SerializeField] Sound[] sounds;

    AudioSource source = null;
	[SerializeField] AudioClip music = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one ManagerSFX");
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        source = StaticStorage.instance.GameManager.GetComponent<AudioSource>();
		source.PlayOneShot(music, 0.33f);
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }

        Debug.LogWarning("AudioSFX: Sound name not found! " + _name);
    }
}

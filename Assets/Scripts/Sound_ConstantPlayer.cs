#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_ConstantPlayer : MonoBehaviour
{
    [Range(0.001f, 1f)]
    [SerializeField] private float volume = 0.02f;
    [SerializeField] private float pitch = 1;
    [SerializeField] private AudioClip constantNoise;
	[Space(20)]
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 20f;

    [Header("Randomization")]
    [SerializeField] private bool randomizePitch = true;
    [Range(0, 2f)]
    [SerializeField] private float pitchVariation = 0f;

    AudioSource source;


    private void Start()
    {
        if (source == null) { gameObject.AddComponent<AudioSource>(); }
        source = GetComponent<AudioSource>();

        PlaySound();
    }

    void PlaySound()
    {
        source.loop = true;
        source.volume = volume;
        source.clip = constantNoise;

		source.spatialBlend = 1;
		source.dopplerLevel = 1;
		source.rolloffMode = AudioRolloffMode.Linear;
		source.minDistance = minDistance;
		source.maxDistance = maxDistance;

        if (randomizePitch)
        {
            source.pitch = 1 + Random.Range(-pitchVariation, pitchVariation);
        }
        else
        {
            source.pitch = pitch;
        }

        source.Play();
    }
}

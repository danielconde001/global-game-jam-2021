using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFootsteps : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] List<AudioClip> footsteps;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();    
    }

    public void PlayFootstepSound()
    {
        audioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Count)]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFootsteps : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> footsteps;
    [SerializeField] private List<AudioClip> idles;
    [SerializeField] private AudioClip attack;
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip thud;
    [SerializeField] private float idleSoundDelay;
    
    private float idleSoundTimer = 0.0f;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();    
    }

    public void PlayFootstepSound()
    {
        audioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Count)]);
    }

    public void PlayAttackSound()
    {
        audioSource.clip = attack;
        audioSource.Play();
    }

    public void PlayDieSound()
    {
        audioSource.clip = die;
        audioSource.Play();
        audioSource.PlayOneShot(thud);
    }

    private void Update()
    {
        if(!enemyHealth.IsDead)
        {
            PlayIdleSound();
        }
    }

    private void PlayIdleSound()
    {
        idleSoundTimer -= Time.deltaTime;

        if(idleSoundTimer <= 0.0f)
        {
            idleSoundTimer = idleSoundDelay;
            audioSource.clip = idles[Random.Range(0, idles.Count)];
            audioSource.Play();
        }
    }
}

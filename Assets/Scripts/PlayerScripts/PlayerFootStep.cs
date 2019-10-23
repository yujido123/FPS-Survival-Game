using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStep : MonoBehaviour
{
    private CharacterController characterController;
    private AudioSource footStepSound;

  
    public AudioClip[] footStepClips;

    [HideInInspector]
    public float volumeMin, volumeMax;

    [HideInInspector]
    public float stepDistance;

    private float accumulatedDistance;
    
    void Awake()
    {
        characterController = GetComponentInParent<CharacterController>();
        footStepSound = GetComponent<AudioSource>();
    }


    void Update()
    {
        CheckToPlayFootstepSound();
    }


    void CheckToPlayFootstepSound()
    {

        
        if (!characterController.isGrounded)
            return;

        
        if (characterController.velocity.sqrMagnitude > 0)
        {
            accumulatedDistance += Time.deltaTime;
            
            if(accumulatedDistance > stepDistance)
            {
                footStepSound.volume = Random.Range(volumeMin, volumeMax);
                footStepSound.clip = footStepClips[Random.Range(0, footStepClips.Length)];
                footStepSound.Play();
                accumulatedDistance = 0f;
            }
        }
        else
        {
            accumulatedDistance = 0f;
        }
    }
}

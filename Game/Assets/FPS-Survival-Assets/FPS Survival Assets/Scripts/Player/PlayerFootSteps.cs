using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerFootSteps : MonoBehaviour
{
    private AudioSource footStep_Sound;

    [SerializeField]
    private AudioClip[] footStep_Clip;

    private CharacterController character_controller;

    [HideInInspector]
    public float volume_Min , volume_Max ;

    private float Accumulated_Distance;

    [HideInInspector]
    public float step_Distance;

    void Awake()
    {
        footStep_Sound = GetComponent<AudioSource>();

        character_controller = GetComponentInParent<CharacterController>();
    }
  

    void Update()
    {
        CheckToPlayFootStepSound();
    }

    void CheckToPlayFootStepSound()
    {
        //If we are not on the ground
        if(!character_controller.isGrounded)
            return;

        if(character_controller.velocity.sqrMagnitude > 0)
        {
            //Accumulated distance is the value how far can i go
            //e.g. make a step or sprint, or move while crouching
            //Until we play footstep sound

            Accumulated_Distance += Time.deltaTime;

            if(Accumulated_Distance > step_Distance)
            {
                footStep_Sound.volume = Random.Range(volume_Min, volume_Max);
                footStep_Sound.clip = footStep_Clip[Random.Range(0, footStep_Clip.Length)];
                footStep_Sound.Play();
             
                Accumulated_Distance = 0f;
            }
        }
        else
        {
            Accumulated_Distance = 0f;
        }
    }
}

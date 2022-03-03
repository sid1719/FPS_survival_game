using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerfootsteps : MonoBehaviour
{   
    private AudioSource footstep_sound;

    [SerializeField]
    private AudioClip[] footstep_clip;

    private CharacterController character_Controller;

    [HideInInspector]
    public float vol_min, vol_max;

    private float accumulated_dist;

    [HideInInspector]
    public float step_dist;


    // Start is called before the first frame update
    void Awake()
    {
        footstep_sound = GetComponent<AudioSource>();
        character_Controller = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        checkToPlayFootstepSound(); 
    }

    void checkToPlayFootstepSound()
    {
        if (!character_Controller.isGrounded)
            return;
        if(character_Controller.velocity.sqrMagnitude>0)
        {
            accumulated_dist += Time.deltaTime;
            
            if(accumulated_dist>step_dist)
            {
                footstep_sound.volume = Random.Range(vol_min, vol_max);
                footstep_sound.clip = footstep_clip[Random.Range(0, footstep_clip.Length)];
                footstep_sound.Play();

                accumulated_dist = 0f;
            }
        }
        else
        {
            accumulated_dist = 0;
        }
    }
}

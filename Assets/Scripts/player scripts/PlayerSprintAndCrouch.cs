using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public float sprint_speed = 8f;
    public float move_speed = 4f;
    public float crouch_speed = 1.5f;

    private Transform look_root;
    private float stand_height = 1.6f;
    private float crouch_height = 1f;

    private bool is_crouch;

    private playerfootsteps player_Footsteps;

    private float sprint_volume = 1f;
    private float crouch_volume = 0.1f;
    private float walk_vol_min = 0.2f, walk_vol_max = 0.6f;
    // Start is called before the first frame update

    private float walk_step_dist = 0.4f;
    private float sprint_step_dist=0.25f;
    private float crouch_step_dist = 0.5f;
    private playerStats player_stats;

    private float sprint_value = 100f;
    public float sprint_threshold = 10f;
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        look_root = transform.GetChild(0);
        player_stats = GetComponent<playerStats>();
        player_Footsteps = GetComponentInChildren<playerfootsteps>();
    }

    private void Start()
    {
        player_Footsteps.vol_min = walk_vol_min;

        player_Footsteps.vol_max = walk_vol_max;

        player_Footsteps.step_dist = walk_step_dist;
    }
    // Update is called once per frame
    void Update()
    {
        sprint();
        Crouch();
    }

    void sprint()
    {
        if (sprint_value > 0f)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !is_crouch)
            {
                playerMovement.speed = sprint_speed;

                player_Footsteps.step_dist = sprint_step_dist;
                player_Footsteps.vol_min = sprint_volume;
                player_Footsteps.vol_max = sprint_volume;

            }
            if (Input.GetKeyUp(KeyCode.LeftShift) && !is_crouch)
            {
                playerMovement.speed = move_speed;

                player_Footsteps.vol_min = walk_vol_min;
                player_Footsteps.vol_max = walk_vol_max;
                player_Footsteps.step_dist = walk_step_dist;
            }
        }
        if(Input.GetKey(KeyCode.LeftShift)&& !is_crouch && (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D)))
        {
            sprint_value -= sprint_threshold * Time.deltaTime;

            if(sprint_value<=0f)
            {
                sprint_value = 0f;
                playerMovement.speed = move_speed;
                player_Footsteps.vol_min = walk_vol_min;
                player_Footsteps.vol_max = walk_vol_max;
                player_Footsteps.step_dist = walk_step_dist;
            }
            player_stats.Display_staminaStats(sprint_value);
        }

        else
        {
            if (sprint_value != 100)
            {
                sprint_value += (sprint_threshold / 2f) * Time.deltaTime;
                player_stats.Display_staminaStats(sprint_value);
                if(sprint_value>100f)
                {
                    sprint_value = 100f;
                }
            }
        }
    }

    void Crouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(is_crouch)
            {
                look_root.localPosition = new Vector3(0f, stand_height, 0);
                playerMovement.speed = move_speed;
                is_crouch = false;

                player_Footsteps.vol_min = walk_vol_min;
                player_Footsteps.vol_max = walk_vol_max;
                player_Footsteps.step_dist = walk_step_dist;
            }
            else
            {
                look_root.localPosition = new Vector3(0f,crouch_height, 0);
                playerMovement.speed = crouch_speed;

                player_Footsteps.vol_min = crouch_volume;
                player_Footsteps.vol_max = crouch_volume;
                player_Footsteps.step_dist = crouch_step_dist;

                is_crouch = true;
            }
        }
    }
}

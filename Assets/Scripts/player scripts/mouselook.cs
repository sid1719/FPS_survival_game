using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouselook : MonoBehaviour
{   [SerializeField]
    private Transform playerRoot, lookRoot;

    [SerializeField]
    private bool invert;

    [SerializeField]
    private bool can_unlock = true;

    [SerializeField]
    private float sens = 5f;

    [SerializeField]
    private int smooth_Steps = 10;

    [SerializeField]
    private float smooth_Weight = 0.4f;

    [SerializeField]
    private float roll_Angle = 10f;


    [SerializeField]
    private float roll_speed = 3f;

    [SerializeField]
    private Vector2 default_look_limit=new Vector2(-70f,80f);

    private Vector2 look_Angles;
    private Vector2 current_Mouse_look;
    private Vector2 smooth_Move;
    private float current_roll_angle;
    private int last_look_frame;
    
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        lockandunlockcursor();  

        if(Cursor.lockState==CursorLockMode.Locked)
        {
            lookaround();
        }
    }

    void lockandunlockcursor()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Cursor.lockState==CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else 
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void lookaround()
    {
        current_Mouse_look = new Vector2(Input.GetAxis(mouseaxis.MOUSE_Y), Input.GetAxis(mouseaxis.MOUSE_X));

        look_Angles.x+=current_Mouse_look.x * sens *(invert ? 1f : -1f) ;
        look_Angles.y += current_Mouse_look.y * sens;
        look_Angles.x = Mathf.Clamp(look_Angles.x, default_look_limit.x, default_look_limit.y);
       // current_roll_angle = Mathf.Lerp(current_roll_angle, Input.GetAxisRaw(mouseaxis.MOUSE_X) * roll_Angle,Time.deltaTime*roll_speed);

        lookRoot.localRotation = Quaternion.Euler(look_Angles.x, 0f, current_roll_angle);
        playerRoot.localRotation = Quaternion.Euler(0f, look_Angles.y, 0f);
    }
}

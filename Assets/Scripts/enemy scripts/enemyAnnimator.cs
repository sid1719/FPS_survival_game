using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAnnimator : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame u pdate
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Walk(bool walk)
    {
        anim.SetBool(AnimationTags.WALK_PARAMETER, walk);
    }

    public void Run(bool run)
    {
        anim.SetBool(AnimationTags.RUN_PARAMETER, run);
    }

    public void Attack()
    {
        anim.SetTrigger(AnimationTags.ATTACK_PARAMETER);
    }

    public void Dead()
    {
        anim.SetTrigger(AnimationTags.DEAD_TRIGGER);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

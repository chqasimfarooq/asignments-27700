using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChracterAnimation : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Walk(float speed)
    {
        anim.SetFloat(AnimationTags.Walk, Mathf.Abs(speed));
    }

    public void Jump(bool isJumping)
    {
        anim.SetBool(AnimationTags.Jumping_Bool, isJumping);
    }

    public void Punch1()
    {
        AudioController.audioControler.PlaySound("Punch");
        anim.SetTrigger(AnimationTags.Punch1_Trigger);
    }

    public void Punch2()
    {
        AudioController.audioControler.PlaySound("Punch");
        anim.SetTrigger(AnimationTags.Punch2_Trigger);
    }
    public void Kick()
    {
        AudioController.audioControler.PlaySound("Punch");
        anim.SetTrigger(AnimationTags.Kick_Trigger);
    }

    public void Hurt()
    {
        anim.SetTrigger(AnimationTags.Hurt_Trigger);
    }

    public void Defense(bool isDefense)
    {
        anim.SetBool(AnimationTags.Defense_Bool, isDefense);
    }

    public void Die(bool isDie)
    {
        anim.SetBool(AnimationTags.Die_Bool, isDie);
    }
}

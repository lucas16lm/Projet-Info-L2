using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator[] animators;

    void Start()
    {
        animators=GetComponentsInChildren<Animator>();
    }

    public void SetMovementAnimation(bool isMoving){
        foreach(Animator animator in animators) animator.SetBool("isMoving", isMoving);
    }
}

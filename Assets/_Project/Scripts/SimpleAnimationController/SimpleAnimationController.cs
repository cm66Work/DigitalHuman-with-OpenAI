using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimationController : MonoBehaviour
{

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    public void TriggerTalking()
    {
        if(_animator == null) return;
        _animator.SetTrigger("Talking");
    }

    public void TriggerIdel()
    {
        if(_animator == null) return;
        _animator.SetTrigger("Idle");
    }

}

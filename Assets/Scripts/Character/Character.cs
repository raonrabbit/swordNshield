using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected bool _isDead = false;

    public abstract void Move();
    public abstract void Attack();
    public abstract void Look();
    public void Die(){
        if(!_isDead){
            _isDead = true;
            gameObject.SetActive(false);
        }
    }
}

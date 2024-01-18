using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IEquipment
{
    [SerializeField] private float growRate = 0.2f;
    private Character selfCharacter;
    private BoxCollider2D swordCollider;
    private SpriteRenderer swordSpriteRenderer;
    private float swordLength;

    public float SwordLength{
        get{ return swordLength; }
    }

    private void Start(){
        selfCharacter = transform.root.GetComponent<Character>();
        swordCollider = GetComponent<BoxCollider2D>();
        swordSpriteRenderer = GetComponent<SpriteRenderer>();
        swordLength = swordSpriteRenderer.size.y;
        swordCollider.enabled = false;
    }

    public IEnumerator Use(){
        EnableSwordCollider();
        yield return new WaitForSeconds(selfCharacter.AttackTime);
        DisableSwordCollider();
    }

    private void Grow(){
        swordSpriteRenderer.size += new Vector2(0, growRate);
        swordCollider.size += new Vector2(0, growRate);
        swordLength += growRate;
        swordCollider.offset = new Vector2(swordCollider.size.x, swordCollider.size.y / 2);
    }

    public void EnableSwordCollider(){
        swordCollider.enabled = true;
    }

    public void DisableSwordCollider(){
        swordCollider.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject != selfCharacter.gameObject && other.tag == "Character"){
            Character enemy = other.gameObject.GetComponent<Character>();
            
            if(enemy != null) enemy.GetDamage();
        }
    }
}

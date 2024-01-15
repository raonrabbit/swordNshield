using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    private float growRate = 0.2f;
    private PlayerController playerController;
    private BoxCollider2D swordCollider;
    private SpriteRenderer swordSpriteRenderer;

    private void Start(){
        playerController = transform.root.GetComponent<PlayerController>();
        swordCollider = GetComponent<BoxCollider2D>();
        swordSpriteRenderer = GetComponent<SpriteRenderer>();

        swordCollider.enabled = false;
    }

    private void Grow(){
        swordSpriteRenderer.size += new Vector2(0, growRate);
        swordCollider.size += new Vector2(0, growRate);
        swordCollider.offset = new Vector2(swordCollider.size.x, swordCollider.size.y / 2);
    }

    public void EnableSwordCollider(){
        swordCollider.enabled = true;
    }

    public void DisableSwordCollider(){
        swordCollider.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject != playerController.gameObject && other.tag == "Character"){
            Character character = other.gameObject.GetComponent<Character>();
            
            if(character != null) character.GetDamage();
        }
    }
}

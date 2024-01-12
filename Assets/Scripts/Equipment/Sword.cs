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
    }

    private void Grow(){
        swordSpriteRenderer.size += new Vector2(0, growRate);
        swordCollider.size += new Vector2(0, growRate);
        swordCollider.offset = new Vector2(swordCollider.size.x, swordCollider.size.y / 2);
    }
}

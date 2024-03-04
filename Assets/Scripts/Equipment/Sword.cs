using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour, IEquipment
{
    [SerializeField] private float growRate = 0.2f;
    private Character selfCharacter;
    private BoxCollider2D swordCollider;
    private SpriteRenderer swordSpriteRenderer;
    private float swordLength;

    public float SwordLength{
        get => swordLength;
    }
    public Character OwnerCharacter{
        get => selfCharacter;
    }

    private void Awake(){
        selfCharacter = transform.root.GetComponent<Character>();
        swordCollider = GetComponent<BoxCollider2D>();
        swordSpriteRenderer = GetComponent<SpriteRenderer>();
        swordLength = swordSpriteRenderer.size.y;
        swordCollider.enabled = false;
    }

    public IEnumerator Use(){
        EnableSwordCollider();
        yield return new WaitForSeconds(selfCharacter.Actions["Attack"].ActionTime);
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
}

using System;
using UnityEngine;

public class TransparentObj : MonoBehaviour
{
    private float otherTransparentRate;
    private SpriteRenderer selfSpriteRenderer;
    private Color selfColor;

    private void Awake()
    {
        selfSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        selfColor = selfSpriteRenderer.color;
        otherTransparentRate = 0f;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Character character = other.GetComponent<Character>();
        if (character == null)
        {
            TransparentObj to = other.GetComponent<TransparentObj>();
            if (to != null)
            {
                Function(true);
            }
            return;
        }
        SpriteRenderer[] spriteRenderers = character.GetComponentsInChildren<SpriteRenderer>();
        if (character._photonView.IsMine)
        {
            Function(true);
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                var color = spriteRenderer.color;
                color.a = 0.5f;
                spriteRenderer.color = color;
            }
        }
        else
        {
            var canvasGroup = character.GetComponent<CanvasGroup>();
            canvasGroup.alpha = otherTransparentRate;
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                var color = spriteRenderer.color;
                color.a = otherTransparentRate;
                spriteRenderer.color = color;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Character character = other.GetComponent<Character>();
        if (character == null)
        {
            TransparentObj to = other.GetComponent<TransparentObj>();
            if (to != null) Function(false);
            return;
        }
        if (character._photonView.IsMine)
        {
            Function(false);
        }
        else
        {
            var canvasGroup = character.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
        }
        SpriteRenderer[] spriteRenderers = character.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            var color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
        }
    }

    private void Function(bool isActivate)
    {
        if (isActivate)
        {
            otherTransparentRate = 0.5f;
            selfColor.a = 0.5f;
            selfSpriteRenderer.color = selfColor;
            return;
        }

        otherTransparentRate = 0f;
        selfColor.a = 1f;
        selfSpriteRenderer.color = selfColor;
    }
}

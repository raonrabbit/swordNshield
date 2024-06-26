using Photon.Pun;
using SwordNShield.Controller;
using UnityEngine;

namespace SwordNShield.UI
{
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
            var to = other.GetComponent<TransparentObj>();
            if (to != null) return;
            var spriteRenderers = other.GetComponentsInChildren<SpriteRenderer>();
            var playerPhotonController = other.GetComponent<PlayerPhotonController>();
            if (playerPhotonController == null) return;
            if (playerPhotonController.photonView.IsMine)
            {
                ChangeTransparentRate(true);
                foreach (SpriteRenderer spriteRenderer in spriteRenderers)
                {
                    var color = spriteRenderer.color;
                    color.a = 0.5f;
                    spriteRenderer.color = color;
                }
            }
            else
            {
                var canvasGroup = other.GetComponent<CanvasGroup>();
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
            var otherPhotonView = other.GetComponent<PhotonView>();
            if (otherPhotonView.IsMine)
            {
                ChangeTransparentRate(false);
            }

            var canvasGroup = other.GetComponent<CanvasGroup>();
            if (canvasGroup != null) canvasGroup.alpha = 1f;
            SpriteRenderer[] spriteRenderers = other.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                var color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;
            }
        }

        private void ChangeTransparentRate(bool isActivate)
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
}

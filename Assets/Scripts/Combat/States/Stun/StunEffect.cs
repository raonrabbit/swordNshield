using UnityEngine;
using DG.Tweening;

namespace SwordNShield.Combat.States
{
    public class StunEffect : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Color targetColor;
        private Color originalColor;
        private Sequence sequence;
        void Awake()
        {
            originalColor = spriteRenderer.color;
            targetColor = originalColor;
            originalColor.a = 0f;
            targetColor.a = 1f;
        }

        void OnEnable()
        {
            spriteRenderer.color = originalColor;

            sequence = DOTween.Sequence();
            sequence.Append(transform.DORotate(new Vector3(0, 180, 180), 0.5f).SetEase(Ease.InOutCubic));
            sequence.Join(DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, targetColor, 0.5f)
                .SetEase(Ease.InOutCubic));
        }

        void OnDisable()
        {
            sequence?.Kill();
        }
    }   
}

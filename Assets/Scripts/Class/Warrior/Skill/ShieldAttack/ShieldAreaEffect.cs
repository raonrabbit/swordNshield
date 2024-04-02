using UnityEngine;
using DG.Tweening;

namespace SwordNShield.UI
{
    public class ShieldAreaEffect : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Color targetColor;
        private Color originalColor;
        private Vector3 originalScale;
        private Sequence sequence;

        void Awake()
        {
            originalColor = spriteRenderer.color;
            spriteRenderer.color = originalColor;
            targetColor.a = 0.1f;
            originalScale = transform.localScale;
        }

        void OnEnable()
        {
            transform.localScale = originalScale;
            spriteRenderer.color = originalColor;
            Vector3 targetScale = originalScale * 0.95f;
            sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(targetScale, 0.5f).SetEase(Ease.InOutSine));
            sequence.Join(DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, targetColor, 0.5f)
                .SetEase(Ease.InOutSine));
            sequence.SetLoops(-1, LoopType.Yoyo);
        }

        void OnDisable()
        {
            sequence?.Kill();
        }
    }
}


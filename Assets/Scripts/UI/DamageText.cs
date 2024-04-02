using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace SwordNShield.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        void OnEnable()
        {
            Color currentColor = text.color;
            text.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.8f);
            transform.DOMove(transform.position + new Vector3(0f, 0.5f, 0f), 0.5f);
            text.DOColor(new Color(currentColor.r, currentColor.g, currentColor.b, 0), 0.5f);
            StartCoroutine(EnableThis());
        }

        private IEnumerator EnableThis()
        {
            yield return new WaitForSeconds(0.5f);
            DamageTextManager.Instance.ReturnDamageText(gameObject);
        }
    }
}
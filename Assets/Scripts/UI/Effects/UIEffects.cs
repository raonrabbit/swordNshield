using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace SwordNShield.UI.Effects
{
    public static class UIEffects
    {
        public static Action action;
        public static void FadeOut(GameObject panel, MonoBehaviour coroutineSource)
        {
            CanvasGroup cg = panel.GetComponent<CanvasGroup>();
            if (cg == null) return;
            cg.DOFade(0, 1)
                .OnComplete(() => DOVirtual.DelayedCall(0.1f, () => cg.alpha = 1f));
            coroutineSource.StartCoroutine(OnEnableObject(panel, 1f));
        }

        public static void Shrink(GameObject panel)
        {
        
        }

        private static IEnumerator OnEnableObject(GameObject _gameObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            _gameObject.SetActive(false);
        }
    }
}

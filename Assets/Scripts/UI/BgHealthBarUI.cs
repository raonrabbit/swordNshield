using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SwordNShield.UI
{
    public class BgHealthBarUI : MonoBehaviour
    {
        private static BgHealthBarUI instance = null;
        [SerializeField] private Slider HpSlider;
        [SerializeField] private TMP_Text text;
        void Awake()
        {
            if(instance == null) instance = this;
            else Destroy(this.gameObject);
        }

        public static BgHealthBarUI Instance => instance;

        public void SetHPBar(float maxHP, float currentHP)
        {
            HpSlider.maxValue = maxHP;
            HpSlider.value = currentHP;

            text.text = currentHP + " / " + maxHP;
        }
    }
}

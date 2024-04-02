using UnityEngine;

namespace SwordNShield.Combat.Skills
{
    public class IndicatorManager : MonoBehaviour
    {
        private ISkill currentSkill;
        private Vector2 mousePosition;

        void Update()
        {
            if (currentSkill != null)
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Input.GetKeyUp(currentSkill.GetKeyCode))
                {
                    currentSkill.Indicator.SetActive(false);
                    currentSkill.Play(mousePosition);
                    currentSkill = null;
                }

                if (Input.GetMouseButton(1))
                {
                    currentSkill?.Indicator.SetActive(false);
                    currentSkill = null;
                }
                else RotateIndicatorToMousePosition();
            }
        }

        public void ActivateIndicator(ISkill skill)
        {
            if (!skill.CanExecute) return;
            if (currentSkill != null)
            {
                currentSkill.Indicator.SetActive(false);
            }

            currentSkill = skill;
            currentSkill.Indicator.SetActive(false);
            currentSkill = skill;
            currentSkill.Indicator.SetActive(true);
        }

        public void DeActivateIndicator()
        {
            if (currentSkill != null)
            {
                currentSkill.Indicator.SetActive(false);
                currentSkill = null;
            }
        }

        void RotateIndicatorToMousePosition()
        {
            if (currentSkill == null) return;
            Vector2 direction = mousePosition - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}

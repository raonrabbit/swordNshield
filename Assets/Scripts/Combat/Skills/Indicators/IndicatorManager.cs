using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace SwordNShield.Combat.Skills
{
    public class IndicatorManager : MonoBehaviour
    {
        [SerializeField] private SkillScheduler skillScheduler;
        private Skill currentSkill;
        private Vector2 targetPosition;

        public void ActivateIndicator(Skill skill)
        {
            currentSkill = skill;
            currentSkill.Indicator.SetActive(true);
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            //Arrow타입 Indicator 설정
            if (currentSkill.IndicatorType == IndicatorType.Arrow)
            {
                StartCoroutine(ArrowIndicator());
            }
            
            if (currentSkill.IndicatorType == IndicatorType.Circle)
            {
                StartCoroutine(CircleIndicator());
            }
        }

        IEnumerator ArrowIndicator()
        {
            while (currentSkill != null)
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RotateIndicatorToTargetPosition();
                yield return null;
            }
        }


        IEnumerator CircleIndicator()
        {
            while (currentSkill != null)
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SetPositionToMousePosition();
                yield return null;
            }
        }
        
        public void DeActivateIndicator()
        {
            if (currentSkill != null)
            {
                currentSkill.Indicator.SetActive(false);
                currentSkill = null;
            }
        }

        void RotateIndicatorToTargetPosition()
        {
            if (currentSkill == null) return;
            Vector2 direction = targetPosition - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            currentSkill.Indicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        void SetPositionToMousePosition()
        {
            if (currentSkill == null) return;
            transform.position = targetPosition;
        }
    }
}

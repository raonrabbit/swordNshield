using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SwordNShield.UI
{
    public class DamageTextManager : MonoBehaviour
    {
        public static DamageTextManager Instance;
        public GameObject damageTextPrefab;
        public Transform textParent;

        private Queue<GameObject> damageTextPool = new Queue<GameObject>();

        private void Awake()
        {
            Instance = this;
            Initialize(100);
        }

        private void Initialize(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, textParent);
                obj.SetActive(false);
                damageTextPool.Enqueue(obj);
            }
        }

        public void GetDamageText(Vector2 position, float damage)
        {
            if (damageTextPool.Count == 0)
            {
                Initialize(10);
            }

            GameObject obj = damageTextPool.Dequeue();
            obj.GetComponent<TMP_Text>().text = damage.ToString();
            obj.transform.position = position;
            obj.SetActive(true);
        }

        public void ReturnDamageText(GameObject obj)
        {
            obj.SetActive(false);
            damageTextPool.Enqueue(obj);
        }
    }
}

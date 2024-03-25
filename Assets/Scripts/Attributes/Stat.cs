using UnityEngine;

namespace SwordNShield.Attributes
{
    public class Stat : MonoBehaviour
    {
        [SerializeField] private float healthPoint = 200f;
        [SerializeField] private float moveSpeed = 1.5f;
        [SerializeField] private float rotateSpeed = 750f;

        public float HP => healthPoint;
        public float MoveSpeed => moveSpeed;
        public float RotateSpeed => rotateSpeed;
    }
}

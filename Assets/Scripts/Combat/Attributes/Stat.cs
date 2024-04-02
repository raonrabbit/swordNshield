using UnityEngine;

namespace SwordNShield.Combat.Attributes
{
    public class Stat : MonoBehaviour
    {
        [SerializeField] private float maxHP = 200f;
        [SerializeField] private float moveSpeed = 1.5f;
        [SerializeField] private float rotateSpeed = 750f;
        [SerializeField] private float attackRange;
        [SerializeField] private float attackDamage;
        [SerializeField] private float attackCoolTime;

        public float HP => maxHP;
        public float MoveSpeed => moveSpeed;
        public float RotateSpeed => rotateSpeed;
        public float AttackRange => attackRange;
        public float AttackDamage => attackDamage;
        public float AttackCoolTime => attackCoolTime;
    }
}

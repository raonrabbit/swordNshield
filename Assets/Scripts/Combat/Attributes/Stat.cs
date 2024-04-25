using SwordNShield.Combat.Actions;
using UnityEngine;

namespace SwordNShield.Combat.Attributes
{
    public class Stat : MonoBehaviour
    {
        [Header("Default")]
        [SerializeField] private float maxHP = 200f;
        [SerializeField] private float moveSpeed = 1.5f;
        [SerializeField] private float rotateSpeed = 750f;
        
        [Header("Combat")]
        [SerializeField] private AttackType attackType;
        [SerializeField] private GameObject bullet;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private Vector3 bulletPosition;
        [SerializeField] private float attackRange;
        [SerializeField] private float attackDamage;
        [SerializeField] private float attackCoolTime;

        // Default
        public float HP => maxHP;
        public float MoveSpeed => moveSpeed;
        public float RotateSpeed => rotateSpeed;
        
        // Combat
        public AttackType AttackType => attackType;
        public GameObject Bullet => bullet;
        public float BulletSpeed => bulletSpeed;
        public Vector3 BulletPosition => bulletPosition;
        public float AttackRange => attackRange;
        public float AttackDamage => attackDamage;
        public float AttackCoolTime => attackCoolTime;
    }
}

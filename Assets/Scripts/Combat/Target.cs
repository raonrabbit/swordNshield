using UnityEngine;

namespace SwordNShield.Combat
{
    public struct Target
    {
        [SerializeField] private Vector2 vectorTarget;
        [SerializeField] private GameObject objectTarget;

        public Vector2 VectorTarget
        {
            get => vectorTarget;
            set => vectorTarget = value;
        }

        public GameObject ObjectTarget
        {
            get => objectTarget;
            set => objectTarget = value;
        }
    }
}

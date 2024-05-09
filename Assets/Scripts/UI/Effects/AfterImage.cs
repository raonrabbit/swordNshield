using UnityEngine;

namespace SwordNShield.UI
{
    public class AfterImage : MonoBehaviour
    {
        [SerializeField]private ParticleSystem particleSystem;

        private void Start()
        {
            particleSystem.GetComponentInChildren<ParticleSystem>();
        }

        void Update()
        {
            if (particleSystem != null)
            {
                ParticleSystem.MainModule main = particleSystem.main;

                if (main.startRotation.mode == ParticleSystemCurveMode.Constant)
                {
                    main.startRotation = -transform.eulerAngles.z * Mathf.Deg2Rad;
                }
            }
        }
    }   
}

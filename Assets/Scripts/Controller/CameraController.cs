using UnityEngine;

namespace SwordNShield.Contrller
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform player;

        void LateUpdate()
        {
            transform.position = new Vector3(player.position.x, player.position.y, -10);
        }
    }
}

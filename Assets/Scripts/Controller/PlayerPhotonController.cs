using Photon.Pun;
using SwordNShield.Attributes;
using SwordNShield.Movement;
using UnityEngine;

namespace SwordNShield.Controller
{
    public class PlayerPhotonController : MonoBehaviourPunCallbacks, IPunObservable
    {
        private PhotonView photonView;
        private Vector2 localPosition;
        private float localRotation;
        private Vector2 localVelocity;
        private Rigidbody2D rigidbody2D;
        private float localHP;
        private Health health;
        private Mover mover;
        private Rotater rotater;
        [Range(0, 30)]
        [SerializeField] private float deltaTimeRate;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
            rigidbody2D = GetComponent <Rigidbody2D>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            rotater = GetComponent<Rotater>();
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message){
            if(stream.IsWriting)
            {
                stream.SendNext(rigidbody2D.position);
                stream.SendNext(rigidbody2D.rotation);
                stream.SendNext(rigidbody2D.velocity);
                stream.SendNext(health.HP());
            }
            else
            {
                localPosition = (Vector2)stream.ReceiveNext();
                localRotation = (float)stream.ReceiveNext();
                localVelocity = (Vector2)stream.ReceiveNext();
                localHP = (float)stream.ReceiveNext();
                
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - message.SentServerTime));
                localPosition += localVelocity * lag;
            }
        }

        void FixedUpdate()
        {
            if (photonView.IsMine) return;
            //rigidbody2D.position = Vector3.MoveTowards(rigidbody2D.position, localPosition, Time.fixedDeltaTime * 100);
            mover.StartMoveAction(localPosition, 1.5f);
            rotater.StartRotateAction(localPosition);
        }
    }
}
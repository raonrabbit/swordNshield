using Photon.Pun;
using SwordNShield.Attributes;
using SwordNShield.Movement;
using UnityEngine;

namespace SwordNShield.Controller
{
    public class PlayerPhotonController : MonoBehaviourPunCallbacks, IPunObservable
    {
        private Stat stat;
        //private PhotonView photonView;
        private Vector2 localPosition;
        private float localRotation;
        private Vector2 localVelocity;
        private float localAngularVelocity;
        private Rigidbody2D rigidbody2D;
        private float localHP;
        private Mover mover;
        private Rotater rotater;
        private float moveSpeed;
        private float rotateSpeed;
        [Range(0, 30)]
        [SerializeField] private float deltaTimeRate;
        
        private void Awake()
        {
            stat = GetComponent<Stat>();
            //photonView = GetComponent<PhotonView>();
            rigidbody2D = GetComponent <Rigidbody2D>();
            mover = GetComponent<Mover>();
            rotater = GetComponent<Rotater>();
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message){
            if(stream.IsWriting)
            {
                stream.SendNext(rigidbody2D.position);
                stream.SendNext(rigidbody2D.velocity);
                stream.SendNext(rigidbody2D.rotation);
                stream.SendNext(rigidbody2D.angularVelocity);
                stream.SendNext(stat.HP);
            }
            else
            {
                localPosition = (Vector2)stream.ReceiveNext();
                localVelocity = (Vector2)stream.ReceiveNext();
                localRotation = (float)stream.ReceiveNext();
                localAngularVelocity = (float)stream.ReceiveNext();
                localHP = (float)stream.ReceiveNext();
                
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - message.SentServerTime));
                localPosition += localVelocity * lag;
                localRotation += localAngularVelocity * lag;
            }
        }

        void FixedUpdate()
        {
            if (photonView.IsMine) return;
            //rigidbody2D.position = Vector3.MoveTowards(rigidbody2D.position, localPosition, Time.fixedDeltaTime * 100);
            mover.StartMoveAction(localPosition, stat.MoveSpeed);
            rotater.StartRotateAction(localRotation, stat.RotateSpeed);
        }
    }
}
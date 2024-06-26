using Photon.Pun;
using SwordNShield.Combat.Actions;
using SwordNShield.Combat.Attributes;
using UnityEngine;

namespace SwordNShield.Controller
{
    public class PlayerPhotonController : MonoBehaviourPunCallbacks, IPunObservable
    {
        private Animator animator;
        private Stat stat;
        //private PhotonView photonView;
        private Vector2 localPosition;
        private float localRotation;
        private Vector2 localVelocity;
        private float localAngularVelocity;
        private Rigidbody2D rigidBody2D;
        private Mover mover;
        private Rotater rotater;
        private float moveSpeed;
        private float rotateSpeed;
        [Range(0, 30)]
        [SerializeField] private float deltaTimeRate;
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            stat = GetComponent<Stat>();
            //photonView = GetComponent<PhotonView>();
            rigidBody2D = GetComponent <Rigidbody2D>();
            mover = GetComponent<Mover>();
            rotater = GetComponent<Rotater>();
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message){
            if(stream.IsWriting)
            {
                stream.SendNext(rigidBody2D.position);
                stream.SendNext(rigidBody2D.velocity);
                stream.SendNext(rigidBody2D.rotation);
                stream.SendNext(rigidBody2D.angularVelocity);
            }
            else
            {
                localPosition = (Vector2)stream.ReceiveNext();
                localVelocity = (Vector2)stream.ReceiveNext();
                localRotation = (float)stream.ReceiveNext();
                localAngularVelocity = (float)stream.ReceiveNext();
                
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - message.SentServerTime));
                localPosition += localVelocity * lag;
                localRotation += localAngularVelocity * lag;
            }
        }

        void Update()
        {
            if (photonView.IsMine) return;
            //rigidbody2D.position = Vector3.MoveTowards(rigidbody2D.position, localPosition, Time.fixedDeltaTime * 100);
            if (Vector2.Distance(localPosition, transform.position) > 2f) transform.position = localPosition;
            mover.StartMoveAction(localPosition, stat.MoveSpeed);
            //rotater.StartRotateAction(localRotation, stat.RotateSpeed);
        }
    }
}
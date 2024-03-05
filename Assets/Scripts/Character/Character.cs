using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviourPunCallbacks, IPunObservable
{
    public Sword sword;
    protected Animator animator;
    public PhotonView _photonView;
    protected Rigidbody2D _rigidBody2D;
    protected bool _isDead = false;
    [SerializeField] protected Text _nickName;
    protected Dictionary<string, IAction> _actions;
    //기본 설정
    [SerializeField] protected float _speed = 2f;
    protected int _maxHp = 200;
    [SerializeField] protected int _currentHp = 200;
    protected int _damage = 40;

    //Dash Direction
    public Vector2 dashDirection;

    //동기화
    protected Vector3 _currentPosition;
    protected Quaternion _currentRotation;
    protected void Awake(){
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _photonView = GetComponent<PhotonView>();
        _nickName.text = _photonView.IsMine ? PhotonNetwork.NickName : _photonView.Owner.NickName;
        _nickName.color = _photonView.IsMine ? Color.green : Color.red;
        _actions = new Dictionary<string, IAction>{
            {"Attack", new AttackAction {Owner = this}},
            {"Defend", new DefendAction {Owner = this}},
            {"Dash", new DashAction {Owner = this}},
        };
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message){
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(_currentHp);
        }
        else
        {
            _currentPosition = (Vector3)stream.ReceiveNext();
            _currentRotation = (Quaternion)stream.ReceiveNext();
            _currentHp = (int)stream.ReceiveNext();
        }
    }

    public Animator GetAnimator{ get => animator; }
    public Sword CharacterWeapon{ get => sword; }
    public Rigidbody2D RigidBody{ get => _rigidBody2D; }
    public int GetHp{ get => _currentHp; }
    public Text GetNickName{ get => _nickName; }
    public bool GetPhotonIsMine{ get => _photonView.IsMine; }
    public Dictionary<string, IAction> Actions { get => _actions; }

    public abstract void Move();

    public void GetDamage(Character character){
        if(!_photonView.IsMine) return;
        if(!(_actions["Defend"].Playing && FaceToOther(character.transform.position))){
            _currentHp -= _damage;
            if(_currentHp <= 0) {
                _photonView.RPC("Die", RpcTarget.All);
                Die();
            }
        }
    }
    [PunRPC]
    public virtual void Die(){
        if(!_isDead){
            _isDead = true;
            gameObject.SetActive(false);
            if(_photonView.IsMine) PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    protected void PlayTriggerAnimation(string triggerName){
        animator.SetTrigger(triggerName);
        StartCoroutine(sword.Use());
    }

    public void Look(Vector3 target){
        float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 2f * Time.deltaTime);
    }
    
    public bool FaceToOther(Vector3 otherPosition){
        Vector2 directionToOther = (otherPosition - transform.position).normalized;
        if(Vector2.Angle(transform.up, directionToOther) < 40f){
            return true;
        }
        return false;
    }

    [PunRPC]
    public void Dash(Vector2 direction){
        dashDirection = direction;
        StartCoroutine(_actions["Dash"].Execute());
    }

    public void OnTriggerEnter2D(Collider2D other){
        if(!_photonView.IsMine) return;
        Sword sword = other.gameObject.GetComponent<Sword>();
        if(sword != null){
            if(!this.Equals(sword.OwnerCharacter)) GetDamage(sword.OwnerCharacter);
        }
    }
}

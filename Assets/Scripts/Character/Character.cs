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
    protected PhotonView _photonView;
    protected Rigidbody2D _rigidBody2D;
    protected bool _isDead = false;
    [SerializeField] protected Text _nickName;

    //기본 설정
    [SerializeField] protected float _speed = 2f;
    protected int _maxHp = 100;
    [SerializeField] protected int _currentHp = 100;
    protected int _damage = 40;

    //공격
    protected float _attackCooldown = 2f;
    protected float _attackTime = 0.2f;
    protected bool _isAttacking = false;

    //방어
    protected float _maxDefendTime = 3f;
    protected float _defendCooldown = 3f;
    protected float _defendTime = 4f;
    [SerializeField] protected bool _isDefending = false;
    protected bool _canDefend = true;

    //동기화
    protected Vector3 _currentPosition;
    protected Quaternion _currentRotation;

    protected void Awake(){
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _photonView = GetComponent<PhotonView>();
        _nickName.text = _photonView.IsMine ? PhotonNetwork.NickName : _photonView.Owner.NickName;
        _nickName.color = _photonView.IsMine ? Color.green : Color.red;
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

    public int GetHp{
        get => _currentHp;
    }
    public float AttackTime{
        get => _attackTime;
    }
    public Text GetNickName{
        get => _nickName;
    }
    public bool GetPhotonIsMine{
        get => _photonView.IsMine;
    }
    public abstract void Move();

    public void GetDamage(Character character){
        if(!_photonView.IsMine) return;
        if(!(_isDefending && FaceToOther(character.transform.position))){
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

    public IEnumerator Attack(){
        if(!_isAttacking){
            _isAttacking = true;
            animator.SetTrigger("attack");
            _photonView.RPC("PlayTriggerAnimation", RpcTarget.All, "attack");
            StartCoroutine(sword.Use());
            yield return new WaitForSeconds(_attackCooldown);
            _isAttacking = false;
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
    
    public IEnumerator Defend(){
        _canDefend = false;
        _isDefending = true;
        animator.SetBool("isDefending", true);
        yield return new WaitForSeconds(_defendTime);
        _isDefending = false;
        animator.SetBool("isDefending", false);
        yield return new WaitForSeconds(_defendCooldown);
        _canDefend = true;
    }
    
    public bool FaceToOther(Vector3 otherPosition){
        Vector2 directionToOther = (otherPosition - transform.position).normalized;
        if(Vector2.Angle(transform.up, directionToOther) < 40f){
            return true;
        }
        return false;
    }

    public void OnTriggerEnter2D(Collider2D other){
        if(!_photonView.IsMine) return;
        Sword sword = other.gameObject.GetComponent<Sword>();
        if(sword != null){
            if(!this.Equals(sword.OwnerCharacter)) GetDamage(sword.OwnerCharacter);
        }
    }
}

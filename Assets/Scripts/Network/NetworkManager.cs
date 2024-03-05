using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public InputField nameInput;
    public GameObject lobbyPanel;
    public GameObject restartPanel;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject player;
    public GameObject loadingUI;

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinOrCreateRoom("SwordNShieldRoom", new RoomOptions{ MaxPlayers = 20 }, null);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room: " + message + ". Retrying..");
        PhotonNetwork.JoinOrCreateRoom("SwordNShieldRoom", new RoomOptions{ MaxPlayers = 20 }, null);
    }
/*
    public override void OnJoinedRoom(){
        lobbyPanel.SetActive(false);
        Spawn();
    }
    */

    void Awake(){
        Connect();
        PlayerController.OnDeath += ExitToLobby;
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) PhotonNetwork.Disconnect();
    }

    public void Spawn(){
        loadingUI.SetActive(true);
        if(!PhotonNetwork.IsConnected) Connect();
        StartCoroutine(JoinRoom());
    }

    IEnumerator JoinRoom(){
        while(!PhotonNetwork.InRoom){
            yield return null;
        }
        PhotonNetwork.LocalPlayer.NickName = nameInput.text;
        player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        loadingUI.SetActive(false);
        lobbyPanel.SetActive(false);
        virtualCamera.Follow = player.transform;
    }

    public void ExitToLobby(){
        lobbyPanel.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        lobbyPanel.SetActive(true);
        restartPanel.SetActive(false);
    }
}
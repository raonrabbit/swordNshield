using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using SwordNShield.Controller;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public InputField nameInput;
    public GameObject lobbyPanel;
    public GameObject restartPanel;
    public CinemachineVirtualCamera virtualCamera;
    public GameObject player;
    public GameObject loadingUI;
    public GameObject SameNickNameMessage;

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
        SameNickNameMessage.SetActive(false);
        PlayerController.OnDeath += ExitToLobby;
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) ExitToLobby();
    }

    public void Spawn()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == nameInput.text)
            {
                SameNickNameMessage.SetActive(true);
                return;
            }
        }
        SameNickNameMessage.SetActive(false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Score", 0 } });
        loadingUI.SetActive(true);
        if(!PhotonNetwork.IsConnected) Connect();
        StartCoroutine(JoinRoom());
    }

    IEnumerator JoinRoom(){
        while(!PhotonNetwork.InRoom){
            yield return null;
        }

        var nickname = nameInput.text;
        PhotonNetwork.LocalPlayer.NickName = nickname;
        player = PhotonNetwork.Instantiate("Warrior", Vector3.zero, Quaternion.identity);
        loadingUI.SetActive(false);
        lobbyPanel.SetActive(false);
        virtualCamera.Follow = player.transform;
    }

    public void ExitToLobby()
    {
        PhotonNetwork.LocalPlayer.NickName = "";
        lobbyPanel.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        lobbyPanel.SetActive(true);
        restartPanel.SetActive(false);
    }
}
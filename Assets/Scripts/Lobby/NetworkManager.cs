using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using SwordNShield.UI.Effects;
using TMPro;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Lobby")]
    public GameObject LoadingPanel;
    [SerializeField] private float UpdateRoomDuration;
    private List<RoomInfo> CachedRoomList = new List<RoomInfo>();
    
    [Header("TeamRoom")]
    [SerializeField] private GameObject TeamRoomPanel;
    [SerializeField] private TMP_Text TeamRoomCode;
    [SerializeField] private GameObject TeamListPanel;
    [SerializeField] private GameObject TeamRoom;
    private List<GameObject> TeamRoomList = new List<GameObject>();

    [Header("SoloRoom")]
    [SerializeField] private GameObject SoloRoomPanel;
    [SerializeField] private TMP_Text SoloRoomCode;
    [SerializeField] private GameObject SoloListPanel;
    [SerializeField] private GameObject SoloRoom;
    private List<GameObject> SoloRoomList = new List<GameObject>();
    
    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    
    private Dictionary<string, Room> TeamMatchRooms;
    private Dictionary<string, Room> SoloMatchRooms;
    
    void Awake()
    {
        Connect();
    }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        UIEffects.FadeOut(LoadingPanel, this);
    }

    public void OnClickCreateTeamRoom()
    {
        string roomName = CreateRandomRoomName();
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "GameMode", "Team" } },
            CustomRoomPropertiesForLobby = new[] {"GameMode"}
        };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        LoadingPanel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        UIEffects.FadeOut(LoadingPanel, this);
        ExitGames.Client.Photon.Hashtable customProps = PhotonNetwork.CurrentRoom.CustomProperties;

        if (customProps.ContainsKey("GameMode"))
        {
            string gameMode = (string)customProps["GameMode"];

            if (gameMode == "Team")
            {
                TeamRoomCode.text = "Room Code : " + PhotonNetwork.CurrentRoom.Name;
                TeamRoomPanel.SetActive(true);
            }
            else if (gameMode == "Solo")
            {
                
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (CachedRoomList.Count <= 0)
        {
            CachedRoomList = roomList;
        }
        else
        {
            foreach (var room in roomList)
            {
                for (int i = 0; i < CachedRoomList.Count; i++)
                {
                    if (CachedRoomList[i].Name == room.Name)
                    {
                        List<RoomInfo> newList = CachedRoomList;

                        if (room.RemovedFromList)
                        {
                            newList.Remove(newList[i]);
                        }
                        else
                        {
                            newList[i] = room;
                        }

                        
                        
                        CachedRoomList = newList;
                    }
                    else
                    {
                        CachedRoomList.Add(room);
                    }
                }
            }
        }

        UpdateRoomList();
    }

    public String CreateRandomRoomName()
    {
        const string chars = "0123456789";
        var random = new System.Random();
        //RoomInfo[] roomList = PhotonNetwork.GetCustomRoomList(TypedLobby.Default, null);
        string roomName = new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());

        return roomName;
    }

    private void UpdateRoomList()
    {
        foreach (var oldRoom in TeamRoomList)
        {
            Destroy(oldRoom);
        }

        TeamRoomList.Clear();
        SoloRoomList.Clear();
        
        foreach (RoomInfo newRoom in CachedRoomList)
        {
            string gameMode = (string)newRoom.CustomProperties["GameMode"];

            if (gameMode == "Team")
            {
                Debug.Log("Room Created");
                GameObject room = Instantiate(TeamRoom, TeamListPanel.transform);
                TeamRoomList.Add(room);
                RoomPanel roomPanel = room.GetComponent<RoomPanel>();

                if (roomPanel != null)
                {
                    roomPanel.RoomNumber = newRoom.Name;
                    roomPanel.PlayerCount = $"{newRoom.PlayerCount}/{newRoom.MaxPlayers}";
                }
            }
        }
    }

    public void OnClickLeaveTeamRoom()
    {
        PhotonNetwork.LeaveRoom();
        TeamRoomPanel.SetActive(false);
    }
}

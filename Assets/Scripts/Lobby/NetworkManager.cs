using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using SwordNShield.UI.Effects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Lobby")]
    public GameObject LoadingPanel;

    [SerializeField] private TMP_InputField playerNickname;
    [SerializeField] private float UpdateRoomDuration;
    private Dictionary<string, GameObject> Rooms = new();

    [Header("Room")] 
    [SerializeField] private GameObject ReadyPanel;
    [SerializeField] private TMP_Text RoomCodeText;
    [SerializeField] private GameObject RoomPrefab;
    [SerializeField] private GameObject ReadyButton;
    [SerializeField] private GameObject StartButton;
    
    [SerializeField] private Sprite DefaultButtonImage;
    [SerializeField] private Sprite WarriorImage;
    [SerializeField] private Sprite MageImage;
    
    [SerializeField] private List<ClassSelectButton> CharacterSlots;
    
    [Header("TeamRoom")]
    [SerializeField] private GameObject TeamListContent;
    [SerializeField] private GameObject TeamReadyPanel;

    [Header("SoloRoom")] 
    [SerializeField] private GameObject SoloListContent;
    [SerializeField] private GameObject SoloListPanel;
    
    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    
    void Awake()
    {
        Connect();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        UIEffects.FadeOut(LoadingPanel, this);
        PhotonNetwork.LocalPlayer.NickName = playerNickname.text;
    }

    public void OnClickCreateTeamRoom()
    {
        string roomName = CreateRandomRoomName();
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
            {
                { "GameMode", "Team" },
                { "RoomState", "Ready" }
            },
            CustomRoomPropertiesForLobby = new[] {"GameMode", "RoomState"}
        };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        LoadingPanel.SetActive(true);
    }

    public void OnClickCreateSoloRoom()
    {
        string roomName = CreateRandomRoomName();
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
            {
                { "GameMode", "Solo" },
                { "RoomState", "Ready" }
            },
            CustomRoomPropertiesForLobby = new[] {"GameMode", "RoomState"}
        };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        LoadingPanel.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Rooms.Count == 0)
        {
            foreach (RoomInfo room in roomList)
            {
                string gameMode = (string)room.CustomProperties["GameMode"];
                
                if (gameMode == "Team")
                {
                    GameObject teamRoom = Instantiate(RoomPrefab, TeamListContent.transform);
                    Rooms.Add(room.Name, teamRoom);
                    
                    RoomPanel roomPanel = teamRoom.GetComponent<RoomPanel>();

                    if (roomPanel != null)
                    {
                        roomPanel.RoomNumber = room.Name;
                        roomPanel.PlayerCount = $"{room.PlayerCount}/{room.MaxPlayers}";
                    }
                }
                
                else if (gameMode == "Solo")
                {
                    GameObject soloRoom = Instantiate(RoomPrefab, SoloListContent.transform);
                    Rooms.Add(room.Name, soloRoom);
                    
                    RoomPanel roomPanel = soloRoom.GetComponent<RoomPanel>();

                    if (roomPanel != null)
                    {
                        roomPanel.RoomNumber = room.Name;
                        roomPanel.PlayerCount = $"{room.PlayerCount}/{room.MaxPlayers}";
                    }
                }
            }
        }
        else
        {
            foreach (var room in roomList)
            {
                for (int i = 0; i < Rooms.Count; i++)
                {
                    if (Rooms.ContainsKey(room.Name))
                    {

                        if (room.RemovedFromList)
                        {
                            Destroy(Rooms[room.Name]);
                            Rooms.Remove(room.Name);
                        }
                        else
                        {
                            RoomPanel roomPanel = Rooms[room.Name].GetComponent<RoomPanel>();

                            if (roomPanel != null)
                            {
                                //roomPanel.RoomNumber = room.Name;
                                roomPanel.PlayerCount = $"{room.PlayerCount}/{room.MaxPlayers}";
                            }
                        }
                    }
                    else
                    {
                        string gameMode = (string)room.CustomProperties["GameMode"];
                
                        if (gameMode == "Team")
                        {
                            GameObject teamRoom = Instantiate(RoomPrefab, TeamListContent.transform);
                            Rooms.Add(room.Name, teamRoom);
                    
                            RoomPanel roomPanel = teamRoom.GetComponent<RoomPanel>();

                            if (roomPanel != null)
                            {
                                roomPanel.RoomNumber = room.Name;
                                roomPanel.PlayerCount = $"{room.PlayerCount}/{room.MaxPlayers}";
                            }
                        }
                
                        else if (gameMode == "Solo")
                        {
                            GameObject soloRoom = Instantiate(RoomPrefab, SoloListContent.transform);
                            Rooms.Add(room.Name, soloRoom);
                    
                            RoomPanel roomPanel = soloRoom.GetComponent<RoomPanel>();

                            if (roomPanel != null)
                            {
                                roomPanel.RoomNumber = room.Name;
                                roomPanel.PlayerCount = $"{room.PlayerCount}/{room.MaxPlayers}";
                            }
                        }
                    }
                }
            }
        }
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log("Join");
        ExitGames.Client.Photon.Hashtable customProps = PhotonNetwork.CurrentRoom.CustomProperties;
        if (PhotonNetwork.LocalPlayer.Equals(PhotonNetwork.MasterClient))
        {
            ReadyButton.SetActive(false);
            StartButton.SetActive(true);
            
        }
        else
        {
            ReadyButton.SetActive(true);
            StartButton.SetActive(false);
        }
        
        if (customProps.ContainsKey("GameMode"))
        {
            string gameMode = (string)customProps["GameMode"];

            List<Player> playerList = PhotonNetwork.PlayerListOthers.ToList();
            bool[] playerNumberExist = new bool[PhotonNetwork.CurrentRoom.MaxPlayers];

            foreach (Player player in playerList)
            {
                int playerNumber = (int)player.CustomProperties["Number"];
                playerNumberExist[playerNumber - 1] = true;

                bool isMasterClient = player.Equals(PhotonNetwork.MasterClient);
                CharacterSlots[playerNumber - 1].SetMasterClient(isMasterClient);
                TMP_Text nameText = CharacterSlots[playerNumber - 1].Name;
                nameText.text = player.NickName;
                Image classImage = CharacterSlots[playerNumber - 1].SlotImage;
                if (classImage != null) UpdateCharacterImage(classImage, (ClassType)player.CustomProperties["Class"]);
            }

            for (int n = 1; n <= PhotonNetwork.CurrentRoom.MaxPlayers; n++)
            {
                if (playerNumberExist[n - 1]) continue;
                PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
                {
                    { "Number", n },
                    { "Class", ClassType.Warrior}
                });
                playerNumberExist[n - 1] = true;
                break;
            }

            RoomCodeText.text = "Room Code : " + PhotonNetwork.CurrentRoom.Name;
            ReadyPanel.SetActive(true);
            TeamReadyPanel.SetActive(gameMode == "Team");
        }
        
        UIEffects.FadeOut(LoadingPanel, this);
    }

    public override void OnLeftRoom()
    {
        for (int i = 0; i < 4; i++)
        {
            CharacterSlots[i].Name.text = "";
            CharacterSlots[i].Name.color = Color.black;
            CharacterSlots[i].SlotImage.sprite = DefaultButtonImage;
        }
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        /*
        //여기 아래줄에서 오류 NullReference
        int playerNumber = (int)newPlayer.CustomProperties["Number"];
        Debug.Log("Enter: " + playerNumber);
        TMP_Text nameText = CharacterSlots[playerNumber - 1].GetComponentInChildren<TMP_Text>();

        if (nameText != null) nameText.text = newPlayer.NickName;
        
        Sprite classImage = CharacterSlots[playerNumber - 1].GetComponent<Sprite>();
        if (classImage != null)
        {
            classImage = ClassImages[(ClassType)newPlayer.CustomProperties["Class"]];
        }
        */
    }

    public override void OnPlayerLeftRoom(Player leavedPlayer)
    {
        int playerNumber = (int)leavedPlayer.CustomProperties["Number"];
        TMP_Text nameText = CharacterSlots[playerNumber - 1].Name;
        if (nameText != null) nameText.text = "";

        CharacterSlots[playerNumber - 1].SetMasterClient(false);
        Image classImage = CharacterSlots[playerNumber - 1].SlotImage;
        classImage.sprite = DefaultButtonImage;
    }
    
    public override void OnPlayerPropertiesUpdate(Player player, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (!changedProps.ContainsKey("Number")) return;
        
        int playerNumber = (int)changedProps["Number"];
        CharacterSlots[playerNumber - 1].SetMasterClient(player.Equals(PhotonNetwork.MasterClient));
        TMP_Text nameText = CharacterSlots[playerNumber - 1].Name;

        if (nameText != null) nameText.text = player.NickName;

        if (player.Equals(PhotonNetwork.LocalPlayer))
        {
            nameText.color = Color.green;
        }
        Image classImage = CharacterSlots[playerNumber - 1].SlotImage;
        if (changedProps.ContainsKey("Class") && classImage != null)
        {
            UpdateCharacterImage(classImage, (ClassType)changedProps["Class"]);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (newMasterClient.Equals(PhotonNetwork.LocalPlayer))
        {
            ReadyButton.SetActive(false);
            StartButton.SetActive(true);
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            bool isMasterClient = Equals(player, PhotonNetwork.MasterClient);
            int playerNumber = (int)player.CustomProperties["Number"];
            CharacterSlots[playerNumber - 1].SetMasterClient(isMasterClient);
        }
    }
    
    public String CreateRandomRoomName()
    {
        const string chars = "0123456789";
        var random = new System.Random();
        //RoomInfo[] roomList = PhotonNetwork.GetCustomRoomList(TypedLobby.Default, null);
        string roomName = new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());

        return roomName;
    }

    public void OnClickLeaveSoloRoom()
    {
        PhotonNetwork.LeaveRoom();
        ReadyPanel.SetActive(false);
    }

    public void OnClickShowSoloPanel()
    {
        SoloListPanel.SetActive(true);
    }

    public void OnClickHideSoloPanel()
    {
        SoloListPanel.SetActive(false);
    }

    public void OnInputFieldValueChanged()
    {
        PhotonNetwork.LocalPlayer.NickName = playerNickname.text;
    }

    private void UpdateCharacterImage(Image targetImage, ClassType classType)
    {
        switch (classType)
        {
            case ClassType.Warrior:
                targetImage.sprite = WarriorImage;
                break;
            case ClassType.Mage:
                targetImage.sprite = MageImage;
                break;
        }
    }

    public void OnClickPlayerReady()
    {
        
    }
    
    public void OnClickEnterSoloGame()
    {
        PhotonNetwork.LoadLevel("SoloGame");
    }
}

using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using SwordNShield.Controller;
using UnityEngine;

public class SoloNetworkManager : MonoBehaviourPunCallbacks
{
    public CinemachineVirtualCamera virtualCamera;
    public GameObject player;
    [SerializeField] private List<Vector3> SpawnPositions;

    void Awake()
    {
        PlayerController.OnDeath += Respawn;
        Spawn();
    }

    public void Spawn()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Score", 0 } });
        player = PhotonNetwork.Instantiate(
            "Warrior", 
            SpawnPositions[(int)PhotonNetwork.LocalPlayer.CustomProperties["Number"] - 1], 
            Quaternion.identity);

        virtualCamera.Follow = player.transform;
    }

    void Respawn()
    {
        int randomIndex = Random.Range(0, SpawnPositions.Count);
        player = PhotonNetwork.Instantiate("Warrior", SpawnPositions[randomIndex], Quaternion.identity);
        virtualCamera.Follow = player.transform;
    }
}

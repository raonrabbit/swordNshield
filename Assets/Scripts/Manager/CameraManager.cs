using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    
    void LateUpdate(){
        transform.position = new Vector3(player.position.x, player.position.y, -10);
    }
}

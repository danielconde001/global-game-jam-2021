using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Transform playerSpawnPoint;
    public Transform PlayerSpawnPoint
    {
        get {return playerSpawnPoint;}
    }

    private RoomSpawner roomSpawner;
    public RoomSpawner RoomSpawner
    {
        set {roomSpawner = value;}
    }

    public void HasFinishedRoom()
    {
        roomSpawner.SpawnRoom();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private int timesNormalRoomSpawns = 3;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<GameObject> normalRooms;
    [SerializeField] private List<GameObject> keyRooms;
    [SerializeField] private GameObject finalRoom;

    private GameObject storedRoomClone;
    private Transform playerTransform;
    private int normalRoomCounter = 0;
    private int keyRoomCounter = 0;

    public void SpawnRoom()
    {
        if(keyRoomCounter < keyRooms.Count)
        {
            if(normalRoomCounter < timesNormalRoomSpawns)
            {
                if(storedRoomClone != null)
                {
                    Destroy(storedRoomClone);
                    navMeshSurface.BuildNavMesh();
                }
                StartCoroutine(SpawnRoomDelay());
            }
            else
            {
                //spawn keyRoom
                keyRoomCounter++;
            }
        }
        else
        {
            //spawn finalRoom
        }
    }

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SpawnRoom();
    }

    private IEnumerator SpawnRoomDelay()
    {
        yield return new WaitForEndOfFrame();
        GameObject roomClone = (GameObject)Instantiate(normalRooms[Random.Range(0, normalRooms.Count)], spawnPoint.transform.position, spawnPoint.transform.rotation);
        storedRoomClone = roomClone;
        Room room = roomClone.GetComponent<Room>();
        room.RoomSpawner = this;
        playerTransform.position = room.PlayerSpawnPoint.position;
        playerTransform.rotation = room.PlayerSpawnPoint.rotation;
        navMeshSurface.BuildNavMesh();
        normalRoomCounter++;
    }
}

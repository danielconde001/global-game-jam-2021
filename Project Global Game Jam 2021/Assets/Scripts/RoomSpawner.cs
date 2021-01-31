using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FPSControllerLPFP;
using DG.Tweening;
using UnityEngine.UI;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private Image faderImage;
    [SerializeField] private HandgunScriptLPFP handgunScriptLPFP;
    [SerializeField] private FpsControllerLPFP fpsControllerLPFP;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private int timesNormalRoomSpawns = 3;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<GameObject> normalRooms;
    [SerializeField] private List<GameObject> keyRooms;
    [SerializeField] private GameObject finalRoom;

    private GameObject storedRoomClone;
    private Transform playerTransform;
    private Tween fadeToBlackTween;
    private Tween fadeToClearTween;
    private int normalRoomCounter = 0;
    private int keyRoomCounter = 0;

    public void SpawnRoom()
    {
        StartCoroutine(SpawnTheRoom());
    }

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        fadeToBlackTween = faderImage.DOColor(Color.black, 1.0f);
        fadeToBlackTween.Pause();
        fadeToClearTween = faderImage.DOColor(Color.clear, 1.0f);
        fadeToClearTween.Pause();

        faderImage.color = Color.black;
        StartCoroutine(SpawnRoomFix());
    }

    private IEnumerator SpawnTheRoom()
    {
        fadeToBlackTween.Rewind();
        fadeToBlackTween.Play();
        handgunScriptLPFP.CanUseGun = false;
        fpsControllerLPFP.CanLook = false;
        fpsControllerLPFP.CanMove = false;
        playerHealth.IsInvulnerable = true;

        yield return new WaitForSeconds(1.0f);

        if(keyRoomCounter < keyRooms.Count)
        {
            if(normalRoomCounter < timesNormalRoomSpawns)
            {
                if(storedRoomClone != null)
                {
                    Destroy(storedRoomClone);
                    navMeshSurface.BuildNavMesh();
                }
                StartCoroutine(SpawnRoomFix());
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

    private IEnumerator SpawnRoomFix()
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

        fadeToClearTween.Rewind();
        fadeToClearTween.Play();
        handgunScriptLPFP.CanUseGun = true;
        fpsControllerLPFP.CanLook = true;
        fpsControllerLPFP.CanMove = true;
        playerHealth.IsInvulnerable = false;
    }
}

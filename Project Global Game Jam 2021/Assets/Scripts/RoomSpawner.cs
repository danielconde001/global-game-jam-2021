using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FPSControllerLPFP;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoomSpawner : MonoBehaviour
{
    public static RoomSpawner current;
    [SerializeField] private string url;
    [SerializeField] private Image faderImage;
    [SerializeField] private HandgunScriptLPFP handgunScriptLPFP;
    [SerializeField] private FpsControllerLPFP fpsControllerLPFP;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private int timesNormalRoomSpawns = 3;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<GameObject> normalRooms;
    [SerializeField] private List<GameObject> keyRooms;
    [SerializeField] private GameObject finalRoom;
    [SerializeField] private AudioSource selfAudioSource;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;

    private GameObject storedRoomClone;
    private Transform playerTransform;
    private Tween fadeToBlackTween;
    private Tween fadeToClearTween;
    private int normalRoomCounter = 0;
    private int keyRoomCounter = 0;
    private int lastRoomIndex = -1;

    public void SpawnRoom()
    {
        StartCoroutine(SpawnTheRoom());
    }

    public void PlayRickRoll()
    {
        Application.OpenURL(url);
    }

    public void RestartLevel()
    {
        fadeToBlackTween.Restart();
        fadeToBlackTween.Play();
        StartCoroutine(RestartTheLevel());
    }

    private void Awake()
    {
        current = this;
        Cursor.visible = false;

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
        playerInteract.CanInteract = false;
        selfAudioSource.clip = doorOpen;
        selfAudioSource.Play();

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
                if(storedRoomClone != null)
                {
                    Destroy(storedRoomClone);
                    navMeshSurface.BuildNavMesh();
                }
                StartCoroutine(SpawnKeyRoomFix());
            }
        }
        else
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
                if(storedRoomClone != null)
                {
                    Destroy(storedRoomClone);
                    navMeshSurface.BuildNavMesh();
                }
                StartCoroutine(SpawnFinalRoomFix());
            }
        }

        /*
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
            if(storedRoomClone != null)
            {
                Destroy(storedRoomClone);
                navMeshSurface.BuildNavMesh();
            }
            StartCoroutine(SpawnFinalRoomFix());
        }
        */
    }

    private IEnumerator SpawnRoomFix()
    {
        yield return new WaitForEndOfFrame();
        
        int rng = Random.Range(0, normalRooms.Count);

        while(rng == lastRoomIndex)
            rng = Random.Range(0, normalRooms.Count);
            
        lastRoomIndex = rng;

        GameObject roomClone = (GameObject)Instantiate(normalRooms[rng], spawnPoint.transform.position, spawnPoint.transform.rotation);
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
        playerInteract.CanInteract = true;

        selfAudioSource.clip = doorClose;
        selfAudioSource.Play();
    }
    private IEnumerator SpawnKeyRoomFix()
    {
        yield return new WaitForEndOfFrame();
        GameObject roomClone = (GameObject)Instantiate(keyRooms[keyRoomCounter], spawnPoint.transform.position, spawnPoint.transform.rotation);
        storedRoomClone = roomClone;
        Room room = roomClone.GetComponent<Room>();
        room.RoomSpawner = this;
        playerTransform.position = room.PlayerSpawnPoint.position;
        playerTransform.rotation = room.PlayerSpawnPoint.rotation;
        navMeshSurface.BuildNavMesh();
        keyRoomCounter++;
        normalRoomCounter = 0;

        fadeToClearTween.Rewind();
        fadeToClearTween.Play();
        handgunScriptLPFP.CanUseGun = true;
        fpsControllerLPFP.CanLook = true;
        fpsControllerLPFP.CanMove = true;
        playerHealth.IsInvulnerable = false;
        playerInteract.CanInteract = true;

        selfAudioSource.clip = doorClose;
        selfAudioSource.Play();
    }

    private IEnumerator SpawnFinalRoomFix()
    {
        yield return new WaitForEndOfFrame();
        GameObject roomClone = (GameObject)Instantiate(finalRoom, spawnPoint.transform.position, spawnPoint.transform.rotation);
        storedRoomClone = roomClone;
        Room room = roomClone.GetComponent<Room>();
        room.RoomSpawner = this;
        playerTransform.position = room.PlayerSpawnPoint.position;
        playerTransform.rotation = room.PlayerSpawnPoint.rotation;
        navMeshSurface.BuildNavMesh();

        fadeToClearTween.Rewind();
        fadeToClearTween.Play();
        handgunScriptLPFP.CanUseGun = true;
        fpsControllerLPFP.CanLook = true;
        fpsControllerLPFP.CanMove = true;
        playerHealth.IsInvulnerable = false;
        playerInteract.CanInteract = true;

        selfAudioSource.clip = doorClose;
        selfAudioSource.Play();
    }

    private IEnumerator RestartTheLevel()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

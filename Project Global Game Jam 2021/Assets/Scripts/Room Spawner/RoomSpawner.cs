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
    [SerializeField] private List<GameObject> tutorialRooms;
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
    private Tween fadeToWhiteTween;
    private int normalRoomCounter = 0;
    private int keyRoomCounter = 0;
    private int tutorialRoomCounter = 0;
    private int lastRoomIndex = -1;

    public void SpawnRoom()
    {
        StartCoroutine(SpawnTheRoom());
    }

    public void PlayRickRoll()
    {
        StartCoroutine(PlayEnding());
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
        fadeToWhiteTween = faderImage.DOColor(Color.white, 3.0f);
        fadeToWhiteTween.Pause();

        faderImage.color = Color.black;
        StartCoroutine(SpawnTutorialRoomFix());
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

        if(storedRoomClone != null)
        {
            Destroy(storedRoomClone);
            navMeshSurface.BuildNavMesh();
        }

        if(tutorialRoomCounter < tutorialRooms.Count)
            StartCoroutine(SpawnTutorialRoomFix());
        else
        {
            if(keyRoomCounter < keyRooms.Count)
            {
                if(normalRoomCounter < timesNormalRoomSpawns)
                    StartCoroutine(SpawnRoomFix());
                else
                    StartCoroutine(SpawnKeyRoomFix());
            }
            else
            {
                if(normalRoomCounter < timesNormalRoomSpawns)
                    StartCoroutine(SpawnRoomFix());
                else
                    StartCoroutine(SpawnFinalRoomFix());
            }
        }
    }

    private IEnumerator SpawnTutorialRoomFix()
    {
        yield return new WaitForEndOfFrame();
        GameObject roomClone = (GameObject)Instantiate(tutorialRooms[tutorialRoomCounter], spawnPoint.transform.position, spawnPoint.transform.rotation);
        storedRoomClone = roomClone;
        Room room = roomClone.GetComponent<Room>();
        room.RoomSpawner = this;
        playerTransform.position = room.PlayerSpawnPoint.position;
        playerTransform.rotation = room.PlayerSpawnPoint.rotation;
        tutorialRoomCounter++;

        SetGameStuff();
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
        normalRoomCounter++;

        SetGameStuff();
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
        keyRoomCounter++;
        normalRoomCounter = 0;

        SetGameStuff();
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

        SetGameStuff();
    }

    private void SetGameStuff()
    {
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

    private IEnumerator PlayEnding()
    {
        handgunScriptLPFP.CanUseGun = false;
        fpsControllerLPFP.CanLook = false;
        fpsControllerLPFP.CanMove = false;
        playerHealth.IsInvulnerable = true;
        playerInteract.CanInteract = false;

        fadeToWhiteTween.Rewind();
        fadeToWhiteTween.Play();

        yield return new WaitForSeconds(3.0f);

        Application.OpenURL(url);
        Application.Quit();
    }
}

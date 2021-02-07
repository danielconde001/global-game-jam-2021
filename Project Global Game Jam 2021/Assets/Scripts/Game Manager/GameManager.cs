using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get 
        {
            if (!GameObject.FindObjectOfType<GameManager>())
            {
                GameObject newGameObject = new GameObject("GameManager");
                instance =  newGameObject.AddComponent<GameManager>();
            }
            else if (GameObject.FindObjectOfType<GameManager>()) 
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    #endregion

    [SerializeField] private PauseMenuManager pauseMenuManager;
    [SerializeField] private RoomSpawner roomSpawner;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private AudioManager audioManager;

    public PauseMenuManager PauseMenuManager { get => pauseMenuManager; }
    public RoomSpawner RoomSpawner { get => roomSpawner; }
    public PlayerManager PlayerManager { get => playerManager; }
    public AudioManager AudioManager { get => audioManager; }

    private void Awake() {
        instance = this;
        
        //Create an EventSystem object if it does not exist yet
        if (!FindObjectOfType<UnityEngine.EventSystems.EventSystem>() || 
            !FindObjectOfType<UnityEngine.EventSystems.StandaloneInputModule>())
        {
            if (!FindObjectOfType<UnityEngine.EventSystems.StandaloneInputModule>() && 
                FindObjectOfType<UnityEngine.EventSystems.EventSystem>())
                Destroy(FindObjectOfType<UnityEngine.EventSystems.EventSystem>().gameObject);

            GameObject newGameObject = new GameObject("EventSystem");
            newGameObject.AddComponent<UnityEngine.EventSystems.EventSystem>();
            newGameObject.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        if (!pauseMenuManager) 
        {
            if (FindObjectOfType<PauseMenuManager>()) pauseMenuManager = FindObjectOfType<PauseMenuManager>();
            else 
            {
                GameObject newGameObject = Instantiate(Resources.Load("Pause Menu Manager")) as GameObject;
                pauseMenuManager =  newGameObject.GetComponent<PauseMenuManager>();
            }
        }
        if (!roomSpawner) 
        {
            if (FindObjectOfType<RoomSpawner>()) roomSpawner = FindObjectOfType<RoomSpawner>();
            else 
            {
                GameObject newGameObject = Instantiate(Resources.Load("Room Spawner")) as GameObject;
                roomSpawner =  newGameObject.GetComponent<RoomSpawner>();
            }
        }
        if (!playerManager) 
        {
            if (FindObjectOfType<PlayerManager>()) playerManager = FindObjectOfType<PlayerManager>();
            else 
            {
                GameObject newGameObject = Instantiate(Resources.Load("Player Manager")) as GameObject;
                playerManager =  newGameObject.GetComponent<PlayerManager>();
            }
        }
        if (!audioManager) 
        {
            if (FindObjectOfType<AudioManager>()) audioManager = FindObjectOfType<AudioManager>();
            else 
            {
                GameObject newGameObject = Instantiate(Resources.Load("Audio Manager")) as GameObject;
                audioManager =  newGameObject.GetComponent<AudioManager>();
            }
        }
    }
}

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

    [SerializeField] PauseMenuManager pauseMenuManager;
    [SerializeField] RoomSpawner roomSpawner;
    [SerializeField] PlayerManager playerManager;

    private void Awake() {
        instance = this;

        pauseMenuManager = PauseMenuManager.Instance;
        roomSpawner = RoomSpawner.Instance;
        playerManager = PlayerManager.Instance;
    }
}

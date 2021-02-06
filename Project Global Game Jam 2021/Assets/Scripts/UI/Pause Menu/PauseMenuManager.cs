using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseMenuManager : MonoBehaviour
{
    #region Singleton
    private static PauseMenuManager instance;
    public static PauseMenuManager Instance
    {
        get 
        {
            if (!GameObject.FindObjectOfType<PauseMenuManager>())
            {
                GameObject newGameObject = Instantiate(Resources.Load("Pause Menu Manager")) as GameObject;
                
                if (!newGameObject.GetComponent<PauseMenuManager>())
                    instance =  newGameObject.AddComponent<PauseMenuManager>();

                else { instance = newGameObject.GetComponent<PauseMenuManager>(); }
            }
            else if (GameObject.FindObjectOfType<PauseMenuManager>()) 
            {
                instance = GameObject.FindObjectOfType<PauseMenuManager>();
            }
            return instance;
        }
    }
    #endregion
    
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    private Canvas pauseMenuCanvas;

    bool canPause = true; 
    public bool CanPause { 
        get { return canPause; } 
        set { canPause = value; }
    }

    bool paused = false;

    private void Awake() {
        pauseMenuCanvas = GetComponentInChildren<Canvas>();    
    }

    private void Start() {
        canPause = true;    
    }

    private void Update() {

        if (!canPause) return;

        if (Input.GetKeyDown(pauseKey) && !paused) Pause();
        else if (Input.GetKeyDown(pauseKey) && paused) Unpause();
    }

    private void Pause()
    {
        paused = true;
        PlayerManager.Instance.FpsController.enabled = false;
        PlayerManager.Instance.Handgun.enabled = false;
        PlayerManager.Instance.PlayerCanvas.enabled = false;
        pauseMenuCanvas.enabled = true;
        Time.timeScale = 0;
    }

    private void Unpause()
    {
        paused = false;
        PlayerManager.Instance.FpsController.enabled = true;
        PlayerManager.Instance.Handgun.enabled = true;
        PlayerManager.Instance.PlayerCanvas.enabled = true;
        pauseMenuCanvas.enabled = false;
        Time.timeScale = 1;
    }
}

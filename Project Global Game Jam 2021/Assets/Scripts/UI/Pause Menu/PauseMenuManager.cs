using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    // SINGELTON PRIVILEGES NOW DEFUNCT
    #region Singleton
    /*
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
    */
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

    public void Pause()
    {
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        GameManager.Instance.PlayerManager.FpsController.enabled = false;
        GameManager.Instance.PlayerManager.Handgun.enabled = false;
        GameManager.Instance.PlayerManager.PlayerCanvas.enabled = false;
        pauseMenuCanvas.enabled = true;
        GameManager.Instance.PlayerManager.PlayerInteract.enabled = false;
        GameManager.Instance.PlayerManager.PlayerAudioSource.enabled = false;
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.PlayerManager.FpsController.enabled = true;
        GameManager.Instance.PlayerManager.Handgun.enabled = true;
        GameManager.Instance.PlayerManager.PlayerCanvas.enabled = true;
        pauseMenuCanvas.enabled = false;
        GameManager.Instance.PlayerManager.PlayerInteract.enabled = true;
        GameManager.Instance.PlayerManager.PlayerAudioSource.enabled = true;
        Time.timeScale = 1;
    }

    public void AreYouSurePrompt()
    {
        //disable parent canvas

        //enable areyousure canvas

    }

    public void BackToPauseMenu()
    {
        //enable parent canvas

        //disable areyousure canvas
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

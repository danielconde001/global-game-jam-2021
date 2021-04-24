using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenuManager : MonoBehaviour
{
    enum PauseMenuWindow { Off, PauseMenu, Settings, AreYouSure, Count }
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private Canvas pauseMenuCanvas;
    [SerializeField] private Canvas settingsCanvas;
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private UnityEngine.UI.Text volumeText;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private UnityEngine.UI.Text mouseSensitivityText;
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private UnityEngine.UI.Text volumeInputText;

    bool canPause = true; 
    PauseMenuWindow currentWindow = PauseMenuWindow.Off;

    public bool CanPause { 
        get { return canPause; } 
        set { canPause = value; }
    }

    bool paused = false;

    private void Start() {
        canPause = true;    
    }

    private void Update() {

        if (!canPause) return;

        if (Input.GetKeyDown(pauseKey))
        {
            if (currentWindow == PauseMenuWindow.Off) Pause();
            else if (currentWindow == PauseMenuWindow.PauseMenu) Unpause();
            else if (currentWindow == PauseMenuWindow.Settings) BackToPauseMenu();
            else if (currentWindow == PauseMenuWindow.AreYouSure) BackToPauseMenu();
        }
    }

    public void Pause()
    {
        currentWindow = PauseMenuWindow.PauseMenu;
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
        currentWindow = PauseMenuWindow.Off;
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

    public void GoToAreYouSurePrompt()
    {
        //disable parent canvas

        //enable areyousure canvas

    }

    public void BackToPauseMenu()
    {
        pauseMenuCanvas.enabled = true;

        if (currentWindow == PauseMenuWindow.Settings) 
        { 
            currentWindow = PauseMenuWindow.PauseMenu;
            settingsCanvas.enabled = false;
        }

        if (currentWindow == PauseMenuWindow.AreYouSure)
        {

        }
    }

    public void GoToMainMenu()
    {
        //SceneManager.LoadScene("Main Menu");
        Application.Quit(); // temp
    }

    public void GoToSettings()
    {
        settingsCanvas.enabled = true;
        pauseMenuCanvas.enabled = false;
        currentWindow = PauseMenuWindow.Settings;
    }

    public void SetVolumeViaSlider(float volume)
    {
        mainMixer.SetFloat("Volume", volume);
        volumeText.text = ((int)volume+80).ToString();
    }

    public void SetMouseSensitivityViaSlider(float sensitivity)
    {
        GameManager.Instance.PlayerManager.FpsController.MouseSensitivity = sensitivity;
        mouseSensitivityText.text = ((int)(sensitivity)).ToString();
    }

    public void SetVolumeViaInputField()
    {
        float volume = float.Parse(volumeInputText.text);
        volume = Mathf.Clamp(volume, 0f, 100f);
        volumeText.text = volume.ToString();
        volume = volume - 80;
        mainMixer.SetFloat("Volume", volume);
        volumeSlider.value = volume;
        volumeInputText.text = volumeText.text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] float ButtonEnlargeValue;

    [SerializeField] Scene firstScene;
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;

    public void PlayButtonEnlarge()
    {
        playButton.GetComponent<RectTransform>().localScale = Vector3.one * ButtonEnlargeValue;
    }

    public void PlayButtonReturnToNormalSize()
    {
        playButton.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public void QuitButtonEnlarge()
    {
        quitButton.GetComponent<RectTransform>().localScale = Vector3.one * ButtonEnlargeValue;
    }

    public void QuitButtonReturnToNormalSize()
    {
        quitButton.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(firstScene.name);
    }
}

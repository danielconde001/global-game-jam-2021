using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clipOpenDoor;
    [SerializeField] private AudioClip clipWalk;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private Image faderImage;
    [SerializeField] float ButtonEnlargeValue;
    [SerializeField] string sceneName;
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;

    private Tween fadeToBlackTween;
    private Tween fadeToClearTween;

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
        StartCoroutine(QuitTheGame());
    }

    public void PlayGame()
    {
        StartCoroutine(LoadTheScene());
    }

    private void Awake()
    {
        fadeToBlackTween = faderImage.DOColor(Color.black, 1.0f);
        fadeToBlackTween.Pause();
        fadeToClearTween = faderImage.DOColor(Color.clear, 1.0f);
        fadeToClearTween.Pause();

        faderImage.color = Color.black;
        fadeToClearTween.Rewind();
        fadeToClearTween.Play();

        loadingScreen.SetActive(false);
    }

    private IEnumerator LoadTheScene()
    {
        audioSource.clip = clipOpenDoor;
        audioSource.Play();

        fadeToBlackTween.Rewind();
        fadeToBlackTween.Play();
        
        yield return new WaitForSeconds(1.0f);
        
        loadingScreen.SetActive(true);
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        while (!op.isDone)
        {
            loadingSlider.value = op.progress;
            yield return null;
        }
    }

    private IEnumerator QuitTheGame()
    {
        audioSource.clip = clipWalk;
        audioSource.Play();
        
        fadeToBlackTween.Rewind();
        fadeToBlackTween.Play();

        yield return new WaitForSeconds(1.0f);
        
        Application.Quit();
    }
}
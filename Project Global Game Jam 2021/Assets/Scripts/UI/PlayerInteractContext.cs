using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractContext : MonoBehaviour
{
    public static PlayerInteractContext current;

    [SerializeField] private GameObject interactContextTextPanel;
    [SerializeField] private GameObject interactContextPaperPanel;
    [SerializeField] private TextMeshProUGUI interactContextText;
    [SerializeField] private TextMeshProUGUI interactContextPaperText;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject interactContext;
    [SerializeField] private float textDuration;

    private Transform storedTargetTransform;
    private float timer = 0.0f;
    private bool isLookingAtPaper = false;
    private bool isHidden = false;
    public bool IsHidden
    {
        get {return isHidden;}
    }

    public void ShowText(string text)
    {
        interactContextText.text = text;
        interactContextTextPanel.SetActive(true);
        timer = textDuration;
    }

    public void HideText()
    {
        interactContextText.text = ""; 
        interactContextTextPanel.SetActive(false);
        timer = 0.0f;
    }

    public void ShowInteractContext(Transform targetTransform)
    {
        storedTargetTransform = targetTransform;
        interactContext.SetActive(true);
        isHidden = false;
    }

    public void HideInteractContext()
    {
        storedTargetTransform = null;
        interactContext.SetActive(false);
        isHidden = true;
    }

    public void ShowPaperText(string text)
    {
        HideText();
        
        GameManager.Instance.PlayerManager.Handgun.CanUseGun = false;
        GameManager.Instance.PlayerManager.FpsController.CanLook = false;
        GameManager.Instance.PlayerManager.FpsController.CanMove = false;
        GameManager.Instance.PlayerManager.PlayerInteract.CanInteract = false;

        interactContextPaperText.text = text;
        interactContextPaperPanel.SetActive(true);
        isLookingAtPaper = true;
    }

    public void HidePaperText()
    {
        interactContextPaperText.text = "";
        interactContextPaperPanel.SetActive(false);
        isLookingAtPaper = false;
    }

    private void Start()
    {
        current = this;
        HideInteractContext();
        HideText();
        HidePaperText();
    }

    private void Update()
    {
        if(timer != 0.0f)
            ShowTextTimer();

        if(isLookingAtPaper)
            ContextPaperChecker();
    }

    private void LateUpdate()
    {
        if(storedTargetTransform != null)
            MoveInteractContext();
    }

    private void MoveInteractContext()
    {
        interactContext.transform.position = mainCamera.WorldToScreenPoint(storedTargetTransform.position);
    }

    private void ShowTextTimer()
    {
        timer -= Time.deltaTime;

        if(timer <= 0.0f)
            HideText();
    }

    private void ContextPaperChecker()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            GameManager.Instance.PlayerManager.Handgun.CanUseGun = true;
            GameManager.Instance.PlayerManager.FpsController.CanLook = true;
            GameManager.Instance.PlayerManager.FpsController.CanMove = true;
            StartCoroutine(CanInteractDelay());
            HidePaperText();
        }
    }

    private IEnumerator CanInteractDelay()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.PlayerManager.PlayerInteract.CanInteract = true;
    }
}

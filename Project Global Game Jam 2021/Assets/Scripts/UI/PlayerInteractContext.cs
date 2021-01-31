using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractContext : MonoBehaviour
{
    public static PlayerInteractContext current;

    [SerializeField] private GameObject interactContextTextPanel;
    [SerializeField] private TextMeshProUGUI interactContextText;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject interactContext;
    [SerializeField] private float textDuration;

    private Transform storedTargetTransform;
    private float timer = 0.0f;
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

    private void Start()
    {
        current = this;
        HideInteractContext();
        HideText();
    }

    private void Update()
    {
        if(timer != 0.0f)
            ShowTextTimer();
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
        {
            HideText();
            timer = 0.0f;
        }
    }
}

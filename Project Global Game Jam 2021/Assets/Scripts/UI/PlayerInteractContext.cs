using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractContext : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject interactContext;

    private Transform storedTargetTransform;
    private bool isHidden = false;
    public bool IsHidden
    {
        get {return isHidden;}
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
        HideInteractContext();
    }

    private void Update()
    {
        if(storedTargetTransform != null)
            MoveInteractContext();
    }

    private void MoveInteractContext()
    {
        interactContext.transform.position = mainCamera.WorldToScreenPoint(storedTargetTransform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private PlayerInteractContext playerInteractContext;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float interactRange;
    [SerializeField] private bool canInteract = true;

    private Interactable storedInteractable;

    private void Update()
    {
        if(canInteract)
        {
            SelectInteractable();

            if(Input.GetKeyDown(KeyCode.E))
                InteractInteractable();
        }
    }

    private void SelectInteractable()
    {
        RaycastHit hit;
        if(Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, interactRange, interactableLayer))
        {
            if(hit.collider != null)
            {
                if(hit.collider.gameObject.GetComponent<Interactable>() != null)
                {
                    if(storedInteractable == null)
                    {
                        hit.collider.gameObject.GetComponent<Interactable>().Select();
                        playerInteractContext.ShowInteractContext(hit.collider.gameObject.GetComponent<Interactable>().UIContextPosition);
                        storedInteractable = hit.collider.gameObject.GetComponent<Interactable>();
                    }
                    else
                    {
                        if(storedInteractable != hit.collider.gameObject.GetComponent<Interactable>())
                        {
                            storedInteractable.Unselect();
                            hit.collider.gameObject.GetComponent<Interactable>().Select();
                            playerInteractContext.ShowInteractContext(hit.collider.gameObject.GetComponent<Interactable>().UIContextPosition);
                            storedInteractable = hit.collider.gameObject.GetComponent<Interactable>();
                        }
                    }
                }
                else
                {
                    if(storedInteractable != null)
                    {
                        storedInteractable.Unselect();
                        storedInteractable = null;
                    }

                    if(!playerInteractContext.IsHidden)
                        playerInteractContext.HideInteractContext();
                }
            }
            else
            {
                if(storedInteractable != null)
                {
                    storedInteractable.Unselect();
                    storedInteractable = null;
                }

                if(!playerInteractContext.IsHidden)
                    playerInteractContext.HideInteractContext();
            }
        }
        else
        {
            if(storedInteractable != null)
            {
                storedInteractable.Unselect();
                storedInteractable = null;
            }

            if(!playerInteractContext.IsHidden)
                playerInteractContext.HideInteractContext();
        }
    }

    private void InteractInteractable()
    {
        if(storedInteractable != null)
            storedInteractable.Interact();
    }
}

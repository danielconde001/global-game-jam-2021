using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineInteractable : Interactable
{
    [SerializeField] protected GameObject examinablePrefab;
    [SerializeField] protected GameObject examineCamera;
    [SerializeField] protected Transform examineSpot;

    GameObject spawnObj;

    protected void LateUpdate() {
        if (spawnObj) spawnObj.transform.position = examineSpot.position; 
    }

    public override void Interact()
    {
        base.Interact();

        examineCamera.SetActive(true);

        spawnObj = (GameObject)Instantiate
        (
            examinablePrefab, 
            examineSpot.position,
            Quaternion.identity
        );
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField] private float _MouseSensitivity = 1000f;
    [SerializeField] private Transform _MainBody;

    public float MouseSensitivity 
    { 
        get { return _MouseSensitivity; } 
        set { _MouseSensitivity = value; }
    }

    private float xRotation = 0f;
    private float originalSensitivity;
    private float aimSensitivity;

    void Start()
    {
        originalSensitivity = _MouseSensitivity;
        aimSensitivity = originalSensitivity - playerShoot.AimLookSensitivityDecrease;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        _MouseSensitivity = Mathf.Lerp(originalSensitivity, aimSensitivity, playerShoot.AimBlend);
        float mouseX = Input.GetAxis("Mouse X") * _MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _MouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        _MainBody.Rotate(Vector3.up, mouseX);
    }
}

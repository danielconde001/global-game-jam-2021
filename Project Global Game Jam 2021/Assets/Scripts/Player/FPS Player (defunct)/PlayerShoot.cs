using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Animator gunShootAnimator;
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private float aimSpeed;
    [SerializeField] private float aimFOVDecrease;
    [SerializeField] private float _AimLookSensitivityDecrease;
    [SerializeField] private float _AimMoveSpeedDecrease;

    private bool isAiming = false;
    private float _AimBlend = 0.0f;
    private float originalFOV;
    private float aimFOV;

    public float AimBlend
    {
        get {return _AimBlend;}
    }

    public float AimLookSensitivityDecrease
    {
        get {return _AimLookSensitivityDecrease;}
    }

    public float AimMoveSpeedDecrease
    {
        get {return _AimMoveSpeedDecrease;}
    }

    private void Start()
    {
        originalFOV = playerCamera.fieldOfView;
        aimFOV = originalFOV - aimFOVDecrease;
    }

    private void Update()
    {
        Aim();

        if(Input.GetKeyDown(KeyCode.Mouse1))
            isAiming = true;
        else if(Input.GetKeyUp(KeyCode.Mouse1))
            isAiming = false;

        if(Input.GetKeyDown(KeyCode.Mouse0))
            Shoot();
    }

    private void Aim()
    {
        if(isAiming)
        {
            if(_AimBlend < 1.0f)
            {
                _AimBlend += aimSpeed * Time.deltaTime;
                if(_AimBlend > 1.0f)
                    _AimBlend = 1.0f;
            }
        }
        else if(!isAiming)
        {
            if(_AimBlend > 0.0f)
            {
                _AimBlend -= aimSpeed * Time.deltaTime;
                if(_AimBlend < 0.0f)
                    _AimBlend = 0.0f;
            }
        }
        playerCamera.fieldOfView = Mathf.Lerp(originalFOV, aimFOV, _AimBlend);
        gunAnimator.SetFloat("aimBlend", _AimBlend);
    }

    private void Shoot()
    {
        gunShootAnimator.SetTrigger("Fire");
    }
}

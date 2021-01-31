using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Camera _FPSCamera;
    [SerializeField] private int _Damage = 10;
    [SerializeField] private float _Range = 100f;

    [SerializeField] private GameObject _ImpactEffect;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(_FPSCamera.transform.position, _FPSCamera.transform.forward, out hitInfo, _Range))
        {
            if (hitInfo.collider.GetComponent<Target>())
            {
                hitInfo.collider.GetComponent<Target>().TakeDamage(Random.Range(_Damage-3, _Damage+3));
            }
        
            GameObject impact = Instantiate(_ImpactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(impact, 2f);
        }
    }
}

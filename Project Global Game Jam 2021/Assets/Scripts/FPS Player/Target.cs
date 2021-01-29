using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private int _HealthPoints;

    public void TakeDamage(int damage)
    {
        _HealthPoints -= damage;

        if (_HealthPoints <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}

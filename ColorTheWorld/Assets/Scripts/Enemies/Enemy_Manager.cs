using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{

    [SerializeField] float LifePoints = 100;

    private void Update()
    {
        UpdateLife();
    }

    public void Dammage(float dammages)
    {
        LifePoints -= dammages;
    }

    void UpdateLife()
    {
        if (LifePoints <= 0)
        {
            LifePoints = 0;
            Death();
        }
    }

    void Death()
    {
        gameObject.SetActive(false);
    }
}

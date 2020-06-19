using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Combat_Manager : MonoBehaviour
{
    [SerializeField] private float maxDamage = 15f;
    Enemy_Controller enemy_Controller;

    [SerializeField] private float attackDelay = 2f;
    public  float timer = 2f;

    Character_Manager character_Manager; //player reference

    private void Awake()
    {
        enemy_Controller = GetComponent<Enemy_Controller>();
        character_Manager = GameObject.FindGameObjectWithTag("Player").GetComponent<Character_Manager>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > attackDelay)
            timer = attackDelay;

        Attack();
    }

    void Attack()
    {
        if(enemy_Controller.PlayerDistance() <= 5f && timer >= attackDelay)
        {   
            character_Manager.GetDamages(AttackDamage());
            timer = 0f;
        }
    }

    float AttackDamage()
    {
        float Damage =  Random.Range(0f, maxDamage);
        float TotalDammage = Damage < 1 ? 0 : Mathf.Round(10 - enemy_Controller.PlayerDistance() + Damage);
        //float TotalDammage = enemy_Controller.PlayerDistance() < 2 ? Mathf.Round(0 + Damage) : Mathf.Round(10 - enemy_Controller.PlayerDistance() + Damage);
        return TotalDammage;
    }
}

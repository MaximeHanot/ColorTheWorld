using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Manager : MonoBehaviour
{

    [SerializeField] private float lifePoints = 100f;
    [SerializeField] private float maxLifePoints = 100f;

    [SerializeField] private Image lifeBar;
    [SerializeField] private Image sprintBar;

    [HideInInspector] public float sprintAmount;

    [SerializeField] private Text damageText;

    private void Start()
    {
        damageText.enabled = false;
    }
    private void Update()
    {
        UpdateBars();
    }

    void CheckLife()
    {
        if (lifePoints < 0f)
        {
            //MORT
        }

        if(lifePoints > maxLifePoints)
        {
            lifePoints = maxLifePoints;
        }
    }

    void UpdateBars()
    {
        lifeBar.fillAmount = lifePoints / maxLifePoints;

        sprintBar.fillAmount = sprintAmount;
    }

    public void GetDamages(float damages)
    {
        lifePoints -= damages;

        if (damages != 0)
        {
            damageText.enabled = true;
            damageText.text = "- " + damages + "!";
            StartCoroutine(PrintDammageSend());
        }
        else if( damages == 0)
        {
            damageText.enabled = true;
            damageText.text = "Miss!";
            StartCoroutine(PrintDammageSend());
        }
    }


    IEnumerator PrintDammageSend()
    {
        yield return new WaitForSeconds(2f);
        damageText.enabled = false;
    }
}

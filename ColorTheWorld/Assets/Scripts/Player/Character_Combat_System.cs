using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Combat_System : MonoBehaviour
{

    [SerializeField] private LayerMask AttackLayer;
    [SerializeField] private Text dammageSendText;

    [Header("Weapon")]
    [SerializeField] private string actualWeaponName;
    public Weapon_Profile actualWeapon;
    public List<Weapon_Profile> Weapons;

    [SerializeField] private Transform WeaponsParent;

    float AttackDammage;
    float AttackDistance = 0;
    float TotalDammage;

    private void Awake()
    {
        actualWeapon = Weapons[0];
        SpawnWeaponMesh();
        ActiveWeapon();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            CastAttack();
        }
        else
            TotalDammage = 0;

        if(dammageSendText != null)
            ShowDammages();

        SwitchWeapon();

        actualWeaponName = actualWeapon.WeaponName;
    }

    public void CastAttack() //Send a raycast forward - if the ray touch an object with the layer, send calulated dammages
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, AttackLayer))
        {
            AttackDistance = hit.distance;
            AttackDammage = Random.Range(actualWeapon.AttackDammageMin, actualWeapon.AttackDammageMax);

            if (AttackDistance <= actualWeapon.MaxAttackDistance && AttackDistance >= actualWeapon.MinAttackDistance)
                hit.transform.SendMessage("Dammage", DammageNumber(), SendMessageOptions.DontRequireReceiver);
        }
        
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100, Color.red);
    }

    float DammageNumber() //Calculate the dammages
    {
        TotalDammage = Mathf.Round( (actualWeapon.MaxAttackDistance - AttackDistance ) + AttackDammage);
        return TotalDammage;
    }

    void ShowDammages() //Show on UI the dammages send to the enemies.
    {
        if (TotalDammage != 0)
        {
            dammageSendText.enabled = true;
            dammageSendText.text = "" + TotalDammage + "!";
            StartCoroutine(PrintDammageSend());
        }        
    }
    
   IEnumerator PrintDammageSend()
    {       
        yield return new WaitForSeconds(2f);
        dammageSendText.enabled = false;
    }

    void SwitchWeapon() //Change the current weapon
    {
        for(int i = 1; i <= Weapons.Count; i ++)
        {
            if (Input.GetButtonDown("Select_Weapon_" + i))
            {
                actualWeapon = Weapons[i - 1];
                ActiveWeapon();
            }
        }
    }

    void SpawnWeaponMesh()//Used at Awake : Spawn the mesh set in the scriptable object.
    {
        for (int i = 0; i < Weapons.Count; i++)
        {
            Instantiate(Weapons[i].WeaponMesh, WeaponsParent);
        }
    }

    void ActiveWeapon() //Active the current weapon
    {
        for (int i = 0; i < WeaponsParent.childCount; i++)
        {
            if (WeaponsParent.GetChild(i).gameObject.name.Contains(actualWeapon.WeaponMesh.name))
            {
                WeaponsParent.GetChild(i).gameObject.SetActive(true);
            }
            else
                WeaponsParent.GetChild(i).gameObject.SetActive(false);
        }
    }
}

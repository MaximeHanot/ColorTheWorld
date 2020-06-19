using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameName/Profiles/Weapon")]
public class Weapon_Profile : ScriptableObject
{
    public string WeaponName = "Set Weapon Name Here";

    public GameObject WeaponMesh;

    [Header("Attack Damages")]
    public float AttackDammageMin = 5;
    public float AttackDammageMax = 10;

    [Header("Attack Distance")]
    public float MaxAttackDistance = 6;
    public float MinAttackDistance = 0;

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest_Manager : Singleton<Chest_Manager>
{
    public Character_Inventory character_Inventory;

    public Animator animator;

    private int chestMonney;

    public Text monneyInChest;

    Chest_Content actualChest;

    public void OpeningChest(Chest_Content chestContent)
    {
        animator.SetBool("isOpen", true);
        actualChest = chestContent;
        chestMonney = chestContent.monneyIn;
        monneyInChest.text = "Monney: " + chestMonney;
    }

    public void GiveToPlayer()
    {
        character_Inventory.actualMonney += chestMonney;
        chestMonney = 0;
        actualChest.monneyIn = chestMonney;
        monneyInChest.text = "Monney: " + chestMonney;
    }

    public void CloseChest(Chest_Content chestContent)
    {
        animator.SetBool("isOpen", false);
        chestContent.monneyIn = chestMonney;
    }

}

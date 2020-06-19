using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_controller : MonoBehaviour
{
    public GameObject player;

    public Animator actionButtonAnimator;
    public void ActiveActionUI()
    {
        actionButtonAnimator.SetBool("isOpen", true);
    }
    public void DesactiveActionUI()
    {
        actionButtonAnimator.SetBool("isOpen", false);
    }

    public Animator inventoryAnimator;
    public Character_Inventory inventory;
    public Text inventory_money_text;
    public List<Text> invenotry_weapon_text;

    private void Update()
    {
        UpdateInventoryMenu();
    }

    public void UpdateInventoryMenu()
    {
        inventory_money_text.text = "Monney: " + inventory.actualMonney;

    for (int i = 0; i < invenotry_weapon_text.Count; i++)
    {
        if(player.GetComponentInChildren<Character_Combat_System>().Weapons[i] != null)
            invenotry_weapon_text[i].text = player.GetComponentInChildren<Character_Combat_System>().Weapons[i].WeaponName;
    }

        if(Input.GetButtonDown("Inventory"))
        {
            if (inventoryAnimator.GetBool("isOpen") == false)
                OpenInventory();
            else if (inventoryAnimator.GetBool("isOpen") == true)
                CloseInventory();
        }
    }
    public void OpenInventory()
    {
        player.GetComponent<Character_Controller>().StopMouseMovement();
        inventoryAnimator.SetBool("isOpen", true);
    }
    public void CloseInventory()
    {
        player.GetComponent<Character_Controller>().ActiveMouseMovement();
        inventoryAnimator.SetBool("isOpen", false);
    }


}

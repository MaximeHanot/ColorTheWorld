using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest_Trigger : MonoBehaviour
{
    public Chest_Content chestContent;

    /*public*/ UI_controller ui_controller;

    private void Awake()
    {
        ui_controller = FindObjectOfType<UI_controller>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            ui_controller.ActiveActionUI();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (Input.GetButtonDown("Action"))
            {
                ui_controller.DesactiveActionUI();
                other.GetComponent<Character_Controller>().StopMouseMovement();
                TriggerChest();
            }
            if (Input.GetButtonDown("Cancel"))
            {
                CloseChest();
                other.GetComponent<Character_Controller>().ActiveMouseMovement();
                ui_controller.ActiveActionUI();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ui_controller.DesactiveActionUI();
        if (other.transform.tag == "Player")
        {
            CloseChest();
            other.GetComponent<Character_Controller>().ActiveMouseMovement();
        }
    }

    public void TriggerChest()
    {
        //BLOQUER SOURIS MOUVEMNT
        //Cursor.visible = true;
        Chest_Manager.Instance.OpeningChest(chestContent);
    }

    public void GetFromChest()
    {
        Chest_Manager.Instance.GiveToPlayer();
    }

    public void CloseChest()
    {
        Chest_Manager.Instance.CloseChest(chestContent);
    }

}

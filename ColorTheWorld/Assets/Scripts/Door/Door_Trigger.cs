using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Trigger : MonoBehaviour
{
    public UI_controller ui_controller;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
               OpeningDoor();
                //OPEN DOOR
            }
            if (Input.GetButtonDown("Cancel"))
            {
                ui_controller.ActiveActionUI();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ui_controller.DesactiveActionUI();
        if (other.transform.tag == "Player")
        {
            other.GetComponent<Character_Controller>().ActiveMouseMovement();
            CloseDoor();
            //CLOSE DOOR (si la porte est ouverte) sinon RIEN
        }
    }


    public void OpeningDoor()
    {
        animator.SetBool("isOpen", true);
    }

    public void CloseDoor()
    {
        animator.SetBool("isOpen", false);
    }

}

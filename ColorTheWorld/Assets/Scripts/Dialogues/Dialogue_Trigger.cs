using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Trigger : MonoBehaviour
{
    public Dialogue dialogue;

    /*public*/ UI_controller ui_controller;
    private void Awake()
    {
        ui_controller = FindObjectOfType<UI_controller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            ui_controller.ActiveActionUI();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if(Input.GetButtonDown("Action") )
            {
                ui_controller.DesactiveActionUI();
                TriggerDialogue();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            StopDialogue();
        }
    }

    public void TriggerDialogue()
    {
        Dialogue_Manager.Instance.StartDialogue(dialogue);
    }

    public void StopDialogue()
    {
        Dialogue_Manager.Instance.EndDialogue();
    }
}

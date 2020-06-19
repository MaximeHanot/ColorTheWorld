using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    public UI_controller ui_controller;

    private void Awake()
    {
        ui_controller = FindObjectOfType<UI_controller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag =="Player")
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
                other.SendMessage("ClimbBoolean", true);
                other.transform.rotation = Quaternion.LookRotation( transform.rotation * Vector3.forward );
            }
            if (Input.GetButtonDown("Cancel"))
            {
                ui_controller.ActiveActionUI();
                other.SendMessage("ClimbBoolean", false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ui_controller.DesactiveActionUI();
        if (other.transform.tag == "Player")
        {
            other.SendMessage("ClimbBoolean", false);
        }
    }
}

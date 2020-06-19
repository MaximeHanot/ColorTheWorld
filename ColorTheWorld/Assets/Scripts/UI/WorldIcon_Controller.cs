using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldIcon_Controller : MonoBehaviour
{
	void Update ()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}

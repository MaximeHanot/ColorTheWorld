using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Collectable_Display : MonoBehaviour
{
    public List<GameObject> collectableObjects;

    public List<GameObject> collectedObjects;

    public List<int> collectedObjectNumbers;

    public List<GameObject> collectableUI;

    private void Awake()
    {
        GetAllCollectibles();
        FindAllCollected();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            FindAllCollected(); // check when we collect an object..

            ShowAllCollectedUI();
        }
    }

    void GetAllCollectibles()
    {
        collectableObjects = GameObject.FindGameObjectsWithTag("collectibles").ToList();
    }

    void FindAllCollected()
    {
        for(int i = 0; i < collectableObjects.Count; i++)
        {
            if (collectableObjects[i].GetComponent<Collectable_Trigger>().collectable.isCollected == true)
                collectedObjects.Add(collectableObjects[i]);
        }
        FindAllNumbers();
    }

    void FindAllNumbers()
    {
        for (int i = 0; i < collectedObjects.Count; i++)
        {
            collectedObjectNumbers.Add(collectedObjects[i].GetComponent<Collectable_Trigger>().collectable.collectableNumber);
        }
        
    }

    void ShowAllCollectedUI()
    {
        for (int i = 0; i < collectableUI.Count; i++)
        {
            Debug.Log("on I");
            for (int j = 0; j < collectedObjectNumbers.Count; j++)
            {
                Debug.Log("on J");
                if (collectableUI[collectedObjectNumbers[j]].GetComponentInChildren<Image>().name.Contains("Enable")) //.transform.Find("Use")
                {
                    collectableUI[collectedObjectNumbers[j]].GetComponentInChildren<Image>().enabled = true;// ne marche pas //gameObject.SetActive(true)
                    Debug.Log("on IJ and ENABLE");
                }
                else
                    Debug.Log(collectableUI[collectedObjectNumbers[j]].GetComponentInChildren<Image>().name);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    public GameObject chestClosed;
    public GameObject chestOpen;
    private bool isInRange = false;
    private bool isOpened = false;

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E) && !isOpened)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpened = true;
        chestClosed.SetActive(false);
        chestOpen.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}

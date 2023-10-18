using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum weapons
{
    Sword,
    Dagger,
    Hammer,
    Axe
}

public class rpg_collectable : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private weapons weaponType;
    [SerializeField] private GameObject uiElement;
    [SerializeField] private int uiElementNum;
    [SerializeField] private GameObject inventoryUi;


    private Vector3 initialPosition;
    private float oscillationSpeed = 1.0f;
    private float oscillationDistance = 0.5f;

    private bool isCollectable = true;
    private GameObject weapon;


    private void Start()
    {
        initialPosition = gameObject.transform.position;
        weapon = Instantiate(weaponPrefab, initialPosition, Quaternion.identity);
        uiElement.SetActive(false);
    }

    private void Update()
    {
        if (isCollectable)
        {
            float newY = initialPosition.y + Mathf.Sin(Time.time * oscillationSpeed) * oscillationDistance;
            weapon.transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollectable && other.CompareTag("Player"))
        {
            Destroy(weapon);
            isCollectable = false;
            uiElement.SetActive(true);
            inventoryUi.GetComponent<equipWeapon>().setActiveWeapon(uiElementNum);
        }
    }
}

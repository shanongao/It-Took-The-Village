using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equipWeapon : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject menu;
    [Header("Weapon Prefabs")]
    [SerializeField] private GameObject[] prefabs;

    private GameObject weapon;
    private bool[] activeWeapons;

    private void Start()
    {
        activeWeapons = new bool[4];
        menu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            menu.SetActive(!menu.activeSelf);
        }

        fixWeaponPosition();
        if (Input.GetKeyDown(KeyCode.Alpha1) && activeWeapons[0] && prefabs.Length >= 1)
        {
            InstantiatePrefabAtIndex(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && activeWeapons[1] && prefabs.Length >= 2)
        {
            InstantiatePrefabAtIndex(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && activeWeapons[2] && prefabs.Length >= 3)
        {
            InstantiatePrefabAtIndex(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && activeWeapons[3] && prefabs.Length >= 4)
        {
            Debug.Log("hi");
            InstantiatePrefabAtIndex(3);
        }
    }

    private void InstantiatePrefabAtIndex(int index)
    {
        if (prefabs[index] != null)
        {
           weapon = Instantiate(prefabs[index], transform.position, Quaternion.identity);
            
        }
    }

    private void fixWeaponPosition()
    {
        if (weapon)
        {
            weapon.transform.position = player.transform.position + new Vector3(0.4f, 0.4f, 0);
            weapon.transform.eulerAngles = new Vector3(0, 90, 0);
        }
    }

    public void setActiveWeapon(int key)
    {
        Debug.Log($"Setting {key - 1} true");
        activeWeapons[key - 1] = true;
        Debug.Log(activeWeapons[3]);
    }
}

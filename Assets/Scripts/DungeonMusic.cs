using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMusic : MonoBehaviour
{
    private BGMManager BGM;
    // Start is called before the first frame update
    void Start()
    {
        BGM = GameObject.FindWithTag("Music").GetComponent<BGMManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BGM.PlayDungeon();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BGM.PlayOverWorld();
        }
    }


}

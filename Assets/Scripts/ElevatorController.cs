using System.Diagnostics;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            animator.SetBool("StartElevator", true);
            // UnityEngine.Debug.Log("Elevator Controller is initialized.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // UnityEngine.Debug.Log("Player exited the elevator trigger.");
            animator.SetBool("StartElevator", false);

        }
    }
}

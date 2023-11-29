using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class coin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private int coinValue = 10;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private GameObject[] children;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private string loadCurrency;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
    }

    private void Update()
    {
        foreach (GameObject child in children)
        {
            child.transform.eulerAngles -= new Vector3(0, rotationSpeed * 0.01f, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //PlayCoinSound();
            audioSource.PlayOneShot(coinSound);
            UpdateCurrency();
            gameObject.SetActive(false);
        }
    }

    private void UpdateCurrency()
    {
        string currentCurrency = currencyText.text;
        int.TryParse(currentCurrency.Substring(1), out int originalNumber);
        int newCurr = originalNumber + coinValue;
        currencyText.text = $"${newCurr}";
    }

    private void PlayCoinSound()
    {
        if (coinSound != null)
        {
            audioSource.PlayOneShot(coinSound); // Play the coin sound effect
        }
    }

    //private void OnDisable()
    //{
    //    loadCurrency = currencyText.text;
    //}

    //private void OnEnable()
    //{
    //    currencyText.text = loadCurrency;
    //}
}

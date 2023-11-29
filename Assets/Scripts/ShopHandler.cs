using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopHandler : MonoBehaviour
{
    public static ShopHandler ActiveDialogHandler;

    public GameObject prompt;
    public GameObject dialogCanvas;
    public string promptText;

    private TextMeshProUGUI text;
    private bool inDialog;
    private bool isInRange;
    private GameObject player;
    private NewPlayerController playerController;
    [SerializeField] private TextMeshProUGUI currencyText;


    [SerializeField] private GameObject menu;
    [Header("Weapon Prefabs")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject currWeapon;
    private int cost = 0;

    private bool canPurchase;

    private void Awake()
    {
        text = dialogCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        prompt.SetActive(false);
        dialogCanvas.SetActive(false);
        TextMeshProUGUI tmp = prompt.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        tmp.SetText(promptText);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerController = player.GetComponent<NewPlayerController>(); // Updated to NewPlayerController
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

    private void Update()
    {
        prompt.SetActive(isInRange && !inDialog);

        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (inDialog)
            {
                CloseDialog();
                ResumePlayerMovement();
                inDialog = false;
            } else
            {
                prompt.SetActive(false);
                DisplayDialog();
                PausePlayerMovement();
                inDialog = true;
            }
        }

        if (inDialog && canPurchase && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            if (currWeapon)
            {
                currWeapon.GetComponent<WeaponAttackPower>().UpgradeWeapon();
                CloseDialog();
                ResumePlayerMovement();
                inDialog = false;

                string currentCurrency = currencyText.text;
                int.TryParse(currentCurrency.Substring(1), out int originalNumber);
                int newCurr = originalNumber - cost;
                currencyText.text = $"${newCurr}";
            }
        }
    }

    private void DisplayDialog()
    {
        dialogCanvas.SetActive(true);
        text.SetText(generatePrompt());
    }

    public void CloseDialog()
    {
        dialogCanvas.SetActive(false);
    }

    private void PausePlayerMovement()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    private void ResumePlayerMovement()
    {
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }


    private string generatePrompt()
    {
        
        int weapon = playerController.GetComponent<NewPlayerController>()._equippedWeapon;
        
        if (weapon == -1)
        {
            return "Ehh, what are you doing here! I only sharpen the weapons one" +
                " owns, find a weapon or equip it to upgrade it!";
        } else
        {
            currWeapon = prefabs[weapon];
            string[] weapons = { "Hammer", "Sword", "Axe" };
            int[] prices = { 20, 50, 30 };
            string currentCurrency = currencyText.text;
            int.TryParse(currentCurrency.Substring(1), out int originalNumber);
            int money = originalNumber;
            int price = prices[weapon];
         
            if (price <= money)
            {
                canPurchase = true;
                cost = price;
                return $"Ahh, they all come to me, I can upgrade your precious {weapons[weapon]}" +
                    $" for ${price} and make you mightier! Since you have ${money}, this is perfect " +
                    $"for you! Press Enter to proceees and get one step " +
                    $"closer to defeating your enemies!!";
            } else
            {
                return $"Ahhh, your powerful {weapons[weapon]} will need ${price} to upgrade!" +
                    $" But you only have ${money}. You must earn more money or try a different weapon!" +
                    $" It's worth it! Go warrior and come back to make yourself even stronger!";
            }
        }
    }
}

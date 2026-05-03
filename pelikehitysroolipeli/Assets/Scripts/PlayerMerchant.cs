using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerMerchant : MonoBehaviour
{
    public GameObject merchantUI;

    public TMP_Text nameLabel;

    public GameObject arrowUI;
    public GameObject foodUI;

    // ARROW UI
    public TMP_Dropdown arrowK‰rkiDropdown;
    public TMP_Dropdown arrowPer‰Dropdown;
    public Slider arrowLengthSlider;

    // FOOD UI
    public TMP_Dropdown foodP‰‰Dropdown;
    public TMP_Dropdown foodLisukeDropdown;
    public TMP_Dropdown foodKastikeDropdown;

    private MerchantInfo currentMerchant;

    // MERCHANT UI (animation)

    public RectTransform merchantPanel;

    public Vector2 hiddenPos = new Vector2(0, -600); // off-screen
    public Vector2 visiblePos = new Vector2(0, 0);   // on-screen

    public float slideSpeed = 8f;
    private bool isOpen = false;

    private Coroutine hideRoutine;

    public float closeThreshold = 0.1f;

    // PRICE LABEL

    public TMP_Text priceLabel;

    private void Start()
    {
        merchantUI.SetActive(false);
        merchantPanel.anchoredPosition = hiddenPos;
        isOpen = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        MerchantInfo merchant = other.GetComponent<MerchantInfo>();

        if (merchant == null) return;
        currentMerchant = merchant;

        merchantUI.SetActive(true);
        isOpen = true;

        if (merchant.type == MerchantType.Arrow)
        {
            nameLabel.text = "Arrow Merchant";
            arrowUI.SetActive(true);
            foodUI.SetActive(false);
            SetupArrowUI();
        }
        else if (merchant.type == MerchantType.Food)
        {
            nameLabel.text = "Food Merchant";
            arrowUI.SetActive(false);
            foodUI.SetActive(true);
            SetupFoodUI();
        }

        UpdatePriceUI();

        AudioManager.Instance.PlaySound(AudioManager.SoundEffect.NearMerchant);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<MerchantInfo>() != null)
        {
            isOpen = false;
            currentMerchant = null;
        }
    }

    // ---------------- ARROW ----------------

    void SetupArrowUI()
    {
        arrowK‰rkiDropdown.ClearOptions();
        arrowK‰rkiDropdown.AddOptions(
            new System.Collections.Generic.List<string>(
                System.Enum.GetNames(typeof(K‰rki_tyyppi))
            )
        );

        arrowPer‰Dropdown.ClearOptions();
        arrowPer‰Dropdown.AddOptions(
            new System.Collections.Generic.List<string>(
                System.Enum.GetNames(typeof(Per‰_tyyppi))
            )
        );

        arrowLengthSlider.minValue = 60;
        arrowLengthSlider.maxValue = 100;

        UpdatePriceUI();
    }

    public void BuyArrow()
    {
        K‰rki_tyyppi k‰rki = (K‰rki_tyyppi)System.Enum.Parse(
            typeof(K‰rki_tyyppi),
            arrowK‰rkiDropdown.options[arrowK‰rkiDropdown.value].text.Trim()
        );

        Per‰_tyyppi per‰ = (Per‰_tyyppi)System.Enum.Parse(
            typeof(Per‰_tyyppi),
            arrowPer‰Dropdown.options[arrowPer‰Dropdown.value].text.Trim()
        );

        int pituus = (int)arrowLengthSlider.value;

        Nuolet nuoli = new Nuolet();
        nuoli.k‰rki = k‰rki;
        nuoli.per‰ = per‰;
        nuoli.pituus = pituus;

        int cost = (int)nuoli.PalautaHinta();

        if (PlayerDataManager.Instance.TakeMoney(cost))
        {
            Debug.Log($"Ostit nuolen: {k‰rki}, {per‰}, {pituus}");
        }
    }

    // ---------------- FOOD ----------------

    void SetupFoodUI()
    {
        foodP‰‰Dropdown.ClearOptions();
        foodP‰‰Dropdown.AddOptions(
            new System.Collections.Generic.List<string>(
                System.Enum.GetNames(typeof(P‰‰raaka_aine))
            )
        );

        foodLisukeDropdown.ClearOptions();
        foodLisukeDropdown.AddOptions(
            new System.Collections.Generic.List<string>(
                System.Enum.GetNames(typeof(Lisuke))
            )
        );

        foodKastikeDropdown.ClearOptions();
        foodKastikeDropdown.AddOptions(
            new System.Collections.Generic.List<string>(
                System.Enum.GetNames(typeof(Kastike))
            )
        );

        UpdatePriceUI();
    }

 
    int GetFoodPrice()
    {
        int basePrice = 10;

        int p‰‰ = foodP‰‰Dropdown.value;
        int lisuke = foodLisukeDropdown.value;
        int kastike = foodKastikeDropdown.value;

        return basePrice + p‰‰ + lisuke + kastike;
    }

    public void BuyFood()
    {
        P‰‰raaka_aine p‰‰ = (P‰‰raaka_aine)System.Enum.Parse(
            typeof(P‰‰raaka_aine),
            foodP‰‰Dropdown.options[foodP‰‰Dropdown.value].text.Trim()
        );

        Lisuke lisuke = (Lisuke)System.Enum.Parse(
            typeof(Lisuke),
            foodLisukeDropdown.options[foodLisukeDropdown.value].text.Trim()
        );

        Kastike kastike = (Kastike)System.Enum.Parse(
            typeof(Kastike),
            foodKastikeDropdown.options[foodKastikeDropdown.value].text.Trim()
        );

        int cost = 15;
        int heal = 10;

        if (PlayerDataManager.Instance.TakeMoney(cost))
        {
            PlayerDataManager.Instance.AddHealth(heal);
            Debug.Log($"Sˆit annoksen: {p‰‰}, {lisuke}, {kastike}");
        }
    }

    // ---------------- BUY ----------------

    public void Buy()
{
    if (currentMerchant == null) return;

        int moneyBefore = PlayerDataManager.Instance.Money;

        if (currentMerchant.type == MerchantType.Arrow)
        BuyArrow();
    else
        BuyFood();

    //  jos raha v‰heni -> osto onnistui
    if (PlayerDataManager.Instance.Money < moneyBefore)
    {
        AudioManager.Instance.PlaySound(AudioManager.SoundEffect.Buy);
    }
}

    // ---------------- ANIMATION ----------------

    void Update()
    {
        Vector2 target = isOpen ? visiblePos : hiddenPos;

        merchantPanel.anchoredPosition = Vector2.Lerp(
            merchantPanel.anchoredPosition,
            target,
            Time.deltaTime * slideSpeed
        );

        if (!isOpen && merchantUI.activeSelf)
        {
            float distance = Vector2.Distance(merchantPanel.anchoredPosition, hiddenPos);

            if (distance < closeThreshold)
            {
                merchantUI.SetActive(false);
            }
        }
    }

    // ---------------- PRICE -----------------

    int GetArrowPrice()
    {
        K‰rki_tyyppi k‰rki = (K‰rki_tyyppi)System.Enum.Parse(
            typeof(K‰rki_tyyppi),
            arrowK‰rkiDropdown.options[arrowK‰rkiDropdown.value].text.Trim()
        );

        Per‰_tyyppi per‰ = (Per‰_tyyppi)System.Enum.Parse(
            typeof(Per‰_tyyppi),
            arrowPer‰Dropdown.options[arrowPer‰Dropdown.value].text.Trim()
        );

        int pituus = (int)arrowLengthSlider.value;

        Nuolet nuoli = new Nuolet();
        nuoli.k‰rki = k‰rki;
        nuoli.per‰ = per‰;
        nuoli.pituus = pituus;

        return (int)nuoli.PalautaHinta();
    }

    void UpdatePriceUI()
    {
        if (currentMerchant == null || priceLabel == null) return;

        int price = 0;

        if (currentMerchant.type == MerchantType.Arrow)
            price = GetArrowPrice();
        else
            price = GetFoodPrice();

        priceLabel.text = "Price: " + price;
    }

    public void OnMerchantOptionChanged()
    {
        UpdatePriceUI();
    }

    void Awake()
    {
        // Arrow UI
        arrowK‰rkiDropdown.onValueChanged.AddListener(_ => UpdatePriceUI());
        arrowPer‰Dropdown.onValueChanged.AddListener(_ => UpdatePriceUI());
        arrowLengthSlider.onValueChanged.AddListener(_ => UpdatePriceUI());

        // Food UI
        foodP‰‰Dropdown.onValueChanged.AddListener(_ => UpdatePriceUI());
        foodLisukeDropdown.onValueChanged.AddListener(_ => UpdatePriceUI());
        foodKastikeDropdown.onValueChanged.AddListener(_ => UpdatePriceUI());
    }
}
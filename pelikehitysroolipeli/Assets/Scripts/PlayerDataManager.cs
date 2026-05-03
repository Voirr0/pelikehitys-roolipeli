using UnityEngine;
using TMPro;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }


    
    private int experience = 0;
    private int money = 0;
    private int health = 0;
    private int maxHealth = 20;

    public int Money => money;


    public TMP_Text experienceText;
    public TMP_Text moneyText;
    public TMP_Text healthText;



    public void AddExperience(int amount)
    {
        experience += amount;
        if (experience < 0) experience = 0;
        UpdateUI();
    }

    public void AddHealth(int amount)
    {
        health += amount;

        if (health > maxHealth)
            health = maxHealth;

        if (health < 0)
            health = 0;

        UpdateUI();
    }

    public int RemoveHealth(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0) health = 0;
        UpdateUI();
        return health;
    }

    public int AddMoney(int coinAmount)
    {
        money += coinAmount;
        if (money < 0) money = 0;
        UpdateUI();
        return money;
    }

    public bool TakeMoney(int coinAmount)
    {
        if (money >= coinAmount)
        {
            money -= coinAmount;
            UpdateUI();
            return true;
        }

        return false;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        experienceText.text = "XP: " + experience;
        moneyText.text = "Money: " + money;
        healthText.text = "HP: " + health + " / " + maxHealth;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 30), "Add XP"))
            AddExperience(10);

        if (GUI.Button(new Rect(10, 50, 150, 30), "Take Damage"))
            RemoveHealth(5);

        if (GUI.Button(new Rect(10, 170, 150, 30), "Add Health"))
            AddHealth(5);

        if (GUI.Button(new Rect(10, 90, 150, 30), "Add Money"))
            AddMoney(5);

        if (GUI.Button(new Rect(10, 130, 150, 30), "Spend Money"))
            TakeMoney(5);
    }



}




using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmo : MonoBehaviour
{
    [Header("Bullet Ammo")]
    [SerializeField] private int maxBullets = 10;
    private int currentBullets;

    [Header("Rocket Ammo")]
    [SerializeField] private int maxRockets = 5;
    private int currentRockets;

    [SerializeField] private Text ammoText;
    [SerializeField] private Text rocketText;

    private void Awake()
    {
        currentBullets = maxBullets;
        currentRockets = maxRockets;
        UpdateAmmoUI();
    }

    public bool CanShoot()
    {
        return currentBullets > 0;
    }

    public void UseBullet()
    {
        if (currentBullets > 0)
        {
            currentBullets--;
            UpdateAmmoUI();
        }
    }

    public void AddBullets(int amount)
    {
        currentBullets += amount;
        if (currentBullets > maxBullets)
            currentBullets = maxBullets;
        UpdateAmmoUI();
    }
    public bool CanShootRocket()
    {
        return currentRockets > 0;
    }

    public void UseRocket()
    {
        if (currentRockets > 0)
        {
            currentRockets--;
            UpdateAmmoUI();
        }
    }

    public void AddRockets(int amount)
    {
        currentRockets = Mathf.Min(maxRockets, currentRockets + amount);
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = $"{currentBullets}";

        if (rocketText != null)
            rocketText.text = $"{currentRockets}";
    }
}
using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmo : MonoBehaviour
{
   [SerializeField] private int maxBullets = 10;
    private int currentBullets;

    [SerializeField] private Text ammoText; 

    private void Awake()
    {
        currentBullets = maxBullets;
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

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = $"{currentBullets}";
    }
}
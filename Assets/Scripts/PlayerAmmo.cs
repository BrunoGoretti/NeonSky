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
        if (currentBullets <= 0) return;
        currentBullets--;
        UpdateAmmoUI();
    }

    public void AddBullets(int amount)
    {
        currentBullets = Mathf.Clamp(currentBullets + amount, 0, maxBullets);
        UpdateAmmoUI();
    }

    public void Reload()
    {
        currentBullets = maxBullets;
        UpdateAmmoUI();
    }

    public int GetCurrentBullets()
    {
        return currentBullets;
    }

    public int GetMaxBullets()
    {
        return maxBullets;
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = $"{currentBullets} / {maxBullets}";
    }
}
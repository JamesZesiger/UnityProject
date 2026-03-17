using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHUD : MonoBehaviour
{
    public TMP_Text ammoText;    // instead of Text
    public Image crosshair;

    private Gun currentGun;

    void Update()
    {
        if(currentGun != null)
        {
            ammoText.text = $"{currentGun.CurrentAmmo}/{currentGun.magazineSize}";
        }
    }

    public void SetGun(Gun gun)
    {
        currentGun = gun;
    }
}
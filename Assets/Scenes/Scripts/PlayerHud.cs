using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHUD : MonoBehaviour
{
    public TMP_Text ammoText;    
    public TMP_Text healthText;    
    public Image crosshair;


    private Gun currentGun;
    private int hp = 0;

    void Update()
    {

        healthText.text = $"{hp}%";
        
        if(currentGun != null)
        {
            ammoText.text = $"{currentGun.CurrentAmmo}/{currentGun.magazineSize}";
        }
    }

    public void SetGun(Gun gun)
    {
        currentGun = gun;
    }

    public void SetHealth(int hp)
    {
        this.hp = hp;
    }
}
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public Image hp;
    public Image hpeffect;
    Damageable Damageable;
    private float Health;
    private float MaxHealth;

    public Image atkType0;
    public Image atkType1;
    public TextMeshProUGUI atkType1b;

    public float hurtSpeed = 0.005f;

    // Start is called before the first frame update
    void Start()
    {
        Damageable = GameObject.Find("player").GetComponent<Damageable>();
        MaxHealth = Damageable.MaxHealth;
        Health = MaxHealth;
        atkType0.enabled = true;
        atkType1.enabled = false;
        atkType1b.enabled = false;
    }

    public void UpdateHp()
    {
        StartCoroutine(UpdateHpCo());
    }

    IEnumerator UpdateHpCo()
    {


        Health = Damageable.Health;

        hp.fillAmount = (Health / MaxHealth) * 0.6f + 0.07f;

        while (hpeffect.fillAmount > hp.fillAmount)
        {
            hpeffect.fillAmount -= hurtSpeed;
            yield return new WaitForSeconds(0.005f);
        }
        if (hpeffect.fillAmount < hp.fillAmount)
            hpeffect.fillAmount = hp.fillAmount;
    }

    public void ModeChange(int type)
    {
        if (type == 0)
        {
            atkType0.enabled = true;
            atkType1.enabled = false;
            atkType1b.enabled = false;
        }
        else if (type == 1)
        {
            atkType1.enabled = true;
            atkType1b.enabled = true;
            atkType0.enabled = false;
        }

    }

    public void AmmoCount(float number)
    {
        atkType1b.text = number.ToString();
    }

}

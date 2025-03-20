using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI soulsText;

    int money = 0;
    int souls = 0;

    void Awake() {
        UpdateHUD();
    }

    public int Money {
        get {
            return money;
        }

        set {
            money = value;
            UpdateHUD();
        }
    }

    public int Souls {
        get {
            return souls;
        }

        set {
            souls = value;
            UpdateHUD();
        }
    }

    void UpdateHUD() {
        moneyText.text = money.ToString();
        soulsText.text = souls.ToString();
    }
}

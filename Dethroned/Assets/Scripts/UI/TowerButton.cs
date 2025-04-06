using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerButton : MonoBehaviour
{
    public Image img;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI towerName;
    public TextMeshProUGUI keybind;
    [HideInInspector] public TowerBuilder.TowerSystem towerInfo;

    public void updateTower()
    {
        TowerBuilder.Instance.updateCurrentTower(towerInfo);
    }
}

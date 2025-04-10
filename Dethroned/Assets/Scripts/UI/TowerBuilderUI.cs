using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TowerBuilder;
using UnityEngine.Events;

public class TowerBuilderUI : MonoBehaviour
{
    public static TowerBuilderUI Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public GameObject content;
    public GameObject TowerContentPrefab;
    public Image currentTowerImage;
    public TMP_Dropdown targetPrio;
    public TMP_Text currentTowerText;
    private List<TowerBuilder.TowerSystem> towers => TowerBuilder.Instance.towers;
    private List<GameObject> towersList = new List<GameObject>();

    private void Start()
    {
        foreach (var tower in towers)
        {
            var obj = Instantiate(TowerContentPrefab, content.transform);
            if (obj.TryGetComponent<TowerButton>(out var butt))
            {
                butt.img.sprite = tower.sprite;
                butt.cost.text = tower.Prefab.GetComponent<Tower>().CostOfTower.ToString() + "G";
                butt.towerName.text = tower.name;
                butt.keybind.text = tower.Keybind.ToString();
                butt.towerInfo = tower;
            }
        }
        TowerBuilder.Instance.currentTowerUpdated.AddListener(updateCurrentTowerSprite);
    }

    public void updateCurrentTowerSprite(TowerBuilder.TowerSystem sys)
    {
        currentTowerImage.sprite = sys.sprite;
        currentTowerText.text = sys.name;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [System.Serializable]
    public class TowerSystem
    {
        [Tooltip("What key does the user press to be able to place this tower.")]
        public KeyCode Keybind;
        [Tooltip("What tower does the player spawn")]
        public GameObject Prefab;
    }

    public List<TowerSystem> towers = new List<TowerSystem>();
    private List<GameObject> _createdTowers = new List<GameObject>();
    TowerSystem currentTower;

    private void Start()
    {
        if (towers.Count > 0)
            currentTower = towers[0];
    }

    // Update is called once per frame
    void Update()
    {
        foreach (TowerSystem tower in towers)
        {
            if (Input.GetKeyUp(tower.Keybind))
            {
                currentTower = tower;
            }
        }

        if (GameController.canBuild)
        {

            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos = new Vector3(Mathf.Round(mouseWorldPos.x), Mathf.Round(mouseWorldPos.y), 0f);

            var costOfTower = currentTower.Prefab.GetComponent<TowerBase>().CostOfTower;
            if (Input.GetKeyUp(KeyCode.Mouse0) && costOfTower <= GameController.currency) // create
            {
                _createdTowers.Add(Instantiate(currentTower.Prefab, mouseWorldPos, Quaternion.identity));
                GameController.currency -= costOfTower;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1)) // destroy
            {
                foreach(var tower in _createdTowers)
                {
                    if (tower.transform.position.Equals(mouseWorldPos))
                    {
                        _createdTowers.Remove(tower);
                        var refund = tower.GetComponent<TowerBase>().CostOfTower;
                        GameController.currency += refund;
                        Destroy(tower);
                        break;
                    }
                }
            }
        }

    }
}

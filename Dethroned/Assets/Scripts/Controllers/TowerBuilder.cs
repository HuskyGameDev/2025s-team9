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

            // check if tower already exists at current position.
            bool __towerExists = false;
            GameObject __tower = null;
            foreach (var tower in _createdTowers)
            {
                if (tower.transform.position.Equals(mouseWorldPos))
                {
                    __tower = tower;
                    __towerExists = true;
                    break;
                }
            }

            // if you are trying to build the tower.
            var costOfTower = currentTower.Prefab.GetComponent<Tower>().CostOfTower;
            if (Input.GetKeyUp(KeyCode.Mouse0) && costOfTower <= GameController.currency && !__towerExists) // create
            {
                _createdTowers.Add(Instantiate(currentTower.Prefab, mouseWorldPos, Quaternion.identity));
                GameController.currency -= costOfTower;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1) && __towerExists) // destroy the existing tower.
            {
                _createdTowers.Remove(__tower);
                var refund = __tower.GetComponent<Tower>().CostOfTower;
                GameController.currency += refund;
                Destroy(__tower);
            }
        }

    }
}

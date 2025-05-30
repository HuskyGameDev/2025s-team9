using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TowerBuilder : MonoBehaviour
{

    public static TowerBuilder Instance;
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
        currentTowerUpdated = new UnityEvent<TowerSystem>();
    }

    [System.Serializable]
    public class TowerSystem
    {
        [Header("Tower Information")]
        public string name;
        [Tooltip("What key does the user press to be able to place this tower.")]
        public KeyCode Keybind;
        public Sprite sprite;

        [Header("Tower Models")]
        [Tooltip("What tower does the player spawn")]
        public GameObject Prefab;
    }

    public List<TowerSystem> towers = new List<TowerSystem>();
    public List<GameObject> CreatedTowers { get; private set; }
    public List<GameObject> ActiveTowers()
    {
        return CreatedTowers.FindAll(p => p.activeInHierarchy);
    }
    public TowerSystem currentTower { get; private set; }
    [HideInInspector]public UnityEvent<TowerSystem> currentTowerUpdated;

    public void updateCurrentTower(TowerSystem tower)
    {
        currentTower = tower;
        currentTowerUpdated?.Invoke(tower);
    }

    public List<AudioClip> TowerDestructionAudio = new List<AudioClip>();
    public List<AudioClip> TowerConstructionAudio = new List<AudioClip>();


    private void Start()
    {
        if (towers.Count > 0)
            updateCurrentTower(towers[0]);
        if (CreatedTowers == null) CreatedTowers = new List<GameObject>();
    }

    void Update()
    {
        foreach (TowerSystem tower in towers)
        {
            if (Input.GetKeyUp(tower.Keybind))
            {
                updateCurrentTower(tower);
            }
        }

        if (GameController.canBuild && !EventSystem.current.IsPointerOverGameObject())
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos = new Vector3(Mathf.Round(mouseWorldPos.x), Mathf.Round(mouseWorldPos.y), 0f);

            // check if tower already exists at current position.
            bool __towerExists = false;
            GameObject __tower = null;
            foreach (var tower in CreatedTowers)
            {
                if (tower.transform.position.Equals(mouseWorldPos) && tower.activeInHierarchy)
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
                __tower = DethronedUtility.FetchPooledGameObject(CreatedTowers, currentTower.Prefab);
                __tower.transform.position = mouseWorldPos;
                __tower.transform.rotation = Quaternion.identity;
                __tower.GetComponent<Tower>().targetPrio = (Tower.TargetingPriority)TowerBuilderUI.Instance.targetPrio.value;
                __tower.SetActive(true);
                GameController.currency -= costOfTower;

                if (TowerConstructionAudio.Count > 0)
                    AudioController.Instance.PlaySound(TowerConstructionAudio[Random.Range(0, TowerConstructionAudio.Count)]);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1) && __towerExists) // destroy the existing tower.
            {
                var refund = __tower.GetComponent<Tower>().CostOfTower;
                GameController.currency += refund;
                __tower.SetActive(false);

                if (TowerDestructionAudio.Count > 0)
                    AudioController.Instance.PlaySound(TowerDestructionAudio[Random.Range(0, TowerDestructionAudio.Count)]);
            }


        }

    }
}

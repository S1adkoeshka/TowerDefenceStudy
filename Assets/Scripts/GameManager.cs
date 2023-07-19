using Cells;
using Enemies;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Towers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Text _goldField;
    [SerializeField]
    private Text _waveField;
    [SerializeField]
    private Text _healthField;
    [SerializeField]
    private Text _messagesField;
    [SerializeField]
    private TMP_Text _scoreField;
    [SerializeField]
    private TMP_Text _enemiseKilledField;
    [SerializeField]
    private TMP_Text _goldEarnedField;
    [SerializeField]
    private TMP_Text _goldSpentField;
    [SerializeField]
    private TMP_Text _waveTimeField;
    [SerializeField]
    private TMP_Text _waveMobosStatsField;

    [SerializeField]
    private GameObject _waveStatsBar;
    [SerializeField]
    private GameObject _endGameMenu;


    private int _score;
    private int _enemiesKilled;
    private int _goldEarned = 0;
    private int _goldSpent = 0;
    private int _wave;
    private int _randomEnemyPrefab;
    private int _randomEnemyCount;


    private int _roundHealth;
    private int _roundGold;
    private int _roundEnemiesNumber;
    private int _roundEnemiesPrefabID;
    private int _roundScore;
    private int _roundEnemiesKilled;
    private int _roundGoldEarned;
    private int _roundGoldSpent;
    private int _roundHpLvlChange;
    private int _roundBountyLvlChange;
    private List<CellComponent> _roundCellsWithTower = new List<CellComponent>();

    private Coroutine NewWaveRoutine;

    private Coroutine _messageRoutine;

    private int NumberOfEnemies;

    [SerializeField]
    private GameObject[] _enemiesPrefabs;
    

    private List<GameObject> _enemies = new List<GameObject>();

    [SerializeField]
    private List<CellComponent> _cellsList;

    private ToweBuilder _towerToSet;

    [SerializeField]
    private SpawnerComponent[] Spawners;

    public static GameManager Instance;

    [SerializeField]
    private int _gold = 5;


    private int _health = 100;

    [SerializeField]
    private int _bountyLvlChange;
    [SerializeField]
    private int _hpLvlChange;
    [SerializeField]
    private float _timeChange;

    public bool _gameIsPaused;



    void Start()
    {
        _gameIsPaused = true;
        Time.timeScale = 0f;        
        Instance = this;       
        
        SetTextToStatField(_goldField, "Gold", _gold);
        SetTextToStatField(_healthField, "Health", _health);

        _randomEnemyPrefab = Random.Range(0, _enemiesPrefabs.Length);
        _randomEnemyCount = Random.Range(50, 100) / Spawners.Length;

        NewWaveRoutine = StartCoroutine(NewWave());
    }


    public List<GameObject> GetEnemies()
    {
        return _enemies;
    }

    public List<CellComponent> GetCellsList()
    {
        return _cellsList;
    }

    public void ReduceNumberOfEnemies()
    {
        NumberOfEnemies -= 1;
        if (NumberOfEnemies <= 0)
        {
            StartCoroutine(NewWave());
        }
    }


    public void AddUnitToList(GameObject Unit)
    {
        _enemies.Add(Unit);
    }


    private IEnumerator NewWave()
    {
        _wave += 1;

        _roundEnemiesKilled = _enemiesKilled;
        _roundEnemiesNumber = _randomEnemyCount;
        _roundHealth = _health;
        _roundGold = _gold;
        _roundEnemiesPrefabID = _randomEnemyPrefab;
        _roundScore = _score;
        _roundGoldEarned = _goldEarned;
        _roundGoldSpent = _goldSpent;
        _roundHpLvlChange = _hpLvlChange;
        _roundBountyLvlChange = _bountyLvlChange;

        _roundCellsWithTower.Clear();

        foreach (var cell in _cellsList)
        {
            if (cell.GetTower() != null)
            {
                _roundCellsWithTower.Add(cell);
            }
        }



        SetTextToStatField(_waveField, "Wave", _wave);

        _hpLvlChange += _wave * 3;
        if(_timeChange <= 90f)
        {
            _timeChange += _wave;
        }
   
        _bountyLvlChange += _wave;
        
        _enemies.Clear();

        var EnemyPrefab = _enemiesPrefabs[_randomEnemyPrefab];
        var EnemyPrefebComponent = EnemyPrefab.GetComponentInChildren<EnemyComponent>();

        _waveMobosStatsField.text = "Enemy: " + EnemyPrefebComponent.Name + "\n" + "Resist: " + EnemyPrefebComponent.GetResist().ToString() + "\n" + "Resist %: " + EnemyPrefebComponent.GetResistPersent().ToString();
        _waveTimeField.text = "Remaining time: " + (5f + _timeChange).ToString() + " sec";
        _waveStatsBar.SetActive(true);

        yield return new WaitForSeconds(5f + _timeChange);

        _waveMobosStatsField.text = "";
        _waveTimeField.text = "";
        _waveStatsBar.SetActive(false);


        foreach (SpawnerComponent Spawner in Spawners)
        {
            Spawner.FillEnemies(_randomEnemyCount, EnemyPrefab, _hpLvlChange);
        }

        NumberOfEnemies = _randomEnemyCount * Spawners.Length;

        _randomEnemyPrefab = Random.Range(0, _enemiesPrefabs.Length);
        _randomEnemyCount = Random.Range(50, 100) / Spawners.Length;

        yield return null;       
        
    }

    public int GetHPLvlChange()
    {
        return _hpLvlChange;
    }

    public void IncreaseGold(int Bounty)
    {
        _gold += Bounty + _bountyLvlChange * _wave;
        SetTextToStatField(_goldField, "Gold", _gold);

        _goldEarned += Bounty;
        _goldEarnedField.text = "Gold earned: " + _goldEarned.ToString();

    }

    public void ReduceGold(int Gold)
    {
        _gold -= Gold;
        SetTextToStatField(_goldField, "Gold", _gold);
        _goldSpent += Gold;
        _goldSpentField.text = "Gold spent: " + _goldSpent.ToString();        
    }

    public int GetGold()
    {
        return _gold;
    }

    public ToweBuilder GetTowerToSet()
    {
        return _towerToSet;
    }

    public void SetTowerToSet(ToweBuilder Tower)
    {
        _towerToSet = Tower;
    }

    private void SetTextToStatField(Text Field, string Stat, int StatCount)
    {

        Field.text = Stat + ": " + StatCount.ToString();

    }

    public void ReduceHealth()
    {
        _health -= 1;
        SetTextToStatField(_healthField, "Health", _health);

        if (_health <= 0)
        {
            ShowMessage("Game over");
            _endGameMenu.SetActive(true);
            _gameIsPaused = true;
            Time.timeScale = 0f;
        }
       
    }

    public void IncreaseHealth(int Health)
    {
        var HealthAfterHeal = _health + Health;

        if (HealthAfterHeal > 100)
        {
            _health = 100;
        }
        else
        {
            _health += Health;
        }

        SetTextToStatField(_healthField, "Health", _health);
    }

    public void ShowMessage(string Text)
    {
        if (_messageRoutine != null) StopCoroutine(_messageRoutine);
        _messageRoutine = StartCoroutine(SetTextToMessageField(Text));

    }

    private IEnumerator SetTextToMessageField(string Text)
    {
        _messagesField.text = "";
        _messagesField.text = Text;
        yield return new WaitForSeconds(4f);
        _messagesField.text = "";
    }

    public void SetScoreStats(int score) 
    {
        _enemiesKilled++;
        _enemiseKilledField.text = "Total kills: " + _enemiesKilled.ToString();
        _score += score;
        _scoreField.text = "Score: " + _score.ToString();
    }

    public Dictionary<string, int> GetSaveParams()
    {
        Dictionary<string, int> ReturnedParams = new Dictionary<string, int>();
        ReturnedParams.Add("Health", _roundHealth);
        ReturnedParams.Add("Gold", _roundGold);
        ReturnedParams.Add("EnemiesNumber", _roundEnemiesNumber);
        ReturnedParams.Add("EnemiesPrefabID", _roundEnemiesPrefabID);
        ReturnedParams.Add("Score", _roundScore);
        ReturnedParams.Add("EnemiesKilled", _roundEnemiesKilled);
        ReturnedParams.Add("GoldEarned", _roundGoldEarned);
        ReturnedParams.Add("GoldSpent", _roundGoldSpent);
        ReturnedParams.Add("HpLvlChange", _roundHpLvlChange);
        ReturnedParams.Add("BountyLvlChange", _roundBountyLvlChange);
        ReturnedParams.Add("Wave", _wave);

        return ReturnedParams;
    }

    public List<CellComponent> GetRoundCellsList()
    {
        return _roundCellsWithTower;
    }

    public void LoadGame(Dictionary<string, int> IntParams, List<string> Towers)
    {
        if(NewWaveRoutine != null)
        {
            StopCoroutine(NewWaveRoutine);            
        }

        foreach(var IntParam in IntParams)
        {
            switch (IntParam.Key)
            {
                case "Health":
                    _health = IntParam.Value;
                    _healthField.text = "Health: " + _health.ToString();
                    break;
                case "Gold":
                    _gold = IntParam.Value;
                    _goldField.text = "Gold: " + _gold.ToString();
                    break;
                case "EnemiesNumber":
                    _randomEnemyCount = IntParam.Value;
                    break;
                case "EnemiesPrefabID":
                    _randomEnemyPrefab = IntParam.Value;
                    break;
                case "Score":
                    _score = IntParam.Value;
                    _scoreField.text = "Score: " + _score.ToString();
                    break;
                case "EnemiesKilled":
                    _enemiesKilled = IntParam.Value;
                    _enemiseKilledField.text = "Total kills: " + _enemiesKilled.ToString();
                    break;
                case "GoldEarned":
                    _goldEarned = IntParam.Value;
                    _goldEarnedField.text = "Gold earned: " + _goldEarned.ToString();
                    break;
                case "GoldSpent":
                    _goldSpent = IntParam.Value;
                    _goldSpentField.text = "Gold spent: " + _goldSpent.ToString();
                    break;
                case "HpLvlChange":
                    _hpLvlChange = IntParam.Value;
                    break;
                case "BountyLvlChange":
                    _bountyLvlChange = IntParam.Value;
                    break;
                case "Wave":
                    _wave = IntParam.Value - 1;
                    _waveField.text = _wave.ToString();
                    break;
                default:
                    break;
            }
        }

        foreach(var Tower in Towers)
        {
            string[] TowerParts = Tower.Split(" = ");
            if(TowerParts.Length == 3)
            {
                var Cell = _cellsList.Find(i => i.name == TowerParts[2]);
                if (Cell != null)
                {
                    var NewTower = Instantiate(Resources.Load<GameObject>(TowerParts[1]));
                    Cell.SetTower(NewTower);
                    var NewTowerComponent = NewTower.GetComponentInChildren<TowerComponent>();
                    NewTowerComponent.SetCell(Cell);
                    NewTower.transform.localScale = new Vector3(4f, 4f, 4f);
                    NewTower.transform.position = Cell.transform.position;
                }
            }
        }

        NewWaveRoutine = StartCoroutine(NewWave());

        _gameIsPaused = false;

        Time.timeScale = 1f;

    }



}


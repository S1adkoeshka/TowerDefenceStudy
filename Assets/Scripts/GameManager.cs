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


    private int _score;
    private int _enemiesKilled;

    private Coroutine _messageRoutine;

    private int NumberOfEnemies;

    [SerializeField]
    private GameObject[] _enemiesPrefabs;

    private int _wave;

    private List<GameObject> _enemies = new List<GameObject>();

    [SerializeField]
    private List<CellComponent> _cellsList;

    private ToweBuilder _towerToSet;

    [SerializeField]
    private SpawnerComponent[] Spawners;

    public static GameManager Instance;

    [SerializeField]
    private int _gold = 5;
    private int _goldEarned = 0;
    private int _goldSpent = 5;

    private int _health = 100;

    [SerializeField]
    private int _bountyLvlChange;
    [SerializeField]
    private int _hpLvlChange;
    [SerializeField]
    private float _timeChange;



    void Start()
    {
        Instance = this;
        SetTextToStatField(_goldField, "Gold", _gold);
        SetTextToStatField(_healthField, "Health", _health);

        StartCoroutine(NewWave());
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
        SetTextToStatField(_waveField, "Wave", _wave);

        _hpLvlChange += _wave * 3;
        _timeChange += _wave;
        _bountyLvlChange += _wave;


        int RandomEnemyPrefab = Random.Range(0, _enemiesPrefabs.Length);
        int RandomEnemyCount = Random.Range(50, 100) / Spawners.Length;
        _enemies.Clear();

        var EnemyPrefab = _enemiesPrefabs[RandomEnemyPrefab];
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
            Spawner.FillEnemies(RandomEnemyCount, EnemyPrefab, _hpLvlChange);
        }

        NumberOfEnemies = RandomEnemyCount * Spawners.Length;

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


    
}


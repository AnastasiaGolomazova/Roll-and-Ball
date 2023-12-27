using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int _spawned;

    [Space(10)]
    [SerializeField] private Wave _wave;

    [Space(10)]
    [SerializeField] private Transform _pathSpawnPoint;

    [Space(10)]
    [SerializeField] private Transform _parent;

    [Space(10)]
    [SerializeField] private float _height = 0.5f;

    [SerializeField] private float _maxHeight = 2.0f;

    private Transform[] _spawnPoint;

    private int _currentSpawnPointNumber;

    private int _count;

    private void Start()
    {
        _count = PlayerPrefs.GetInt("Count");
        InitPath();
    }

    private void InitPath()
    {
        _spawnPoint = new Transform[_pathSpawnPoint.childCount];

        for (int i = 0; i < _pathSpawnPoint.childCount; i++)
        {
            _spawnPoint[i] = _pathSpawnPoint.GetChild(i);
        }
    }

    private void Update()
    {
        if (_spawned == _count)
            return;

        InstantiateEnemy();
        _spawned++;
    }

    private void InstantiateEnemy()
    {
        if (_currentSpawnPointNumber == _spawnPoint.Length - 1)
            _currentSpawnPointNumber = 0;
        else
            _currentSpawnPointNumber++;

        Instantiate(_wave.Tempalate[Random.Range(0, _wave.Tempalate.Length)],
            _spawnPoint[_currentSpawnPointNumber].position,
            _spawnPoint[_currentSpawnPointNumber].rotation, _parent);
    }

    public void SetCount()
    {
        _count += 2;
    }

    public void RaiseObject()
    {
        if (_parent.transform.position.y <= _maxHeight)
        {
            Vector3 newPosition = _parent.transform.position;
            newPosition.y += _height;
            _parent.transform.position = newPosition;
        }
    }
}

[System.Serializable]
public class Wave
{
    public GameObject[] Tempalate;
}
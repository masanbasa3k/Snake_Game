using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _foodPrefab;
    [SerializeField] private int _borderLeft = 0;
    [SerializeField] private int _borderRight = 0;
    [SerializeField] private int _borderTop = 0;
    [SerializeField] private int _borderBottom = 0;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        InvokeRepeating("Spawn", 3f, 3f);
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }

    private void Spawn()
    {
        if (_gameManager.CurrentState != GameState.GameInProgress)
        {
            CancelInvoke();
            return;
        }

        int x, y;

        while (true)
        {
            x = (int)Random.Range(_borderLeft, _borderRight);
            y = (int)Random.Range(_borderBottom, _borderTop);

            if(!_gameManager.FindScreenObjerct(x,y))
            {
                break;
            }
        }

        GameObject food = Instantiate(_foodPrefab, new Vector2(x, y), Quaternion.identity);

        _gameManager.AddScreenObject(food.transform);
    }
}

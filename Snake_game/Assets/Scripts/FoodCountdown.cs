using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodCountdown : MonoBehaviour
{
    [SerializeField] private TextMeshPro _countdownText;
    [SerializeField] private int _countdownTime = 9;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        if (_gameManager.CurrentState != GameState.GameInProgress)
        {
            return;
        }

        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (_countdownTime > 0)
        {
            _countdownText.text = _countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            _countdownTime--;

            if (_gameManager.CurrentState != GameState.GameInProgress)
            {
                break;
            }
        }

        _gameManager.RemoveScreenObject(transform);
        Destroy(gameObject);
    }
}

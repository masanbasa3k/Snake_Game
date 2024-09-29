using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class SnakeMovement : MonoBehaviour
{
    [SerializeField] private GameObject _snakeTailPrefab;
    private PlayerControls _controls;
    private Vector2 _currentDirection = Vector2.zero;
    private bool _isMoving;
    private float _moveSpeed = 0.5f;
    private float _moveTimer = 0f;
    private GameManager _gameManager;

    private List<GameObject> _snakeTail = new List<GameObject>();
    private bool _foodEaten;

    private LayerMask layerMask;
    private Vector2 direction;
    private Vector3 oldPos;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Wall","Food");
        _controls = new PlayerControls();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (_gameManager.CurrentState != GameState.GameInProgress)
        {
            return;
        }

        var position = Vector2.zero;

        

        

        transform.position += (Vector3)direction * _moveSpeed * Time.deltaTime;

        Vector3 rayCastDirection = (transform.position - oldPos).normalized;
        RaycastHit2D hit = Physics2D.Raycast(oldPos, rayCastDirection, Vector2.Distance(transform.position, oldPos), layerMask);
        if (hit)
        {
            if (hit.collider.isTrigger)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Food"))
                {
                    FoodEaten(hit.collider.transform.parent.parent.gameObject);
                }
                else
                {
                    _gameManager.SetGameState(GameState.GameOver);
                }      
            }
        }
        else
        {
            _moveTimer += Time.deltaTime;
            if (_moveTimer >= _moveSpeed)
            {
                _moveTimer = 0f;
                Vector3 previousPosition = transform.position;

                position += _currentDirection;

                if (_foodEaten)
                {
                    GameObject tail = Instantiate(_snakeTailPrefab, previousPosition, Quaternion.identity);
                    _snakeTail.Insert(0, tail);
                    _gameManager.AddScreenObject(tail.transform);
                    _foodEaten = false;
                }
                else if (_snakeTail.Count > 0)
                {
                    _snakeTail.Last().transform.position = previousPosition;
                    _snakeTail.Insert(0, _snakeTail.Last());
                    _snakeTail.RemoveAt(_snakeTail.Count - 1);
                }
            }
            transform.Translate(position);
        }
        oldPos = transform.position;
    }

    private void FoodEaten(GameObject collider)
    {
        _gameManager.Score++;
        _gameManager.RemoveScreenObject(collider.transform);
        Destroy(collider);
        _foodEaten = true;
    }

    private void OnEnable()
    {
        _controls.Player.Enable();
        _controls.Player.Movement.performed += Movement;
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
        _controls.Player.Movement.performed -= Movement;
    }

    private void Movement(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        if (direction == _currentDirection && _isMoving)
        {
            return;
        }

        _currentDirection = direction;
        _isMoving = true;
    }
}

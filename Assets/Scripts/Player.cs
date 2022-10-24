using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;
    private float _speedMultiplier = 2;
    private PlayerInput _playerInput;
    private Vector3 _moveInput;
    private InputAction _moveAction;
    private InputAction _fireAction;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.2f;
    private float _nextFire = 0;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;



    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Movement"];
        _fireAction = _playerInput.actions["Fire"];
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _fireAction.Enable();

        _fireAction.performed += Fire;
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _fireAction.Disable();

        _fireAction.performed -= Fire;
    }
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager not found!");
        }
    }

    private void Update()
    {
        CalculateMovement();
    }


    void Fire(InputAction.CallbackContext ctx)
    {
        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            var position = transform.position + new Vector3(0, 1, 0);
            Instantiate(_isTripleShotActive ? _tripleShotPrefab : _laserPrefab, position, Quaternion.identity);
        }
    }

    void CalculateMovement()
    {
        _moveInput = _moveAction.ReadValue<Vector3>();

        transform.Translate(_moveInput * Time.deltaTime * _speed * (_isSpeedBoostActive ? _speedMultiplier : 1));

        var yPosition = Mathf.Clamp(transform.position.y, -4, 0);
        var xPosition = transform.position.x;

        if (xPosition >= 11.5f)
        {
            xPosition = -11.5f;
        }
        else if (xPosition <= -11.5f)
        {
            xPosition = 11.5f;
        }
        transform.position = new Vector3(xPosition, yPosition, 0);
    }

    public void Damage()
    {
        _lives--;

        if (_lives == 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
        }
    }

    public void toggleTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(tripleShotTimer());
    }

    private IEnumerator tripleShotTimer()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

    public void toggleSpeedBoost()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(speedBoostTimer());
    }

    private IEnumerator speedBoostTimer()
    {
        yield return new WaitForSeconds(5);
        _isSpeedBoostActive = false;
    }
}

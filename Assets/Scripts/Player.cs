using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;
    private float _speedMultiplier = 2;
    private bool _isInvulnerable = false;
    private float _invulnerableTime = 2;

    private PlayerInput _playerInput;
    private Vector3 _moveInput;
    private InputAction _moveAction;
    private InputAction _fireAction;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private Animator _playerAnimator;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject[] _damagedEngines;

    [SerializeField]
    private float _fireRate = 0.2f;
    private float _nextFire = 0;
    private int _firstEngineDamaged;

    [SerializeField]
    private AudioClip _laserAudioClip;
    [SerializeField]
    private AudioClip _powerupAudioClip;
    private AudioSource _audioSource;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score = 0;
    private float _powerupDuration = 5;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private bool _resetTripleShot = false;
    private bool _resetSpeedBoost = false;



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

        Assert.IsNotNull(_spawnManager);

        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();

        Assert.IsNotNull(_uiManager);

        _audioSource = GetComponent<AudioSource>();

        Assert.IsNotNull(_audioSource);
    }

    private void Update()
    {
        CalculateMovement();
    }

    IEnumerator BecomeInvulnerable(float time = 0.5f)
    {
        _isInvulnerable = true;
        yield return new WaitForSeconds(time);
        _isInvulnerable = false;
    }

    IEnumerator InvulnerableFlash()
    {
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();

        Assert.IsNotNull(playerSprite);

        while (_isInvulnerable)
        {
            playerSprite.enabled = false;
            yield return new WaitForSeconds(0.2f);
            playerSprite.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }


    void Fire(InputAction.CallbackContext ctx)
    {
        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            var position = transform.position + new Vector3(0, 1, 0);
            Instantiate(_isTripleShotActive ? _tripleShotPrefab : _laserPrefab, position, Quaternion.identity);
            _audioSource.PlayOneShot(_laserAudioClip);
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
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            StartCoroutine(BecomeInvulnerable());
            return;
        }

        if (_isInvulnerable) return;

        _lives--;

        _uiManager.updateLives(_lives);

        if (_lives == 2)
        {
            _firstEngineDamaged = Random.Range(0, 2);
            _damagedEngines[_firstEngineDamaged].SetActive(true);
        }
        else if (_lives == 1)
        {
            if (_firstEngineDamaged == 0)
            {
                _damagedEngines[1].SetActive(true);
            }
            else
            {
                _damagedEngines[0].SetActive(true);
            }
        }
        else
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
        }

        StartCoroutine(BecomeInvulnerable(_invulnerableTime));
        StartCoroutine(InvulnerableFlash());
    }

    public void toggleTripleShot()
    {
        StartCoroutine(tripleShotTimer());
    }

    private IEnumerator tripleShotTimer()
    {
        if (!_isTripleShotActive)
        {
            _isTripleShotActive = true;
            float timeStamp = Time.time;

            while (Time.time < timeStamp + _powerupDuration)
            {
                if (_resetTripleShot)
                {
                    _resetTripleShot = false;
                    timeStamp = Time.time;
                }
                yield return null;
            }
            _isTripleShotActive = false;
        }
        else
        {
            _resetTripleShot = true;
        }
    }

    public void toggleSpeedBoost()
    {
        StartCoroutine(speedBoostTimer());
    }

    private IEnumerator speedBoostTimer()
    {
        if (!_isSpeedBoostActive)
        {
            _isSpeedBoostActive = true;
            float timeStamp = Time.time;

            while (Time.time < timeStamp + _powerupDuration)
            {
                if (_resetSpeedBoost)
                {
                    _resetSpeedBoost = false;
                    timeStamp = Time.time;
                }
                yield return null;
            }
            _isSpeedBoostActive = false;
        }
        else
        {
            _resetSpeedBoost = true;
        }

    }

    public void toggleShield()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void addToScore(int points)
    {
        _score += points;
        _uiManager.updateScore(_score);
    }

    public void playPowerupAudio()
    {
        _audioSource.PlayOneShot(_powerupAudioClip);
    }
}

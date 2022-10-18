using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;
    private PlayerInput _playerInput;
    private Vector3 _moveInput;
    private InputAction _moveAction;
    private InputAction _fireAction;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.2f;
    private float _nextFire = 0;
    [SerializeField]
    private int _lives = 3;


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
            var position = transform.position + new Vector3(0, 0.8f, 0);
            Instantiate(_laserPrefab, position, Quaternion.identity);
        }
    }

    void CalculateMovement()
    {
        _moveInput = _moveAction.ReadValue<Vector3>();

        transform.Translate(_moveInput * Time.deltaTime * _speed);

        var yPosition = Mathf.Clamp(transform.position.y, -4, 0);
        var xPosition = transform.position.x;

        if (xPosition >= 8)
        {
            xPosition = -8;
        }
        else if (xPosition <= -8)
        {
            xPosition = 8;
        }
        transform.position = new Vector3(xPosition, yPosition, 0);
    }

    public void Damage()
    {
        _lives--;

        if (_lives == 0)
        {
            Destroy(gameObject);
        }
    }
}

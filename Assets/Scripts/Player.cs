using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;
    private PlayerActions _playerActions;
    private Vector3 _moveInput;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.2f;
    private float _nextFire = 0;


    private void Awake()
    {
        _playerActions = new PlayerActions();
    }

    private void OnEnable()
    {
        _playerActions.Player_Map.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Player_Map.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _playerActions.Player_Map.Fire.performed += _ => Fire();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void Fire()
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
        _moveInput = _playerActions.Player_Map.Movement.ReadValue<Vector3>();

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
}

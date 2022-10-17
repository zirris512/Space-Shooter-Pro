using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;
    private PlayerActions _playerActions;
    private Vector3 _moveInput;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _playerActions = new PlayerActions();
        _playerActions.Player_Map.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        _moveInput = _playerActions.Player_Map.WASD.ReadValue<Vector3>();

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

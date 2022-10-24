using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;

    private Player _player;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player not found!");
        }

    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.8f)
        {
            transform.position = setRandomPosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                Player player = other.transform.GetComponent<Player>();

                if (player != null)
                {
                    player.Damage();
                }

                Destroy(gameObject);
                break;
            case "Laser":
                Destroy(other.gameObject);
                _player.addToScore(10);
                Destroy(gameObject);
                break;
        }
    }

    Vector3 setRandomPosition()
    {
        var randomX = Random.Range(-5.8f, 5.8f);
        return new Vector3(randomX, 8, 0);
    }
}

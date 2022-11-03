using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;
    private float _fireRate = 2;
    private float _canFire = 0;
    private bool _isDead = false;
    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;
    private Animator _animator;

    [SerializeField]
    private AudioClip _explosionAudioClip;
    private AudioSource _audioSource;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player not found!");
        }

        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator not found!");
        }

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Enemy AudioSource not found!");
        }
        else
        {
            _audioSource.clip = _explosionAudioClip;
        }
    }

    void Update()
    {
        CalculateMovement();
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3, 6);
            _canFire = Time.time + _fireRate;
            if (!_isDead)
            {
                EnemyFire();
            }
        }
    }

    void EnemyFire()
    {
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        foreach (Laser laser in lasers)
        {
            laser.AssignEnemyLaser();
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -9f)
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

                OnDeath();
                break;
            case "Laser":
                Laser laser = other.transform.GetComponent<Laser>();
                if (laser.IsEnemyLaser())
                {
                    return;
                }
                Destroy(other.gameObject);
                _player.addToScore(10);
                OnDeath();
                break;
        }
    }

    void OnDeath()
    {
        _isDead = true;
        Destroy(GetComponent<BoxCollider2D>());
        _speed = 2.5f;
        _animator.SetTrigger("OnEnemyDeath");
        _audioSource.Play();
        Destroy(gameObject, 2.5f);
    }

    Vector3 setRandomPosition()
    {
        var randomX = Random.Range(-5.8f, 5.8f);
        return new Vector3(randomX, 8, 0);
    }
}

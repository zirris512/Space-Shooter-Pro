using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;

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
                Destroy(other.gameObject);
                _player.addToScore(10);
                OnDeath();
                break;
        }
    }

    void OnDeath()
    {
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

using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;

    void Start()
    {
        transform.position = setRandomPosition();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.8f)
        {
            transform.position = setRandomPosition();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                Player player = other.transform.GetComponent<Player>();
                if (player != null)
                {
                    player.Damage();
                }
                break;
            case "Laser":
                Destroy(other.gameObject);
                break;
        }
        Destroy(gameObject);
    }

    Vector3 setRandomPosition()
    {
        var randomX = Random.Range(-5.8f, 5.8f);
        return new Vector3(randomX, 8, 0);
    }
}

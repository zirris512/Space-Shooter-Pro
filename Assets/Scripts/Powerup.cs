using UnityEngine;

public class Powerup : MonoBehaviour
{
    private enum TypeEnum
    {
        TripleShot,
        Speed,
        Shield
    }
    [SerializeField]
    private float _speed = 3;
    [SerializeField]
    private TypeEnum _type;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                switch (_type)
                {
                    case TypeEnum.TripleShot:
                        player.toggleTripleShot();
                        break;
                    case TypeEnum.Speed:
                        player.toggleSpeedBoost();
                        break;
                    case TypeEnum.Shield:
                        Debug.Log("Collected shield boost");
                        break;

                }
                Destroy(gameObject);
            }
        }
    }
}

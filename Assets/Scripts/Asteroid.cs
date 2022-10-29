using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 18;
    [SerializeField]
    private GameObject _explosionPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            GameObject newExplosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(newExplosion, 2.5f);
            Destroy(other.gameObject);
            Destroy(gameObject, 0.2f);
        }
    }
}

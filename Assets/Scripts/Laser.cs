using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8;

    // Update is called once per frame
    void Update()
    {
        //translate laser up
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y >= 7)
        {
            Destroy(gameObject);
        }
    }
}

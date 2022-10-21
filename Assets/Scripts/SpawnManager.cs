using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (!_stopSpawning)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, setRandomPosition(), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    Vector3 setRandomPosition()
    {
        var randomX = Random.Range(-9, 9);
        return new Vector3(randomX, 8, 0);
    }

}

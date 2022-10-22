using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private Enemy _enemyPrefab;
    [SerializeField]
    private Powerup _powerupPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;

    private void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (!_stopSpawning)
        {
            Enemy newEnemy = Instantiate(_enemyPrefab, setRandomPosition(), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (!_stopSpawning)
        {
            int spawnTime = Random.Range(3, 8);
            Instantiate(_powerupPrefab, setRandomPosition(), Quaternion.identity);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    Vector3 setRandomPosition()
    {
        var randomX = Random.Range(-9, 10);
        return new Vector3(randomX, 8, 0);
    }

}

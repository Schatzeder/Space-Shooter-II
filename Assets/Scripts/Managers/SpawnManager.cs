using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy = null;
    [SerializeField]
    private GameObject _enemyContainer = null;
    [SerializeField]
    private GameObject[] _powerUp = null;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartRoutines()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        Debug.Log("Enemy Spawn Routine STARTED");
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            //float randomX = Random.Range(-8f, 8f);
            Vector3 spawnPos = new Vector3(Random.Range(-8f, 8f), 7, 0);

            GameObject newEnemy = Instantiate(_enemy, spawnPos, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;
            float spawnDelay = Random.Range(1.5f, 2.5f);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        while (_stopSpawning == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-8f, 8f), 7, 0);

            GameObject powerUp = Instantiate(_powerUp[Random.Range(0, 3)], spawnPos, Quaternion.identity);

            float spawnDelay = Random.Range(3f, 7f);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

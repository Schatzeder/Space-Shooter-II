using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemy = null;
    [SerializeField]
    private GameObject _enemyContainer = null;
    [SerializeField]
    private GameObject[] _powerUp = null;

    private Coroutine _spawnEnemyRoutine;
    private Coroutine _spawnPowerupRoutine;

    private bool _stopSpawning = false;

    private Player _player = null;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void StartRoutines()
    {
        _spawnEnemyRoutine = StartCoroutine(SpawnEnemyRoutine());
        _spawnPowerupRoutine = StartCoroutine(SpawnPowerupRoutine());
    }


    private void RandomSetUp()
    {

    }

    private void Randomizer()
    {
        float rand = Random.value;

        //Norm = 100%

        //Norm = 75% chance
        //Orbit = 25% chance

        //Norm = 60% chance
        //Orbit = 20% chance
        //Ram = 20% chance

        float[] _spawnProb = new float[5];
        //GameObject[] _enemyArray = new GameObject[5];

        _spawnProb[0] = 0.70f;
        _spawnProb[1] = 0.20f;
        _spawnProb[2] = 0.10f;

        Debug.Log("Random value = " + rand);
        if (rand <= _spawnProb[0])
        {
            Debug.Log("Condition 0: " + _spawnProb[0]);
        }
        else if (rand > _spawnProb[0] && rand <= (_spawnProb[0] + _spawnProb[1]))
        {
            Debug.Log("Condition 1: " + _spawnProb[1]);
        }
        else if (rand > (_spawnProb[0] + _spawnProb[1]))
        {
            Debug.Log("Condition 2: " + _spawnProb[2]);
        }
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        Debug.Log("Enemy Spawn Routine STARTED");
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            //float randomX = Random.Range(-8f, 8f);
            Vector3 spawnPos = new Vector3(Random.Range(-8f, 8f), 7, 0);

            GameObject newEnemy = Instantiate(_enemy[Random.Range(0, 3)], spawnPos, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;
            float spawnDelay = Random.Range(1.5f, 2.5f);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void HellStart()
    {
        //StartCoroutine(HellRoutine());
    }

    /*public IEnumerator HellRoutine()
    {
        float randomX = Random.Range(-8f, 8f);
        float randomY = Random.Range(-6f, 9f);
        _spawnPos[0] = new Vector3(randomX, 7, 0);
        _spawnPos[1] = new Vector3(randomX, -6, 0);
        _spawnPos[2] = new Vector3(11.5f, randomY, 0);
        _spawnPos[3] = new Vector3(-11.5f, randomY, 0);

        Debug.Log("Hell Routine started.");
        while (_stopSpawning == false)
        {
            //CLAMP ENEMY POSITION ON ZED
            //CLAMP ENEMY ROTATION TO ONE AXIS
            GameObject newEnemy = Instantiate(_enemy, _spawnPos[Random.Range(0,4)], Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            //newEnemy.transform.LookAt(_player.playerPos);
            yield return new WaitForSeconds(0.3f);
        }
        yield return null;
    }*/

    private IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        while (_stopSpawning == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-8f, 8f), 7, 0);

            GameObject powerUp = Instantiate(_powerUp[Random.Range(0, 5)], spawnPos, Quaternion.identity);

            float spawnDelay = Random.Range(3f, 7f);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _rotationSpeed = 20f;

    [SerializeField]
    private GameObject _explosion = null;
    private CircleCollider2D _asteroidCollider = null;

    private SpawnManager _spawnManager = null;

    public bool _asteroidBool;

    public void AsteroidMethod()
    {

    }



    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _asteroidCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            _asteroidCollider.enabled = !_asteroidCollider.enabled;
            _spawnManager.StartRoutines();
            GameObject newExplosion = Instantiate(_explosion, new Vector3 (0, 3.88f, 0), Quaternion.identity);
            Destroy(collision.gameObject, 0.12f);
            Destroy(newExplosion, 2.5f);
            Destroy(this.gameObject, 0.5f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;
    
    //Deciding whether or not to shoot a laser on the first go
    [SerializeField]
    private int _laserID = 0;

    private bool _enemyDead = false;
    private bool _firing = false;

    [SerializeField]
    private GameObject _enemyLaser = null;

    [SerializeField]
    private Color _enemyGun;
    [SerializeField]
    private float _gunDelta = 0;

    private UIManager _uiManager = null;

    private BoxCollider2D _enemyCollider = null;
    private Animator _enemyAnimator = null;
    private AudioSource _explosion = null;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        _enemyGun = this.transform.GetChild(0).GetComponent<SpriteRenderer>().color;

        _enemyCollider = GetComponent<BoxCollider2D>();
        _enemyAnimator = GetComponent<Animator>();
        _explosion = GetComponent<AudioSource>();

        _laserID = (Random.Range(0, 3));
        StartCoroutine(LaserRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y <= -4.5f && _enemyDead == false)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            
            if (player != null)
            {
                player.Damage();
            }

            EnemyDeath();
        }
        else if (other.tag == "Laser")
        {
            _uiManager.UpdateScore(10);
            Destroy(other.gameObject);

            EnemyDeath();
        }
        else if (other.tag == "Asteroid")
        {
            //Logic for rogue asteroid collisions
            EnemyDeath();
        }
    }

    private IEnumerator LaserRoutine()
    {
        if (_laserID == 2)
        {
            _firing = true;
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            StartCoroutine(EnemyFireLaser());
            yield return new WaitForSeconds(1.5f);
            _firing = false;
        }

        while (true)
        {
            if (_firing == false)
            {
                yield return new WaitForSeconds(Random.Range(1.5f, 5.0f));
                StartCoroutine(EnemyFireLaser());
            }
        }
    }

    private IEnumerator EnemyFireLaser()
    {
        //Get Handle on Thruster Sprite
            for (int i = 0; i < 12; i++)
            {
                _gunDelta += 0.025f;
                //NONOPTIMAL -- Using GetComponent repeatedly
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, _gunDelta);
                yield return new WaitForSeconds(0.1f);

            if (_gunDelta >= 0.3f && _enemyDead == false)
                {
                    GameObject eLaser = Instantiate(_enemyLaser, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity, this.transform);
                    eLaser.GetComponent<Laser>()._laserID = 2;
                    _gunDelta = 0;
                    this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = _enemyGun;
                }
            }
    }

    private void EnemyDeath()
    {
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = _enemyGun;
        _enemyDead = true;
        _explosion.Play();
        _enemySpeed *= 0.6f;
        _enemyCollider.enabled = !_enemyCollider.enabled;
        _enemyAnimator.SetBool("OnEnemyDeath", true);
        Destroy(this.gameObject, 2.45f);
    }
}

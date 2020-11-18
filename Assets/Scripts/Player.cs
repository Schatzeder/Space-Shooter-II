using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class Player : MonoBehaviour
            //Player class INHERITS MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    [SerializeField]
    private float _fireRate = 0.2f;
    private float _canFire = -1f;

    [SerializeField]
    private int _playerHealth = 3;

    [SerializeField]
    private bool _tripleShotActive = false;
    private bool _shieldActive = false;
    private bool _playerAlive = true;

    [SerializeField]
    private GameObject _laserPrefab = null;
    [SerializeField]
    private GameObject _tripleShotPrefab = null;
    [SerializeField]
    private GameObject _laserContainer = null;
    [SerializeField]
    private GameObject _playerShield = null;
    [SerializeField]
    private GameObject _thruster = null;
    [SerializeField]
    private GameObject[] _playerHurtVisual = null;
    [SerializeField]
    private GameObject _onDeathExplosion = null;

    private SpriteRenderer _playerSprite = null;
    private AudioSource _playerExplodeAudio = null;

    /*[SerializeField]
    private PostProcessVolume _postProcessVolume = null;*/

    private SpawnManager _spawnManager;
    private UIManager _uiManager;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        _playerSprite = GetComponent<SpriteRenderer>();
        _playerExplodeAudio = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        ThrusterSwitch();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    //Take player inputs and retain player position within the screen
    {
        if (_playerAlive == true)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
            transform.Translate(direction * _speed * Time.deltaTime);

            //Y AXIS RESTRAINTS (-3.8f < y < 0f)
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

            //X AXIS WRAPAROUND (10.5f > x > -10.5f)
            if (transform.position.x >= 10.5f)
            {
                transform.position = new Vector3(-10.5f, transform.position.y, 0);
            }
            else if (transform.position.x <= -10.5f)
            {
                transform.position = new Vector3(10.5f, transform.position.y, 0);
            }
        }
    }

    void ThrusterSwitch()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _thruster.SetActive(true);
        }

        else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            _thruster.SetActive(false);
        }
    }

    void FireLaser()
    {
        //Set laser cooldown
        _canFire = Time.time + _fireRate;

        if (_tripleShotActive == true)
        {
            GameObject newLaser = Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            newLaser.transform.parent = _laserContainer.transform;
        }
        else
        {
            GameObject newLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.0f, 0), Quaternion.identity);
            newLaser.transform.parent = _laserContainer.transform;
            newLaser.GetComponent<Laser>()._laserID = 1;
        }
    }

    public void Damage()
    {
        if (_shieldActive == true)
        {
            _shieldActive = false;
            _playerShield.SetActive(false);
            return;
        }
        else if (_shieldActive == false)
        {
            _playerHealth--;
            _uiManager.UpdateLifeSprite(_playerHealth);
            _playerHurtVisual[Random.Range(0, 2)].SetActive(true);

            if (_playerHealth == 1)
            {
                _playerHurtVisual[0].SetActive(true);
                _playerHurtVisual[1].SetActive(true);
            }
            if (_playerHealth < 1)
            {
                StartCoroutine(PlayerDeath());
            }
        }
    }

    private IEnumerator PlayerDeath()
    {
        _spawnManager.OnPlayerDeath();
        _playerAlive = false;
        _onDeathExplosion.SetActive(true);
        yield return new WaitForSeconds(0.65f);
        _uiManager.GameOverScreen();
        _playerExplodeAudio.Play();
        _playerSprite.enabled = !_playerSprite.enabled;
        _playerHurtVisual[0].SetActive(false);
        _playerHurtVisual[1].SetActive(false);
        
        Destroy(this.gameObject, 2f);
    }

    public void TripleShotActivate()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _tripleShotActive = false;
    }

    public void SpeedActivate()
    {
        StartCoroutine(SpeedPowerDownRoutine());
    }

    private IEnumerator SpeedPowerDownRoutine()
    {
        _speed *= 2f;
        _fireRate *= 0.5f;
        yield return new WaitForSeconds(5f);
        _speed *= 0.5f;
        _fireRate *= 2f;
    }

    public void ShieldActivate()
    {
        _shieldActive = true;
        _playerShield.SetActive(true);
    }
}

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
    private float _warpCharge = 0f;
    [SerializeField]
    private float _warpChargeFill = 0f;

    [SerializeField]
    private int _playerHealth = 3;
    private int _shieldValue = 0;
    [SerializeField]
    private int _ammoValue = 15;
    int _quickInt = 2;

    [SerializeField]
    private bool _tripleShotActive = false;
    private bool _shieldActive = false;
    private bool _playerAlive = true;
    private bool _warpReady = false;
    [SerializeField]
    private bool _damageImmune = false;
    public bool _hellMode = false;

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
    [SerializeField]
    private GameObject _warpChargeMeter = null;
    private Text _warpChargeText;
    [SerializeField]
    private GameObject _warpShieldVisual = null;
    [SerializeField]
    private GameObject _warpDemo = null;

    private Image _warpChargeImage = null;
    private Color _warpChargingColor = new Color(0.2031f, 0.2893f, 0.7830f, 1f);
    private Color _warpReadyColor = new Color(0, 1, 0.2765f, 1);

    private SpriteRenderer _playerSprite = null;
    private AudioSource _playerAudioSource = null;
    [SerializeField]
    private AudioClip _explosionClip = null;
    [SerializeField]
    private AudioClip _warpUpClip = null;

    [SerializeField]
    public Vector3 playerPos;

    /*[SerializeField]
    private PostProcessVolume _postProcessVolume = null;*/

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private CameraController _camera;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _camera = GameObject.Find("Main Camera").GetComponent<CameraController>();

        _playerSprite = GetComponent<SpriteRenderer>();
        _playerAudioSource = GetComponent<AudioSource>();

        _warpChargeImage = _warpChargeMeter.GetComponent<Image>();
        _warpChargeImage.color = _warpChargingColor;
        _warpChargeText = _warpChargeMeter.transform.GetChild(0).GetComponent<Text>();

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
        ChargeWarp();
        WarpDemo();
        //HellDemo();
        playerPos = this.transform.position;

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            //Set laser cooldown
            _canFire = Time.time + _fireRate;
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

            if (_warpReady && Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(WarpMethod(horizontalInput));
            }
        }
    }

    void WarpDemo()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _warpDemo.SetActive(true);
        }
    }

    void HellDemo()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _hellMode = true;
            _spawnManager.HellStart();
        }
    }

    private IEnumerator WarpMethod(float xDirection)
    {
        if (xDirection != 0)
        {
            WarpDown();
            _damageImmune = true;
            _warpShieldVisual.SetActive(true);

            for (int i = 0; i < 8; i++)
            {
                Vector3 direction = new Vector3(xDirection, 0, 0);
                transform.Translate(direction * 0.5f);
                yield return new WaitForSeconds(0.02f);
                FireLaser();

                if (i == 7)
                {
                    _warpShieldVisual.SetActive(false);
                    yield return new WaitForSeconds(0.25f);
                    _damageImmune = false;
                }
            }
        }
    }

    void WarpUp()
    {
        _warpReady = true;
        //_playerAudioSource.clip = _warpUpClip;
        _playerAudioSource.PlayOneShot(_warpUpClip);
        _warpChargeImage.color = _warpReadyColor;
        _warpChargeText.text = ("WARP DRIVE READY");
        //_warpChargeText.fontStyle = FontStyle.Bold;
        _warpChargeText.color = Color.white;
    }

    void WarpDown()
    {
        _warpReady = false;
        _warpChargeImage.color = _warpChargingColor;
        _warpChargeText.color = Color.grey;
        _warpChargeText.text = ("WARP DRIVE");
        _warpCharge = 0f;
    }

    void ThrusterSwitch()
    {
        if (_playerAlive == true)
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
    }

    void ChargeWarp()
    {
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && _warpReady == false && _warpCharge <= 2f)
        {
            _warpCharge += 0.01f;
            _warpChargeFill = (_warpCharge / 2);
            _warpChargeImage.fillAmount = _warpChargeFill;

            //Bar fills via Image/Fill Amount
            //Bar changes color to Green on ready
            //Powerup noise on full
            //Text changes to bold white and says READY when full

            //Text changes to regular grey and deletes READY on cast
            //Warp noise on cast
            //Bar depletes and changes back to blue on cast
        }

        if (_warpChargeFill >= 1f && _warpReady == false)
        {
            WarpUp();
        }
    }

    void FireLaser()
    {
        if (_ammoValue > 0)
        {
            if (_tripleShotActive == true)
            {
                GameObject newLaser = Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                newLaser.transform.parent = _laserContainer.transform;
                //subtract 3 lasers
                AmmoChange(-3);
            }
            else
            {
                GameObject newLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.0f, 0), Quaternion.identity);
                newLaser.transform.parent = _laserContainer.transform;
                newLaser.GetComponent<Laser>()._laserID = 1;
                //subtract 1 laser
                AmmoChange(-1);
            }
        }
    }

    public void AmmoChange(int change)
    {
        //Need recharge ammo option
        _ammoValue += change;
        _ammoValue = Mathf.Clamp(_ammoValue, 0, 15);
        /*if (_ammoValue > 15)
        {
            _ammoValue = 15;
        }
        else if (_ammoValue < 0)
        {
            _ammoValue = 0;
        }*/
        _uiManager.UpdateAmmoVisual(_ammoValue);
    }

    public void Damage()
    {
        if (_shieldActive == true && _damageImmune == false)
        {
            _shieldValue--;
            _uiManager.UpdateShieldVisual(_shieldValue);
            if (_shieldValue == 0)
            {
                _shieldActive = false;
                _playerShield.SetActive(false);
            }
        }
        else if (_shieldActive == false && _damageImmune == false)
        {
            UpdatePlayerHealth(-1);
            _camera.CameraShake();
            //_camera.CameraShake(_quickInt);
            //_quickInt--;
            /*
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
            */
        }
    }

    private IEnumerator PlayerDeath()
    {
        _spawnManager.OnPlayerDeath();
        _playerAlive = false;
        _onDeathExplosion.SetActive(true);
        yield return new WaitForSeconds(0.65f);
        _uiManager.GameOverScreen();
        _playerAudioSource.clip = _explosionClip;
        _playerAudioSource.Play();
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
        _shieldValue = 3;
        _shieldActive = true;
        _uiManager.UpdateShieldVisual(_shieldValue);
        _playerShield.SetActive(true);
    }

    public void UpdatePlayerHealth(int change)
    {
        _playerHealth = (_playerHealth + change);

        if (_playerHealth >= 3)
        {
            _playerHealth = 3;
            _playerHurtVisual[0].SetActive(false);
            _playerHurtVisual[1].SetActive(false);
        }
        if (_playerHealth == 2)
        {
            _playerHurtVisual[Random.Range(0, 2)].SetActive(true);
        }
        else if (_playerHealth == 1)
        {
            //Fix visuals to deactivate if health goes up
            _playerHurtVisual[0].SetActive(true);
            _playerHurtVisual[1].SetActive(true);
        }
        else if (_playerHealth < 1)
        {
            StartCoroutine(PlayerDeath());
        }
        _uiManager.UpdateLifeSprite(_playerHealth);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private bool _lowAmmo = false;

    private int _score = 0;
    [SerializeField]
    private Text _scoreValue = null;
    [SerializeField]
    private Text _gameOverText = null;
    [SerializeField]
    private Text _restartText = null;
    [SerializeField]
    private Text _emptyAmmoText = null;

    private bool _gameOver = false;

    [SerializeField]
    private Sprite[] _lifeSprites = null;
    [SerializeField]
    private GameObject[] _shieldVisual = null;
    [SerializeField]
    private GameObject[] _ammoVisual = null;

    [SerializeField]
    private Image _lifeDisplay = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameOver == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(1);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void UpdateScore(int points)
    {
        _score += points;
        _scoreValue.text = _score.ToString();
    }

    public void UpdateLifeSprite(int lives)
    {   //Changes life image display
        if (lives >= 0)
        {
            _lifeDisplay.sprite = _lifeSprites[lives];
        }
    }

    public void UpdateShieldVisual(int value)
    {
        if (value == 3)
        {
            for (int i = value; i > 0; i--)
            {
                _shieldVisual[i-1].SetActive(true);
            }
            /*_shieldVisual[0].SetActive(true);
            _shieldVisual[1].SetActive(true);
            _shieldVisual[2].SetActive(true);*/
        }
        if (value <= 2)
        {
            _shieldVisual[value].SetActive(false);
        }
    }

    public void UpdateAmmoVisual(int ammo)
    {
        for (int i = ammo; i < 15; i++)
        {
            _ammoVisual[i].SetActive(false);
        }
        if (ammo == 15)
        {
            _lowAmmo = false;
            for (int i = ammo; i > 0; i--)
            {
                _ammoVisual[i-1].SetActive(true);
            }
        }
        if (ammo == 0)
        {
            _lowAmmo = true;
            StartCoroutine(AmmoFlickerText());
        }
        else
        {
            _lowAmmo = false;
        }
    }

    public void GameOverScreen()
    {
        _gameOver = true;
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
    }

    private IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.7f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.7f);
        }
    }

    private IEnumerator AmmoFlickerText()
    {
        while (_lowAmmo == true)
        {
            _emptyAmmoText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.7f);
            _emptyAmmoText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.7f);
        }
    }
}
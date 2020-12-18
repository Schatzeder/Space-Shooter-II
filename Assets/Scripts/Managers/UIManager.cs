using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
    [SerializeField] //DEBUG
    private GameObject[] _ammoVisual = null;
    [SerializeField] //DEBUG
    private Image[] _ammoVisualImage = null;
    [SerializeField]
    private GameObject _ammoContainer = null;

    [SerializeField]
    private Image _lifeDisplay = null;

    // Start is called before the first frame update
    void Start()
    {
        AssignAmmo();
    }

    // Update is called once per frame
    void Update()
    {
        MenuInputs();
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
        }
        if (value <= 2)
        {
            _shieldVisual[value].SetActive(false);
        }
    }

    private void AssignAmmo()
    {
        _ammoContainer = GameObject.Find("Ammo_Container");
        //Assigns the length of an Array    IMPORTANT: REQUIRES "using System;"
        Array.Resize(ref _ammoVisual, _ammoContainer.transform.childCount);
        Array.Resize(ref _ammoVisualImage, _ammoContainer.transform.childCount);
        //_ammoVisual = new GameObject[_ammoContainer.transform.childCount];

        if (_ammoContainer == null)
        {
            Debug.Log("_ammoContainer is null");
        }

        for (int i = _ammoContainer.transform.childCount - 1; i >= 0; i--)
        {
            //Transform child = _ammoContainer.transform.GetChild(i);
            Image child = _ammoContainer.transform.GetChild(i).GetComponent<Image>();
            //_ammoVisual[i] = child.gameObject;
            _ammoVisualImage[i] = child;
        }
    }

    public void UpdateAmmoVisual(int ammo)
    {
        //DIVIDE AMMO BY 15
        //WHOLE NUMBER MEANS LASER OBJECT
        //DECIMAL MEANS FILL VALUE

        if (ammo == 150)
        {
            _ammoVisualImage[14].fillAmount = 1.0f;
        }
        else
        {
            int ammoInt = (ammo) / 10; //ammoInt tells me that every 10 shots, I need to change which AmmoVisual I'm adjusting        
            float ammoFloat = (ammo - (ammoInt * 10f)) / 10f; //ammoFloat tells me the Fill Amount I should be changing each AmmoVisual.Image to

            _ammoVisualImage[ammoInt].fillAmount = ammoFloat;

            if (ammoFloat == 0f && ammo != 0)
            {   //Ensures that, on denominations of 10 Ammo, both relevant _ammoVisualImages are adjusted
                _ammoVisualImage[ammoInt - 1].fillAmount = 1f;
            }

            //Debug.Log("Ammo = " + ammo);
            //Debug.Log("AmmoInt = " + ammoInt);
            //Debug.Log("AmmoFloat = " + ammoFloat);
        }
    }

    /*
for (int i = ammo; i < 15; i++)
{
    _ammoVisual[i].SetActive(false);
}
if (ammo == 15)
{
    _lowAmmo = false;
    for (int i = ammo; i > 0; i--)
    {
        _ammoVisual[i - 1].SetActive(true);
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
*/

    private void MenuInputs()
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
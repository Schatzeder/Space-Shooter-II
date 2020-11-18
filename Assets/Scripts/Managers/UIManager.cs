using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private int _score = 0;
    [SerializeField]
    private Text _scoreValue = null;
    [SerializeField]
    private Text _gameOverText = null;
    [SerializeField]
    private Text _restartText = null;

    private bool _gameOver = false;

    [SerializeField]
    private Sprite[] _lifeSprites = null;

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
        _lifeDisplay.sprite = _lifeSprites[lives];
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
}
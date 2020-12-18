using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TIV_Player : MonoBehaviour
{
    [SerializeField]
    private int _lives = 0;

    [SerializeField] //Debugging only
    private float _thrusterValue = 0;

    private TIV_UIManager _tivUIManager = null;

    // Start is called before the first frame update
    void Start()
    {
        _lives = 3;
        _thrusterValue = 300;

        _tivUIManager = GameObject.Find("Canvas").GetComponent<TIV_UIManager>();

        if (_tivUIManager == null)
        {
            Debug.LogError("UIManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement(); //Basic player movement + boundaries
        PlayerInput(); //Q subtracts a life, E adds a life

        ThrusterVisual();
    }

    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UpdateLives(-1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateLives(1);
        }
    }

    void UpdateLives(int livesDelta)
    {
        _lives += livesDelta;
        _lives = Mathf.Clamp(_lives, 0, 3);

        _tivUIManager.UpdateLivesVisual(_lives);

        //_lifeImage.sprite = _lifeSprites[_lives];
    }

    void ThrusterVisual()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _thrusterValue--;
        }
        else
        {
            _thrusterValue++;
        }

        _thrusterValue = Mathf.Clamp(_thrusterValue, 0, 300);

        float _thrusterFill = _thrusterValue / 300; //Calculating a percentage to pass as fill amount
        _tivUIManager.UpdateThrusterVisual(_thrusterFill);

        //_thrusterVisual.fillAmount = _thrusterFill;
    }

    /*
        void AdjustLives(int livesDelta)
        {
            _lives += livesDelta;
            _lifeImage.sprite = _lifeSprites[_lives];
        }


        void ThrusterVisual()
        {
            //Thruster is always changing no matter what
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _thrusterValue--;
            }
            else
            {
                _thrusterValue++;
            }
            _thrusterValue = Mathf.Clamp(_thrusterValue, 0, 300);

            float _thrusterPercent = _thrusterValue / 300;

            _thrusterVisual.fillAmount = _thrusterPercent;
        }
    */


    void CalculateMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical);

        transform.Translate(direction * 8 * Time.deltaTime);
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8f, 8f), Mathf.Clamp(transform.position.y, -3, 5));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerUpSpeed = 3.0f;

    [SerializeField]
    //0-Triple Shot --- 1-Speed --- 2-Shield
    private int _powerupID = 0;

    private AudioSource _powerupAudio = null;
    private SpriteRenderer _powerupSprite = null;
    private CircleCollider2D _powerupCollider = null;

    private void Start()
    {
        _powerupCollider = GetComponent<CircleCollider2D>();
        _powerupSprite = GetComponent<SpriteRenderer>();
        _powerupAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);

        if (transform.position.y <= -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActivate();
                        break;
                    case 1:
                        player.SpeedActivate();
                        break;
                    case 2:
                        player.ShieldActivate();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }

                /*
                if (_powerupID == 0)
                {
                    player.TripleShotActivate();
                }
                else if (_powerupID == 1)
                {
                    player.SpeedActivate();
                }
                else if (_powerupID == 2)
                {
                    player.ShieldActivate();
                }
                */
            }

            StartCoroutine(PowerUpDown());
        }
    }

    private IEnumerator PowerUpDown()
    {
        _powerupAudio.Play();
        _powerupCollider.enabled = !_powerupCollider.enabled;
        _powerupSprite.enabled = !_powerupSprite.enabled;
        Destroy(this.gameObject, 1f);
        yield return null;
    }
}

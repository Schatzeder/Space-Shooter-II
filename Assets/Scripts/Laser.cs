using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8.0f;
    public int _laserID = 0; //1 is friendly, 2 is enemy

    // Update is called once per frame
    void Update()
    {
        if (_laserID == 1)
        {
            transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        }
        if (_laserID == 2)
        {
            transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);
        }



        if (transform.position.y > 8.0f || transform.position.y < -5.0f)
        {
            Destroy(this.gameObject, 0.25f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_laserID == 2 && collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
    }

    //BUG: Sometimes lasers are destroying themselves after a short time before reaching the end of the map.
}

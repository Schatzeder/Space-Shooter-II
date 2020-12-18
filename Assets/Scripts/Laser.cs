using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 1.0f;
    public int _laserID = 0; //1 is friendly, 2 is enemy

    [SerializeField]
    private Vector3 _laserDirection;
    private Vector3 _orbitCenter;

    private void Start()
    {
        _orbitCenter = new Vector3(0, -9, 0);

        //float ratio = Mathf.Atan(transform.rotation.z / Mathf.Deg2Rad);
        //Debug.Log("ratio = " + ratio);
    }

    // Update is called once per frame
    void Update()
    {
        if (_laserID == 1)
        {
            transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        }
        else if (_laserID == 2)
        {
            transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);
            //transform.position = (Vector3.MoveTowards(transform.position, _orbitCenter, 0.1f));
        }

        /*if (_laserID == 1)
        {
            transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        }
        if (_laserID == 2)
        {
            transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);
        }*/

        //transform.Translate();

        //transform.Translate(direction * _laserSpeed * Time.deltaTime);


        if (transform.position.y > 8.5f || transform.position.y < -6.5f || transform.position.x > 13f || transform.position.x < -13f)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    private float _canFire;
    private float _fireRate = 2f;
    private Vector3 direction;

    [SerializeField]
    private int _type;

    RaycastHit2D hit;

    [SerializeField]
    private GameObject _enemyLaser = null;

/*    [SerializeField]
    private float _circleCastRadius = 2f; //2 units to the center, or 4 units wide*/
    [SerializeField]
    private Vector3 _boxCastSize = new Vector3(2, 4, 0); //2 units wide, 4 units long
    [SerializeField]
    private Vector3 _castOffset = new Vector3(0, -2, 0);

    [SerializeField]
    private Vector3 _vec3 = new Vector3(1, 1, 1);

    [SerializeField]
    private Color _color = new Color(0, 0, 0, 0);
    private Vector4 _vec4 = new Vector4(0, 0, 0, 0);

    [SerializeField]
    private Quaternion _quat = new Quaternion(0, 0, 0, 0);

    [SerializeField]
    private SpriteRenderer _spriteRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector3.left;

        _spriteRenderer = GetComponent<SpriteRenderer>();

        //_vec.x = 0f;

        _color.r = 0f;

        //transform.position.x = 0f;
        //transform.rotation.x = 0f;
        //_spriteRenderer.color.r = 0f;

        _spriteRenderer.color = new Vector4(1, 0, 0, 0);

        //_spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement();
        RayCastMethod();

        if (Input.GetKeyDown(KeyCode.E)) //TESTING
        {
            //transform.localScale *= 1.1f;
            //transform.localScale = new Vector3(transform.localScale.x + 0.1f, transform.localScale.y + 0.1f, transform.localScale.z);
            //_vec[1] += 1;
            //_vec.x += 1;
            //_color[3] += 0.1f;
            //_color.a += 0.1f;
            _color.a = 0.5f;
        }
    }

    private void OnDrawGizmos()
    {
            Gizmos.DrawWireCube(transform.position + _castOffset, _boxCastSize);
            //Gizmos.DrawWireSphere(transform.position + _castOffset, _circleCastRadius);
    }

    void RayCastMethod()
    {
        //Variable called "hit" of type "RaycastHit2D"
        //Store information for any Collider detected in a specified direction

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down * 10);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + _castOffset, _boxCastSize, 0f, Vector2.zero);
        //RaycastHit2D hit = Physics2D.CircleCast(transform.position + _castOffset, _circleCastRadius, Vector2.zero, 0f);

        if (hit.collider != null) //If RayCast hits a Collider
        {
            //Draw Ray from this object downward for 10 spaces, of the color green, and display that line for 0.01 seconds
            //Debug.DrawRay(transform.position, Vector2.down * 5, Color.green, 0.01f);

            //Draw Line between this object and the object collided with, of the color red, and display that line for 0.01 seconds
           Debug.DrawLine(transform.position, hit.point, Color.red, 0.01f);

            //State the name of the object holding the collider we hit
            Debug.Log("Object Detected " + hit.collider.name);

            //State the distance between the RayCast and the Collider of the object we hit
            //Debug.Log("Distance from Origin to Object = " + hit.distance);
        }
    }







    void Movement()
    {
        transform.Translate(direction * 3 * Time.deltaTime);

        if (transform.position.x < -9.5 || transform.position.x > 9.5)
        {
            direction *= -1;
        }
    }

    void FireLaser()
    {
        if (Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_enemyLaser, transform.position, Quaternion.identity);
        }
    }
}
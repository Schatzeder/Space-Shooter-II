using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptExample : MonoBehaviour
{
    private Vector3 _castoffset = new Vector3(0, -1, 0);
    private Vector3 _boxCastParameters = new Vector3(2, 4, 0);

    bool closeEnough = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RayCastMethod();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + _castoffset, _boxCastParameters);
    }

    void RayCastMethod()
    {
        //Variable called "hit" of type "RaycastHit2D"
        //Store information for any Collider detected in a specified direction
        //Collider2D playerInCircle = Physics2D.OverlapCircle(transform.position, 2f);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + _castoffset, _boxCastParameters, 0f, Vector2.zero);
        //RaycastHit2D hitinfo = Physics2D.BoxCast(transform.position + _castoffset, _boxCastParameters, 0f, Vector2.zero);

/*        if (playerInCircle != null)
        {
            Debug.Log("################### works");
        }*/
        if (hit.collider != null)
        {
            Debug.Log("HIT");
            //Debug.DrawRay(transform.position, Vector2.down * 10, Color.green, 0.02f);
            //Debug.DrawRay(transform.position, Vector2.up * 10, Color.green, 0.02f);

            Debug.DrawLine(transform.position, hit.collider.transform.position, Color.red, 0.01f);
            // detects laser active ddodge 
            closeEnough = true;
            //Debug.Log("Objectdetected by raytrace beam 7777 " + hit.collider.name);
        }

/*        if (hitinfo.collider != null)
        {
            Debug.DrawLine(transform.position, hitinfo.collider.transform.position, Color.green, 5f);
            closeEnough = true;
            Debug.Log("The Spherething is indeed happening 7777 " + hitinfo.collider.name);
        }*/
    }
}

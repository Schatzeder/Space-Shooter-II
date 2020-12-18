using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traj_Sphere : MonoBehaviour
{
    private Rigidbody2D _sphereRB;

    [SerializeField]
    private Vector2 _sRBVel;

    [SerializeField]
    private Vector2 _sRBVelRad;
    [SerializeField]
    private Vector2 _grav;

    [SerializeField]
    private float xVel;

    [SerializeField]
    private float yVel;



    // Start is called before the first frame update
    void Start()
    {
        _sphereRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

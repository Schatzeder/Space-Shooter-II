﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TRAJECTORY PROJECTION WHILE THROWING AN OBJECT
//Needs a Shape with a Collider2D, Rigidbody2D, LineRenderer
//In order to measure MousePosition easily in 2D, Camera needs to be set to Orthographic

//TLDR: Calculate Physics, then assign these points to a LineRenderer for display

//Currently does not account for 3-D, Drag, Gravity Scale, Bounces, Ground Velocity, and Existing Velocity (only launch when standing still)

public class Traj_Fling : MonoBehaviour
{
    private LineRenderer _castLine; //Line to display Launch Direction & Force
    private LineRenderer _lr; //Line to predict trajectory

    private Rigidbody2D _rb; //Rigidbody who's taking Force

    private Vector2 _launchDir; //Direction to launch this object in
    private Vector3 _mouseOffset = new Vector3(0, 0.9f, 0); //Mouse offset to fix CastLine display

    private float _collisionCheckRadius = 0.1f; //Collision radius of SimulationArc, to communicate with it when to stop simulating. Currently using IgnoreRaycast Layer on some objects, suboptimal
    private float _mass; //Mass of affected Object's Rigidbody (_rb)
    private float _launchForce; //Force, decided by the length of the line you cast while holding mouse button down
    private float _vel; //Initial Velocity, calculated via V = Force / Mass * fixedTime (0.02)

    private bool _fling = false; //Display SimulationArc when true


    void Start()
    {
        _castLine = GameObject.Find("CastLine").GetComponent<LineRenderer>();
        _castLine.startColor = Color.white;

        _lr = GetComponent<LineRenderer>();
        _lr.startColor = Color.white;

        _rb = GetComponent<Rigidbody2D>();
        _mass = _rb.mass;
    }

    void Update()
    {
        Fling(); //Take commands to commence THE FLING
    }

    void DrawTrajectory()
    {
        _lr.positionCount = SimulateArc().Count;    //Optimization: At some point, just pass _launchDir and _dist through as method arguments? For now keep global
        for (int a = 0; a < _lr.positionCount; a++)
        {
            _lr.SetPosition(a, SimulateArc()[a]); //Add each Calculated Step to a LineRenderer to display a Trajectory. Look inside LineRenderer in Unity to see exact points and amount of them
        }
    }

    private List<Vector2> SimulateArc()
    {
        List<Vector2> trajectoryPoints = new List<Vector2>(); //Reset trajectoryPoints List to make a new one

        float simulateForDuration = 5f;//INPUT amount of total time for simulation
        float simulationStep = 0.1f;//INPUT amount of time between each position check
        Vector2 launchPosition = transform.position; //INPUT launch position (Important to make sure RayCast is ignoring some layers (easiest to use default Layer 2))

        int steps = (int)(simulateForDuration / simulationStep);//Calculates amount of steps simulation will iterate for
        _vel = _launchForce / _mass * Time.fixedDeltaTime; //Velocity = Force / Mass * time

        for (int i = 0; i < steps; ++i) //Iterate a ForLoop over number of Steps
        {
            //Remember f(t) = (x0 + x*t, y0 + y*t - 9.81t²/2)
            //To calculate new Position at each Step... Origin + (LaunchDirection * (LaunchSpeed * Current Step * Length of a Step)
            Vector2 calculatedPosition = launchPosition + (_launchDir * _vel * i * simulationStep); //Calculate new Vector at flat speed
            calculatedPosition.y += Physics2D.gravity.y / 2 * Mathf.Pow((i * simulationStep), 2); //Factor in Gravity, affecting only the Y-Axis (y0 + y*t - 9.81t²/2)

            trajectoryPoints.Add(calculatedPosition); //Add this to the next entry on the list

            if (CheckForCollision(calculatedPosition)) //if you hit something, stop adding positions
            {
                break; //stop adding positions
            }
        }
        return trajectoryPoints;
    }

    private bool CheckForCollision(Vector2 position) //Measure collision via a small circle at the latest position, dont continue simulating Arc if hit
    {   
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, _collisionCheckRadius);
        if (hits.Length > 0)
        {
            return true;    //Return true if something is hit, stopping Arc simulation
        }
        return false;
    }

    void Fling() //Click mouse to start trajectory, longer trajectory means more force, up to 3f distance or 500 force
    {
        if (Input.GetMouseButtonDown(0))
        {
            _fling = true;
            _castLine.enabled = true;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _mouseOffset; //Calculate MousePosition in World (Need Orthographic Camera when doing this in 2D)
            _castLine.SetPosition(0, mousePos); //Set Initial position of CastLine
            StartCoroutine(FlingCalc(mousePos)); //Begin Coroutine to continually update drawing CastLine towards the current mouse position
        }
        if (Input.GetMouseButtonUp(0))
        {
            _fling = false;
            _castLine.enabled = false;

            if (_launchForce > 0.1f) //TOSS THAT PUPPY
            {
                _rb.AddForce(_launchDir * _launchForce); //Add Force in Launch Direction * Launch Force, Angle and Maximum Force visualized by CastLine
            }
        }
    }

    private IEnumerator FlingCalc(Vector3 origin)
    {
        while (_fling == true)
        {
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _mouseOffset; //Find current mouse position
            Vector3 dir = endPos - origin; //Distance between current mousePos and old one
            float dist = Mathf.Clamp(Vector3.Distance(origin, endPos), 0, 3f); //Clamping distance of CastLine at 3, AKA visualizing Maximum Force
            endPos = origin + (dir.normalized * dist); //Fixing endPos to reflect maximum length of 3

            _castLine.SetPosition(1, endPos); //Set castLine to visualize how max throw length and throw direction

            _launchDir = -dir.normalized; //More or less make Launch Direction a Ratio between -1 and 1
            _launchForce = dist * 600/3; //Max Force = 600 AKA @ Line Distance 3, Force = 600
            //_mass = _rb.mass; //Only needed if Object's Mass is being changed at runtime

            DrawTrajectory();
            yield return null;
        }
        yield return null;
    }
}

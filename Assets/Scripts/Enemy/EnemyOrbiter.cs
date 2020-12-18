using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOrbiter : EnemyBehaviour
{
    float[] xFloat;
    float xSpawn;
    float xDirec; //Used to decide the direction in which the enemy orbits, and the outer boundary

    protected override void Start()
    {
        _rotateTarget = GameObject.Find("Orbit_Center");
        VariableAssignment();
        _fireRoutine = StartCoroutine(EnemyFireLaser());
        base.Start();
    }

    private void Update()
    {
        BoundaryCheck(transform.position.x);
        StaticMovement(_eDirection);
        EnemyLookAt();
    }

    protected override void Respawn()
    {
        if (_eDead == false)
        {
            xDirec *= -1;
            _eDirection *= -1; //Reverse direciton
            _eBounds *= -1; //Reverse boundary
        }
    }

    protected override void VariableAssignment()
    {
        //Complete all functions in base method, then with override, do its own stuff
        base.VariableAssignment();

        _eType = 1;
        _eSpeed = 4f;
        _eHealth = 2;
        _eGunChargeDelay = 11;
        xFloat = new float[2];
        xFloat[0] = -12f;
        xFloat[1] = 12f;

        xSpawn = xFloat[Random.Range(0, xFloat.Length)]; //Randomize SpawnPos.x

        xDirec = Mathf.Abs(xSpawn) / xSpawn; //Set direction to negative or positive, depending on spawn position
        _eBounds = -xSpawn + (xDirec * 2.5f); //Set boundary to negative or positive + offset, depending on spawn position
        _eDirection = Vector3.left * xDirec;

        _eSpawnPos = new Vector3(xSpawn, 0, 0);
    }
}

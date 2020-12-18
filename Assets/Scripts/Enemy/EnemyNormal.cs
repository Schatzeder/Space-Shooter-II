using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormal : EnemyBehaviour
{

    protected override void Start()
    {
        VariableAssignment();
        Respawn();
    }

    private void Update()
    {
        BoundaryCheck(transform.position.y);
        StaticMovement(_eDirection);
    }

    protected override void Respawn()
    {
        randomX = Random.Range(-9f, 9f);
        _eSpawnPos = new Vector3(randomX, 8, 0);
        _eGun.color = _eGunColor;
        _fireRoutine = StartCoroutine(EnemyFireLaser());
        base.Respawn();
    }

    protected override void VariableAssignment()
    {
        //Complete all functions in base method, then with override, do its own stuff
        base.VariableAssignment();
        _eType = 0;
        _eSpeed = 4f;
        _eHealth = 1;
        _eBounds = -6f;
        _eDirection = Vector3.down;
    }
}

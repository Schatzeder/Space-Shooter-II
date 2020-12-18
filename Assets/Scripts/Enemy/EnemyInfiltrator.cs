using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfiltrator : EnemyBehaviour
{
    //BUG: On death doesn't always turn off Thrusters

    //Flies evasive, attempts to avoid the player or lasers
    //Once it reaches y < 0, turn around rapidly and thrust to slow down speed
    //Now while floating downward more slowly, turn towards the player
    //Once facing player, charge laser method to fire three times rapidly

    //EITHER Vector3.down needs to be worldspace

    //Ideally, turn thruster logic just like the Wasp

    Vector3 direction;

    [SerializeField]
    private bool _downward = true;

    [SerializeField]
    private SpriteRenderer[] _eInfDodgeThruster = new SpriteRenderer[2];


    // Start is called before the first frame update
    protected override void Start()
    {
        VariableAssignment();
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (_dodging == false)
        {
            transform.Translate(Vector3.down * _eSpeed * Time.deltaTime, Space.World); //Working transform.Translate in worldspace
        }

        //transform.Rotate(0, 0, 0.5f); //Working rotation for turning around
        TurnThrusterVisual();
        BoundaryCheck(transform.position.y);

        if (_downward == true) 
        {
            EnemyDodgeLogic(); //Only dodge while travelling downward
        }

        if (transform.position.y <= 1 && _downward == true && _eDead == false)
        {
            _downward = false;
            _turnRoutine =  StartCoroutine(InfiltratorAboutFace());
        }
    }

    protected override void Respawn()
    {
        if (_fireRoutine != null)
        {
            StopCoroutine(_fireRoutine);
        }

        randomX = Random.Range(-9f, 9f);
        _eSpawnPos = new Vector3(randomX, 8, 0);
        transform.rotation = Quaternion.identity;
        _eThruster[0].enabled = false;
        _eThruster[1].enabled = false;
        _eSpeed = 2f;
        _eHealth = 1;
        _downward = true;
        //base.Respawn(); //PUT ME BACK
    }

    protected override void VariableAssignment()
    {
        base.VariableAssignment();
        _eSpeed = 2.0f;
        _eType = 3;
        _eHealth = 1;
        _eBounds = -7f;
        _eGunChargeDelay = 7;

        _eInfDodgeThruster[0] = transform.GetChild(2).GetComponent<SpriteRenderer>();
        _eInfDodgeThruster[1] = transform.GetChild(3).GetComponent<SpriteRenderer>();
    }

    private IEnumerator InfiltratorAboutFace()
    {
        //BUG: Sometimes rotates twice as fast as it should.
        //BUG: Lasers need to instantiate with respect to the objects most forward transform.position. They look offset at angles
        //Randomize direction in which enemy turns
        float[] aboutFaceSpeed = new float[2];
        aboutFaceSpeed[0] = -3.0f;
        aboutFaceSpeed[1] = 3.0f;
        float aFSpeed = aboutFaceSpeed[Random.Range(0, aboutFaceSpeed.Length)];

        //float time = Time.deltaTime;
        //Debug.Log("About face!!! Speed = " + aFSpeed + ", Y = " + transform.position.y + ", Time = " + time);

        for (int i = 0; i < (180/Mathf.Abs(aFSpeed)); i++)
        {
            //Debug.Log("TURNING " + transform.rotation.z);
            _zRot = transform.rotation.z;
            transform.Rotate(0, 0, aFSpeed);
            _zDelta = transform.rotation.z - _zRot; //Calculating zDelta so ThrusterVisuals can display properly
            yield return new WaitForSeconds(0.02f);
        }

        //Debug.Log("Slowing at y: " + transform.position.y + " @ " + (Time.deltaTime - time));
        _thrustCheck = false;
        _zDelta = 0;
        _eThruster[0].enabled = true;
        _eThruster[1].enabled = true;

        for (int i = 0; i < 6; i++)
        {
            _eSpeed -= 0.2f;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.2f);
        _thrustCheck = true;
        yield return new WaitForSeconds(0.3f);

        //Begin EnemyLookAt
        StartCoroutine(TargetLock());
    }

    protected IEnumerator TargetLock()
    {
        _turnSpeed = 60f;
        _turnTrue = true;
        Debug.Log("Turning to " + _rotateTarget);

        while (_eDead == false && _turnTrue == true)
        {
            if (_rotateTarget != null)
            {
                EnemyLookAt();
            }

            if (transform.rotation == targetRotation)
            {
                _turnTrue = false;
                _zDelta = 0;
                Debug.Log("Target spotted");
                _fireRoutine = StartCoroutine(LaserFire());
            }
            yield return null;
        }
        yield return null;
    }

    protected override void EnemyDeath()
    {
        _zDelta = 0f;

        if (_turnRoutine != null)
        {
            StopCoroutine(_turnRoutine);
        }

        base.EnemyDeath();
        _eThruster[0].enabled = false;
        _eThruster[1].enabled = false;
    }
}

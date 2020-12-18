using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWasp : EnemyBehaviour
{
    /*[SerializeField]
    private SpriteRenderer[] _eThruster = null;*/

    private AudioSource _waspAudioSource;
    [SerializeField]
    private AudioClip _explosion = null;
    [SerializeField]
    private AudioClip _waspLaunch = null;

    private float[] _wBounds;

    // Start is called before the first frame update
    protected override void Start()
    {
        VariableAssignment();
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {
        BoundaryCheck(transform.position.y);
        StaticMovement(_eDirection);
        TurnThrusterVisual();
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
        base.Respawn();

        _fireRoutine = StartCoroutine(RamMethod());
    }

    protected override void VariableAssignment()
    {
        base.VariableAssignment();
        _eType = 2;
        _eBounds = -6f;
        _wBounds = new float[4];
        _wBounds[0] = -11f;
        _wBounds[1] = 11f;
        _wBounds[2] = -6f;
        _wBounds[3] = 8f;
        _eDirection = Vector3.down;

        _waspAudioSource = GetComponent<AudioSource>();
/*        _eThruster = new SpriteRenderer[2];
        _eThruster[0] = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
        _eThruster[1] = this.transform.GetChild(2).GetComponent<SpriteRenderer>();
        _rotateTarget = GameObject.Find("Player");*/

        //Floats downward for a moment, turns on thruster, rapidly rotates toward character, accelerates once, and then flies in one direction until it leaves the screen
    }

    protected IEnumerator RamMethod()
    {
        yield return new WaitForSeconds(Random.Range(1.5f, 3f));
        _turnTrue = true;
        _turnSpeed = 60f;

        //_eThruster[0].color += new Color(0, 0, 0, 0.2f);
        //_eThruster[1].color += new Color(0, 0, 0, 0.2f);

        while (_eDead == false && _turnTrue == true)
        {
            Debug.Log("TURNING to " + targetRotation);

            if (_rotateTarget != null)
            {
                EnemyLookAt();
            }

/*            //if (targetRotation.z > 0)
            if (_zDelta > 0.005)
            {
                _eThruster[0].enabled = true;
                _eThruster[1].enabled = false;
            }
            else if (_zDelta < 0.005)
            {
                _eThruster[0].enabled = false;
                _eThruster[1].enabled = true;
            }*/

            if (transform.rotation == targetRotation)
            {
                StartCoroutine(LaunchRam());
                _turnTrue = false;
                _zDelta = 0;
                Debug.Log("Target spotted");
            }
            yield return null;
        }
        //Turn thrusters on
        //Rotate towards character
        //Once locked on character, increase speed and stop rotating
        //Drift off screen and make sure to set Bounds to X as well as Y
        yield return null;
    }

    protected IEnumerator LaunchRam()
    {
        //_eThruster[0].color += new Color(0, 0, 0, 0.8f);
        //_eThruster[1].color += new Color(0, 0, 0, 0.8f);
        _waspAudioSource.clip = _waspLaunch;
        _waspAudioSource.Play();
        _eThruster[0].enabled = true;
        _eThruster[1].enabled = true;
        Debug.Log("RAMMING");

        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < 8; i++)
        {
            if (_eDead == false)
            {
                _eSpeed += 1.1f;
                yield return new WaitForSeconds(0.05f);
            }
        }
        yield return null;
    }

    protected override void EnemyDeath()
    {
        //_eThruster[0].color -= new Color(0, 0, 0, 1f);
        //_eThruster[0].color -= new Color(0, 0, 0, 1f);
        _eThruster[0].enabled = false;
        _eThruster[1].enabled = false;
        _waspAudioSource.clip = _explosion;
        base.EnemyDeath();
    }

    protected override void BoundaryCheck(float pos)
    {
        if (transform.position.x < _wBounds[0] || transform.position.x > _wBounds[1] || transform.position.y < _wBounds[2] || transform.position.y > _wBounds[3])
        {
            Respawn();
        }
    }
}

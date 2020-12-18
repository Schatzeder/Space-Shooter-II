using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    //Overrides
    [SerializeField]
    protected int _eType;
    [SerializeField]
    protected int _eHealth = 0;
    [SerializeField]
    protected float _eSpeed = 0f;
    protected float _turnSpeed = 20f;
    protected float _eBounds;
    protected float _zRot;
    [SerializeField]
    protected float _zDelta = 0f;
    //float circleRadius = 0.8f;
    protected Vector3 _eDirection;

    [SerializeField]
    protected float _eGunChargeDelay;

    protected float randomX = 0f;

    [SerializeField]
    protected bool _eDead = false;
    protected bool _firing = false;
    protected bool _turnTrue = false;
    protected bool _thrustCheck = true;
    protected bool _fireAgainCheck = false;
    [SerializeField]
    protected bool _dodging = false;

    [SerializeField]
    protected Vector3 _eSpawnPos;
    protected Vector3 _impactPos;
    [SerializeField]
    protected Vector3 _boxCastSize;
    [SerializeField]
    protected Vector3 _raycastOffsetPos = new Vector3(0, -1.8f, 0);
    protected Quaternion targetRotation;

    [SerializeField] //ASSIGNED IN UNITY
    protected GameObject _eLaser;
    [SerializeField]
    protected GameObject _dmgExplode;
    [SerializeField]
    protected GameObject _rotateTarget;
    [SerializeField]
    protected SpriteRenderer[] _eThruster = null;

    protected Transform _laserContainer;

    protected Coroutine _moveRoutine = null;
    protected Coroutine _fireRoutine = null;
    protected Coroutine _thrusterVisualRoutine = null;
    protected Coroutine _turnRoutine = null;

    protected SpriteRenderer _eGun = null;
    protected Color _eGunColor;

    private UIManager _uiManager = null;
    private Player _player = null;
    private BoxCollider2D _eCollider = null;
    private Animator _eAnimator = null;
    private AudioSource _eAudioSource = null;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        transform.position = _eSpawnPos;
    }

    protected virtual void VariableAssignment()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _laserContainer = GameObject.Find("Laser_Container").transform;

        _eGun = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        _eGunColor = this.transform.GetChild(0).GetComponent<SpriteRenderer>().color;

        _eCollider = GetComponent<BoxCollider2D>();
        _eAnimator = GetComponent<Animator>();
        _eAudioSource = GetComponent<AudioSource>();

        _raycastOffsetPos = new Vector3(0, -1.8f, 0);

        if (_eType == 2 || _eType ==3)
        {
            _eThruster = new SpriteRenderer[2];
            _eThruster[0] = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
            _eThruster[1] = this.transform.GetChild(2).GetComponent<SpriteRenderer>();
            _rotateTarget = GameObject.Find("Player");


            _boxCastSize = new Vector3(_eCollider.size.x * transform.lossyScale.x, 2.5f, 0); //Set BoxCast Width to slightly larger than Collider size
        }

        //transform.position = _eSpawnPos;
    }

    protected virtual void Respawn()
    {
        if (_eDead == false)
        {
            transform.position = _eSpawnPos;
            _eDead = false;
        }
    }

    protected virtual void StaticMovement(Vector3 direction)
    {
        transform.Translate(direction * _eSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + _raycastOffsetPos, _boxCastSize);
        //Gizmos.DrawWireSphere(transform.position + _raycastOffsetPos, circleRadius);
    }

    protected virtual void EnemyLookAt()
    {
        if (_eDead == false)
        {
            Vector3 target = _rotateTarget.transform.position;
            Vector3 origin = this.transform.position;
            float xDiff = origin.x - target.x;
            float yDiff = origin.y - target.y;
            float angleA = Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg;

            targetRotation = Quaternion.Euler(new Vector3(0, 0, angleA - 90));

            if (_eType == 1) //Enemy Orb
            {
                transform.rotation = targetRotation;
            }
            if (_eType == 2 || _eType == 3) //Enemy Wasp
            {
                _zRot = transform.rotation.z;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);
                _zDelta = transform.rotation.z - _zRot;
                //Debug.Log("zDelta = " + _zDelta);
                //TurnThrusterVisual();
            }
        }
    }

    protected virtual void TurnThrusterVisual()
    {
        if (_thrustCheck == true)
        {
            if (_zDelta > 0.0001)
            {
                _eThruster[0].enabled = true;
                _eThruster[1].enabled = false;
            }
            else if (_zDelta < -0.0001)
            {
                _eThruster[0].enabled = false;
                _eThruster[1].enabled = true;
            }
            else if (_zDelta == 0)
            {
                _eThruster[0].enabled = false;
                _eThruster[1].enabled = false;
            }
        }
    }

    /*protected virtual IEnumerator TurnThrusterVisual()
    {
        while (true)
        {
            _zRot = transform.position.z;
            yield return new WaitForEndOfFrame();
            _zDelta = transform.position.z - _zRot;

            if (_zDelta > 0.005)
            {
                _eThruster[0].enabled = true;
                _eThruster[1].enabled = false;
            }
            else if (_zDelta < -0.005)
            {
                _eThruster[0].enabled = false;
                _eThruster[1].enabled = true;
            }

            Debug.Log("zDelta = " + _zDelta + " == rotation.z: " + transform.rotation.z + "- _zRot: " + _zRot);
            yield return null;
        }
    }*/

    protected virtual void BoundaryCheck(float pos)
    {
        if (_eBounds > 0)
        {
            if (pos > _eBounds)
            {
                Respawn();
            }
        }
        else if (_eBounds < 0)
        {
            if (pos < _eBounds)
            {
                Respawn();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Player player = other.transform.GetComponent<Player>();

            if (_player != null)
            {
                _player.Damage();
            }

            EnemyDeath();
        }
        else if (other.tag == "Laser")
        {
            _uiManager.UpdateScore(10);
            Destroy(other.gameObject);

            EnemyDamage();
        }
        else if (other.tag == "Asteroid")
        {   //Logic for rogue asteroid collisions
            EnemyDeath();
        }
    }

    protected virtual void EnemyDamage()
    {
        _eHealth--;

        if (_eHealth > 0)
        {
            _impactPos = new Vector3((transform.position.x + Random.Range(-0.7f, 0.7f)), (transform.position.y + Random.Range(-0.7f, 0.7f)), 0f);
            GameObject dmgExplode = Instantiate(_dmgExplode, _impactPos, Quaternion.identity, this.transform);
            Destroy(dmgExplode, 2.5f);
        }

        if (_eHealth <= 0)
        {
            EnemyDeath();
        }
    }

    protected virtual void EnemyDeath()
    {
        Debug.Log("ENEMY DEATH");
        _eDead = true;
        if (_eType == 0 || _eType == 1)
        {
            _eGun.enabled = false;
        }
        _eCollider.enabled = false;
        _eAudioSource.Play();
        _eSpeed /= 2f;
        _eAnimator.SetBool("OnEnemyDeath", true);
        Destroy(this.gameObject, 2.45f);
    }

    protected IEnumerator EnemyFireLaser()
    {
        {   //Repeat always while enemy is alive. CHANGE FOR RESPAWN???
            while (_eDead == false)
            {   
                if (_firing == false) //Fire only if not preparing to do so, prevents duplicate IEnums
                {  
                    _firing = true;
                    yield return new WaitForSeconds(Random.Range(0.3f, 1.6f));

                    StartCoroutine(LaserFire());
                }
                yield return null;
            }
        }
    }

    protected virtual IEnumerator LaserFire()
    {
        float alphaDelta = 1 / _eGunChargeDelay;
        Debug.Log(this.name + ": alphaDelta = " + alphaDelta);

        for (int i = 0; i <= _eGunChargeDelay; i++)
        {   //Adding no color + some alpha every tenth of a second
            _eGun.color += new Color(0, 0, 0, alphaDelta);
            yield return new WaitForSeconds(0.1f);

            if (i == _eGunChargeDelay && _eDead == false)
            {
                GameObject eLaser = Instantiate(_eLaser, transform.position + new Vector3(0, -0.8f, 0), transform.rotation, _laserContainer);
                yield return new WaitForSeconds(0.1f);
                _eGun.color = _eGunColor;
                _firing = false;

                if (_eType == 3)
                {
                    yield return new WaitForSeconds(0.25f);
                    GameObject eeLaser = Instantiate(_eLaser, transform.position + new Vector3(0, -0.8f, 0), transform.rotation, _laserContainer);
                    //GameObject eeLaser = Instantiate(_eLaser, transform.position + new Vector3(0, -0.8f, 0), Quaternion.identity, _laserContainer);
                }
            }
        }
        yield return null;
    }

    protected virtual void EnemyDodgeLogic()
    {
        //Evade player lasers to an extent
        //Can either be delayed dodge, AKA the closer you fire the less time they have to react
        //Can be a one time dodge or dodge meter which exhausts as they go
        //Can be a charge count of teleports for certain enemies

        //RaycastHit2D hit = Physics2D.Raycast((transform.position + _raycastOffsetPos), Vector2.down, 5f, 5);

        //RaycastHit2D hit = Physics2D.CircleCast(transform.position + _raycastOffsetPos, circleRadius, Vector2.zero);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + _raycastOffsetPos, _boxCastSize, 0f, Vector2.zero);

        //Debug.DrawRay((transform.position + _raycastOffsetPos), Vector2.down * 5f, Color.red, 0.01f);

        if (hit.collider != null && hit.collider.tag == "Laser" && _eDead == false) //Will react to lasers if alive
        {
            //Debug.Log("hitpoint.x = " + hit.point.x + "my pos.x = " + transform.position.x);
            float posDiff = hit.point.x - transform.position.x;
            float dodgeDirection;
            if (posDiff != 0)
            {
                dodgeDirection = posDiff / Mathf.Abs(posDiff);
            }
            else
            {
                dodgeDirection = -1; //Default direction if measurement is ever 0 (dead center)
            }

            Debug.Log("dodgeDirec = " + dodgeDirection);
            _dodging = true; //BUG: Inefficient, Bool doesn't need to be set every single frame, or maybe it does...
            DodgeMethod(dodgeDirection);
            //Start Dodge coroutine here and be sure not to have it called in repetition
        }
        else
        {
            _dodging = false;
        }
    }


    protected virtual IEnumerator DodgeCoroutine()
    {
        //Set BoxCast back to 1x size of Collider
        //Move fixed distance out of the way of the laser
        //While moving and for a short while afterward, cannot dodge again. Can only dodge once???
        //DODGE INDICATORS ON LEFT AND RIGHT? CAN DODGE ONCE IN EACH DIRECTION?!
        
        yield return null;
    }


    protected virtual void DodgeMethod(float direc)
    {
        Vector2 direction = new Vector2(direc, -1); //Move slightly upward and dodge
        transform.Translate(-direction * 2 * Time.deltaTime);
    }
}

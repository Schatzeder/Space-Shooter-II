using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullRadius : MonoBehaviour
{

    private SpriteRenderer _pullColor;

    private Coroutine _colorRoutine = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        Debug.Log("Awake");
        _pullColor = GetComponent<SpriteRenderer>();
        _pullColor.color = new Color(0, 0, 1, 0.05f);
        _colorRoutine = StartCoroutine(ColorPulse());
    }

    private void OnDisable()
    {
        StopCoroutine(_colorRoutine);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Collided with " + collision);
        if (collision.tag == "Powerup")
        {
            collision.transform.position = Vector3.MoveTowards(collision.transform.position, transform.position, 0.07f);
        }
    }

    private IEnumerator ColorPulse()
    {
        while(true)
        {
            _pullColor.color += new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.03f);
            _pullColor.color += new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.03f);
            _pullColor.color += new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.03f);
            _pullColor.color += new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.03f);
            _pullColor.color += new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.03f);

            _pullColor.color -= new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.03f);
            _pullColor.color -= new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.03f);
            _pullColor.color -= new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.03f);
            _pullColor.color -= new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.03f);
            _pullColor.color -= new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.03f);

            yield return null;
        }
    }
}

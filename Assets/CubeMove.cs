using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    Vector2 direction = Vector2.left;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SwitchDirection());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * 2 * Time.deltaTime);
    }

    private IEnumerator SwitchDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 2f));
            direction *= -1;
        }
    }
}

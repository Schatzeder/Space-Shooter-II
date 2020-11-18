using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField]
    private GameObject _engineExplosion = null;

    private Renderer _sprite = null;

    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(EngineDamageVisual());
    }

    private IEnumerator EngineDamageVisual()
    {
        GameObject explosion = Instantiate(_engineExplosion, this.transform);
        Destroy(explosion, 2.5f);
        yield return new WaitForSeconds(.25f);
        _sprite.enabled = true;
        yield return null;
    }
}

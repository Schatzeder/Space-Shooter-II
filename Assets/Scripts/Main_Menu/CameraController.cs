using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float _cameraFOV;
    private float _cameraROT;
    // Start is called before the first frame update
    void Start()
    {
        _cameraFOV = 60f;
        _cameraROT = 0f;
    }

    public void CameraShake()
    {
        int i = Random.Range(1, 3);
        StartCoroutine(CameraShakeEnum(i));
    }

    private IEnumerator CameraShakeEnum(int i)
    {
        if (i == 1)
        {
            Debug.Log("Camera Shake ONE");
            _cameraFOV = 55f;
            yield return new WaitForSeconds(0.05f);
            _cameraFOV = 60f;
            yield return new WaitForSeconds(0.05f);
            _cameraFOV = 57f;
            yield return new WaitForSeconds(0.05f);
            _cameraFOV = 60f;
            yield return new WaitForSeconds(0.05f);
            _cameraFOV = 56f;
            yield return new WaitForSeconds(0.05f);
            _cameraFOV = 60f;
        }
        if (i == 2)
        {
            Debug.Log("Camera Shake TWO");
            _cameraROT = -4f;
            //yield return new WaitForSeconds(0.05f);
            yield return new WaitForEndOfFrame();
            _cameraROT = 0f;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            _cameraROT = 8f;
            //yield return new WaitForSeconds(0.05f);
            yield return new WaitForEndOfFrame();
            _cameraROT = 0f;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            _cameraROT = -12f;
            //yield return new WaitForSeconds(0.05f);
            yield return new WaitForEndOfFrame();
            _cameraROT = 0f;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            _cameraROT = 16f;
            //yield return new WaitForSeconds(0.05f);
            yield return new WaitForEndOfFrame();
            _cameraROT = 0f;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            _cameraROT = -8f;
            //yield return new WaitForSeconds(0.05f);
            yield return new WaitForEndOfFrame();
            _cameraROT = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.fieldOfView = _cameraFOV;
        Camera.main.transform.Rotate(0, 0, _cameraROT);
    }
}

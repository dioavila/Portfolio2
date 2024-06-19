using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float shaketime, float strength)
    {
        Vector3 origPOS = transform.localPosition;

        float StartShakeTime = 0.0f;

        while (StartShakeTime < shaketime)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(0, 1f) * strength;

            transform.localPosition = new Vector3(x, y, origPOS.z);

            StartShakeTime += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = origPOS;
    }
}

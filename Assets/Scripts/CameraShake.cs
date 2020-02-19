using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    

    private void Awake()
    {
       
    }
    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator cShake (float duration, float amount, Vector3 originalPos)
    {
        originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * amount;
            float y = Random.Range(-1f, 1f) * amount;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;

    }
}

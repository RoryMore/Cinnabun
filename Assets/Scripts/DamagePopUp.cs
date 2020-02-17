using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DamagePopUp : MonoBehaviour
{
    private static int sortingOrder;
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }
    public void SetUp(int damageAmount, bool isCrit)
    {
        textMesh.SetText(damageAmount.ToString());

        if (!isCrit)
        {
            //Normal hit
            textMesh.fontSize = 36;
            textColor = Color.white;
        }
        else
        {
            // Critical Hit
            textMesh.fontSize = 45;
            textColor = Color.red;
        }

        
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
    }

    private void Update()
    {
        float moveYSPeed = 2f;
        transform.position += new Vector3(0, moveYSPeed) * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * 0.5f)
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

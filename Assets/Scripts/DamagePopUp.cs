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
    //[SerializeField] private Transform damageNumbers;


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
            textMesh.fontSize = 20;
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
        transform.position += new Vector3(0, moveYSPeed) * Time.unscaledDeltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * 0.9f)
        {
            float increaseScaleAmount = 3f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.unscaledDeltaTime;
        }
        else {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.unscaledDeltaTime;
        }

        disappearTimer -= Time.unscaledDeltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.unscaledDeltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

   /* public void Create(Vector3 position, int damageAmount, bool crit)
    {
        Transform damagePopUpTransform = Instantiate(damageNumbers, position, Quaternion.identity);

        DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
        damagePopUp.SetUp(damageAmount, crit);
    }*/
}

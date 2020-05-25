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
    MeshRenderer meshRenderer;
    Material materialInstance;

    [System.Serializable]
    public struct MaterialColours
    {
        public Color faceColour;
        public Color outlineColour;
    }
    [SerializeField]
    MaterialColours normalColours;
    [SerializeField]
    MaterialColours critColours;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();

        meshRenderer = GetComponent<MeshRenderer>();

        materialInstance = meshRenderer.material;
    }

    public void SetUp(int damageAmount, bool isCrit)
    {
        textMesh.SetText(damageAmount.ToString());

        if (!isCrit)
        {
            //Normal hit
            textMesh.fontSize = 20;
            textColor = Color.white;

            materialInstance.SetColor("_FaceColor", normalColours.faceColour);
            materialInstance.SetColor("_OutlineColor", normalColours.outlineColour);
        }
        else
        {
            // Critical Hit
            textMesh.fontSize = 37.5f;
            textColor = Color.white;
            materialInstance.SetColor("_FaceColor", critColours.faceColour);
            materialInstance.SetColor("_OutlineColor", critColours.outlineColour);
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

        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);

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
                //materialInstance.SetColor("_FaceColor", normalColours.faceColour);
                //materialInstance.SetColor("_OutlineColor", normalColours.outlineColour);
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

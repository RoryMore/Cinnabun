using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumbers : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform damageNumbers;
    public CameraShake cameraShake;
   //Vector3 randomPosition = new Vector3(93.5f, 51.6f, 72.5f);
    public void Start()
    {

        bool isCrit = Random.Range(0, 100) < 30;
        Create(Vector3.zero, 254, isCrit);
        //return damagePopUp;
       
    }



    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.T))
        { bool isCrit = Random.Range(0, 100) < 30;
            Create(Vector3.zero, 254, isCrit);
            StartCoroutine(cameraShake.cShake(.15f,.4f));
        }
    }

    void Create(Vector3 position, int damageAmount, bool crit)
    {
        Transform damagePopUpTransform = Instantiate(damageNumbers, position, Quaternion.identity);

        DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
        damagePopUp.SetUp(damageAmount, crit);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumbers : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform damageNumbers;
   //Vector3 randomPosition = new Vector3(93.5f, 51.6f, 72.5f);
    public void Start()
    {
        //return damagePopUp;
       
        // =============================================
        // What I would do:
        // Class has -> Property: TextMeshPro
        // 
        // Have an Initialise function with paramaters (damageValue, ifCrit)
        // Initialise definition will edit the TextMeshPro with the given value.
        // This objects Update() will control the lifespan of the text until deletion, and movement.
        //
        // Outside of this class have the object Entity we spawn from have a GameObject property set as the prefab that has this script.
        // Outside class (Entity) Instantiates this prefab at it's position + added Y value. Call instatiated objects Initialise function we made.
    }

    // Update is called once per frame
    void Update()
    {
       
    }

 
}

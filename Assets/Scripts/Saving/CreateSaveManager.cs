using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSaveManager : MonoBehaviour
{
    [SerializeField]
    GameObject saveManagerPrefab;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveManager saveCheck = FindObjectOfType<SaveManager>();
        if (saveCheck == null)
        {
            //Debug.Log("SaveManager created");
            Instantiate(saveManagerPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

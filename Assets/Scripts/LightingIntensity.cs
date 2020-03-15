using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingIntensity : MonoBehaviour
{
    [Tooltip("Intesity of the light per each CurrentTime")] 
    public List<float> inensitys;
    public int currentLight;
    public float IntensityChangingStr;

    //starting Light Intesity
    [Tooltip("starting Light Intesity")]
    public float intensLight=0;


    Light lighting;
    // Start is called before the first frame update
    void Start()
    {
        lighting = GetComponent<Light>();
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //if the intensity is not what it should changing it
        if (lighting.intensity != inensitys[currentLight])
        {
                if (lighting.intensity > inensitys[currentLight])
            {
                intensLight -= IntensityChangingStr;

            }
            else
            {
                intensLight += IntensityChangingStr;

            }
        }
        lighting.intensity = intensLight;
    }
    //change what the ideal intensity it should be
    public void SetCurrentLight()
    {
        Debug.Log(inensitys.Count-1);
        if (currentLight == inensitys.Count-1)
        {
            currentLight = 0;
        }
        else
        {
            currentLight++;
        }
        
    }
}

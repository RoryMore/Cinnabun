using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntensityOfCycle : MonoBehaviour
{
    public List<float> inensitys;
    public int currentLight;
    public float insiyStr;

   public float intensLight=0;


    Light lighting;
    // Start is called before the first frame update
    void Start()
    {
        lighting = GetComponent<Light>();
        intensLight = lighting.intensity;
    }

    // Update is called once per frame
    void Update()
    {

        if (lighting.intensity != inensitys[currentLight])
        {
            //Debug.Log(lighting.intensity < inensitys[currentLight]);
            //Debug.Log();
                if (lighting.intensity > inensitys[currentLight])
            {
                intensLight -= 0.01f;

            }
            else
            {
                intensLight += 0.01f;

            }
        }
        //lighting.intensity = intensLight;
    }
    public void SetCurrentLight()
    {
        if (currentLight+1 == inensitys.Count)
        {
            currentLight = 0;
        }
        else
        {
            currentLight++;
        }
        
    }
}

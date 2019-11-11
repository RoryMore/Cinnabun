using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureClear : MonoBehaviour
{
    public RenderTexture targetRenderTexture;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.RenderTexture.active = targetRenderTexture;
        GL.Clear(true, true, Color.clear);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FootL()
    {
        SoundManager.leftFootstep.Play();
    }

    public void FootR()
    {
        SoundManager.rightFootstep.Play();
    }
}

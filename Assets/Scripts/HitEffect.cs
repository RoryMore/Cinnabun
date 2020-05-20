using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitEffect : MonoBehaviour
{
    public Image HitEffectImage;
    public Color HitEffectImageColor;

    public Player player;
    public bool fade;
    public float MaxplayerHealth;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        MaxplayerHealth = player.currentHP;
        //FlashHitEffect();

        HitEffectImageColor = HitEffectImage.color;

        HitEffectImageColor.a = 0.0f;
        HitEffectImage.color = HitEffectImageColor;
}

    public void FlashHitEffect()
    {

        HitEffectImageColor.a = 255 - player.currentHP;

        if(HitEffectImageColor.a <= 0)
        {
            HitEffectImageColor.a -= Time.deltaTime;
        }
        
        


        //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), HitEffectImage);
    }

}

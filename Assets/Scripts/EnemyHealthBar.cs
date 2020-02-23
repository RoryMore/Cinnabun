using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    // Start is called before the first frame update

    SimpleEnemy enemy;

    public Image enemeyHealthBar;
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private float disappearTimer;
    private Color textColor;
    //[SerializeField] private Transform damageNumbers;


    private void Awake()
    {
        enemy = FindObjectOfType<SimpleEnemy>();
    }

    private void Update()
    {
        HealthUpdate();
    }

    void HealthUpdate()
    {

        enemeyHealthBar.fillAmount = (float)enemy.currentHP / (float)enemy.maxHP;

        if (enemy.currentHP == enemy.maxHP)
        {
            Color temp = enemeyHealthBar.color;
            temp.a = 0f;
            enemeyHealthBar.color = temp;
        }
        else
        {
            Color temp = enemeyHealthBar.color;
            temp.a = 1f;
            enemeyHealthBar.color = temp;
        }
        
    }


}

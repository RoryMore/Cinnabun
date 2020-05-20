using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUIManager : MonoBehaviour
{
    EnemyManager enemyManager = null;

    [Header("Wave Text")]
    [SerializeField]
    Text waveCounterText = null;
    [SerializeField]
    Text waveTypeText = null;
    [SerializeField]
    Text waveTimerText = null;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyManager != null)
        {
            waveCounterText.text = "WAVE #" + (enemyManager.numOfClearedEncounters + 1).ToString();
            if (!enemyManager.WaveActive)
            {
                waveTypeText.text = "NO WAVE ACTIVE";

                waveTimerText.text = "NEXT WAVE IN <color=maroon>" + Mathf.Round(enemyManager.WaveCooldownTimer) + "</color>";
            }
            else
            {
                if (enemyManager.enemyMangerCurrentEncounter != null)
                {
                    switch (enemyManager.enemyMangerCurrentEncounter.waveType)
                    {
                        case Encounter.WaveType.SLAUGHTER:
                            {
                                waveTypeText.text = "SLAUGHTER";
                                waveTimerText.text = "KILL THEM ALL";
                                break;
                            }
                        case Encounter.WaveType.ENDLESS:
                            {
                                waveTypeText.text = "ENDLESS";
                                waveTimerText.text = "SURVIVE FOR <color=maroon>" + Mathf.Round(enemyManager.enemyMangerCurrentEncounter.WaveOverTicker) + "s</color>";
                                break;
                            }
                        case Encounter.WaveType.MINIBOSS:
                            {
                                waveTypeText.text = "MINI-BOSS";
                                waveTimerText.text = "THE BIG ONE COMES";
                                break;
                            }
                    }
                }
            }
        }
    }
}

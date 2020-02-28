﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorPositionControl : MonoBehaviour
{
    [SerializeField]
    BaseSkill skill;

    Projector projector;

    float distanceFromCaster;
    Vector3 faceDownAngle;
    Quaternion qAngle;

    private void Awake()
    {
        projector = GetComponent<Projector>();

        qAngle = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        faceDownAngle = new Vector3(90.0f, 0.0f, 0.0f);
        qAngle.eulerAngles = faceDownAngle;
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (skill.fillType)
        {
            case BaseSkill.CastFillType.LINEAR:
                {
                    distanceFromCaster = skill.skillData.maxRange * 0.5f;
                    projector.orthographicSize = distanceFromCaster;
                    break;
                }

            case BaseSkill.CastFillType.CIRCULAR:
                {
                    distanceFromCaster = skill.skillData.maxRange;
                    projector.orthographicSize = distanceFromCaster;
                    break;
                }
        }
        projector.farClipPlane = skill.skillData.verticalRange * 0.5f;
        projector.nearClipPlane = -skill.skillData.verticalRange * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // If there are any on-the-fly changes to range. make the necessary changes here
        if ((skill.skillData.maxRange * 0.5f + skill.skillData.minRange) != distanceFromCaster)
        {
            switch (skill.fillType)
            {
                case BaseSkill.CastFillType.LINEAR:
                    {
                        distanceFromCaster = skill.skillData.maxRange * 0.5f;
                        projector.orthographicSize = distanceFromCaster;
                        break;
                    }

                case BaseSkill.CastFillType.CIRCULAR:
                    {
                        distanceFromCaster = skill.skillData.maxRange;
                        projector.orthographicSize = distanceFromCaster;
                        break;
                    }
            }
        }

        switch(skill.moveType)
        {
            // Will the indicator move away from the caster ever
            case BaseSkill.IndicatorMoveType.ALWAYSNEARCASTER:
                {
                    // Based on fill type, specify where the indicator should be relative to the caster
                    switch(skill.fillType)
                    {
                        case BaseSkill.CastFillType.LINEAR:
                            {
                                transform.position = skill.casterSelf.transform.position + skill.casterSelf.transform.forward * distanceFromCaster;

                                transform.rotation = skill.casterSelf.transform.rotation;
                                transform.rotation *= qAngle;
                                break;
                            }
                        case BaseSkill.CastFillType.CIRCULAR:
                            {
                                transform.position = skill.casterSelf.transform.position;
                                break;
                            }
                    }
                    break;
                }
        }
    }
}

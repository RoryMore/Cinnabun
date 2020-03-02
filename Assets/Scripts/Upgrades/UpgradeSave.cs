using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeSave
{
    public CharacterUpgrade teleportUpgrade = new CharacterUpgrade(CharacterUpgrade.UpgradeType.TeleportRange);
    public CharacterUpgrade playerMovespeed = new CharacterUpgrade(CharacterUpgrade.UpgradeType.PlayerMoveSpeed);
    public CharacterUpgrade bloodOrbEffectiveness = new CharacterUpgrade(CharacterUpgrade.UpgradeType.BloodOrbEffectiveness);
    public CharacterUpgrade blastExplosionRadius = new CharacterUpgrade(CharacterUpgrade.UpgradeType.BlastExplosionRadius);

    public int upgradeMoney = 0;
}

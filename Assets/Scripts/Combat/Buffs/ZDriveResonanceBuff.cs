using UnityEngine;

public class ZDriveResonanceBuff : Buff
{
    public ZDriveResonanceBuff(float duration, BuffManager manager, string source) : base(manager)
    {
        this.value = 1;
        base.duration = duration;
        base.source = source;
        manager.simulationManager.ShowText($"{manager.stats.name} Has {value} stack of Z-Drive Resonance For {duration} Seconds!");
    }

    public override void Update()
    {
        duration -= Time.fixedDeltaTime;
        if (value >= 3)
        {
            manager.combat.targetCombat.UpdateTotalDamage(ref manager.combat.targetCombat.pSum, 4, new Damage(Ekko.passiveDamageFlat[manager.combat.targetStats.level - 1] + (manager.combat.targetStats.AP * 0.9f), SkillDamageType.Spell, skillComponentType: (SkillComponentTypes)32896), "Z-Drive Resonance");
            value = 0;
            duration = 5f;
            manager.simulationManager.AddCastLog(manager.combat.myCastLog, 4);
        }
        if (duration <= 0) Kill();
    }
    public override void Kill()
    {
        manager.simulationManager.ShowText($"{manager.stats.name}'s Z-Drive Resonance Stacks Ended!");
        manager.buffs.Remove("ZDriveResonance");
    }
}
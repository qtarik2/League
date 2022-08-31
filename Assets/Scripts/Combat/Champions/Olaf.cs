using Simulator.Combat;
using System.Collections;
using UnityEngine;

public class Olaf : ChampionCombat
{
    public override void UpdatePriorityAndChecks()
    {
        combatPrio = new string[] { "R", "E", "W", "Q", "A" };
        checksQ.Add(new CheckIfRagnorok(this));
        checksW.Add(new CheckIfRagnorok(this));
        checksE.Add(new CheckIfRagnorok(this));
        checksR.Add(new CheckIfRagnorok(this));
        checksA.Add(new CheckIfRagnorok(this));
        checksQ.Add(new CheckIfCasting(this));
        checksW.Add(new CheckIfCasting(this));
        checksE.Add(new CheckIfCasting(this));
        checksR.Add(new CheckIfCasting(this));
        checksA.Add(new CheckIfCasting(this));
        checksQ.Add(new CheckCD(this, "Q"));
        checksW.Add(new CheckCD(this, "W"));
        checksE.Add(new CheckCD(this, "E"));
        checksR.Add(new CheckCD(this, "R"));
        checksA.Add(new CheckCD(this, "A"));
        autoattackcheck = new OlafAACheck(this);
        checkTakeDamage.Add(new CheckOlafPassive(this));
        checkTakeDamageAA.Add(new CheckOlafPassive(this));
        myUI.combatPriority.text = string.Join(", ", combatPrio);
        base.UpdatePriorityAndChecks();
    }

    public override IEnumerator ExecuteQ()
    {
        if (!CheckForAbilityControl(checksQ)) yield break;

        yield return StartCoroutine(StartCastingAbility(myStats.qSkill.basic.castTime));
        UpdateAbilityTotalDamage(ref qSum, 0, myStats.qSkill, 4);
        myStats.qCD = myStats.qSkill.basic.coolDown[4];
        targetStats.buffManager.buffs.Add(myStats.qSkill.basic.name, new ArmorReductionBuff(4, targetStats.buffManager, myStats.qSkill.basic.name, 20f, myStats.qSkill.basic.name));
    }

    public override IEnumerator ExecuteE()
    {
        if (!CheckForAbilityControl(checksE)) yield break;

        yield return StartCoroutine(StartCastingAbility(myStats.eSkill.basic.castTime));
        TakeDamage(75 + (myStats.AD * 0.15f), myStats.eSkill.basic.name, SkillList.SkillDamageType.True); //health cost
        UpdateAbilityTotalDamage(ref eSum, 2, myStats.eSkill, 4);
        myStats.eCD = myStats.eSkill.basic.coolDown[4];
        if (myStats.buffManager.buffs.TryGetValue(myStats.rSkill.basic.name, out Buff value))
        {
            value.duration += 2.5f;
        }
        if (myStats.buffManager.buffs.TryGetValue(myStats.rSkill.basic.name + " ", out Buff val))
        {
            val.duration += 2.5f;
        }
    }

    public override IEnumerator ExecuteR()
    {
        if (!CheckForAbilityControl(checksR)) yield break;

        yield return StartCoroutine(StartCastingAbility(myStats.rSkill.basic.castTime));
        UpdateAbilityTotalDamage(ref qSum, 3, myStats.rSkill, 2);
        myStats.rCD = myStats.rSkill.basic.coolDown[2];
        myStats.buffManager.buffs.Add(myStats.rSkill.basic.name, new AttackDamageBuff(3,myStats.buffManager, myStats.rSkill.basic.name, 30 + (int)(myStats.AD * 0.25f), myStats.rSkill.basic.name));
        myStats.buffManager.buffs.Add(myStats.rSkill.basic.name + " ", new ImmuneToCCBuff(3, myStats.buffManager, myStats.rSkill.basic.name, myStats.rSkill.basic.name + " "));

    }
}
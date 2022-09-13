using System.Collections;
using Simulator.Combat;
using Unity.VisualScripting;
using UnityEngine;

public class Katarina : ChampionCombat
{
    public override void UpdatePriorityAndChecks()
    {
        combatPrio = new string[] { "E", "Q", "W", "R", "A" };

        checksQ.Add(new CheckCD(this, "Q"));
        checksW.Add(new CheckCD(this, "W"));
        checksE.Add(new CheckCD(this, "E"));
        checksR.Add(new CheckCD(this, "R"));
        checksA.Add(new CheckCD(this, "A"));

        checksQ.Add(new CheckIfCasting(this)); 
        checksW.Add(new CheckIfCasting(this));
        checksE.Add(new CheckIfCasting(this));
        checksR.Add(new CheckIfCasting(this));
        checksA.Add(new CheckIfCasting(this));
        checksQ.Add(new CheckIfDisrupt(this));
        checksW.Add(new CheckIfDisrupt(this));
        checksE.Add(new CheckIfDisrupt(this));
        checksR.Add(new CheckIfDisrupt(this));
        checksA.Add(new CheckIfTotalCC(this));
        checksQ.Add(new CheckIfChanneling(this));
        checksW.Add(new CheckIfChanneling(this));
        checksE.Add(new CheckIfChanneling(this));
        checksR.Add(new CheckIfChanneling(this));
        checksA.Add(new CheckIfChanneling(this));
        checksE.Add(new CheckIfImmobilize(this));
        checksA.Add(new CheckIfDisarmed(this));

        qKeys.Add("Magic Damage");
        eKeys.Add("Magic Damage");
        rKeys.Add("Physical Damage Per Dagger");  //0
        rKeys.Add("Magic Damage Per Dagger");     //1

        base.UpdatePriorityAndChecks();
    }

    protected void Passive()
    {
        UpdateAbilityTotalDamage(ref pSum,0, Constants.KatarinaPassiveFlatDamageByLevel[myStats.level] + (myStats.bonusAD * 0.6f) + (Constants.GetKatPassiveAPPercentByLevel(myStats.level) * 0.01f * myStats.AP), myStats.passiveSkill.skillName,SkillDamageType.Spell);
        myStats.eCD = myStats.eSkill[0].basic.coolDown[0] - (Constants.GetKatPassiveECooldownReduction(myStats.level) * 0.01f * myStats.eSkill[0].basic.coolDown[0]);
    }

    public override IEnumerator ExecuteQ()
    {
        yield return base.ExecuteQ();
        yield return new WaitForSeconds(1f);
        Passive();
    }

    public override IEnumerator ExecuteW()
    {
        if (!CheckForAbilityControl(checksW)) yield break;

        yield return StartCoroutine(StartCastingAbility(myStats.wSkill[0].basic.castTime));
        myStats.wCD = myStats.wSkill[0].basic.coolDown[4];
        yield return new WaitForSeconds(1.25f);
        Passive();
    }

    public override IEnumerator ExecuteE()
    {
        yield return base.ExecuteE();
        attackCooldown = 0.0f;
    }
    public override IEnumerator ExecuteR()
    {
        if (!CheckForAbilityControl(checksR)) yield break;

        yield return StartCoroutine(StartCastingAbility(myStats.rSkill[0].basic.castTime));
        myStats.buffManager.buffs.Add("Channeling", new ChannelingBuff(2.5f, myStats.buffManager, myStats.rSkill[0].basic.name, "Death Lotus"));
        StartCoroutine(DeathLotus(0));
        myStats.rCD = myStats.rSkill[0].basic.coolDown[2];
    }

    public IEnumerator DeathLotus(float time)
    {
        yield return new WaitForSeconds(0.166f);
        time += 0.166f;
        UpdateAbilityTotalDamage(ref rSum, 3, myStats.rSkill[0], 2, rKeys[0]);
        UpdateAbilityTotalDamage(ref rSum, 3, myStats.rSkill[0], 2, rKeys[1]);
        if(targetStats.buffManager.buffs.TryGetValue(myStats.rSkill[0].basic.name, out Buff value))
        {
            value.duration = 3f;
        }
        else
        {
            targetStats.buffManager.buffs.Add(myStats.rSkill[0].basic.name, new GrievousWoundsBuff(3, targetStats.buffManager, myStats.rSkill[0].basic.name, 40f, myStats.rSkill[0].basic.name));
        }
        if (time < 2.5f) StartCoroutine(DeathLotus(time));
    }


    public override void StopChanneling(string uniqueKey)
    {
        StopCoroutine(uniqueKey);
    }

}

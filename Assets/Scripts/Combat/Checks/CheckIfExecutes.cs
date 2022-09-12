using Simulator.Combat;

public class CheckIfExecutes : Check
{
    private string skill;
    private int stack;
    private float multiplier;
    public CheckIfExecutes(ChampionCombat ccombat, string skill, int stack = 0, float multiplier = 1) : base(ccombat)
    {
        this.skill = skill;
        this.stack = stack;
        this.multiplier = multiplier;
    }

    public override float Control(float damage)
    {
        throw new System.NotImplementedException();
    }

    public override bool Control()
    {
        switch (skill)
        {
            case "Q":
                return combat.myStats.qSkill[0].UseSkill(4, combat.qKeys[0], combat.myStats, combat.targetStats) >= combat.targetStats.currentHealth;
            case "W":
                return combat.myStats.wSkill[0].UseSkill(4, combat.wKeys[0], combat.myStats, combat.targetStats) >= combat.targetStats.currentHealth;
            case "E":
                return combat.myStats.eSkill[0].UseSkill(4, combat.eKeys[0], combat.myStats, combat.targetStats) >= combat.targetStats.currentHealth;
            case "R":
                return (combat.myStats.rSkill[0].UseSkill(2, combat.rKeys[0], combat.myStats, combat.targetStats) * multiplier) + stack >= combat.targetStats.currentHealth;
            case "Riven":
                return combat.myStats.rSkill[1].UseSkill(2, combat.rKeys[0], combat.myStats, combat.targetStats) * (1 + ((combat.targetStats.maxHealth - combat.targetStats.currentHealth) / combat.targetStats.maxHealth) > 0.75f ? 2 : (combat.targetStats.maxHealth - combat.targetStats.currentHealth) * 2.667f) >= combat.targetStats.currentHealth;
            case "Kalista":
                if (combat.targetStats.buffManager.buffs.TryGetValue("Rend", out Buff value))
                {
                    return combat.myStats.eSkill[0].UseSkill(4, combat.eKeys[0], combat.myStats, combat.targetStats) + ((value.value - 1) * combat.myStats.eSkill[0].UseSkill(4, combat.eKeys[1], combat.myStats, combat.targetStats)) >= combat.targetStats.currentHealth;
                }
                return combat.myStats.eSkill[0].UseSkill(4, combat.eKeys[0], combat.myStats, combat.targetStats) >= combat.targetStats.currentHealth;
            default:
                return false;
        }
    }
}
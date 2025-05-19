namespace BlazedProgressKnight.Game.Data;

public class Skill
{
    public string InternalName { get; set; }
    public string DisplayName { get; set; }
    public SkillCategory Category { get; set; }
    public int MaxLevel { get; set; }

    public double BaseCost { get; set; }
    public double CostMultiplier { get; set; } = 1.1;

    public double BaseXpToLevel { get; set; } = 100; // Base XP for level 1 to 2
    public double XpToLevelMultiplier { get; set; } = 1.2; // XP_for_lvl_N = BaseXpToLevel * (XpToLevelMultiplier ^ (N-1))

    public List<Requirement> RequirementsToUnlock { get; set; } = new List<Requirement>();
    public List<Effect> EffectsPerLevel { get; set; } = new List<Effect>();

    public Skill(string internalName, string displayName, SkillCategory category, int maxLevel, double baseCost, double costMultiplier = 1.1, double baseXpToLevel = 100, double xpToLevelMultiplier = 1.2)
    {
        InternalName = internalName;
        DisplayName = displayName;
        Category = category;
        MaxLevel = maxLevel;
        BaseCost = baseCost;
        CostMultiplier = costMultiplier;
        BaseXpToLevel = baseXpToLevel;
        XpToLevelMultiplier = xpToLevelMultiplier;
    }

    public double GetCostForLevel(int targetLevel) // Cost to reach targetLevel from targetLevel-1
    {
        if (targetLevel <= 0) return 0;
        return BaseCost * Math.Pow(CostMultiplier, Math.Max(0, targetLevel - 1));
    }

    public double GetXpForNextLevel(int currentLevel)
    {
        if (currentLevel < 0) return BaseXpToLevel; // XP for level 0 to 1
        if (currentLevel >= MaxLevel) return double.MaxValue; // Already max level
        return BaseXpToLevel * Math.Pow(XpToLevelMultiplier, currentLevel);
    }
}
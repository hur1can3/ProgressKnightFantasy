namespace BlazedProgressKnight.Game.Data;

public class Job
{
    public string InternalName { get; set; }
    public string DisplayName { get; set; }
    public JobCategory Category { get; set; }
    public int MaxLevel { get; set; } = 100;

    public double BaseIncome { get; set; }
    public double BaseExperience { get; set; } // XP for the job itself
    public double BaseDuration { get; set; }

    // Key: Skill InternalName or SkillCategory.ToString(), Value: XP amount for that skill/category
    public Dictionary<string, double> ExperienceRewards { get; set; } = new Dictionary<string, double>();

    public List<Requirement> Requirements { get; set; } = new List<Requirement>();
    // public List<Effect> EffectsOnCompletion { get; set; } = new List<Effect>(); // e.g. stat gain on completion

    public Job(string internalName, string displayName, JobCategory category, double baseIncome, double baseExperience, double baseDuration, int maxLevel = 100)
    {
        InternalName = internalName;
        DisplayName = displayName;
        Category = category;
        BaseIncome = baseIncome;
        BaseExperience = baseExperience; // XP for this job's level
        BaseDuration = baseDuration;
        MaxLevel = maxLevel;
    }
}
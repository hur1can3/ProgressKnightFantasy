namespace BlazedProgressKnight.Game.Data;

public static class GameData
{
    public static Dictionary<string, Job> AllJobs { get; private set; } = new Dictionary<string, Job>();
    public static Dictionary<string, Skill> AllSkills { get; private set; } = new Dictionary<string, Skill>();

    private static bool _isInitialized = false;

    public static void Initialize()
    {
        if (_isInitialized) return;

        InitializeSkills(); // Skills first, as jobs might reference them in requirements or XP rewards
        InitializeJobs();

        _isInitialized = true;
    }

    private static void InitializeSkills()
    {
        // --- FUNDAMENTALS ---
        var strength = new Skill("strength", "Strength", SkillCategory.Fundamentals, 100, 10, 1.1, 50, 1.15);
        strength.EffectsPerLevel.Add(new Effect(EffectType.JobExperienceMultiplier, 0.01, JobCategory.Mindless.ToString())); // +1% XP for Mindless jobs
        strength.EffectsPerLevel.Add(new Effect(EffectType.Strength, 1)); // +1 Strength (flat) per level
        AllSkills[strength.InternalName] = strength;

        var intelligence = new Skill("intelligence", "Intelligence", SkillCategory.Fundamentals, 100, 10, 1.1, 50, 1.15);
        intelligence.EffectsPerLevel.Add(new Effect(EffectType.SkillExperienceMultiplier, 0.01, SkillCategory.Fundamentals.ToString())); // +1% XP for Fundamental skills
        intelligence.EffectsPerLevel.Add(new Effect(EffectType.Intelligence, 1)); // +1 Int (flat) per level
        AllSkills[intelligence.InternalName] = intelligence;

        var dexterity = new Skill("dexterity", "Dexterity", SkillCategory.Fundamentals, 100, 10, 1.1, 50, 1.15);
        dexterity.EffectsPerLevel.Add(new Effect(EffectType.Dexterity, 1));
        AllSkills[dexterity.InternalName] = dexterity;

        var constitution = new Skill("constitution", "Constitution", SkillCategory.Fundamentals, 100, 10, 1.1, 50, 1.15);
        constitution.EffectsPerLevel.Add(new Effect(EffectType.Constitution, 1));
        AllSkills[constitution.InternalName] = constitution;

        // Custom Begging Skill (as an example, not directly from PK2's "Begging" which is a job)
        // This skill will enhance the "Beggar" job.
        var beggingMastery = new Skill("begging_mastery", "Begging Mastery", SkillCategory.Mental, 50, 5, 1.25, 30, 1.2);
        beggingMastery.EffectsPerLevel.Add(new Effect(EffectType.JobIncomeMultiplier, 0.05, "beggar")); // +5% income for Beggar job
        beggingMastery.EffectsPerLevel.Add(new Effect(EffectType.JobDurationReduction, 0.02, "beggar")); // 2% faster Beggar job
        AllSkills[beggingMastery.InternalName] = beggingMastery;


        // --- COMBAT ---
        var combat = new Skill("combat", "Combat", SkillCategory.Combat, 100, 20, 1.12, 75, 1.2);
        // Effect: unlocks combat jobs, or global combat effectiveness
        AllSkills[combat.InternalName] = combat;

        var melee = new Skill("melee", "Melee Combat", SkillCategory.Combat, 100, 30, 1.15, 100, 1.22);
        melee.RequirementsToUnlock.Add(new Requirement(RequirementType.SkillLevel, "combat", 5));
        melee.RequirementsToUnlock.Add(new Requirement(RequirementType.SkillLevel, "strength", 10));
        // Effects: increase damage/accuracy for melee jobs/actions
        AllSkills[melee.InternalName] = melee;

        // --- MAGIC ---
        var magic = new Skill("magic", "Magic", SkillCategory.Magic, 100, 20, 1.12, 75, 1.2);
        AllSkills[magic.InternalName] = magic;

        var elemental = new Skill("elemental_magic", "Elemental Magic", SkillCategory.Magic, 100, 30, 1.15, 100, 1.22);
        elemental.RequirementsToUnlock.Add(new Requirement(RequirementType.SkillLevel, "magic", 5));
        elemental.RequirementsToUnlock.Add(new Requirement(RequirementType.SkillLevel, "intelligence", 10));
        // Effects: increase power of elemental spells
        AllSkills[elemental.InternalName] = elemental;
    }


    private static void InitializeJobs()
    {
        // --- HOMELESS (Mindless Category) ---
        var beggar = new Job("beggar", "Beggar", JobCategory.Mindless, 1, 5, 5, 20); // Income, Job XP, Duration
        beggar.ExperienceRewards["begging_mastery"] = 2; // XP for Begging Mastery skill
        beggar.ExperienceRewards[SkillCategory.Mental.ToString()] = 1; // XP for Mental skills category (example)
        AllJobs[beggar.InternalName] = beggar;

        var streetPerformer = new Job("street_performer", "Street Performer", JobCategory.Mindless, 3, 8, 8, 30);
        streetPerformer.Requirements.Add(new Requirement(RequirementType.SkillLevel, "begging_mastery", 5));
        streetPerformer.ExperienceRewards["begging_mastery"] = 3;
        streetPerformer.ExperienceRewards[SkillCategory.Mental.ToString()] = 2;
        AllJobs[streetPerformer.InternalName] = streetPerformer;

        // --- FUNDAMENTALS (Mindless Category) ---
        var farmer = new Job("farmer", "Farmer", JobCategory.Mindless, 5, 10, 10, 50);
        farmer.Requirements.Add(new Requirement(RequirementType.SkillLevel, "strength", 5));
        farmer.ExperienceRewards["strength"] = 2;
        farmer.ExperienceRewards[SkillCategory.Fundamentals.ToString()] = 1;
        AllJobs[farmer.InternalName] = farmer;

        var fisher = new Job("fisher", "Fisher", JobCategory.Mindless, 7, 12, 12, 50);
        fisher.Requirements.Add(new Requirement(RequirementType.SkillLevel, "dexterity", 5));
        fisher.ExperienceRewards["dexterity"] = 2;
        fisher.ExperienceRewards[SkillCategory.Fundamentals.ToString()] = 1;
        AllJobs[fisher.InternalName] = fisher;

        var miner = new Job("miner", "Miner", JobCategory.Mindless, 10, 15, 15, 75);
        miner.Requirements.Add(new Requirement(RequirementType.SkillLevel, "strength", 10));
        miner.Requirements.Add(new Requirement(RequirementType.SkillLevel, "constitution", 5));
        miner.ExperienceRewards["strength"] = 3;
        miner.ExperienceRewards["constitution"] = 1;
        miner.ExperienceRewards[SkillCategory.Fundamentals.ToString()] = 2;
        AllJobs[miner.InternalName] = miner;

        // --- MILITARY (Combat Category) ---
        var recruit = new Job("recruit", "Recruit", JobCategory.Combat, 8, 20, 10, 25);
        recruit.Requirements.Add(new Requirement(RequirementType.SkillLevel, "combat", 1));
        recruit.Requirements.Add(new Requirement(RequirementType.SkillLevel, "strength", 5));
        recruit.ExperienceRewards["combat"] = 5;
        recruit.ExperienceRewards["strength"] = 2;
        recruit.ExperienceRewards[SkillCategory.Combat.ToString()] = 2;
        AllJobs[recruit.InternalName] = recruit;

        var warrior = new Job("warrior", "Warrior", JobCategory.Combat, 20, 50, 12, 50);
        warrior.Requirements.Add(new Requirement(RequirementType.JobLevel, "recruit", 10));
        warrior.Requirements.Add(new Requirement(RequirementType.SkillLevel, "melee", 5));
        warrior.Requirements.Add(new Requirement(RequirementType.SkillLevel, "strength", 15));
        warrior.ExperienceRewards["melee"] = 10;
        warrior.ExperienceRewards["strength"] = 5;
        warrior.ExperienceRewards[SkillCategory.Combat.ToString()] = 5;
        AllJobs[warrior.InternalName] = warrior;


        // --- ARCANE (Magic Category) ---
        var apprentice = new Job("apprentice_mage", "Apprentice Mage", JobCategory.Magic, 7, 18, 10, 25);
        apprentice.Requirements.Add(new Requirement(RequirementType.SkillLevel, "magic", 1));
        apprentice.Requirements.Add(new Requirement(RequirementType.SkillLevel, "intelligence", 5));
        apprentice.ExperienceRewards["magic"] = 5;
        apprentice.ExperienceRewards["intelligence"] = 2;
        apprentice.ExperienceRewards[SkillCategory.Magic.ToString()] = 2;
        AllJobs[apprentice.InternalName] = apprentice;

        var elementalist = new Job("elementalist", "Elementalist", JobCategory.Magic, 18, 45, 12, 50);
        elementalist.Requirements.Add(new Requirement(RequirementType.JobLevel, "apprentice_mage", 10));
        elementalist.Requirements.Add(new Requirement(RequirementType.SkillLevel, "elemental_magic", 5));
        elementalist.Requirements.Add(new Requirement(RequirementType.SkillLevel, "intelligence", 15));
        elementalist.ExperienceRewards["elemental_magic"] = 10;
        elementalist.ExperienceRewards["intelligence"] = 5;
        elementalist.ExperienceRewards[SkillCategory.Magic.ToString()] = 5;
        AllJobs[elementalist.InternalName] = elementalist;
    }
}
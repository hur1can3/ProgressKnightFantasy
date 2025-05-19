namespace BlazedProgressKnight.Game.Data;


public enum JobCategory
{
    Mindless, // Homeless, Fundamentals in data.js
    Combat,   // Military in data.js
    Magic,    // Arcane in data.js
    // Add other categories from data.js as needed: e.g. Dark, Galactic
}

public enum SkillCategory
{
    Fundamentals, // Corresponds to "Fundamentals" in data.js skillData
    Combat,
    Magic,
    DarkMagic, // Corresponds to "Dark Magic"
    Scientific, // Corresponds to "Scientific"
    Physical, // Corresponds to "Physical"
    Mental, // Corresponds to "Mental"
    // Add other categories from data.js as needed
}

public enum RequirementType
{
    Money,
    SkillLevel, // e.g., "SkillInternalName": level
    JobLevel,   // e.g., "JobInternalName": level
    Item,       // e.g., "ItemInternalName": quantity
    Statistic,   // e.g., "Strength": value
    Age // Example, if age becomes a mechanic
}

public enum EffectType
{
    // Job related
    JobIncomeMultiplier,        // Multiplies income for a specific job or category (value is % increase, e.g., 0.01 for 1%)
    JobExperienceMultiplier,    // Multiplies XP gain for a specific job or category
    JobDurationReduction,       // Reduces time for a specific job or category (value is % reduction, e.g., 0.01 for 1%)

    // Skill related
    SkillExperienceMultiplier,
    SkillCostReduction,
    MaxSkillLevelIncrease,

    // Global
    GlobalIncomeMultiplier,
    GlobalExperienceMultiplier,
    GlobalLearningRate, // General XP boost for everything

    // Stats
    Strength,
    Intelligence,
    Dexterity,
    Constitution,
    AttackSpeed,
    CriticalChance,
    CriticalDamage,
    Evasion,
    Armor,
    MagicResistance,
    ReducedCooldown,
    IncreasedBuffDuration,

    // Misc
    HousingCapacity,
    GymEffectiveness,
    ItemEffect,
    PrestigeCurrencyGain,
    MaxAge, // Example
    // Add more specific effect types as identified in data.js
}
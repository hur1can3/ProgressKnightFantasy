﻿// =============================================================
// File: ProgressKnightFantasy/Game/Models/GameData.cs
// =============================================================
namespace ProgressKnightFantasy.Game.Models
{
    using System;
    using System.Collections.Generic;

    public class GameData
    {
        // Core Progression
        public Dictionary<string, double> TaskData { get; set; } = new Dictionary<string, double>(); // Current XP for tasks
        public Dictionary<string, int> TaskLevels { get; set; } = new Dictionary<string, int>(); // Current levels of tasks
        public Dictionary<string, double> SkillData { get; set; } =
            new Dictionary<string, double>(); // Skill levels
        public HashSet<string> UnlockedTaskIds { get; set; } = new HashSet<string>();
        public HashSet<string> UnlockedSkillIds { get; set; } = new HashSet<string>();
        public HashSet<string> UnlockedJobIds { get; set; } = new HashSet<string>(); // For Professions/Callings

        // Currencies & Time
        public double Coins { get; set; } // Or "Silver Pieces", "Gold Crowns"
        public double Days { get; set; } = 365 * 14; // Age of the character
        public double Evil { get; set; } // Rebirth currency - "Ancient Power"
        public double Essence { get; set; } // Another potential currency
        public double EssenceKredits { get; set; } // Premium-like currency

        // Shop Items
        public Dictionary<string, int> ShopItemLevels { get; set; } = new Dictionary<string, int>();

        // Current Activities
        public string? CurrentJobId { get; set; } // Current Profession/Calling ID
        public Dictionary<string, double> JobData { get; set; } = new Dictionary<string, double>(); // XP for professions
        public Dictionary<string, int> JobLevels { get; set; } = new Dictionary<string, int>(); // Levels in professions

        public string? CurrentTaskName { get; set; } // Task ID
        public bool IsWorking { get; set; } // True if doing a task or profession

        // Combat State
        public string? CurrentEnemyId { get; set; }
        public double CurrentEnemyHealth { get; set; }

        // Game State
        public bool Paused { get; set; }
        public double TimePlayedTotal { get; set; }
        public DateTime LastSaveTime { get; set; } = DateTime.MinValue; // Initialize to indicate no save yet
        public DateTime LastTickTime { get; set; } // For more precise offline calculation

        // Rebirth & Challenges
        public RebirthData CurrentRun { get; set; } = new RebirthData();
        public Dictionary<string, int> EvilPerks { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> CompletedChallenges { get; set; } =
            new Dictionary<string, int>(); // Tracks completion count
        public string CurrentChallengeId { get; set; } = string.Empty; // Active challenge ID

        // Settings & UI State
        public GameSettings Settings { get; set; } = new GameSettings();
        public GameOptions Options { get; set; } = new GameOptions(); // Potentially for more UI/thematic options
        public double BuyAmount { get; set; } = 1;
        public int MaxTab { get; set; }
        public string CurrentThemeKey { get; set; } = "default_dark"; // For dynamic theming

        // Player Stats & Milestones
        public GameStats Stats { get; set; } = new GameStats();
        public double MaxEssenceKredits { get; set; }
        public int MaxMilestones { get; set; }

        // Tutorial & UI Flags
        public bool CompletedTutorial { get; set; }
        public bool HasShownTutorial { get; set; }
        public bool HasClickedSomething { get; set; }

        public Dictionary<string, double> Resources { get; set; } =
            new Dictionary<string, double>();
        public string? CurrentMentorQuestId { get; set; }
        public HashSet<string> CompletedMentorQuestIds { get; set; } = new HashSet<string>();

        // MODIFIED: Boss Defeat Flags (example)
        public bool IsGatekeeperDefeated_ForestGuardian { get; set; } = false;

        public GameData()
        {
            if (Stats != null)
                Stats.FastestRebirthTime = double.MaxValue;
            LastTickTime = DateTime.UtcNow; // Set initial tick time
        }
    }

    public class RebirthData // Data for the current run/life
    {
        public double PlayerLevel { get; set; } // Overall adventurer level
        public double PlayerXp { get; set; }
        public double TimeThisRun { get; set; }
        public double PlayerCurrentHealth { get; set; } = 100;
    }

    public class GameSettings
    {
        public string? SelectedTheme { get; set; } = "default_dark";
        public string? SelectedNotation { get; set; } = "Standard";
        public string? SelectedFont { get; set; } = "Roboto"; // MODIFIED: Default font for better readability
        public bool AutoMaxAll { get; set; }
        public bool AutoPromote { get; set; }
        public bool AutoRebirth { get; set; }
        public double AutoRebirthLevel { get; set; } = 100;
        public bool RebirthSkipEvilConfirmation { get; set; }
        public bool PauseOffline { get; set; } = false;
        public bool ShowDamageNumbers { get; set; } = true;
        public double MasterVolume { get; set; } = 0.8;
        public int AutoSaveIntervalSeconds { get; set; } = 60;
        public Dictionary<string, bool> Confirmations { get; set; } =
            new Dictionary<string, bool>();
    }

    public class GameStats
    {
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public double FastestRebirthTime { get; set; } = double.MaxValue;
        public int TotalRebirths { get; set; }
        public double MaxPlayerLevelAchieved { get; set; }
        public long TotalCoinsEarned { get; set; }
        public long TotalEvilEarned { get; set; } // Total Ancient Power
        public int TotalTasksCompleted { get; set; } // Sum of all task levels achieved perhaps
        public int TotalProfessionsMastered { get; set; } // Count of max level professions
        public int TotalEnemiesVanquished { get; set; }
        public Dictionary<string, int> EnemiesVanquishedByType { get; set; } =
            new Dictionary<string, int>();
        public Dictionary<string, int> HighestJobLevelAchieved { get; set; } =
            new Dictionary<string, int>();
    }

    public class GameOptions
    { /* For future UI/thematic options */
    }
}

// =============================================================
// File: ProgressKnightFantasy/Game/Models/TaskDefinition.cs
// =============================================================
namespace ProgressKnightFantasy.Game.Models
{
    using System.Collections.Generic;

    public class TaskDefinition // Represents quests, training, activities
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; // Flavor text
        public double XpGainPerTick { get; set; } // Task-specific XP (e.g., "Meditation XP")
        public double PlayerXpRewardPerTick { get; set; } = 0.1; // Adventurer Level XP
        public double BaseMaxXp { get; set; }
        public double XpScalingFactor { get; set; } = 1.1;
        public string Category { get; set; } = "General Training"; // e.g., "Combat Drills", "Arcane Studies", "Wilderness Survival"
        public string ThemeColor { get; set; } = MudBlazor.Color.Default.ToString();
        public bool IsUnlockedByDefault { get; set; } = false;
        public Dictionary<string, int> Requirements { get; set; } = new Dictionary<string, int>();
        public List<string> Unlocks { get; set; } = new List<string>(); // Can unlock tasks, skills, professions
        public Dictionary<string, double> RewardsPerLevel { get; set; } =
            new Dictionary<string, double>(); // e.g. "coins", "skill:id_xp_direct"
        public string? GatedByBossId { get; set; } // MODIFIED: New field to gate tasks
    }
}

// =============================================================
// File: ProgressKnightFantasy/Game/Models/SkillDefinition.cs
// =============================================================
namespace ProgressKnightFantasy.Game.Models
{
    public class SkillDefinition // Represents player abilities and talents
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; // Flavor text
        public string Category { get; set; } = "General"; // e.g., "Combat", "Magic", "Crafting", "Social"
        public string ThemeColor { get; set; } = MudBlazor.Color.Default.ToString();
        public string ThemeKey { get; set; } = "default_dark";
        public bool IsUnlockedByDefault { get; set; } = false;
        public int MaxLevel { get; set; } = 20; // MODIFIED: Example max level for talents
        public Func<int, double> XpCostForNextLevelFormula { get; set; } =
            currentLevel => 50 * Math.Pow(1.5, currentLevel); // MODIFIED: Cost in Player XP
        public string EffectDescription { get; set; } = string.Empty; // Describes the skill's mechanical effect
        public string DetailedEffectTooltip { get; set; } = string.Empty; // MODIFIED: For clearer tooltips
    }
}

// =============================================================
// File: ProgressKnightFantasy/Game/Models/EvilPerkDefinition.cs
// =============================================================
namespace ProgressKnightFantasy.Game.Models
{
    using System;

    public class EvilPerkDefinition // Represents "Echoes of Power"
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty; // e.g., "Echoes of Valor", "Forgotten Lore"
        public string DescriptionTemplate { get; set; } = string.Empty; // e.g., "Increases melee damage by {0}%."
        public string DetailedEffectTooltip { get; set; } = string.Empty; // MODIFIED: For clearer tooltips

        public int MaxLevel { get; set; } = 10;
        public Func<int, double> CostFormula { get; set; } = level => 10 * Math.Pow(2, level); // Cost in "Ancient Power"
        public Func<int, double> EffectValueFormula { get; set; } = level => level * 5; // MODIFIED: This is now the direct Func

        public string PerkType { get; set; } = "Generic"; // e.g., "MeleeDamageBonus", "SpellPowerBonus"
        public bool IsUnlockedByDefault { get; set; } = true; // Some perks might be locked initially
        public string? GatedByBossId { get; set; } // MODIFIED: New field

        public string GetDescription(int currentLevel)
        {
            return string.Format(DescriptionTemplate, EffectValueFormula(currentLevel))
                + (currentLevel >= MaxLevel ? " (Maxed)" : "");
        }

        public string GetNextLevelDescription(int currentLevel)
        {
            return currentLevel >= MaxLevel
                ? "Max Level Reached"
                : $"Next: {string.Format(DescriptionTemplate, EffectValueFormula(currentLevel + 1))}";
        }
    }
}

// =============================================================
// File: ProgressKnightFantasy/Game/Models/ShopItemDefinition.cs
// =============================================================
namespace ProgressKnightFantasy.Game.Models
{
    using System;
    using System.Collections.Generic;

    public enum ShopItemType
    {
        LeveledUpgrade,
        OneTimePurchase,
    }

    public class ShopItemDefinition // Represents items bought with coins, reset on rebirth
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty; // e.g., "Whetstone", "Mana Crystal"
        public string DescriptionTemplate { get; set; } = string.Empty;
        public ShopItemType ItemType { get; set; } = ShopItemType.LeveledUpgrade;
        public string DetailedEffectTooltip { get; set; } = string.Empty; // MODIFIED: For clearer tooltips

        public int MaxLevel { get; set; } = 10;
        public Func<int, double> CostFormula { get; set; } =
            currentLevel => 100 * Math.Pow(1.5, currentLevel); // MODIFIED: Direct Func
        public Func<int, double> EffectValueFormula { get; set; } =
            currentLevel => currentLevel * 2.0; // MODIFIED: Direct Func
        public string EffectTarget { get; set; } = string.Empty; // e.g., "PlayerAttackFlat", "SpellDamageMultiplier"
        public Dictionary<string, int> Requirements { get; set; } = new Dictionary<string, int>();
        public string? GatedByBossId { get; set; } // MODIFIED: New field

        public string GetDescription(int currentLevel)
        {
            return string.Format(DescriptionTemplate, EffectValueFormula(currentLevel))
                + (
                    ItemType == ShopItemType.LeveledUpgrade && currentLevel >= MaxLevel ? " (Maxed)"
                    : ItemType == ShopItemType.OneTimePurchase && currentLevel > 0 ? " (Acquired)"
                    : ""
                );
        }

        public string GetNextLevelDescription(int currentLevel)
        {
            if (ItemType == ShopItemType.LeveledUpgrade && currentLevel >= MaxLevel)
                return "Max Level Reached";
            if (ItemType == ShopItemType.OneTimePurchase && currentLevel > 0)
                return "Already Acquired";
            return $"Next: {string.Format(DescriptionTemplate, EffectValueFormula(currentLevel + 1))}";
        }
    }
}

// =============================================================
// File: ProgressKnightFantasy/Game/Models/EnemyDefinition.cs
// =============================================================
namespace ProgressKnightFantasy.Game.Models
{
    public class EnemyDefinition
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty; // e.g., "Goblin Skirmisher", "Orc Berserker", "Dark Sorcerer"
        public string Description { get; set; } = string.Empty;
        public double MaxHealth { get; set; } = 10;
        public double Attack { get; set; } = 1;
        public double Defense { get; set; } = 0;
        public double PlayerXpReward { get; set; } = 5;
        public double CoinReward { get; set; } = 2;
        public int RequiredPlayerLevel { get; set; } = 0;
        public bool IsBoss { get; set; } = false; // MODIFIED: To identify bosses
        public string? GatedByBossId { get; set; } // MODIFIED: If this enemy only appears after another boss

        public string Faction { get; set; } = "Monsters"; // e.g., "Orc Clan", "Undead", "Wild Beasts"
    }
}

// =============================================================
// File: ProgressKnightFantasy/Game/Models/JobDefinition.cs
// =============================================================
namespace ProgressKnightFantasy.Game.Models
{
    using System.Collections.Generic;

    public class JobDefinition // Represents Professions or Callings
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty; // e.g., "Village Guard", "Adept Herbalist", "Battle Mage"
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "General"; // e.g., "Martial", "Arcane", "Crafting", "Gathering"
        public string ThemeColor { get; set; } = MudBlazor.Color.Default.ToString();
        public string ThemeKey { get; set; } = "default_dark";
        public double BaseIncomePerTick { get; set; } = 1; // "Income" can be coins, or perhaps other resources
        public double XpPerTick { get; set; } = 1; // Profession XP
        public double BaseMaxXp { get; set; } = 100;
        public double XpScalingFactor { get; set; } = 1.2;
        public double PlayerXpRewardPerTick { get; set; } = 1;
        public int MaxLevel { get; set; } = 10; // Max rank in this profession
        public bool IsUnlockedByDefault { get; set; } = false;
        public Dictionary<string, double> RewardsPerTick { get; set; } =
            new Dictionary<string, double>();
        public Dictionary<string, int> SkillRequirements { get; set; } =
            new Dictionary<string, int>();
        public Dictionary<string, int> TaskRequirements { get; set; } =
            new Dictionary<string, int>();
        public int PlayerLevelRequirement { get; set; } = 0;
        public string? IncomeSkillModifierId { get; set; }
        public double IncomeSkillEffectPerLevel { get; set; } = 0.01;
        public string? SpeedSkillModifierId { get; set; }
        public double SpeedSkillEffectPerLevel { get; set; } = 0.01;
        public List<string> Unlocks { get; set; } = new List<string>();
        public string? GatedByBossId { get; set; } // MODIFIED: New field
    }
}

// =============================================================
// File: ProgressKnightFantasy/Game/Models/MentorQuestDefinition.cs (NEW FILE)
// =============================================================
namespace ProgressKnightFantasy.Game.Models
{
    using System.Collections.Generic;

    public enum MentorQuestObjectiveType
    {
        ReachTaskLevel,
        ReachSkillLevel,
        ReachJobLevel,
        ReachPlayerLevel,
        EarnCoins,
        DefeatEnemyType,
        DefeatSpecificBoss,
        PerformRebirth,
        AcquireResource,
    }

    public class MentorQuestObjective
    {
        public MentorQuestObjectiveType Type { get; set; }
        public string TargetId { get; set; } = string.Empty; // TaskId, SkillId, JobId, EnemyId, ResourceId
        public double RequiredValue { get; set; } // Level, Amount, Count
        public string Description { get; set; } = string.Empty; // e.g., "Reach Level 3 in Basic Sword Drills"

        [System.Text.Json.Serialization.JsonIgnore] // Not serialized, tracked in GameData
        public bool IsCompleted { get; set; } = false;
    }

    public class MentorQuestDefinition
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string IntroductionText { get; set; } = string.Empty; // Flavor text from mentor
        public List<MentorQuestObjective> Objectives { get; set; } =
            new List<MentorQuestObjective>();
        public Dictionary<string, double> Rewards { get; set; } = new Dictionary<string, double>(); // e.g., {"coins": 50, "player_xp": 100, "resource:iron_ore": 5}
        public string? UnlocksNextMentorQuestId { get; set; }
        public List<string> UnlocksGameEntityIds { get; set; } = new List<string>(); // e.g. "task:new_task", "skill:new_skill"
        public int SortOrder { get; set; } = 0; // For displaying in order
    }
}

// =============================================================
// File: ProgressKnightFantasy/Game/Models/ChallengeDefinition.cs
// =============================================================
namespace ProgressKnightFantasy.Game.Models
{
    public enum ChallengeRewardType
    {
        AncientPowerMultiplier, // Multiplies Ancient Power gained on rebirth IF challenge is active
        UnlockPerk, // Unlocks a specific (possibly hidden) EvilPerk on first completion
        PermanentStatBoost, // e.g. Small permanent boost to base health or attack
    }

    public class ChallengeDefinition
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; // What the challenge entails
        public string RestrictionDescription { get; set; } = string.Empty; // What limitations are imposed
        public int MaxCompletions { get; set; } = 1; // How many times it can be completed for scaling rewards or just once

        public double ActiveRewardFactor { get; set; } = 1.0; // e.g., 1.2 means +20% Ancient Power if active

        public ChallengeRewardType FirstCompletionRewardType { get; set; }
        public string FirstCompletionRewardValue { get; set; } = string.Empty; // e.g. PerkID, or "PlayerBaseAttack:1"
        public string FirstCompletionRewardDescription { get; set; } = string.Empty;

        public bool IsUnlocked { get; set; } = true; // Some challenges might need unlocking
    }
}

// =============================================================
// File: ProgressKnightFantasy/Game/GameDataDefinitions.cs
// =============================================================
namespace ProgressKnightFantasy.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MudBlazor;
    using ProgressKnightFantasy.Game.Models;

    public static class GameDataDefinitions
    {
        public static readonly double MinPlayerLevelForRebirth = 10;
        public static readonly int MaxOfflineTicksCap = 8 * 60 * 60; // Max 8 hours of offline ticks (1 tick per sec)

        public static readonly List<TaskDefinition> Tasks = new List<TaskDefinition>
        {
            new TaskDefinition
            {
                Id = "basic_sword_drills",
                Name = "Basic Sword Drills",
                Description = "Practice basic stances and swings with a wooden sword.",
                XpGainPerTick = 1.8,
                PlayerXpRewardPerTick = 0.75,
                BaseMaxXp = 70,
                XpScalingFactor = 1.10,
                Category = "Combat Training",
                ThemeColor = Color.Error.ToString(), // MODIFIED: Increased PlayerXpRewardPerTick
                IsUnlockedByDefault = true,
                RewardsPerLevel = new Dictionary<string, double> { { "coins", 10 } },
                Unlocks = new List<string>
                {
                    "task:patrol_village_outskirts",
                    "skill:swordsmanship",
                    "job:village_aspirant",
                },
            },
            new TaskDefinition
            {
                Id = "study_ancient_runes",
                Name = "Study Ancient Runes",
                Description = "Decipher forgotten symbols of power from weathered tablets.",
                XpGainPerTick = 1.5,
                PlayerXpRewardPerTick = 0.85,
                BaseMaxXp = 100,
                XpScalingFactor = 1.15,
                Category = "Arcane Studies",
                ThemeColor = Color.Primary.ToString(), // MODIFIED: Increased PlayerXpRewardPerTick
                IsUnlockedByDefault = true,
                RewardsPerLevel = new Dictionary<string, double>
                {
                    { "skill:arcane_lore_xp_direct", 0.25 },
                },
                Unlocks = new List<string> { "skill:spellcraft", "job:apprentice_scribe" },
            },
            new TaskDefinition
            {
                Id = "forage_for_herbs",
                Name = "Forage for Herbs",
                Description = "Search the nearby woods and fields for valuable medicinal plants.",
                XpGainPerTick = 1.2,
                PlayerXpRewardPerTick = 0.5,
                BaseMaxXp = 60,
                XpScalingFactor = 1.07,
                Category = "Wilderness Survival",
                ThemeColor = Color.Success.ToString(), // MODIFIED: Increased PlayerXpRewardPerTick
                IsUnlockedByDefault = true,
                RewardsPerLevel = new Dictionary<string, double> { { "coins", 6 } },
                Unlocks = new List<string> { "skill:herbalism", "task:brew_minor_potion" },
            },
            new TaskDefinition
            {
                Id = "patrol_village_outskirts",
                Name = "Patrol Village Outskirts",
                Description = "Keep an eye out for wild beasts or goblin scouts near the village.",
                XpGainPerTick = 1.3,
                PlayerXpRewardPerTick = 0.6,
                BaseMaxXp = 90,
                XpScalingFactor = 1.12,
                Category = "Guard Duty",
                ThemeColor = Color.Warning.ToString(), // MODIFIED
                Requirements = new Dictionary<string, int> { { "task:basic_sword_drills", 2 } },
                Unlocks = new List<string> { "job:village_guard" },
            },
            new TaskDefinition
            {
                Id = "brew_minor_potion",
                Name = "Brew Minor Healing Draught",
                Description = "Attempt to create a simple potion to mend minor wounds.",
                XpGainPerTick = 0.9,
                PlayerXpRewardPerTick = 0.3,
                BaseMaxXp = 150,
                XpScalingFactor = 1.20,
                Category = "Alchemy",
                ThemeColor = Color.Info.ToString(), // MODIFIED
                Requirements = new Dictionary<string, int> { { "skill:herbalism", 1 } },
                Unlocks = new List<string> { "skill:alchemy" },
            },
            new TaskDefinition
            {
                Id = "mine_surface_ore",
                Name = "Mine Surface Ore",
                Description = "Chip away at exposed ore veins near the foothills.",
                XpGainPerTick = 1.0,
                PlayerXpRewardPerTick = 0.2,
                BaseMaxXp = 100,
                XpScalingFactor = 1.1,
                Category = "Mining",
                ThemeColor = Color.Default.ToString(),
                Requirements = new Dictionary<string, int> { { "task:basic_sword_drills", 2 } }, // Example requirement
                RewardsPerLevel = new Dictionary<string, double> { { "resource:iron_ore", 1.0 } },
                Unlocks = new List<string> { "job:novice_miner" }, // Example unlock
                GatedByBossId = "forest_guardian", // MODIFIED: Gated by boss
            },
        };
        public static readonly List<SkillDefinition> Skills = new List<SkillDefinition>
        {
            new SkillDefinition
            {
                Id = "swordsmanship",
                Name = "Swordsmanship",
                Description = "Proficiency with blades.",
                Category = "Combat",
                ThemeColor = Colors.Red.Default,
                ThemeKey = "warrior_theme",
                EffectDescription = "+1 Attack per 2 levels.",
                DetailedEffectTooltip =
                    "Each level increases your base attack power, making your strikes more potent.",
                MaxLevel = 50,
                XpCostForNextLevelFormula = lvl => 50 * Math.Pow(1.20, lvl),
            }, // Reduced cost
            new SkillDefinition
            {
                Id = "arcane_lore",
                Name = "Arcane Lore",
                Description = "Knowledge of magical principles.",
                Category = "Magic",
                ThemeColor = Colors.Blue.Default,
                ThemeKey = "mage_theme",
                EffectDescription = "Unlocks magical tasks & professions.",
                MaxLevel = 30,
                XpCostForNextLevelFormula = lvl => 120 * Math.Pow(1.3, lvl),
            },
            new SkillDefinition
            {
                Id = "herbalism",
                Name = "Herbalism",
                Description = "Identifying and using medicinal plants.",
                Category = "Crafting",
                ThemeColor = Colors.Green.Default,
                ThemeKey = "druid_theme",
                EffectDescription = "Improves potion effects or gathering yields.",
                MaxLevel = 25,
                XpCostForNextLevelFormula = lvl => 70 * Math.Pow(1.22, lvl),
            },
            new SkillDefinition
            {
                Id = "spellcraft",
                Name = "Spellcraft",
                Description = "The art of weaving magic.",
                Category = "Magic",
                ThemeColor = Colors.Purple.Default,
                ThemeKey = "warlock_theme",
                EffectDescription = "Increases spell effectiveness.",
                MaxLevel = 40,
                XpCostForNextLevelFormula = lvl => 180 * Math.Pow(1.38, lvl),
            },
            new SkillDefinition
            {
                Id = "fortitude",
                Name = "Fortitude",
                Description = "Resilience and physical toughness.",
                Category = "Physical",
                IsUnlockedByDefault = true,
                ThemeColor = Colors.Brown.Default,
                ThemeKey = "default_dark",
                EffectDescription = "+5 Max HP & +0.2 Defense per level.",
                DetailedEffectTooltip =
                    "Increases your maximum health and ability to withstand blows.",
                MaxLevel = 100,
                XpCostForNextLevelFormula = lvl => 30 * Math.Pow(1.15, lvl),
            }, // Reduced cost
            new SkillDefinition
            {
                Id = "alchemy",
                Name = "Alchemy",
                Description = "Transmuting substances and brewing concoctions.",
                Category = "Crafting",
                ThemeColor = Colors.LightGreen.Accent3,
                ThemeKey = "monk_theme",
                EffectDescription = "Allows crafting of powerful items.",
                MaxLevel = 30,
                XpCostForNextLevelFormula = lvl => 110 * Math.Pow(1.28, lvl),
            },
            new SkillDefinition
            {
                Id = "orc_slaying_tactics",
                Name = "Orc Slaying Tactics",
                Description = "Specialized techniques for fighting orcs.",
                Category = "Combat",
                ThemeColor = Colors.Orange.Darken2,
                ThemeKey = "hunter_theme",
                EffectDescription = "+10% damage against Orcs per level.",
                MaxLevel = 10,
                XpCostForNextLevelFormula = lvl => 220 * Math.Pow(1.45, lvl),
            },
            new SkillDefinition
            {
                Id = "diligence",
                Name = "Diligence",
                Description = "A strong work ethic.",
                Category = "Work Ethic",
                IsUnlockedByDefault = true,
                ThemeColor = Colors.Pink.Lighten2,
                ThemeKey = "paladin_theme",
                EffectDescription = "+1% Profession XP gain per level.",
                MaxLevel = 50,
                XpCostForNextLevelFormula = lvl => 40 * Math.Pow(1.20, lvl),
            }, // MODIFIED: Reduced initial cost
        };
        public static readonly List<EvilPerkDefinition> EvilPerks = new List<EvilPerkDefinition> // "Echoes of Power"
        {
            new EvilPerkDefinition
            {
                Id = "echo_valor",
                Name = "Echo of Valor",
                DescriptionTemplate =
                    "All combat skills are +{0}% more effective in their direct stat contributions.",
                MaxLevel = 20,
                CostFormula = level => 1 * Math.Pow(1.5, level),
                EffectValueFormula = level => level * 2.5,
                PerkType = "CombatSkillBoost",
            },
            new EvilPerkDefinition
            {
                Id = "echo_wisdom",
                Name = "Echo of Wisdom",
                DescriptionTemplate =
                    "All magic & crafting skills are +{0}% more effective in their primary benefits.",
                MaxLevel = 15,
                CostFormula = level => 2 * Math.Pow(1.8, level),
                EffectValueFormula = level => level * 2,
                PerkType = "NonCombatSkillBoost",
            },
            new EvilPerkDefinition
            {
                Id = "ancestral_memory",
                Name = "Ancestral Memory",
                DescriptionTemplate =
                    "Start with +{0} levels in all default unlocked skills after rebirth.",
                MaxLevel = 5,
                CostFormula = level => 5 * Math.Pow(2.5, level),
                EffectValueFormula = level => level * 1,
                PerkType = "StartingSkillLevels",
            },
            new EvilPerkDefinition
            {
                Id = "power_attunement",
                Name = "Power Attunement",
                DescriptionTemplate = "Gain +{0}% more 'Ancient Power' on Rebirth.",
                MaxLevel = 10,
                CostFormula = level => 10 * Math.Pow(2, level),
                EffectValueFormula = level => level * 5,
                PerkType = "EvilGainBoost",
            },
            new EvilPerkDefinition
            { // Perk unlocked by challenge
                Id = "frugal_mind_perk",
                Name = "Frugal Mind",
                DescriptionTemplate =
                    "All Silver Piece costs for Market Wares are reduced by {0}%. (Max 25%)",
                MaxLevel = 5,
                CostFormula = level => 50 * Math.Pow(3, level),
                EffectValueFormula = level => level * 5,
                PerkType = "CostReduction",
                IsUnlockedByDefault = false,
            },
        };
        public static readonly List<ShopItemDefinition> ShopItems = new List<ShopItemDefinition>
        {
            new ShopItemDefinition
            {
                Id = "sharpening_stone",
                Name = "Dwarven Sharpening Stone",
                DescriptionTemplate = "Increases melee damage by +{0} flat points.",
                ItemType = ShopItemType.LeveledUpgrade,
                MaxLevel = 10,
                CostFormula = level => 50 * Math.Pow(1.7, level),
                EffectValueFormula = level => level * 1,
                EffectTarget = "PlayerAttackFlatBonus",
                Requirements = new Dictionary<string, int> { { "playerlevel", 2 } },
            },
            new ShopItemDefinition
            {
                Id = "mana_crystal_shard",
                Name = "Mana Crystal Shard",
                DescriptionTemplate = "Increases spell power by +{0}%.",
                ItemType = ShopItemType.LeveledUpgrade,
                MaxLevel = 8,
                CostFormula = level => 75 * Math.Pow(1.9, level),
                EffectValueFormula = level => level * 1.5,
                EffectTarget = "SpellPowerMultiplier",
                Requirements = new Dictionary<string, int> { { "skill:arcane_lore", 1 } },
            },
            new ShopItemDefinition
            {
                Id = "sturdy_leather_gloves",
                Name = "Sturdy Leather Gloves",
                DescriptionTemplate = "Increases income from Gathering professions by +{0}%.",
                ItemType = ShopItemType.LeveledUpgrade,
                MaxLevel = 5,
                CostFormula = level => 200 * Math.Pow(2, level),
                EffectValueFormula = level => level * 4,
                EffectTarget = "JobIncomeMultiplier_Category_Gathering",
                Requirements = new Dictionary<string, int> { { "joblevel:village_aspirant", 2 } },
            },
            new ShopItemDefinition
            {
                Id = "scroll_of_lesser_warding",
                Name = "Scroll of Lesser Warding",
                DescriptionTemplate = "Grants +{0} temporary Defense for your current Age.",
                ItemType = ShopItemType.OneTimePurchase,
                MaxLevel = 1,
                CostFormula = level => 300,
                EffectValueFormula = level => level * 5,
                EffectTarget = "PlayerDefenseFlatBonus_Temporary",
                Requirements = new Dictionary<string, int> { { "playerlevel", 3 } },
            },
            new ShopItemDefinition
            {
                Id = "work_uniform",
                Name = "Sturdy Work Tunic",
                DescriptionTemplate = "Increases income from all professions by +{0}%.",
                ItemType = ShopItemType.LeveledUpgrade,
                MaxLevel = 10,
                CostFormula = level => 300 * Math.Pow(1.8, level),
                EffectValueFormula = level => level * 3,
                EffectTarget = "JobIncomeMultiplier_Global",
                Requirements = new Dictionary<string, int> { { "playerlevel", 5 } },
            },
            new ShopItemDefinition
            {
                Id = "efficiency_manual",
                Name = "Tome of Efficient Labor",
                DescriptionTemplate = "Increases XP gain from all professions by +{0}%.",
                ItemType = ShopItemType.LeveledUpgrade,
                MaxLevel = 8,
                CostFormula = level => 400 * Math.Pow(1.9, level),
                EffectValueFormula = level => level * 2.5,
                EffectTarget = "JobXpMultiplier_Global",
                Requirements = new Dictionary<string, int> { { "joblevel:village_aspirant", 2 } },
            },
        };
        public static readonly List<EnemyDefinition> Enemies = new List<EnemyDefinition>
        {
            new EnemyDefinition
            {
                Id = "giant_spider",
                Name = "Giant Spider",
                MaxHealth = 30,
                Attack = 4,
                Defense = 2,
                PlayerXpReward = 12,
                CoinReward = 6,
                RequiredPlayerLevel = 1,
                Faction = "Wild Beasts",
                Description =
                    "A monstrous arachnid from the dark woods, its fangs drip with venom.",
            },
            new EnemyDefinition
            {
                Id = "goblin_raider",
                Name = "Goblin Raider",
                MaxHealth = 45,
                Attack = 7,
                Defense = 3,
                PlayerXpReward = 20,
                CoinReward = 10,
                RequiredPlayerLevel = 2,
                Faction = "Goblinoids",
                Description =
                    "Vicious and cowardly, these small humanoids are dangerous in groups, armed with crude weapons.",
            },
            new EnemyDefinition
            {
                Id = "orc_grunt",
                Name = "Orc Grunt",
                MaxHealth = 80,
                Attack = 12,
                Defense = 5,
                PlayerXpReward = 45,
                CoinReward = 25,
                RequiredPlayerLevel = 5,
                Faction = "Orc Clans",
                Description = "A brutish warrior of the green-skin hordes, wielding a notched axe.",
            },
            new EnemyDefinition
            {
                Id = "skeletal_warrior",
                Name = "Skeletal Warrior",
                MaxHealth = 60,
                Attack = 10,
                Defense = 8,
                PlayerXpReward = 35,
                CoinReward = 18,
                RequiredPlayerLevel = 4,
                Faction = "Undead",
                Description =
                    "Animated bones of a long-dead soldier, driven by dark magic and clattering with every move.",
            },
            new EnemyDefinition
            {
                Id = "forest_guardian_boss",
                Name = "Forest Guardian (BOSS)",
                MaxHealth = 500,
                Attack = 25,
                Defense = 10,
                PlayerXpReward = 250,
                CoinReward = 200,
                RequiredPlayerLevel = 10,
                Faction = "Ancient Protectors",
                IsBoss = true,
                Description =
                    "An ancient, moss-covered sentinel of the deep woods. Its eyes glow with an eerie light.",
            },
            new EnemyDefinition
            {
                Id = "orc_chieftain",
                Name = "Orc Chieftain (BOSS)",
                MaxHealth = 1000,
                Attack = 40,
                Defense = 15,
                PlayerXpReward = 500,
                CoinReward = 400,
                RequiredPlayerLevel = 15,
                Faction = "Orc Clans",
                IsBoss = true,
                GatedByBossId = "forest_guardian_boss",
                Description = "A hulking Orc leader, scarred and brutal.",
            },
        };
        public static readonly List<JobDefinition> Jobs = new List<JobDefinition>
        {
            new JobDefinition
            {
                Id = "village_aspirant",
                Name = "Village Aspirant",
                Description = "Help around the village with various menial tasks.",
                Category = "General Labor",
                ThemeColor = Colors.BlueGray.Default,
                ThemeKey = "default_dark",
                BaseIncomePerTick = 0.25,
                XpPerTick = 0.6,
                BaseMaxXp = 50,
                XpScalingFactor = 1.2,
                MaxLevel = 5,
                IsUnlockedByDefault = false,
                PlayerLevelRequirement = 1,
                TaskRequirements = new Dictionary<string, int>
                {
                    { "task:basic_sword_drills", 1 },
                    { "task:forage_for_herbs", 1 },
                },
                SpeedSkillModifierId = "diligence",
                SpeedSkillEffectPerLevel = 0.015,
                Unlocks = new List<string> { "job:village_guard", "job:apprentice_herbalist" },
            },
            new JobDefinition
            {
                Id = "apprentice_scribe",
                Name = "Apprentice Scribe",
                Description = "Copy scrolls and learn ancient languages.",
                Category = "Scholarly",
                ThemeColor = Colors.Blue.Lighten2,
                ThemeKey = "mage_theme",
                BaseIncomePerTick = 0.15,
                XpPerTick = 0.8,
                BaseMaxXp = 90,
                XpScalingFactor = 1.28,
                MaxLevel = 7,
                PlayerLevelRequirement = 2,
                SkillRequirements = new Dictionary<string, int> { { "arcane_lore", 1 } },
                IncomeSkillModifierId = "arcane_lore",
                IncomeSkillEffectPerLevel = 0.02,
                SpeedSkillModifierId = "diligence",
                SpeedSkillEffectPerLevel = 0.01,
            },
            new JobDefinition
            {
                Id = "village_guard",
                Name = "Village Guard",
                Description = "Protect the village from minor threats.",
                Category = "Martial",
                ThemeColor = Colors.Red.Lighten1,
                ThemeKey = "warrior_theme",
                BaseIncomePerTick = 0.4,
                XpPerTick = 0.7,
                BaseMaxXp = 130,
                XpScalingFactor = 1.32,
                MaxLevel = 8,
                PlayerLevelRequirement = 4,
                SkillRequirements = new Dictionary<string, int>
                {
                    { "swordsmanship", 2 },
                    { "fortitude", 1 },
                },
                IncomeSkillModifierId = "swordsmanship",
                IncomeSkillEffectPerLevel = 0.01,
                Unlocks = new List<string> { "skill:orc_slaying_tactics" },
            },
            new JobDefinition
            {
                Id = "apprentice_herbalist",
                Name = "Apprentice Herbalist",
                Description = "Learn to identify and prepare potent herbs.",
                Category = "Gathering",
                ThemeColor = Colors.Green.Lighten1,
                ThemeKey = "druid_theme",
                BaseIncomePerTick = 0.25,
                XpPerTick = 0.7,
                BaseMaxXp = 80,
                XpScalingFactor = 1.18,
                MaxLevel = 6,
                PlayerLevelRequirement = 3,
                SkillRequirements = new Dictionary<string, int> { { "herbalism", 2 } },
            },
            new JobDefinition
            {
                Id = "master_blacksmith",
                Name = "Master Blacksmith",
                Description = "Forge legendary weapons and armor.",
                Category = "Crafting",
                ThemeColor = Colors.Orange.Darken3,
                ThemeKey = "warrior_theme",
                RewardsPerTick = new Dictionary<string, double>
                {
                    { "coins", 2.5 },
                    { "resource:iron_ore", -0.5 },
                }, // Consumes ore
                XpPerTick = 1.5,
                BaseMaxXp = 500,
                XpScalingFactor = 1.5,
                MaxLevel = 10,
                PlayerLevelRequirement = 12,
                SkillRequirements = new Dictionary<string, int>
                {
                    { "fortitude", 10 }, /* Placeholder for a smithing skill */
                },
                GatedByBossId = "forest_guardian_boss", // MODIFIED: Gated
            },
        };
        public static readonly List<ChallengeDefinition> Challenges = new List<ChallengeDefinition>
        {
            new ChallengeDefinition
            {
                Id = "no_skills_challenge",
                Name = "Trial of Raw Talent",
                Description = "Complete a rebirth without leveling any Talents (skills).",
                RestrictionDescription = "All Talents are disabled and cannot be leveled.",
                ActiveRewardFactor = 1.5, // +50% Ancient Power if completed while active
                FirstCompletionRewardType = ChallengeRewardType.PermanentStatBoost,
                FirstCompletionRewardValue = "PlayerBaseAttack:1", // +1 permanent base attack
                FirstCompletionRewardDescription = "Permanently gain +1 to your base Attack.",
                MaxCompletions = 1,
            },
            new ChallengeDefinition
            {
                Id = "poverty_challenge",
                Name = "Vow of Poverty",
                Description = "Reach Adventurer Level 20 without your Silver Pieces exceeding 100.",
                RestrictionDescription =
                    "If Silver Pieces exceed 100, the challenge fails for this run.",
                ActiveRewardFactor = 1.3,
                FirstCompletionRewardType = ChallengeRewardType.UnlockPerk,
                FirstCompletionRewardValue = "frugal_mind_perk", // ID of a new EvilPerk
                FirstCompletionRewardDescription =
                    "Unlocks the 'Frugal Mind' Ancient Power Perk (reduces Market costs by 5% per rank).",
                MaxCompletions = 1,
            },
            new ChallengeDefinition
            {
                Id = "speed_run_challenge",
                Name = "Race Against Time",
                Description = "Rebirth within 1 hour of game time in the current Age.",
                RestrictionDescription = "No specific gameplay restrictions, just a time limit.",
                ActiveRewardFactor = 2.0, // Double Ancient Power
                FirstCompletionRewardType = ChallengeRewardType.AncientPowerMultiplier,
                FirstCompletionRewardValue = "0.1", // +10% permanent bonus to Ancient Power gain
                FirstCompletionRewardDescription =
                    "Permanently gain +10% more Ancient Power from all rebirths.",
                MaxCompletions = 5,
            },
        };

        public static readonly List<MentorQuestDefinition> MentorQuests =
            new List<MentorQuestDefinition>
            {
                new MentorQuestDefinition
                {
                    Id = "mq001_first_steps",
                    Title = "First Steps",
                    SortOrder = 1,
                    IntroductionText =
                        "Welcome, young one. Your journey begins now. Let's see if you have what it takes. Start by honing your basic combat skills.",
                    Objectives = new List<MentorQuestObjective>
                    {
                        new MentorQuestObjective
                        {
                            Type = MentorQuestObjectiveType.ReachTaskLevel,
                            TargetId = "basic_sword_drills",
                            RequiredValue = 2,
                            Description = "Practice Basic Sword Drills until Level 2.",
                        },
                    },
                    Rewards = new Dictionary<string, double>
                    {
                        { "coins", 50 },
                        { "player_xp", 100 },
                    },
                    UnlocksNextMentorQuestId = "mq002_first_profession",
                    UnlocksGameEntityIds = new List<string> { "skill:fortitude" }, // Example unlock
                },
                new MentorQuestDefinition
                {
                    Id = "mq002_first_profession",
                    Title = "A Modest Living",
                    SortOrder = 2,
                    IntroductionText =
                        "Good. Now, every adventurer needs a way to earn their keep. Seek out the Village Aspirant profession and learn its ways.",
                    Objectives = new List<MentorQuestObjective>
                    {
                        new MentorQuestObjective
                        {
                            Type = MentorQuestObjectiveType.ReachJobLevel,
                            TargetId = "village_aspirant",
                            RequiredValue = 1,
                            Description = "Become a Village Aspirant (Rank 1).",
                        },
                    },
                    Rewards = new Dictionary<string, double>
                    {
                        { "coins", 100 },
                        { "player_xp", 150 },
                        { "resource:lumber", 5 },
                    },
                    UnlocksNextMentorQuestId = "mq003_first_talent",
                },
                new MentorQuestDefinition
                {
                    Id = "mq003_first_talent",
                    Title = "Awakening Potential",
                    SortOrder = 3,
                    IntroductionText =
                        "You've earned some Adventurer XP. It's time to awaken your innate Talents. Focus on your Fortitude.",
                    Objectives = new List<MentorQuestObjective>
                    {
                        new MentorQuestObjective
                        {
                            Type = MentorQuestObjectiveType.ReachSkillLevel,
                            TargetId = "fortitude",
                            RequiredValue = 2,
                            Description = "Increase your Fortitude Talent to Level 2.",
                        },
                    },
                    Rewards = new Dictionary<string, double>
                    {
                        { "coins", 75 },
                        { "player_xp", 200 },
                    },
                    UnlocksNextMentorQuestId = "mq004_forest_guardian_prep",
                },
                new MentorQuestDefinition
                {
                    Id = "mq004_forest_guardian_prep",
                    Title = "The Forest's Challenge",
                    SortOrder = 4,
                    IntroductionText =
                        "The ancient Forest Guardian blocks the path to further renown. Prepare yourself and then face it on the Battlegrounds.",
                    Objectives = new List<MentorQuestObjective>
                    {
                        new MentorQuestObjective
                        {
                            Type = MentorQuestObjectiveType.ReachPlayerLevel,
                            TargetId = "Player",
                            RequiredValue = 5,
                            Description = "Reach Adventurer Level 5.",
                        },
                        new MentorQuestObjective
                        {
                            Type = MentorQuestObjectiveType.DefeatSpecificBoss,
                            TargetId = "forest_guardian_boss",
                            RequiredValue = 1,
                            Description = "Vanquish the Forest Guardian.",
                        },
                    },
                    Rewards = new Dictionary<string, double>
                    {
                        { "coins", 250 },
                        { "player_xp", 500 },
                        { "resource:ancient_relics", 1 },
                    },
                    UnlocksGameEntityIds = new List<string> { "task:mine_surface_ore" }, // Unlocks gated content
                },
                // More quests can be added here
            };

        public static TaskDefinition? GetTaskById(string id) =>
            Tasks.FirstOrDefault(t => t.Id == id);

        public static SkillDefinition? GetSkillById(string id) =>
            Skills.FirstOrDefault(s => s.Id == id);

        public static EvilPerkDefinition? GetEvilPerkById(string id) =>
            EvilPerks.FirstOrDefault(p => p.Id == id);

        public static ShopItemDefinition? GetShopItemById(string id) =>
            ShopItems.FirstOrDefault(item => item.Id == id);

        public static EnemyDefinition? GetEnemyById(string id) =>
            Enemies.FirstOrDefault(e => e.Id == id);

        public static JobDefinition? GetJobById(string id) => Jobs.FirstOrDefault(j => j.Id == id);

        public static ChallengeDefinition? GetChallengeById(string id) =>
            Challenges.FirstOrDefault(c => c.Id == id);

        public static MentorQuestDefinition? GetMentorQuestById(string id) =>
            MentorQuests.FirstOrDefault(q => q.Id == id);

        public static double CalculatePlayerXpForNextLevel(double currentLevel) =>
            100 * Math.Pow(1.25, currentLevel);
    }
}

// =============================================================
// File: ProgressKnightFantasy/Services/ThemeService.cs
// =============================================================
namespace ProgressKnightFantasy.Services
{
    using System;
    using System.Collections.Generic;
    using MudBlazor;
    using static MudBlazor.CategoryTypes;

    public class ThemeService
    {
        public MudTheme CurrentTheme { get; private set; }
        public event Action? OnThemeChanged;

        // Make _availableThemes public for MainLayout to access for simple toggle
        public readonly Dictionary<string, MudTheme> AvailableThemes =
            new Dictionary<string, MudTheme>();

        public ThemeService()
        {
            AvailableThemes["default_dark"] = new MudTheme()
            {
                PaletteDark = new PaletteDark()
                {
                    Primary = Colors.BlueGray.Lighten1,
                    Secondary = Colors.Brown.Lighten2,
                    Background = "#1E1E1E", // Darker Background
                    AppbarBackground = Colors.Gray.Darken4,
                    DrawerBackground = "#242424", // Darker Drawer
                    Surface = Colors.Gray.Darken3, // Darker Surface
                    TextPrimary = Colors.Shades.White,
                    TextSecondary = Colors.Gray.Lighten2,
                    ActionDefault = Colors.Gray.Lighten3,
                },
                Typography = GetFantasyTypography(),
                LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "6px" },
            };
            AvailableThemes["default_light"] = new MudTheme()
            {
                PaletteLight = new PaletteLight()
                {
                    Primary = Colors.Brown.Default,
                    Secondary = Colors.Orange.Darken2,
                    Background = "#C8AD7F", // Darker Parchment/Tan
                    AppbarBackground = Colors.Brown.Darken1, // Darker Appbar
                    DrawerBackground = "#E0C9A6", // Darker Drawer
                    Surface = "#EAE0C8", // Darker Surface
                    TextPrimary = Colors.Brown.Darken4,
                    TextSecondary = Colors.Brown.Darken2,
                    ActionDefault = Colors.Brown.Darken1,
                },
                Typography = GetFantasyTypography(),
                LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "6px" },
            };
            AvailableThemes["warrior_theme"] = new MudTheme()
            {
                PaletteDark = new PaletteDark()
                {
                    Primary = "#C69B6D",
                    Secondary = Colors.Red.Accent4,
                    AppbarBackground = "#3B2E25",
                    Surface = "#4A3B31",
                },
                Typography = GetFantasyTypography(),
                LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "6px" },
            };
            AvailableThemes["mage_theme"] = new MudTheme()
            {
                PaletteDark = new PaletteDark()
                {
                    Primary = "#3FC7EB",
                    Secondary = Colors.Purple.Accent2,
                    AppbarBackground = "#1F2B38",
                    Surface = "#2C3E50",
                },
                Typography = GetFantasyTypography(),
                LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "6px" },
            };
            AvailableThemes["druid_theme"] = new MudTheme()
            {
                PaletteDark = new PaletteDark()
                {
                    Primary = "#FF7C0A",
                    Secondary = Colors.Green.Darken2,
                    AppbarBackground = "#3B2720",
                    Surface = "#4A3128",
                },
                Typography = GetFantasyTypography(),
                LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "6px" },
            };
            AvailableThemes["warlock_theme"] = new MudTheme()
            {
                PaletteDark = new PaletteDark()
                {
                    Primary = "#8788EE",
                    Secondary = Colors.Green.Accent4,
                    AppbarBackground = "#2E1F3C",
                    Surface = "#3D2C50",
                },
                Typography = GetFantasyTypography(),
                LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "6px" },
            };
            AvailableThemes["hunter_theme"] = new MudTheme()
            {
                PaletteDark = new PaletteDark()
                {
                    Primary = "#AAD372",
                    Secondary = Colors.Brown.Default,
                    AppbarBackground = "#273B27",
                    Surface = "#314A31",
                },
                Typography = GetFantasyTypography(),
                LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "6px" },
            };
            AvailableThemes["paladin_theme"] = new MudTheme()
            {
                PaletteLight = new PaletteLight()
                {
                    Primary = "#F48CBA",
                    Secondary = Colors.Yellow.Accent4,
                    AppbarBackground = "#EABFD0",
                    Surface = "#FAD0E0",
                },
                Typography = GetFantasyTypography(),
                LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "6px" },
            };
            AvailableThemes["monk_theme"] = new MudTheme()
            {
                PaletteDark = new PaletteDark()
                {
                    Primary = "#00FF98",
                    Secondary = Colors.Brown.Lighten1,
                    AppbarBackground = "#003C2D",
                    Surface = "#004D3B",
                },
                Typography = GetFantasyTypography(),
                LayoutProperties = new LayoutProperties() { DefaultBorderRadius = "6px" },
            };
            CurrentTheme = AvailableThemes["default_dark"];
        }

        private static Typography GetFantasyTypography()
        {
            return new Typography()
            {
                Default = new DefaultTypography()
                {
                    FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "0.875rem",
                },
                H1 = new H1Typography()
                {
                    FontFamily = new[] { "MedievalSharp", "cursive" },
                    FontSize = "3rem",
                    FontWeight = "400",
                },
                H2 = new H2Typography()
                {
                    FontFamily = new[] { "MedievalSharp", "cursive" },
                    FontSize = "2.5rem",
                    FontWeight = "400",
                },
                H3 = new H3Typography()
                {
                    FontFamily = new[] { "MedievalSharp", "cursive" },
                    FontSize = "2rem",
                    FontWeight = "400",
                },
                H4 = new H4Typography()
                {
                    FontFamily = new[] { "MedievalSharp", "cursive" },
                    FontSize = "1.75rem",
                    FontWeight = "400",
                },
                H5 = new H5Typography()
                {
                    FontFamily = new[] { "MedievalSharp", "cursive" },
                    FontSize = "1.35rem",
                    FontWeight = "700",
                }, // Bolder H5
                H6 = new H6Typography()
                {
                    FontFamily = new[] { "MedievalSharp", "cursive" },
                    FontSize = "1.15rem",
                    FontWeight = "700",
                }, // Bolder H6
                Subtitle1 = new Subtitle1Typography()
                {
                    FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "0.95rem",
                },
                Subtitle2 = new Subtitle2Typography()
                {
                    FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "0.85rem",
                },
                Body1 = new Body1Typography()
                {
                    FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "0.875rem",
                },
                Body2 = new Body2Typography()
                {
                    FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "0.8rem",
                },
                Button = new ButtonTypography()
                {
                    FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                    FontWeight = "500",
                    FontSize = "0.8rem",
                    TextTransform = "none",
                }, // Normal case buttons
                Caption = new CaptionTypography()
                {
                    FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "0.75rem",
                },
                Overline = new OverlineTypography()
                {
                    FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                },
            };
        }

        public void SetTheme(string themeKey)
        {
            if (AvailableThemes.TryGetValue(themeKey, out var newTheme))
            {
                CurrentTheme = newTheme;
                OnThemeChanged?.Invoke();
            }
            else if (AvailableThemes.TryGetValue("default_dark", out var defaultTheme))
            {
                CurrentTheme = defaultTheme;
                OnThemeChanged?.Invoke();
            }
        }
    }
}

// =============================================================
// File: ProgressKnightFantasy/Services/GameStateService.cs
// =============================================================
namespace ProgressKnightFantasy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Timers;
    using Microsoft.JSInterop;
    using ProgressKnightFantasy.Game;
    using ProgressKnightFantasy.Game.Models;

    public class GameStateService : IDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ThemeService _themeService;
        public GameData CurrentGameData { get; private set; }
        public event Action? OnChange;
        private System.Timers.Timer? _gameLoopTimer;
        private System.Timers.Timer? _autoSaveTimer;
        private List<string> _combatLog = new List<string>();
        private double globalJobIncomeMultiplier;
        private double categoryJobIncomeMultiplier;

        public List<string> OfflineProgressReport { get; private set; } = new List<string>();

        public List<TaskDefinition> AllTasks => GameDataDefinitions.Tasks;
        public List<SkillDefinition> AllSkills => GameDataDefinitions.Skills;
        public List<EvilPerkDefinition> AllEvilPerks =>
            GameDataDefinitions
                .EvilPerks.Where(p =>
                    p.IsUnlockedByDefault
                    || (CurrentGameData?.EvilPerks.ContainsKey(p.Id) ?? false)
                    || IsPerkUnlockedByChallenge(p.Id)
                )
                .ToList();
        public List<ShopItemDefinition> AllShopItems => GameDataDefinitions.ShopItems;
        public List<EnemyDefinition> AllEnemies => GameDataDefinitions.Enemies;
        public List<JobDefinition> AllJobs => GameDataDefinitions.Jobs;
        public List<ChallengeDefinition> AllChallenges => GameDataDefinitions.Challenges;
        public List<MentorQuestDefinition> AllMentorQuests => GameDataDefinitions.MentorQuests;

        public IReadOnlyList<string> CombatLog => _combatLog.AsReadOnly();

        public GameStateService(IJSRuntime jsRuntime, ThemeService themeService)
        {
            _jsRuntime = jsRuntime;
            _themeService = themeService;
            CurrentGameData = new GameData();
            // InitializeNewGameData(CurrentGameData); // This will be called by LoadGameAsync if no save exists or after loading

            Task.Run(async () =>
            {
                await LoadGameAsync();
                _themeService.SetTheme(CurrentGameData.CurrentThemeKey ?? "default_dark");
                InitializeGameLoop();
                InitializeAutoSaveTimer();
            });
        }

        public bool IsPerkUnlockedByChallenge(string perkId)
        {
            // Check if any completed challenge unlocks this perk
            return GameDataDefinitions.Challenges.Any(c =>
                c.FirstCompletionRewardType == ChallengeRewardType.UnlockPerk
                && c.FirstCompletionRewardValue == perkId
                && (CurrentGameData?.CompletedChallenges.ContainsKey(c.Id) ?? false)
                && CurrentGameData.CompletedChallenges[c.Id] > 0
            );
        }

        private void InitializeNewGameData(GameData gameData)
        {
            gameData.Days = 365 * 14;
            gameData.Coins = 50; // Slightly more starting coins for rebalanced start
            gameData.CurrentTaskName = null;
            gameData.CurrentJobId = null;
            gameData.IsWorking = false;
            gameData.Paused = gameData.Paused; // Preserve pause state

            gameData.TaskData.Clear();
            gameData.TaskLevels.Clear();
            gameData.SkillData.Clear();
            gameData.ShopItemLevels.Clear();
            gameData.JobData.Clear();
            gameData.JobLevels.Clear();
            gameData.UnlockedTaskIds.Clear();
            gameData.UnlockedSkillIds.Clear();
            gameData.UnlockedJobIds.Clear();
            gameData.Resources.Clear();

            gameData.CurrentEnemyId = null;
            gameData.CurrentEnemyHealth = 0;
            _combatLog.Clear();
            gameData.CurrentChallengeId = string.Empty;
            gameData.LastTickTime = DateTime.UtcNow;

            gameData.CurrentRun = new RebirthData
            {
                PlayerLevel = 0,
                PlayerXp = 0,
                TimeThisRun = 0,
                PlayerCurrentHealth = CalculatePlayerMaxHealth(), // Initialize with max health based on skills/perks
            };

            foreach (var taskDef in AllTasks)
            {
                gameData.TaskLevels[taskDef.Id] = 0;
                gameData.TaskData[taskDef.Id] = 0;
                if (taskDef.IsUnlockedByDefault)
                    gameData.UnlockedTaskIds.Add(taskDef.Id);
            }

            int startingSkillLevels = GetEvilPerkLevel("ancestral_memory");
            foreach (var skillDef in AllSkills)
            {
                gameData.SkillData[skillDef.Id] = skillDef.IsUnlockedByDefault
                    ? startingSkillLevels
                    : 0;
                if (skillDef.IsUnlockedByDefault)
                    gameData.UnlockedSkillIds.Add(skillDef.Id);
            }
            foreach (var jobDef in AllJobs)
            {
                gameData.JobLevels[jobDef.Id] = 0;
                gameData.JobData[jobDef.Id] = 0;
                if (jobDef.IsUnlockedByDefault)
                    gameData.UnlockedJobIds.Add(jobDef.Id);
            }
            foreach (var shopItemDef in AllShopItems)
            {
                gameData.ShopItemLevels[shopItemDef.Id] = 0;
            }

            // Mentor Quests
            gameData.CurrentMentorQuestId = null;
            gameData.CompletedMentorQuestIds.Clear();
            if (GameDataDefinitions.MentorQuests.Any())
            {
                gameData.CurrentMentorQuestId = GameDataDefinitions
                    .MentorQuests.OrderBy(q => q.SortOrder)
                    .First()
                    .Id;
            }
            // Boss Flags
            gameData.IsGatekeeperDefeated_ForestGuardian = false;

            gameData.CurrentThemeKey = "default_dark";
            gameData.CurrentRun.PlayerCurrentHealth = CalculatePlayerMaxHealth(); // Ensure health is maxed after all initializations
        }

        private void InitializeGameLoop()
        {
            _gameLoopTimer = new System.Timers.Timer(1000);
            _gameLoopTimer.Elapsed += GameTick;
            _gameLoopTimer.AutoReset = true;
            _gameLoopTimer.Enabled = true;
        }

        private void InitializeAutoSaveTimer()
        {
            if (_autoSaveTimer != null)
            {
                _autoSaveTimer.Stop();
                _autoSaveTimer.Dispose();
            }
            _autoSaveTimer = new System.Timers.Timer(
                CurrentGameData.Settings.AutoSaveIntervalSeconds * 1000
            );
            _autoSaveTimer.Elapsed += async (sender, e) => await AutoSaveGameAsync(sender, e);
            _autoSaveTimer.AutoReset = true;
            _autoSaveTimer.Enabled = CurrentGameData.Settings.AutoSaveIntervalSeconds > 0;
        }

        private async Task AutoSaveGameAsync(object? sender, ElapsedEventArgs e)
        {
            if (!CurrentGameData.Paused)
            {
                await SaveGameAsync();
                Console.WriteLine($"Game auto-saved at {DateTime.Now}");
            }
        }

        public async Task ProcessOfflineProgress()
        {
            OfflineProgressReport.Clear();
            if (
                CurrentGameData.Settings.PauseOffline
                || CurrentGameData.LastSaveTime == DateTime.MinValue
            )
            {
                CurrentGameData.LastTickTime = DateTime.UtcNow;
                NotifyStateChanged();
                return;
            }
            TimeSpan offlineDuration = DateTime.UtcNow - CurrentGameData.LastTickTime;
            if (offlineDuration.TotalSeconds <= 10)
            {
                CurrentGameData.LastTickTime = DateTime.UtcNow;
                NotifyStateChanged();
                return;
            }
            int offlineTicks = (int)
                Math.Min(offlineDuration.TotalSeconds, GameDataDefinitions.MaxOfflineTicksCap);
            OfflineProgressReport.Add(
                $"You were away for {FormatTimeSpan(offlineDuration)} (Simulated {FormatTimeSpan(TimeSpan.FromSeconds(offlineTicks))} of progress)."
            );
            double initialCoins = CurrentGameData.Coins;
            double initialPlayerXp = CurrentGameData.CurrentRun.PlayerXp;
            bool wasPaused = CurrentGameData.Paused;
            CurrentGameData.Paused = false;
            for (int i = 0; i < offlineTicks; i++)
            {
                CurrentGameData.Days += 1.0 / (24 * 60 * 60);
                CurrentGameData.CurrentRun.TimeThisRun += 1;
                CurrentGameData.TimePlayedTotal += 1;
                if (!string.IsNullOrEmpty(CurrentGameData.CurrentChallengeId))
                {
                    if (
                        CurrentGameData.CurrentChallengeId == "poverty_challenge"
                        && CurrentGameData.Coins > 100
                    )
                    {
                        OfflineProgressReport.Add(
                            "Vow of Poverty failed: coin limit exceeded offline."
                        );
                        break;
                    }
                    if (CurrentGameData.CurrentChallengeId == "no_skills_challenge")
                    { /* Skills are already 0 */
                    }
                }
                if (!string.IsNullOrEmpty(CurrentGameData.CurrentJobId))
                {
                    ProcessJobProgressionOffline();
                    ProcessPlayerXpGainOffline();
                }
                else if (!string.IsNullOrEmpty(CurrentGameData.CurrentTaskName))
                {
                    ProcessTaskProgressionOffline();
                    ProcessPlayerXpGainOffline();
                }
            }
            CurrentGameData.Paused = wasPaused;
            if (CurrentGameData.Coins > initialCoins)
            {
                double coinsGainedOffline = CurrentGameData.Coins - initialCoins;
                OfflineProgressReport.Add($"Gained {coinsGainedOffline:N0} Silver Pieces.");
                CurrentGameData.Stats.TotalCoinsEarned += (long)coinsGainedOffline;
            }
            if (CurrentGameData.CurrentRun.PlayerXp > initialPlayerXp)
                OfflineProgressReport.Add(
                    $"Gained {CurrentGameData.CurrentRun.PlayerXp - initialPlayerXp:N0} Adventurer XP."
                );
            CurrentGameData.LastTickTime = DateTime.UtcNow;
            NotifyStateChanged();
        }

        private void ProcessPlayerXpGainOffline()
        {
            if (CurrentGameData.IsWorking)
            {
                double playerXpPerTick = 0;
                if (!string.IsNullOrEmpty(CurrentGameData.CurrentTaskName))
                {
                    var taskDef = GetTaskDefinitionById(CurrentGameData.CurrentTaskName);
                    if (taskDef != null)
                        playerXpPerTick = taskDef.PlayerXpRewardPerTick;
                }
                // else if (!string.IsNullOrEmpty(CurrentGameData.CurrentJobId)) { /* If jobs give player XP */ }
                if (playerXpPerTick > 0)
                {
                    double evilPlayerXpBoost = 1.0;
                    double shopPlayerXpBoost = 1.0;
                    CurrentGameData.CurrentRun.PlayerXp +=
                        playerXpPerTick * evilPlayerXpBoost * shopPlayerXpBoost;
                }
            }
            double xpForNextPlayerLevel = GameDataDefinitions.CalculatePlayerXpForNextLevel(
                CurrentGameData.CurrentRun.PlayerLevel
            );
            while (CurrentGameData.CurrentRun.PlayerXp >= xpForNextPlayerLevel)
            {
                CurrentGameData.CurrentRun.PlayerXp -= xpForNextPlayerLevel;
                CurrentGameData.CurrentRun.PlayerLevel++;
                if (
                    CurrentGameData.CurrentRun.PlayerLevel
                    > CurrentGameData.Stats.MaxPlayerLevelAchieved
                )
                    CurrentGameData.Stats.MaxPlayerLevelAchieved = CurrentGameData
                        .CurrentRun
                        .PlayerLevel;
                xpForNextPlayerLevel = GameDataDefinitions.CalculatePlayerXpForNextLevel(
                    CurrentGameData.CurrentRun.PlayerLevel
                );
            }
        }

        private void ProcessTaskProgressionOffline()
        {
            if (string.IsNullOrEmpty(CurrentGameData.CurrentTaskName))
            {
                CurrentGameData.IsWorking = false;
                return;
            }
            CurrentGameData.IsWorking = true;
            var taskDef = GetTaskDefinitionById(CurrentGameData.CurrentTaskName);
            if (taskDef == null || !IsTaskActuallyAvailable(taskDef.Id))
            {
                CurrentGameData.IsWorking = false;
                CurrentGameData.CurrentTaskName = null;
                return;
            }
            double currentTaskXp = CurrentGameData.TaskData[taskDef.Id];
            double taskXpMultiplier = 1.0;
            currentTaskXp += taskDef.XpGainPerTick * taskXpMultiplier;
            int currentLevel = CurrentGameData.TaskLevels[taskDef.Id];
            double xpForNextLevel = CalculateXpForNextLevel(taskDef, currentLevel);
            while (currentTaskXp >= xpForNextLevel)
            {
                currentTaskXp -= xpForNextLevel;
                currentLevel++;
                CurrentGameData.TaskLevels[taskDef.Id] = currentLevel;
                CurrentGameData.Stats.TotalTasksCompleted++;
                xpForNextLevel = CalculateXpForNextLevel(taskDef, currentLevel);
            }
            CurrentGameData.TaskData[taskDef.Id] = currentTaskXp;
        }

        private void ProcessJobProgressionOffline()
        {
            if (string.IsNullOrEmpty(CurrentGameData.CurrentJobId))
            {
                CurrentGameData.IsWorking = false;
                return;
            }
            var jobDef = GetJobDefinitionById(CurrentGameData.CurrentJobId);
            if (jobDef == null)
            {
                CurrentGameData.IsWorking = false;
                CurrentGameData.CurrentJobId = null;
                return;
            }
            CurrentGameData.IsWorking = true;
            double incomeSkillModValue = 0;
            if (!string.IsNullOrEmpty(jobDef.IncomeSkillModifierId))
                incomeSkillModValue =
                    GetSkillLevel(jobDef.IncomeSkillModifierId) * jobDef.IncomeSkillEffectPerLevel;
            double speedSkillModValue = 0;
            if (!string.IsNullOrEmpty(jobDef.SpeedSkillModifierId))
                speedSkillModValue =
                    GetSkillLevel(jobDef.SpeedSkillModifierId) * jobDef.SpeedSkillEffectPerLevel;
            double globalJobIncomeMultiplier =
                1.0
                + (GetShopItemEffectValue("JobIncomeMultiplier_Global") / 100.0)
                + (GetEvilPerkEffectValue("JobIncomeGlobalBoost") / 100.0);
            double categoryJobIncomeMultiplier =
                1.0
                + (
                    GetShopItemEffectValue($"JobIncomeMultiplier_Category_{jobDef.Category}")
                    / 100.0
                );
            double globalJobXpMultiplier =
                1.0
                + (GetShopItemEffectValue("JobXpMultiplier_Global") / 100.0)
                + (GetEvilPerkEffectValue("JobXpGlobalBoost") / 100.0);

            foreach (var reward in jobDef.RewardsPerTick)
            {
                double baseAmount = reward.Value;
                double finalAmount = baseAmount;
                if (reward.Key == "coins")
                {
                    finalAmount =
                        baseAmount
                        * (1 + incomeSkillModValue)
                        * globalJobIncomeMultiplier
                        * categoryJobIncomeMultiplier;
                    CurrentGameData.Coins += finalAmount;
                }
                else if (reward.Key.StartsWith("resource:"))
                {
                    var resourceId = reward.Key.Substring(9);
                    if (!CurrentGameData.Resources.ContainsKey(resourceId))
                        CurrentGameData.Resources[resourceId] = 0;
                    CurrentGameData.Resources[resourceId] += finalAmount;
                }
            }

            double actualJobXpGain =
                jobDef.XpPerTick * (1 + speedSkillModValue) * globalJobXpMultiplier;
            CurrentGameData.JobData[jobDef.Id] += actualJobXpGain;
            int currentJobLevel = GetJobLevel(jobDef.Id);
            if (currentJobLevel < jobDef.MaxLevel)
            {
                double xpForNextJobLevel = CalculateJobXpForNextLevel(jobDef, currentJobLevel);
                while (
                    CurrentGameData.JobData[jobDef.Id] >= xpForNextJobLevel
                    && currentJobLevel < jobDef.MaxLevel
                )
                {
                    CurrentGameData.JobData[jobDef.Id] -= xpForNextJobLevel;
                    currentJobLevel++;
                    CurrentGameData.JobLevels[jobDef.Id] = currentJobLevel;
                    if (
                        currentJobLevel
                        > (
                            CurrentGameData.Stats.HighestJobLevelAchieved.TryGetValue(
                                jobDef.Id,
                                out var highLvl
                            )
                                ? highLvl
                                : 0
                        )
                    )
                        CurrentGameData.Stats.HighestJobLevelAchieved[jobDef.Id] = currentJobLevel;
                    if (currentJobLevel == jobDef.MaxLevel)
                        CurrentGameData.Stats.TotalProfessionsMastered++;
                    if (currentJobLevel < jobDef.MaxLevel)
                        xpForNextJobLevel = CalculateJobXpForNextLevel(jobDef, currentJobLevel);
                    else
                        CurrentGameData.JobData[jobDef.Id] = 0;
                }
            }
        }

        private void GameTick(object? sender, ElapsedEventArgs e)
        {
            CurrentGameData.LastTickTime = DateTime.UtcNow;
            if (CurrentGameData.Paused)
                return;
            CurrentGameData.Days += 1;
            CurrentGameData.TimePlayedTotal += 1;
            CurrentGameData.CurrentRun.TimeThisRun += 1;

            CheckMentorQuestCompletion();

            if (!string.IsNullOrEmpty(CurrentGameData.CurrentChallengeId))
            {
                if (
                    CurrentGameData.CurrentChallengeId == "poverty_challenge"
                    && CurrentGameData.Coins > 100
                )
                {
                    AddCombatLog("Vow of Poverty: Coin limit exceeded! Trial may fail.");
                }
                if (CurrentGameData.CurrentChallengeId == "no_skills_challenge")
                {
                    foreach (var skillId in CurrentGameData.SkillData.Keys.ToList())
                        CurrentGameData.SkillData[skillId] = 0;
                }
            }

            if (!string.IsNullOrEmpty(CurrentGameData.CurrentEnemyId))
            {
                ProcessCombatTurn();
            }
            else if (!string.IsNullOrEmpty(CurrentGameData.CurrentJobId))
            {
                ProcessJobProgression();
                ProcessPlayerXpGain();
            }
            else if (!string.IsNullOrEmpty(CurrentGameData.CurrentTaskName))
            {
                ProcessTaskProgression();
                ProcessPlayerXpGain();
            }
            else
            {
                CurrentGameData.IsWorking = false;
                if (CurrentGameData.CurrentRun.PlayerCurrentHealth < CalculatePlayerMaxHealth())
                {
                    CurrentGameData.CurrentRun.PlayerCurrentHealth = Math.Min(
                        CalculatePlayerMaxHealth(),
                        CurrentGameData.CurrentRun.PlayerCurrentHealth
                            + CalculatePlayerHealthRegen()
                    );
                }
            }

            if (
                CurrentGameData.Settings.AutoRebirth
                && CurrentGameData.CurrentRun.PlayerLevel
                    >= CurrentGameData.Settings.AutoRebirthLevel
                && CanRebirth()
            )
            {
                PerformRebirth();
            }
            NotifyStateChanged();
        }

        private void ProcessPlayerXpGain()
        {
            if (CurrentGameData.IsWorking)
            {
                double playerXpPerTick = 0;
                if (!string.IsNullOrEmpty(CurrentGameData.CurrentTaskName))
                {
                    var taskDef = GetTaskDefinitionById(CurrentGameData.CurrentTaskName);
                    if (taskDef != null)
                        playerXpPerTick = taskDef.PlayerXpRewardPerTick;
                }
                else if (!string.IsNullOrEmpty(CurrentGameData.CurrentJobId))
                {
                    var jobDef = GetJobDefinitionById(CurrentGameData.CurrentJobId);
                    if (jobDef != null)
                        playerXpPerTick = jobDef.PlayerXpRewardPerTick; // If jobs give player XP
                }

                if (playerXpPerTick > 0)
                {
                    double evilPlayerXpBoost = 1.0; // + (GetEvilPerkEffectValue("PlayerXpBoost") / 100.0);
                    double shopPlayerXpBoost = 1.0; // + (GetShopItemEffectValue("PlayerXpMultiplier") / 100.0);
                    CurrentGameData.CurrentRun.PlayerXp +=
                        playerXpPerTick * evilPlayerXpBoost * shopPlayerXpBoost;
                }
            }
            // Player leveling logic
            double xpForNextPlayerLevel = GameDataDefinitions.CalculatePlayerXpForNextLevel(
                CurrentGameData.CurrentRun.PlayerLevel
            );
            while (CurrentGameData.CurrentRun.PlayerXp >= xpForNextPlayerLevel)
            {
                CurrentGameData.CurrentRun.PlayerXp -= xpForNextPlayerLevel;
                CurrentGameData.CurrentRun.PlayerLevel++;
                if (
                    CurrentGameData.CurrentRun.PlayerLevel
                    > CurrentGameData.Stats.MaxPlayerLevelAchieved
                )
                {
                    CurrentGameData.Stats.MaxPlayerLevelAchieved = CurrentGameData
                        .CurrentRun
                        .PlayerLevel;
                }
                xpForNextPlayerLevel = GameDataDefinitions.CalculatePlayerXpForNextLevel(
                    CurrentGameData.CurrentRun.PlayerLevel
                );
            }
        }

        private void ProcessTaskProgression()
        {
            if (string.IsNullOrEmpty(CurrentGameData.CurrentTaskName))
            {
                CurrentGameData.IsWorking = false;
                return;
            }
            CurrentGameData.IsWorking = true;
            var taskDef = GetTaskDefinitionById(CurrentGameData.CurrentTaskName);
            if (taskDef == null || !IsTaskActuallyAvailable(taskDef.Id))
            {
                CurrentGameData.IsWorking = false;
                CurrentGameData.CurrentTaskName = null;
                return;
            }

            double currentTaskXp = CurrentGameData.TaskData[taskDef.Id];
            double taskXpMultiplier = 1.0; // + (GetShopItemEffectValue($"TaskXpMultiplier_Category_{taskDef.Category}") / 100.0);
            // + (GetEvilPerkEffectValue("TaskXpGlobalBoost") / 100.0);
            currentTaskXp += taskDef.XpGainPerTick * taskXpMultiplier;

            int currentLevel = CurrentGameData.TaskLevels[taskDef.Id];
            double xpForNextLevel = CalculateXpForNextLevel(taskDef, currentLevel);
            while (currentTaskXp >= xpForNextLevel)
            {
                currentTaskXp -= xpForNextLevel;
                currentLevel++;
                CurrentGameData.TaskLevels[taskDef.Id] = currentLevel;
                ApplyTaskRewards(taskDef);
                ProcessUnlocks(taskDef.Unlocks);
                xpForNextLevel = CalculateXpForNextLevel(taskDef, currentLevel);
            }
            CurrentGameData.TaskData[taskDef.Id] = currentTaskXp;
        }

        private void ProcessJobProgression()
        {
            if (string.IsNullOrEmpty(CurrentGameData.CurrentJobId))
            {
                CurrentGameData.IsWorking = false;
                return;
            }
            var jobDef = GetJobDefinitionById(CurrentGameData.CurrentJobId);
            if (jobDef == null)
            {
                CurrentGameData.IsWorking = false;
                CurrentGameData.CurrentJobId = null;
                return;
            }
            CurrentGameData.IsWorking = true;

            double incomeSkillModValue = 0;
            if (!string.IsNullOrEmpty(jobDef.IncomeSkillModifierId))
                incomeSkillModValue =
                    GetSkillLevel(jobDef.IncomeSkillModifierId) * jobDef.IncomeSkillEffectPerLevel;
            double speedSkillModValue = 0;
            if (!string.IsNullOrEmpty(jobDef.SpeedSkillModifierId))
                speedSkillModValue =
                    GetSkillLevel(jobDef.SpeedSkillModifierId) * jobDef.SpeedSkillEffectPerLevel;

            foreach (var reward in jobDef.RewardsPerTick)
            {
                double baseAmount = reward.Value;
                double finalAmount = baseAmount;

                if (reward.Key == "coins")
                {
                    double globalJobIncomeMultiplier =
                        1.0
                        + (GetShopItemEffectValue("JobIncomeMultiplier_Global") / 100.0)
                        + (GetEvilPerkEffectValue("JobIncomeGlobalBoost") / 100.0);
                    double categoryJobIncomeMultiplier =
                        1.0
                        + (
                            GetShopItemEffectValue(
                                $"JobIncomeMultiplier_Category_{jobDef.Category}"
                            ) / 100.0
                        );
                    finalAmount =
                        baseAmount
                        * (1 + incomeSkillModValue)
                        * globalJobIncomeMultiplier
                        * categoryJobIncomeMultiplier;
                    CurrentGameData.Coins += finalAmount;
                    CurrentGameData.Stats.TotalCoinsEarned += (long)finalAmount;
                }
                else if (reward.Key.StartsWith("resource:"))
                {
                    var resourceId = reward.Key.Substring(9);
                    if (!CurrentGameData.Resources.ContainsKey(resourceId))
                        CurrentGameData.Resources[resourceId] = 0;
                    CurrentGameData.Resources[resourceId] += finalAmount;
                }
            }

            double globalJobXpMultiplier =
                1.0
                + (GetShopItemEffectValue("JobXpMultiplier_Global") / 100.0)
                + (GetEvilPerkEffectValue("JobXpGlobalBoost") / 100.0);
            double actualIncome =
                jobDef.BaseIncomePerTick
                * (1 + incomeSkillModValue)
                * globalJobIncomeMultiplier
                * categoryJobIncomeMultiplier;
            CurrentGameData.Coins += actualIncome;
            CurrentGameData.Stats.TotalCoinsEarned += (long)actualIncome;

            double actualJobXpGain =
                jobDef.XpPerTick * (1 + speedSkillModValue) * globalJobXpMultiplier;
            CurrentGameData.JobData[jobDef.Id] += actualJobXpGain;

            int currentJobLevel = GetJobLevel(jobDef.Id);
            if (currentJobLevel < jobDef.MaxLevel)
            {
                double xpForNextJobLevel = CalculateJobXpForNextLevel(jobDef, currentJobLevel);
                while (
                    CurrentGameData.JobData[jobDef.Id] >= xpForNextJobLevel
                    && currentJobLevel < jobDef.MaxLevel
                )
                {
                    CurrentGameData.JobData[jobDef.Id] -= xpForNextJobLevel;
                    currentJobLevel++;
                    CurrentGameData.JobLevels[jobDef.Id] = currentJobLevel;
                    if (
                        currentJobLevel
                        > (
                            CurrentGameData.Stats.HighestJobLevelAchieved.TryGetValue(
                                jobDef.Id,
                                out var highLvl
                            )
                                ? highLvl
                                : 0
                        )
                    )
                    {
                        CurrentGameData.Stats.HighestJobLevelAchieved[jobDef.Id] = currentJobLevel;
                    }
                    if (currentJobLevel == jobDef.MaxLevel)
                    {
                        CurrentGameData.Stats.TotalProfessionsMastered++;
                    }
                    ProcessUnlocks(jobDef.Unlocks);
                    if (currentJobLevel < jobDef.MaxLevel)
                        xpForNextJobLevel = CalculateJobXpForNextLevel(jobDef, currentJobLevel);
                    else
                        CurrentGameData.JobData[jobDef.Id] = 0;
                }
            }
        }

        public bool IsChallengeActive(string challengeId)
        {
            return CurrentGameData.CurrentChallengeId == challengeId;
        }

        public int GetChallengeCompletions(string challengeId)
        {
            return CurrentGameData.CompletedChallenges.TryGetValue(challengeId, out var count)
                ? count
                : 0;
        }

        public bool CanStartChallenge(string challengeId)
        {
            var challengeDef = GameDataDefinitions.GetChallengeById(challengeId);
            if (challengeDef == null || !challengeDef.IsUnlocked)
                return false;
            if (!string.IsNullOrEmpty(CurrentGameData.CurrentChallengeId))
                return false;
            if (
                GetChallengeCompletions(challengeId) >= challengeDef.MaxCompletions
                && challengeDef.MaxCompletions > 0
            )
                return false;
            return true;
        }

        public void StartChallenge(string challengeId)
        {
            if (!CanStartChallenge(challengeId))
                return;
            CurrentGameData.CurrentChallengeId = challengeId;
            if (challengeId == "no_skills_challenge")
            {
                foreach (var skillId in CurrentGameData.SkillData.Keys.ToList())
                    CurrentGameData.SkillData[skillId] = 0;
            }
            AddCombatLog($"Trial Begun: {GameDataDefinitions.GetChallengeById(challengeId)?.Name}");
            NotifyStateChanged();
        }

        public void AbandonChallenge()
        {
            if (!string.IsNullOrEmpty(CurrentGameData.CurrentChallengeId))
            {
                AddCombatLog(
                    $"Trial Abandoned: {GameDataDefinitions.GetChallengeById(CurrentGameData.CurrentChallengeId)?.Name}"
                );
                CurrentGameData.CurrentChallengeId = string.Empty;
                NotifyStateChanged();
            }
        }

        private void ApplyChallengeCompletionRewards(string challengeId)
        {
            var challengeDef = GameDataDefinitions.GetChallengeById(challengeId);
            if (challengeDef == null)
                return;
            int completions = GetChallengeCompletions(challengeId);
            if (completions == 0)
            {
                switch (challengeDef.FirstCompletionRewardType)
                {
                    case ChallengeRewardType.PermanentStatBoost:
                        var parts = challengeDef.FirstCompletionRewardValue.Split(':');
                        if (parts.Length == 2)
                        {
                            Console.WriteLine(
                                $"TODO: Permanent stat boost: {parts[0]} by {parts[1]}"
                            );
                        }
                        break;
                    case ChallengeRewardType.UnlockPerk:
                        var perkToUnlock = GameDataDefinitions.GetEvilPerkById(
                            challengeDef.FirstCompletionRewardValue
                        );
                        if (perkToUnlock != null)
                        {
                            Console.WriteLine($"Perk Unlocked: {perkToUnlock.Name}");
                            perkToUnlock.IsUnlockedByDefault = true;
                        }
                        break;
                    case ChallengeRewardType.AncientPowerMultiplier:
                        break;
                }
                AddCombatLog(
                    $"First completion reward for '{challengeDef.Name}' earned: {challengeDef.FirstCompletionRewardDescription}"
                );
            }
            CurrentGameData.CompletedChallenges[challengeId] = completions + 1;
        }

        public void PerformRebirth()
        {
            if (!CanRebirth())
                return;
            double evilGainedBase = CalculateEvilOnRebirth();
            double evilGainedFinal = evilGainedBase;
            string activeChallengeIdBeforeRebirth = CurrentGameData.CurrentChallengeId;
            if (!string.IsNullOrEmpty(activeChallengeIdBeforeRebirth))
            {
                var challengeDef = GameDataDefinitions.GetChallengeById(
                    activeChallengeIdBeforeRebirth
                );
                if (challengeDef != null)
                {
                    bool conditionsMet = true;
                    if (
                        activeChallengeIdBeforeRebirth == "poverty_challenge"
                        && CurrentGameData.Coins > 100
                    )
                        conditionsMet = false;
                    if (
                        activeChallengeIdBeforeRebirth == "speed_run_challenge"
                        && CurrentGameData.CurrentRun.TimeThisRun > 3600
                    )
                        conditionsMet = false;
                    if (conditionsMet)
                    {
                        evilGainedFinal *= challengeDef.ActiveRewardFactor;
                        ApplyChallengeCompletionRewards(activeChallengeIdBeforeRebirth);
                        AddCombatLog(
                            $"Trial '{challengeDef.Name}' successful! Ancient Power bonus applied."
                        );
                    }
                    else
                    {
                        AddCombatLog(
                            $"Trial '{challengeDef.Name}' conditions not met this Age. No active bonus."
                        );
                    }
                }
            }
            evilGainedFinal *= (1 + GetEvilPerkEffectValue("power_attunement") / 100.0);
            CurrentGameData.Evil += evilGainedFinal;
            CurrentGameData.Stats.TotalRebirths++;
            CurrentGameData.Stats.TotalEvilEarned += (long)evilGainedFinal;
            if (CurrentGameData.CurrentRun.TimeThisRun < CurrentGameData.Stats.FastestRebirthTime)
                CurrentGameData.Stats.FastestRebirthTime = CurrentGameData.CurrentRun.TimeThisRun;
            var preservedEvil = CurrentGameData.Evil;
            var preservedPerks = new Dictionary<string, int>(CurrentGameData.EvilPerks);
            var preservedStats = CurrentGameData.Stats;
            var preservedSettings = CurrentGameData.Settings;
            var preservedCompletedChallenges = new Dictionary<string, int>(
                CurrentGameData.CompletedChallenges
            );
            var preservedTotalTime = CurrentGameData.TimePlayedTotal;
            var lastSave = CurrentGameData.LastSaveTime;
            CurrentGameData = new GameData
            {
                Evil = preservedEvil,
                EvilPerks = preservedPerks,
                Stats = preservedStats,
                Settings = preservedSettings,
                CompletedChallenges = preservedCompletedChallenges,
                TimePlayedTotal = preservedTotalTime,
                LastSaveTime = lastSave,
            };
            InitializeNewGameData(CurrentGameData);
            _themeService.SetTheme(CurrentGameData.CurrentThemeKey);
            AddCombatLog(
                $"You have been reborn, carrying {evilGainedFinal:N0} Ancient Power into your next life!"
            );
            NotifyStateChanged();
        }

        private void InitializeGameStateAfterLoad()
        {
            foreach (var taskDef in AllTasks)
            {
                if (!CurrentGameData.TaskLevels.ContainsKey(taskDef.Id))
                    CurrentGameData.TaskLevels[taskDef.Id] = 0;
                if (!CurrentGameData.TaskData.ContainsKey(taskDef.Id))
                    CurrentGameData.TaskData[taskDef.Id] = 0;
            }
            foreach (var skillDef in AllSkills)
            {
                if (!CurrentGameData.SkillData.ContainsKey(skillDef.Id))
                    CurrentGameData.SkillData[skillDef.Id] = 0;
            }
            foreach (var jobDef in AllJobs)
            {
                if (!CurrentGameData.JobLevels.ContainsKey(jobDef.Id))
                    CurrentGameData.JobLevels[jobDef.Id] = 0;
                if (!CurrentGameData.JobData.ContainsKey(jobDef.Id))
                    CurrentGameData.JobData[jobDef.Id] = 0;
            }
            foreach (var perkDef in GameDataDefinitions.EvilPerks)
            {
                if (!CurrentGameData.EvilPerks.ContainsKey(perkDef.Id))
                    CurrentGameData.EvilPerks[perkDef.Id] = 0;
            }
            foreach (var shopItemDef in AllShopItems)
            {
                if (!CurrentGameData.ShopItemLevels.ContainsKey(shopItemDef.Id))
                    CurrentGameData.ShopItemLevels[shopItemDef.Id] = 0;
            }
            if (CurrentGameData.CompletedChallenges == null)
                CurrentGameData.CompletedChallenges = new Dictionary<string, int>();
            if (string.IsNullOrEmpty(CurrentGameData.CurrentChallengeId))
                CurrentGameData.CurrentChallengeId = string.Empty;
            else
            {
                var challengeDef = GameDataDefinitions.GetChallengeById(
                    CurrentGameData.CurrentChallengeId
                );
                if (challengeDef == null || !challengeDef.IsUnlocked)
                    CurrentGameData.CurrentChallengeId = string.Empty;
            }
            if (CurrentGameData.Resources == null)
                CurrentGameData.Resources = new Dictionary<string, double>();
            if (CurrentGameData.CompletedMentorQuestIds == null)
                CurrentGameData.CompletedMentorQuestIds = new HashSet<string>();
            if (string.IsNullOrEmpty(CurrentGameData.CurrentMentorQuestId) && AllMentorQuests.Any())
            {
                CurrentGameData.CurrentMentorQuestId = AllMentorQuests
                    .OrderBy(q => q.SortOrder)
                    .FirstOrDefault(q => !CurrentGameData.CompletedMentorQuestIds.Contains(q.Id))
                    ?.Id;
            }

            if (CurrentGameData.CurrentRun == null)
                CurrentGameData.CurrentRun = new RebirthData();
            if (!string.IsNullOrEmpty(CurrentGameData.CurrentEnemyId))
            {
                var enemyDef = GameDataDefinitions.GetEnemyById(CurrentGameData.CurrentEnemyId);
                if (
                    enemyDef == null
                    || CurrentGameData.CurrentEnemyHealth <= 0
                    || CurrentGameData.CurrentEnemyHealth > enemyDef.MaxHealth
                )
                {
                    CurrentGameData.CurrentEnemyId = null;
                    CurrentGameData.CurrentEnemyHealth = 0;
                }
            }
            double maxHealth = CalculatePlayerMaxHealth();
            if (
                CurrentGameData.CurrentRun.PlayerCurrentHealth > maxHealth
                || (
                    CurrentGameData.CurrentRun.PlayerCurrentHealth <= 0
                    && string.IsNullOrEmpty(CurrentGameData.CurrentEnemyId)
                )
            )
            {
                CurrentGameData.CurrentRun.PlayerCurrentHealth = maxHealth;
            }
            else if (
                CurrentGameData.CurrentRun.PlayerCurrentHealth <= 0
                && !string.IsNullOrEmpty(CurrentGameData.CurrentEnemyId)
            )
            {
                CurrentGameData.CurrentRun.PlayerCurrentHealth = 0;
            }
            _combatLog.Clear();
            CurrentGameData.LastTickTime = DateTime.UtcNow;
            _themeService.SetTheme(CurrentGameData.CurrentThemeKey ?? "default_dark");
            InitializeAutoSaveTimer();
            Task.Run(async () => await ProcessOfflineProgress());
        }

        public bool CanLevelUpTalent(string skillId)
        {
            var skillDef = GetSkillDefinitionById(skillId);
            if (skillDef == null || !CurrentGameData.UnlockedSkillIds.Contains(skillId))
                return false;
            int currentLevel = (int)GetSkillLevel(skillId);
            if (currentLevel >= skillDef.MaxLevel)
                return false;
            double cost = skillDef.XpCostForNextLevelFormula(currentLevel);
            return CurrentGameData.CurrentRun.PlayerXp >= cost;
        }

        public void LevelUpTalent(string skillId)
        {
            if (!CanLevelUpTalent(skillId))
            {
                Console.WriteLine(
                    $"Cannot level up talent {skillId}. Conditions not met or already max level."
                );
                return;
            }
            var skillDef = GetSkillDefinitionById(skillId)!;
            int currentLevel = (int)GetSkillLevel(skillId);
            double cost = skillDef.XpCostForNextLevelFormula(currentLevel);
            Console.WriteLine(
                $"Attempting to level up Talent '{skillDef.Name}'. Current XP: {CurrentGameData.CurrentRun.PlayerXp}, Cost: {cost}"
            );
            CurrentGameData.CurrentRun.PlayerXp -= cost;
            CurrentGameData.SkillData[skillId] = currentLevel + 1;
            AddCombatLog($"Talent '{skillDef.Name}' advanced to Level {currentLevel + 1}!");
            Console.WriteLine(
                $"Talent '{skillDef.Name}' leveled up to {currentLevel + 1}. Remaining XP: {CurrentGameData.CurrentRun.PlayerXp}"
            );
            NotifyStateChanged();
        }

        public MentorQuestDefinition? GetCurrentMentorQuest()
        {
            if (string.IsNullOrEmpty(CurrentGameData.CurrentMentorQuestId))
                return null;
            return GameDataDefinitions.GetMentorQuestById(CurrentGameData.CurrentMentorQuestId);
        }

        private void CheckMentorQuestCompletion()
        {
            var currentQuest = GetCurrentMentorQuest();
            if (currentQuest == null)
                return;
            bool allObjectivesMet = true;
            foreach (var objective in currentQuest.Objectives)
            {
                bool objectiveMetThisTick = false;
                switch (objective.Type)
                {
                    case MentorQuestObjectiveType.ReachTaskLevel:
                        if (GetTaskLevel(objective.TargetId) >= objective.RequiredValue)
                            objectiveMetThisTick = true;
                        break;
                    case MentorQuestObjectiveType.ReachSkillLevel:
                        if (GetSkillLevel(objective.TargetId) >= objective.RequiredValue)
                            objectiveMetThisTick = true;
                        break;
                    case MentorQuestObjectiveType.ReachJobLevel:
                        if (GetJobLevel(objective.TargetId) >= objective.RequiredValue)
                            objectiveMetThisTick = true;
                        break;
                    case MentorQuestObjectiveType.EarnCoins:
                        if (CurrentGameData.Coins >= objective.RequiredValue)
                            objectiveMetThisTick = true;
                        break;
                    case MentorQuestObjectiveType.DefeatSpecificBoss:
                        if (
                            objective.TargetId == "forest_guardian_boss"
                            && CurrentGameData.IsGatekeeperDefeated_ForestGuardian
                        )
                            objectiveMetThisTick = true;
                        break;
                    case MentorQuestObjectiveType.AcquireResource:
                        if (
                            CurrentGameData.Resources.TryGetValue(objective.TargetId, out var rVal)
                            && rVal >= objective.RequiredValue
                        )
                            objectiveMetThisTick = true;
                        break;
                }
                if (!objectiveMetThisTick)
                {
                    allObjectivesMet = false;
                    break;
                }
            }
            if (allObjectivesMet)
            {
                CompleteMentorQuest(currentQuest);
            }
        }

        private void CompleteMentorQuest(MentorQuestDefinition quest)
        {
            AddCombatLog($"Mentor Quest Completed: {quest.Title}!");
            CurrentGameData.CompletedMentorQuestIds.Add(quest.Id);
            foreach (var reward in quest.Rewards)
            {
                if (reward.Key == "coins")
                {
                    CurrentGameData.Coins += reward.Value;
                    CurrentGameData.Stats.TotalCoinsEarned += (long)reward.Value;
                    AddCombatLog($"Received {reward.Value} Silver Pieces.");
                }
                else if (reward.Key == "player_xp")
                {
                    CurrentGameData.CurrentRun.PlayerXp += reward.Value;
                    AddCombatLog($"Received {reward.Value} Adventurer XP.");
                }
                else if (reward.Key.StartsWith("resource:"))
                {
                    var resourceId = reward.Key.Substring(9);
                    if (!CurrentGameData.Resources.ContainsKey(resourceId))
                        CurrentGameData.Resources[resourceId] = 0;
                    CurrentGameData.Resources[resourceId] += reward.Value;
                    AddCombatLog($"Received {reward.Value} {resourceId.Replace("_", " ")}.");
                }
            }
            ProcessUnlocks(quest.UnlocksGameEntityIds);
            if (!string.IsNullOrEmpty(quest.UnlocksNextMentorQuestId))
            {
                CurrentGameData.CurrentMentorQuestId = quest.UnlocksNextMentorQuestId;
                var nextQuest = GetCurrentMentorQuest();
                if (nextQuest != null)
                    AddCombatLog($"New Mentor Quest: {nextQuest.Title}");
            }
            else
            {
                CurrentGameData.CurrentMentorQuestId = null;
                AddCombatLog("You have completed all available mentor quests for now!");
            }
            NotifyStateChanged();
        }

        public string FormatTimeSpan(TimeSpan ts)
        {
            if (ts.TotalDays >= 1)
                return $"{ts.TotalDays:N0} days, {ts.Hours}h";
            if (ts.TotalHours >= 1)
                return $"{ts.TotalHours:N0} hours, {ts.Minutes}m";
            if (ts.TotalMinutes >= 1)
                return $"{ts.TotalMinutes:N0} minutes, {ts.Seconds}s";
            return $"{ts.TotalSeconds:N0} seconds";
        }

        public void TogglePause()
        {
            CurrentGameData.Paused = !CurrentGameData.Paused;
            NotifyStateChanged();
        }

        public async Task SaveGameAsync()
        {
            CurrentGameData.LastSaveTime = DateTime.UtcNow;
            CurrentGameData.LastTickTime = DateTime.UtcNow;
            var jsonData = JsonSerializer.Serialize(
                CurrentGameData,
                new JsonSerializerOptions { WriteIndented = true }
            );
            await _jsRuntime.InvokeVoidAsync(
                "localStorageInterop.saveGame",
                "progressKnightFantasySave",
                jsonData
            );
            NotifyStateChanged();
        }

        public async Task LoadGameAsync()
        {
            try
            {
                var jsonData = await _jsRuntime.InvokeAsync<string>(
                    "localStorageInterop.loadGame",
                    "progressKnightFantasySave"
                );
                if (!string.IsNullOrEmpty(jsonData))
                {
                    var loadedData = JsonSerializer.Deserialize<GameData>(jsonData);
                    if (loadedData != null)
                    {
                        CurrentGameData = loadedData;
                        InitializeGameStateAfterLoad();
                    }
                }
                else
                {
                    CurrentGameData = new GameData();
                    InitializeNewGameData(CurrentGameData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game: {ex.Message}. Starting new game.");
                CurrentGameData = new GameData();
                InitializeNewGameData(CurrentGameData);
            }
            NotifyStateChanged();
        }

        public async Task DeleteSaveAsync()
        {
            await _jsRuntime.InvokeVoidAsync(
                "localStorageInterop.deleteSave",
                "progressKnightFantasySave"
            );
            CurrentGameData = new GameData();
            InitializeNewGameData(CurrentGameData);
            NotifyStateChanged();
        }

        public void NotifyStateChanged() => OnChange?.Invoke();

        public void Dispose()
        {
            _gameLoopTimer?.Stop();
            _gameLoopTimer?.Dispose();
            _autoSaveTimer?.Stop();
            _autoSaveTimer?.Dispose();
        }

        public bool CanRebirth() =>
            CurrentGameData.CurrentRun.PlayerLevel >= GameDataDefinitions.MinPlayerLevelForRebirth;

        public double CalculateEvilOnRebirth()
        {
            if (!CanRebirth())
                return 0;
            double levelFactor = Math.Pow(CurrentGameData.CurrentRun.PlayerLevel, 1.5) / 10.0;
            double timeFactor = (CurrentGameData.CurrentRun.TimeThisRun / 60.0) / 5.0;
            return Math.Floor(levelFactor + timeFactor);
        }

        public int GetEvilPerkLevel(string perkId) =>
            CurrentGameData.EvilPerks.TryGetValue(perkId, out var level) ? level : 0;

        public double GetEvilPerkEffectValue(string perkIdOrType)
        {
            var perkDef = GameDataDefinitions.GetEvilPerkById(perkIdOrType);
            if (perkDef == null)
                return 0;
            return perkDef.EffectValueFormula(GetEvilPerkLevel(perkDef.Id));
        }

        public bool CanBuyEvilPerk(string perkId)
        {
            var perkDef = GameDataDefinitions.GetEvilPerkById(perkId);
            if (
                perkDef == null
                || (!perkDef.IsUnlockedByDefault && !IsPerkUnlockedByChallenge(perkId))
            )
                return false;
            int currentLevel = GetEvilPerkLevel(perkId);
            if (currentLevel >= perkDef.MaxLevel)
                return false;
            return CurrentGameData.Evil >= perkDef.CostFormula(currentLevel);
        }

        public void BuyEvilPerk(string perkId)
        {
            if (!CanBuyEvilPerk(perkId))
                return;
            var perkDef = GameDataDefinitions.GetEvilPerkById(perkId)!;
            int currentLevel = GetEvilPerkLevel(perkId);
            CurrentGameData.Evil -= perkDef.CostFormula(currentLevel);
            CurrentGameData.EvilPerks[perkId] = currentLevel + 1;
            NotifyStateChanged();
        }

        public int GetShopItemLevel(string itemId) =>
            CurrentGameData.ShopItemLevels.TryGetValue(itemId, out var level) ? level : 0;

        public double GetShopItemEffectValue(string effectTarget)
        {
            double totalEffect = 0;
            foreach (var itemDef in AllShopItems)
            {
                if (itemDef.EffectTarget == effectTarget)
                {
                    int itemLevel = GetShopItemLevel(itemDef.Id);
                    if (itemLevel > 0)
                        totalEffect += itemDef.EffectValueFormula(itemLevel);
                }
            }
            return totalEffect;
        }

        public bool CanBuyShopItem(string itemId)
        {
            var itemDef = GameDataDefinitions.GetShopItemById(itemId);
            if (itemDef == null)
                return false;
            int currentLevel = GetShopItemLevel(itemId);
            if (
                (
                    itemDef.ItemType == ShopItemType.LeveledUpgrade
                    && currentLevel >= itemDef.MaxLevel
                ) || (itemDef.ItemType == ShopItemType.OneTimePurchase && currentLevel > 0)
            )
                return false;
            if (CurrentGameData.Coins < itemDef.CostFormula(currentLevel))
                return false;
            foreach (var req in itemDef.Requirements)
            {
                if (req.Key == "playerlevel" && CurrentGameData.CurrentRun.PlayerLevel < req.Value)
                    return false;
                else if (
                    req.Key.StartsWith("task:")
                    && GetTaskLevel(req.Key.Substring(5)) < req.Value
                )
                    return false;
                else if (
                    req.Key.StartsWith("skill:")
                    && GetSkillLevel(req.Key.Substring(6)) < req.Value
                )
                    return false;
                else if (
                    req.Key.StartsWith("joblevel:")
                    && GetJobLevel(req.Key.Substring(9)) < req.Value
                )
                    return false;
            }
            return true;
        }

        public void BuyShopItem(string itemId)
        {
            if (!CanBuyShopItem(itemId))
                return;
            var itemDef = GameDataDefinitions.GetShopItemById(itemId)!;
            int currentLevel = GetShopItemLevel(itemId);
            CurrentGameData.Coins -= itemDef.CostFormula(currentLevel);
            CurrentGameData.ShopItemLevels[itemId] = currentLevel + 1;
            NotifyStateChanged();
        }

        public string GetShopItemRequirementsTooltip(string itemId)
        {
            var itemDef = GameDataDefinitions.GetShopItemById(itemId);
            if (itemDef == null || !itemDef.Requirements.Any())
                return string.Empty;
            var reqTexts = new List<string>();
            foreach (var req in itemDef.Requirements)
            {
                string name = req.Key;
                double currentVal = 0;
                if (req.Key == "playerlevel")
                {
                    name = "Adventurer Level";
                    currentVal = CurrentGameData.CurrentRun.PlayerLevel;
                }
                else if (req.Key.StartsWith("task:"))
                {
                    var tId = req.Key.Substring(5);
                    name = GetTaskDefinitionById(tId)?.Name ?? tId;
                    currentVal = GetTaskLevel(tId);
                }
                else if (req.Key.StartsWith("skill:"))
                {
                    var sId = req.Key.Substring(6);
                    name = GetSkillDefinitionById(sId)?.Name ?? sId;
                    currentVal = GetSkillLevel(sId);
                }
                else if (req.Key.StartsWith("joblevel:"))
                {
                    var jId = req.Key.Substring(9);
                    name = GetJobDefinitionById(jId)?.Name ?? jId;
                    currentVal = GetJobLevel(jId);
                }
                reqTexts.Add($"{name} {req.Value} (Cur: {currentVal:N0})");
            }
            return "Requires: " + string.Join(", ", reqTexts);
        }

        public double CalculateXpForNextLevel(TaskDefinition taskDef, int currentLevel) =>
            taskDef.BaseMaxXp * Math.Pow(taskDef.XpScalingFactor, currentLevel);

        public bool IsTaskActuallyAvailable(string taskId)
        {
            var taskDef = GetTaskDefinitionById(taskId);
            if (taskDef == null || !CurrentGameData.UnlockedTaskIds.Contains(taskId))
                return false;
            if (!string.IsNullOrEmpty(taskDef.GatedByBossId))
            {
                if (
                    taskDef.GatedByBossId == "forest_guardian_boss"
                    && !CurrentGameData.IsGatekeeperDefeated_ForestGuardian
                )
                    return false;
            }
            foreach (var req in taskDef.Requirements)
            {
                if (req.Key.StartsWith("task:") && GetTaskLevel(req.Key.Substring(5)) < req.Value)
                    return false;
                else if (
                    req.Key.StartsWith("skill:")
                    && GetSkillLevel(req.Key.Substring(6)) < req.Value
                )
                    return false;
            }
            return true;
        }

        public string GetTaskRequirementsTooltip(string taskId)
        {
            var taskDef = GetTaskDefinitionById(taskId);
            if (taskDef == null || !taskDef.Requirements.Any())
                return string.Empty;
            var reqTexts = taskDef.Requirements.Select(req =>
            {
                string name = req.Key;
                double current = 0;
                if (req.Key.StartsWith("task:"))
                {
                    name = GetTaskDefinitionById(req.Key.Substring(5))?.Name ?? name;
                    current = GetTaskLevel(req.Key.Substring(5));
                }
                else if (req.Key.StartsWith("skill:"))
                {
                    name = GetSkillDefinitionById(req.Key.Substring(6))?.Name ?? name;
                    current = GetSkillLevel(req.Key.Substring(6));
                }
                return $"{name} Lvl {req.Value} (Cur: {current:N0})";
            });
            return "Requires: " + string.Join(", ", reqTexts);
        }

        public void SetCurrentTask(string? taskId)
        {
            if (taskId != null && !IsTaskActuallyAvailable(taskId))
                return;
            if (!string.IsNullOrEmpty(CurrentGameData.CurrentEnemyId))
                return;
            CurrentGameData.CurrentTaskName = taskId;
            CurrentGameData.CurrentJobId = null;
            CurrentGameData.IsWorking = !string.IsNullOrEmpty(taskId);
            NotifyStateChanged();
        }

        public int GetTaskLevel(string taskId) =>
            CurrentGameData.TaskLevels.TryGetValue(taskId, out var level) ? level : 0;

        public double GetTaskCurrentXp(string taskId) =>
            CurrentGameData.TaskData.TryGetValue(taskId, out var xp) ? xp : 0;

        public TaskDefinition? GetTaskDefinitionById(string taskId) =>
            AllTasks.FirstOrDefault(t => t.Id == taskId);

        public SkillDefinition? GetSkillDefinitionById(string skillId) =>
            AllSkills.FirstOrDefault(s => s.Id == skillId);

        public double GetTaskProgressPercent(string taskId)
        {
            var taskDef = GetTaskDefinitionById(taskId);
            if (taskDef == null)
                return 0;
            int currentLevel = GetTaskLevel(taskId);
            double currentXp = GetTaskCurrentXp(taskId);
            double xpForNext = CalculateXpForNextLevel(taskDef, currentLevel);
            return xpForNext > 0
                ? Math.Min(100, (currentXp / xpForNext) * 100)
                : (currentXp > 0 ? 100 : 0);
        }

        public double GetSkillLevel(string skillId) =>
            CurrentGameData.SkillData.TryGetValue(skillId, out var level) ? level : 0;

        public void IncreaseSkillLevel(string skillId, double amount = 1)
        {
            if (!CurrentGameData.SkillData.ContainsKey(skillId))
                CurrentGameData.SkillData[skillId] = 0;
            CurrentGameData.SkillData[skillId] += amount;
            NotifyStateChanged();
        }

        private void ApplyTaskRewards(TaskDefinition taskDef)
        {
            double coinGainMultiplier =
                1.0
                + (GetEvilPerkEffectValue("CoinGainBoost") / 100.0)
                + (GetShopItemEffectValue("TaskCoinGainMultiplier") / 100.0);
            foreach (var reward in taskDef.RewardsPerLevel)
            {
                if (reward.Key == "coins")
                {
                    double coinsGained = reward.Value * coinGainMultiplier;
                    CurrentGameData.Coins += coinsGained;
                    CurrentGameData.Stats.TotalCoinsEarned += (long)coinsGained;
                }
                else if (reward.Key.StartsWith("skill:") && reward.Key.EndsWith("_xp_direct"))
                {
                    var skillId = reward.Key.Replace("skill:", "").Replace("_xp_direct", "");
                    IncreaseSkillLevel(skillId, reward.Value);
                }
                else if (reward.Key.StartsWith("resource:"))
                {
                    var resourceId = reward.Key.Substring(9);
                    if (!CurrentGameData.Resources.ContainsKey(resourceId))
                        CurrentGameData.Resources[resourceId] = 0;
                    CurrentGameData.Resources[resourceId] += reward.Value;
                }
            }
            CurrentGameData.Stats.TotalTasksCompleted++;
        }

        public JobDefinition? GetJobDefinitionById(string jobId) =>
            AllJobs.FirstOrDefault(j => j.Id == jobId);

        public int GetJobLevel(string jobId) =>
            CurrentGameData.JobLevels.TryGetValue(jobId, out var level) ? level : 0;

        public double GetJobCurrentXp(string jobId) =>
            CurrentGameData.JobData.TryGetValue(jobId, out var xp) ? xp : 0;

        public double CalculateJobXpForNextLevel(JobDefinition jobDef, int currentLevel) =>
            jobDef.BaseMaxXp * Math.Pow(jobDef.XpScalingFactor, currentLevel);

        public double GetJobProgressPercent(string jobId)
        {
            var jobDef = GetJobDefinitionById(jobId);
            if (jobDef == null)
                return 0;
            int currentLevel = GetJobLevel(jobId);
            if (currentLevel >= jobDef.MaxLevel)
                return 100;
            double currentXp = GetJobCurrentXp(jobId);
            double xpForNext = CalculateJobXpForNextLevel(jobDef, currentLevel);
            return xpForNext > 0
                ? Math.Min(100, (currentXp / xpForNext) * 100)
                : (currentXp > 0 ? 100 : 0);
        }

        public bool IsJobActuallyAvailable(string jobId)
        {
            var jobDef = GetJobDefinitionById(jobId);
            if (jobDef == null || !CurrentGameData.UnlockedJobIds.Contains(jobId))
                return false;
            if (!string.IsNullOrEmpty(jobDef.GatedByBossId))
            {
                if (
                    jobDef.GatedByBossId == "forest_guardian_boss"
                    && !CurrentGameData.IsGatekeeperDefeated_ForestGuardian
                )
                    return false;
            }
            if (CurrentGameData.CurrentRun.PlayerLevel < jobDef.PlayerLevelRequirement)
                return false;
            foreach (var req in jobDef.SkillRequirements)
            {
                if (GetSkillLevel(req.Key) < req.Value)
                    return false;
            }
            foreach (var req in jobDef.TaskRequirements)
            {
                if (GetTaskLevel(req.Key) < req.Value)
                    return false;
            }
            return true;
        }

        public string GetJobRequirementsTooltip(string jobId)
        {
            var jobDef = GetJobDefinitionById(jobId);
            if (jobDef == null)
                return string.Empty;
            var reqTexts = new List<string>();
            if (jobDef.PlayerLevelRequirement > 0)
                reqTexts.Add(
                    $"Adventurer Lvl {jobDef.PlayerLevelRequirement} (Cur: {CurrentGameData.CurrentRun.PlayerLevel:N0})"
                );
            foreach (var req in jobDef.SkillRequirements)
            {
                reqTexts.Add(
                    $"Talent: {GetSkillDefinitionById(req.Key)?.Name ?? req.Key} Lvl {req.Value} (Cur: {GetSkillLevel(req.Key):N0})"
                );
            }
            foreach (var req in jobDef.TaskRequirements)
            {
                reqTexts.Add(
                    $"Activity: {GetTaskDefinitionById(req.Key)?.Name ?? req.Key} Lvl {req.Value} (Cur: {GetTaskLevel(req.Key):N0})"
                );
            }
            return reqTexts.Any() ? "Requires: " + string.Join(", ", reqTexts) : string.Empty;
        }

        public void StartJob(string jobId)
        {
            if (!IsJobActuallyAvailable(jobId))
                return;
            if (!string.IsNullOrEmpty(CurrentGameData.CurrentEnemyId))
                return;
            CurrentGameData.CurrentJobId = jobId;
            CurrentGameData.CurrentTaskName = null;
            CurrentGameData.IsWorking = true;
            NotifyStateChanged();
        }

        public void StopJob()
        {
            CurrentGameData.CurrentJobId = null;
            CurrentGameData.IsWorking = false;
            NotifyStateChanged();
        }

        private void ProcessUnlocks(List<string> unlockIds)
        {
            foreach (var unlockId in unlockIds)
            {
                if (unlockId.StartsWith("task:"))
                {
                    var id = unlockId.Substring(5);
                    if (!CurrentGameData.UnlockedTaskIds.Contains(id))
                    {
                        CurrentGameData.UnlockedTaskIds.Add(id);
                        if (!CurrentGameData.TaskLevels.ContainsKey(id))
                            CurrentGameData.TaskLevels[id] = 0;
                        if (!CurrentGameData.TaskData.ContainsKey(id))
                            CurrentGameData.TaskData[id] = 0;
                    }
                }
                else if (unlockId.StartsWith("skill:"))
                {
                    var id = unlockId.Substring(6);
                    if (!CurrentGameData.UnlockedSkillIds.Contains(id))
                    {
                        CurrentGameData.UnlockedSkillIds.Add(id);
                        if (!CurrentGameData.SkillData.ContainsKey(id))
                            CurrentGameData.SkillData[id] = 0;
                    }
                }
                else if (unlockId.StartsWith("job:"))
                {
                    var id = unlockId.Substring(4);
                    if (!CurrentGameData.UnlockedJobIds.Contains(id))
                    {
                        CurrentGameData.UnlockedJobIds.Add(id);
                        if (!CurrentGameData.JobLevels.ContainsKey(id))
                            CurrentGameData.JobLevels[id] = 0;
                        if (!CurrentGameData.JobData.ContainsKey(id))
                            CurrentGameData.JobData[id] = 0;
                    }
                }
            }
        }

        public double CalculatePlayerMaxHealth()
        {
            double baseHealth = 50;
            double fortitudeBonus = GetSkillLevel("fortitude") * 10;
            return baseHealth + fortitudeBonus;
        }

        public double CalculatePlayerAttack()
        {
            double baseAttack = 3;
            double swordsmanshipBonus = GetSkillLevel("swordsmanship") * 0.5;
            double flatShopBonus = GetShopItemEffectValue("PlayerAttackFlatBonus");
            return (baseAttack + swordsmanshipBonus + flatShopBonus);
        }

        public double CalculatePlayerDefense()
        {
            double baseDefense = 0;
            double fortitudeDefenseBonus = GetSkillLevel("fortitude") * 0.2;
            return baseDefense + fortitudeDefenseBonus;
        }

        public double CalculatePlayerHealthRegen()
        {
            return Math.Max(
                0.5,
                CalculatePlayerMaxHealth() * 0.002 + GetSkillLevel("fortitude") * 0.05
            );
        }

        public void StartCombat(string enemyId)
        {
            if (!string.IsNullOrEmpty(CurrentGameData.CurrentEnemyId))
                return;
            var enemyDef = GameDataDefinitions.GetEnemyById(enemyId);
            if (enemyDef == null)
                return;
            CurrentGameData.CurrentEnemyId = enemyId;
            CurrentGameData.CurrentEnemyHealth = enemyDef.MaxHealth;
            CurrentGameData.CurrentTaskName = null;
            CurrentGameData.CurrentJobId = null;
            CurrentGameData.IsWorking = false;
            _combatLog.Clear();
            AddCombatLog($"You face the {enemyDef.Name}!");
            NotifyStateChanged();
        }

        public void EndCombat()
        {
            CurrentGameData.CurrentEnemyId = null;
            CurrentGameData.CurrentEnemyHealth = 0;
            NotifyStateChanged();
        }

        public void FleeCombat()
        {
            if (!string.IsNullOrEmpty(CurrentGameData.CurrentEnemyId))
            {
                AddCombatLog("You cautiously retreat from the battle!");
                EndCombat();
            }
        }

        private void AddCombatLog(string message)
        {
            _combatLog.Insert(
                0,
                $"[{CurrentGameData.Days:N0} Day {TimeSpan.FromSeconds(CurrentGameData.CurrentRun.TimeThisRun):hh\\:mm\\:ss}] {message}"
            );
            if (_combatLog.Count > 25)
                _combatLog.RemoveAt(_combatLog.Count - 1);
        }

        private void ProcessCombatTurn()
        {
            if (string.IsNullOrEmpty(CurrentGameData.CurrentEnemyId))
                return;
            var enemyDef = GameDataDefinitions.GetEnemyById(CurrentGameData.CurrentEnemyId);
            if (enemyDef == null)
            {
                EndCombat();
                return;
            }
            double playerAttack = CalculatePlayerAttack();
            double vsOrcBonus = 0;
            if (enemyDef.Faction == "Orc Clans" && GetSkillLevel("orc_slaying_tactics") > 0)
            {
                vsOrcBonus = playerAttack * (GetSkillLevel("orc_slaying_tactics") * 0.1);
            }
            double playerDamage = Math.Max(1, (playerAttack + vsOrcBonus) - enemyDef.Defense);
            CurrentGameData.CurrentEnemyHealth -= playerDamage;
            AddCombatLog(
                $"You strike the {enemyDef.Name} for {playerDamage:F0} damage. ({CurrentGameData.CurrentEnemyHealth:F0}/{enemyDef.MaxHealth:F0} HP)"
            );
            if (CurrentGameData.CurrentEnemyHealth <= 0)
            {
                AddCombatLog($"{enemyDef.Name} vanquished!");
                double coinsGained = enemyDef.CoinReward;
                CurrentGameData.Coins += coinsGained;
                CurrentGameData.Stats.TotalCoinsEarned += (long)coinsGained;
                CurrentGameData.CurrentRun.PlayerXp += enemyDef.PlayerXpReward;
                AddCombatLog($"Gained {coinsGained} silver and {enemyDef.PlayerXpReward:F0} XP.");
                CurrentGameData.Stats.TotalEnemiesVanquished++;
                if (!CurrentGameData.Stats.EnemiesVanquishedByType.ContainsKey(enemyDef.Id))
                    CurrentGameData.Stats.EnemiesVanquishedByType[enemyDef.Id] = 0;
                CurrentGameData.Stats.EnemiesVanquishedByType[enemyDef.Id]++;
                if (enemyDef.IsBoss)
                {
                    if (enemyDef.Id == "forest_guardian_boss")
                        CurrentGameData.IsGatekeeperDefeated_ForestGuardian = true;
                    AddCombatLog(
                        $"The formidable {enemyDef.Name} has been vanquished! New paths may now be open to you."
                    );
                }
                EndCombat();
                NotifyStateChanged();
                return;
            }
            double enemyAttack = enemyDef.Attack;
            double playerDefense = CalculatePlayerDefense();
            double enemyDamage = Math.Max(1, enemyAttack - playerDefense);
            CurrentGameData.CurrentRun.PlayerCurrentHealth -= enemyDamage;
            AddCombatLog(
                $"{enemyDef.Name} retaliates, hitting you for {enemyDamage:F0} damage. ({CurrentGameData.CurrentRun.PlayerCurrentHealth:F0}/{CalculatePlayerMaxHealth():F0} HP)"
            );
            if (CurrentGameData.CurrentRun.PlayerCurrentHealth <= 0)
            {
                AddCombatLog("You have fallen in battle!");
                CurrentGameData.CurrentRun.PlayerCurrentHealth = 0;
                double coinsLost = Math.Min(
                    CurrentGameData.Coins,
                    Math.Max(10, CurrentGameData.Coins * 0.15)
                );
                CurrentGameData.Coins -= coinsLost;
                AddCombatLog($"You lost {coinsLost:F0} silver.");
                EndCombat();
                NotifyStateChanged();
                return;
            }
            NotifyStateChanged();
        }
    }
}

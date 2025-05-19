using BlazedProgressKnight.Game.Data;

namespace BlazedProgressKnight.Game;

public class GameService : IDisposable
{
    public GameState State { get; private set; }
    private Timer? _timer;
    private readonly ILogger<GameService> _logger;
    private const double GAME_TICK_INTERVAL_SECONDS = 0.1;

    public event Action? OnStateChange;

    public GameService(ILogger<GameService> logger)
    {
        State = new GameState();
        _logger = logger;
        GameData.Initialize(); // Ensure static game data is loaded
        InitializePlayerState();
        _logger.LogInformation("GameService initialized. GameData loaded. Player state initialized.");
    }

    private void InitializePlayerState()
    {
        State.PlayerSkillLevels.Clear();
        State.PlayerSkillExperience.Clear();
        State.PlayerJobLevels.Clear();
        State.PlayerJobExperience.Clear();

        foreach (var skillDef in GameData.AllSkills.Values)
        {
            State.PlayerSkillLevels[skillDef.InternalName] = 0;
            State.PlayerSkillExperience[skillDef.InternalName] = 0;
        }
        foreach (var jobDef in GameData.AllJobs.Values)
        {
            State.PlayerJobLevels[jobDef.InternalName] = 0;
            State.PlayerJobExperience[jobDef.InternalName] = 0;
        }
    }

    public void StartGameLoop()
    {
        if (_timer == null)
        {
            _logger.LogInformation("Starting game loop.");
            _timer = new Timer(GameTick, null, TimeSpan.Zero, TimeSpan.FromSeconds(GAME_TICK_INTERVAL_SECONDS));
        }
    }

    private void GameTick(object? stateInfo)
    {
        bool stateChanged = false;

        if (State.CurrentJobInternalName != "Unemployed" && GameData.AllJobs.TryGetValue(State.CurrentJobInternalName, out var currentJobDef))
        {
            if (State.CurrentActionName != currentJobDef.DisplayName) // Job started or changed
            {
                StartJobAction(currentJobDef.InternalName); // Ensure action parameters are fresh
            }

            State.CurrentActionTimeElapsed += GAME_TICK_INTERVAL_SECONDS;

            // Recalculate duration here if effects can change dynamically during an action (e.g. buffs)
            // For now, duration is set when action starts.
            // State.CurrentActionDuration = GetJobDuration(currentJobDef);


            if (State.CurrentActionDuration > 0)
            {
                State.CurrentActionProgress = Math.Min(100, (State.CurrentActionTimeElapsed / State.CurrentActionDuration) * 100);
            }

            if (State.CurrentActionTimeElapsed >= State.CurrentActionDuration)
            {
                // Action complete
                double incomeGained = GetJobIncome(currentJobDef);
                State.Money += incomeGained;
                _logger.LogInformation($"{currentJobDef.DisplayName} complete. Rewarded: {incomeGained:F2} Money. New Money: {State.Money:F2}");

                // Grant Job XP
                double jobXpGained = GetJobExperience(currentJobDef);
                GrantJobExperience(currentJobDef.InternalName, jobXpGained);


                // Grant Skill XP (based on job's XP rewards defined in GameData)
                foreach (var xpReward in currentJobDef.ExperienceRewards)
                {
                    if (GameData.AllSkills.ContainsKey(xpReward.Key)) // Key is Skill InternalName
                    {
                        GrantSkillExperience(xpReward.Key, xpReward.Value * (1 + GetGlobalSkillExperienceModifier())); // Value is base XP for that skill
                    }
                    else if (Enum.TryParse<SkillCategory>(xpReward.Key, true, out var category)) // Key might be a category
                    {
                        // Grant XP to all skills in this category (more complex, needs skill to category mapping)
                        // For now, we'll assume xpReward.Key is a direct skill internal name.
                    }
                }


                // Restart action for continuous work
                StartJobAction(currentJobDef.InternalName);
            }
            stateChanged = true;
        }
        else if (State.CurrentJobInternalName == "Unemployed" && State.CurrentActionProgress > 0)
        {
            // Clear progress if job is quit
            State.CurrentActionName = "Idle";
            State.CurrentActionProgress = 0;
            State.CurrentActionTimeElapsed = 0;
            State.CurrentActionDuration = 0;
            State.MoneyPerSecondEquivalent = 0;
            stateChanged = true;
        }


        if (stateChanged)
        {
            OnStateChange?.Invoke();
        }
    }

    public double GetJobDuration(Job job)
    {
        double modifiedDuration = job.BaseDuration;
        double durationMultiplier = 1.0; // Multiplicative bonuses (e.g., 0.9 for 10% faster)
        double flatReduction = 0.0;      // Additive bonuses

        foreach (var skillInternalName in State.PlayerSkillLevels.Keys)
        {
            if (State.PlayerSkillLevels[skillInternalName] > 0 && GameData.AllSkills.TryGetValue(skillInternalName, out var skillDef))
            {
                foreach (var effect in skillDef.EffectsPerLevel)
                {
                    if (effect.Type == EffectType.JobDurationReduction &&
                        (effect.TargetInternalName == job.InternalName ||
                         effect.TargetInternalName == job.Category.ToString() ||
                         string.IsNullOrEmpty(effect.TargetInternalName))) // Global or matches job/category
                    {
                        // Effect value for duration reduction: 0.01 means 1% faster.
                        // If effect.Value is 0.01 (for 1% reduction per level):
                        // durationMultiplier *= (1 - (effect.Value * State.PlayerSkillLevels[skillInternalName]));
                        // Let's assume effect.Value is the total multiplier for that skill level (e.g. 0.9 for 10% reduction)
                        // This requires effects to be defined as total multipliers or the application logic to sum them correctly.
                        // For simplicity, let's assume effect.Value is a percentage reduction (0.01 for 1%) per level.
                        durationMultiplier -= (effect.Value * State.PlayerSkillLevels[skillInternalName]);
                    }
                }
            }
        }
        modifiedDuration *= Math.Max(0.01, durationMultiplier); // Ensure duration doesn't become zero or negative
        modifiedDuration -= flatReduction;

        return Math.Max(0.1, modifiedDuration); // Minimum duration of 0.1s
    }

    public double GetJobIncome(Job job)
    {
        double modifiedIncome = job.BaseIncome;
        double incomeMultiplier = 1.0 + GetGlobalIncomeModifier(); // Start with global bonuses

        foreach (var skillInternalName in State.PlayerSkillLevels.Keys)
        {
            if (State.PlayerSkillLevels[skillInternalName] > 0 && GameData.AllSkills.TryGetValue(skillInternalName, out var skillDef))
            {
                foreach (var effect in skillDef.EffectsPerLevel)
                {
                    if (effect.Type == EffectType.JobIncomeMultiplier &&
                        (effect.TargetInternalName == job.InternalName ||
                         effect.TargetInternalName == job.Category.ToString() ||
                         string.IsNullOrEmpty(effect.TargetInternalName)))
                    {
                        // Assuming effect.Value is a percentage increase (0.01 for 1%) per level
                        incomeMultiplier += (effect.Value * State.PlayerSkillLevels[skillInternalName]);
                    }
                }
            }
        }
        modifiedIncome *= incomeMultiplier;
        return modifiedIncome;
    }

    public double GetJobExperience(Job job) // Gets base XP for the job itself
    {
        double modifiedExperience = job.BaseExperience; // This is job's own XP, not skill XP
        // Apply global job XP modifiers if any (similar to income)
        return modifiedExperience;
    }


    private double GetGlobalIncomeModifier()
    {
        double modifier = 0;
        foreach (var skillInternalName in State.PlayerSkillLevels.Keys)
        {
            if (State.PlayerSkillLevels[skillInternalName] > 0 && GameData.AllSkills.TryGetValue(skillInternalName, out var skillDef))
            {
                foreach (var effect in skillDef.EffectsPerLevel)
                {
                    if (effect.Type == EffectType.GlobalIncomeMultiplier && string.IsNullOrEmpty(effect.TargetInternalName))
                    {
                        modifier += (effect.Value * State.PlayerSkillLevels[skillInternalName]);
                    }
                }
            }
        }
        return modifier;
    }
    private double GetGlobalSkillExperienceModifier()
    {
        double modifier = 0;
        // Similar to GetGlobalIncomeModifier, but for EffectType.GlobalSkillExperienceMultiplier
        // ... implementation ...
        return modifier;
    }


    private void UpdateMoneyPerSecondEquivalent(Job? job = null)
    {
        if (job != null && State.CurrentActionName == job.DisplayName && State.CurrentActionDuration > 0)
        {
            State.MoneyPerSecondEquivalent = GetJobIncome(job) / State.CurrentActionDuration;
        }
        else
        {
            State.MoneyPerSecondEquivalent = 0;
        }
    }

    public void AssignJob(string jobInternalName)
    {
        _logger.LogInformation($"Assigning job: {jobInternalName}");

        if (jobInternalName == "Unemployed" || !GameData.AllJobs.TryGetValue(jobInternalName, out var jobDef))
        {
            State.CurrentJobInternalName = "Unemployed";
            State.CurrentJobDisplayName = "Unemployed";
            State.CurrentActionName = "Idle";
            State.CurrentActionProgress = 0;
            State.CurrentActionTimeElapsed = 0;
            State.CurrentActionDuration = 0;
            UpdateMoneyPerSecondEquivalent();
        }
        else
        {
            if (!AreRequirementsMet(jobDef.Requirements))
            {
                _logger.LogInformation($"Cannot assign job {jobDef.DisplayName}: Requirements not met.");
                // Optionally provide feedback to user via an event or message system
                return;
            }
            State.CurrentJobInternalName = jobDef.InternalName;
            State.CurrentJobDisplayName = jobDef.DisplayName;
            StartJobAction(jobDef.InternalName);
        }
        OnStateChange?.Invoke();
    }

    private void StartJobAction(string jobInternalName)
    {
        if (!GameData.AllJobs.TryGetValue(jobInternalName, out var jobDef)) return;

        State.CurrentActionName = jobDef.DisplayName;
        State.CurrentActionDuration = GetJobDuration(jobDef); // Calculate duration with effects
        State.CurrentActionTimeElapsed = 0;
        State.CurrentActionProgress = 0;
        UpdateMoneyPerSecondEquivalent(jobDef);
        _logger.LogInformation($"Started job action: {jobDef.DisplayName}. Duration: {State.CurrentActionDuration:F2}s");
    }

    public void TrainSkill(string skillInternalName)
    {
        if (!GameData.AllSkills.TryGetValue(skillInternalName, out var skillToTrain))
        {
            _logger.LogWarning($"Skill definition not found for training: {skillInternalName}");
            return;
        }

        int currentLevel = State.PlayerSkillLevels.TryGetValue(skillInternalName, out int lvl) ? lvl : 0;

        if (currentLevel >= skillToTrain.MaxLevel)
        {
            _logger.LogInformation($"{skillToTrain.DisplayName} is already at max level ({currentLevel}).");
            return;
        }

        if (!AreRequirementsMet(skillToTrain.RequirementsToUnlock, currentLevel + 1)) // Check for next level too if applicable
        {
            _logger.LogInformation($"Cannot train skill {skillToTrain.DisplayName}: Requirements not met for next level.");
            return;
        }


        double cost = skillToTrain.GetCostForLevel(currentLevel + 1);

        if (State.Money >= cost)
        {
            State.Money -= cost;
            State.PlayerSkillLevels[skillInternalName]++;
            _logger.LogInformation($"{skillToTrain.DisplayName} skill trained. New level: {State.PlayerSkillLevels[skillInternalName]}");

            // If currently performing a job, its duration/income might change. Re-calculate.
            if (State.CurrentJobInternalName != "Unemployed" && GameData.AllJobs.TryGetValue(State.CurrentJobInternalName, out var currentJobDef))
            {
                // Preserve progress percentage of current action
                double progressPercent = State.CurrentActionProgress;
                StartJobAction(State.CurrentJobInternalName); // This will recalculate duration based on new skill level
                State.CurrentActionTimeElapsed = (progressPercent / 100.0) * State.CurrentActionDuration; // Restore time elapsed based on new duration
                State.CurrentActionProgress = progressPercent; // Restore progress percentage
            }
            OnStateChange?.Invoke();
        }
        else
        {
            _logger.LogInformation($"Not enough money to train {skillToTrain.DisplayName}. Need {cost:F2}, have {State.Money:F2}");
        }
    }

    private void GrantSkillExperience(string skillInternalName, double xpAmount)
    {
        if (!GameData.AllSkills.TryGetValue(skillInternalName, out var skillDef) || xpAmount <= 0) return;

        State.PlayerSkillExperience.TryGetValue(skillInternalName, out double currentXp);
        currentXp += xpAmount;
        _logger.LogInformation($"Granted {xpAmount:F2} XP to {skillDef.DisplayName}. Total XP: {currentXp:F2}");


        bool leveledUp = false;
        // Level up skill if XP requirement met
        // This needs a formula: XP_to_next_level = base_xp_req * multiplier ^ current_level
        // For now, let's use a simple placeholder: 100 XP per level
        double xpForNextLevel = 100 * Math.Pow(1.5, State.PlayerSkillLevels[skillInternalName]); // Example scaling

        while (currentXp >= xpForNextLevel && State.PlayerSkillLevels[skillInternalName] < skillDef.MaxLevel)
        {
            currentXp -= xpForNextLevel;
            State.PlayerSkillLevels[skillInternalName]++;
            leveledUp = true;
            _logger.LogInformation($"{skillDef.DisplayName} leveled up to {State.PlayerSkillLevels[skillInternalName]}!");
            xpForNextLevel = 100 * Math.Pow(1.5, State.PlayerSkillLevels[skillInternalName]); // XP for the *new* next level
        }
        State.PlayerSkillExperience[skillInternalName] = currentXp;

        if (leveledUp)
        {
            // If skill level up changed job params, re-evaluate current job action
            if (State.CurrentJobInternalName != "Unemployed" && GameData.AllJobs.TryGetValue(State.CurrentJobInternalName, out var currentJobDef))
            {
                double progressPercent = State.CurrentActionProgress;
                StartJobAction(State.CurrentJobInternalName);
                State.CurrentActionTimeElapsed = (progressPercent / 100.0) * State.CurrentActionDuration;
                State.CurrentActionProgress = progressPercent;
            }
        }
    }

    private void GrantJobExperience(string jobInternalName, double xpAmount)
    {
        if (!GameData.AllJobs.TryGetValue(jobInternalName, out var jobDef) || xpAmount <= 0) return;

        State.PlayerJobExperience.TryGetValue(jobInternalName, out double currentXp);
        currentXp += xpAmount;
        _logger.LogInformation($"Granted {xpAmount:F2} Job XP to {jobDef.DisplayName}. Total Job XP: {currentXp:F2}");

        // Level up job (placeholder logic)
        double xpForNextJobLevel = 200 * Math.Pow(1.8, State.PlayerJobLevels[jobInternalName]);
        if (currentXp >= xpForNextJobLevel && State.PlayerJobLevels[jobInternalName] < jobDef.MaxLevel)
        {
            currentXp -= xpForNextJobLevel;
            State.PlayerJobLevels[jobInternalName]++;
            _logger.LogInformation($"{jobDef.DisplayName} (Job) leveled up to {State.PlayerJobLevels[jobInternalName]}!");
            // Job level ups might unlock things or provide passive bonuses (to be implemented)
        }
        State.PlayerJobExperience[jobInternalName] = currentXp;
    }


    public bool AreRequirementsMet(List<Requirement> requirements, int forNextLevel = 0) // forNextLevel can be used if reqs scale
    {
        if (requirements == null || !requirements.Any()) return true;

        foreach (var req in requirements)
        {
            if (!req.IsMet(State, this)) // Pass GameService if Requirement.IsMet needs it
            {
                _logger.LogInformation($"Requirement not met: {req.Type} {req.Name} needs {req.Value}");
                return false;
            }
        }
        return true;
    }


    public void ResetGame()
    {
        _logger.LogInformation("Resetting game.");
        State.Reset();
        InitializePlayerState(); // Re-initialize skill levels after reset
        UpdateMoneyPerSecondEquivalent();
        OnStateChange?.Invoke();
    }

    public void Dispose()
    {
        _logger.LogInformation("Disposing GameService.");
        _timer?.Dispose();
    }
}
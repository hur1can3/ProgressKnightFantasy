namespace BlazedProgressKnight.Game.Data;

// Defines a requirement for unlocking or performing actions.


public class Requirement
{
    public RequirementType Type { get; set; }
    public string Name { get; set; } // Internal name of skill, job, item, or statistic
    public double Value { get; set; } // Required level, amount, or quantity

    public Requirement(RequirementType type, string name, double value)
    {
        Type = type;
        Name = name;
        Value = value;
    }

    public bool IsMet(GameState gameState, GameService gameService)
    {
        switch (Type)
        {
            case RequirementType.Money:
                return gameState.Money >= Value;
            case RequirementType.SkillLevel:
                return gameState.PlayerSkillLevels.TryGetValue(Name, out int currentSkillLevel) && currentSkillLevel >= Value;
            case RequirementType.JobLevel:
                return gameState.PlayerJobLevels.TryGetValue(Name, out int currentJobLevel) && currentJobLevel >= Value;
            // case RequirementType.Item:
            // return gameState.Inventory.ContainsKey(Name) && gameState.Inventory[Name] >= Value;
            case RequirementType.Statistic:
                // This requires a way to get arbitrary stats from GameState, potentially via reflection or a dedicated method
                // Example for Strength:
                if (Name.Equals("Strength", StringComparison.OrdinalIgnoreCase)) return gameState.Strength >= Value;
                if (Name.Equals("Intelligence", StringComparison.OrdinalIgnoreCase)) return gameState.Intelligence >= Value;
                // ... and so on for other stats
                // gameService?._logger.LogWarning($"Unhandled statistic requirement: {Name}"); // Null conditional for gameService
                return false; // Or true if stat not found means no requirement
            default:
                // gameService?._logger.LogWarning($"Unhandled requirement type: {Type} for {Name}");
                return false;
        }
    }
}
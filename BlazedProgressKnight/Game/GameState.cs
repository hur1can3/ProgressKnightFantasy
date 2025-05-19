namespace BlazedProgressKnight.Game;

// 1. GameState.cs
// This class holds the current state of the game.
// (e.g., MyBlazorGame/Data/GameState.cs)
// No changes needed in this file from the previous version for this specific update.
// Player-specific progress (like current skill levels) will still be here.
// Game definitions (like all available jobs) will be in GameData.cs.

public class GameState
{
    public double Money { get; set; } = 0;
    public string CurrentJobInternalName { get; set; } = "Unemployed"; // Store internal name for logic
    public string CurrentJobDisplayName { get; set; } = "Unemployed"; // Store display name for UI

    // Properties for job progress
    public string CurrentActionName { get; set; } = "Idle"; // Usually same as CurrentJobDisplayName when active
    public double CurrentActionProgress { get; set; } = 0; // Percentage 0-100
    public double CurrentActionTimeElapsed { get; set; } = 0; // Seconds
    public double CurrentActionDuration { get; set; } = 0; // Seconds for current action to complete (calculated with effects)

    public double MoneyPerSecondEquivalent { get; set; } = 0;

    public Dictionary<string, int> PlayerSkillLevels { get; set; } = new Dictionary<string, int>();
    public Dictionary<string, double> PlayerSkillExperience { get; set; } = new Dictionary<string, double>();
    public Dictionary<string, int> PlayerJobLevels { get; set; } = new Dictionary<string, int>();
    public Dictionary<string, double> PlayerJobExperience { get; set; } = new Dictionary<string, double>();

    // Player base stats (can be modified by effects)
    public double Strength { get; set; } = 1;
    public double Intelligence { get; set; } = 1;
    public double Dexterity { get; set; } = 1;
    public double Constitution { get; set; } = 1;


    public void Reset()
    {
        Money = 0;
        CurrentJobInternalName = "Unemployed";
        CurrentJobDisplayName = "Unemployed";
        CurrentActionName = "Idle";
        CurrentActionProgress = 0;
        CurrentActionTimeElapsed = 0;
        CurrentActionDuration = 0;
        MoneyPerSecondEquivalent = 0;

        PlayerSkillLevels.Clear();
        PlayerSkillExperience.Clear();
        PlayerJobLevels.Clear();
        PlayerJobExperience.Clear();

        Strength = 1;
        Intelligence = 1;
        Dexterity = 1;
        Constitution = 1;

        // Initialize skill levels to 0 after clearing (GameService will handle this based on GameData)
    }
}
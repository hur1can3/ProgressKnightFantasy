namespace BlazedProgressKnight.Game.Data;

// Defines an effect provided by a skill, item, or job.

public class Effect
{
    public EffectType Type { get; set; }
    public string TargetInternalName { get; set; } // Optional: internal name of job, skill, category, or "" for global
    public double Value { get; set; } // e.g., 0.05 for +5% or flat value like +10 Strength
    // public bool IsMultiplier { get; set; } // This can be inferred from EffectType for more clarity

    public Effect(EffectType type, double value, string targetInternalName = "")
    {
        Type = type;
        Value = value;
        TargetInternalName = targetInternalName;
    }
}
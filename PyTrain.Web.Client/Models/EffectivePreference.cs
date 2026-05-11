namespace PyTrain.Web.Client.Models;

public sealed record EffectivePreference<T>(T Value, bool IsTenantEnforced);
using System;
[Serializable]
public sealed class OptionalFloat
{
    public bool set;
    public float value;
    public bool HasValue => set;
    public static OptionalFloat Of(float value) => new OptionalFloat { set = true, value = value };
}

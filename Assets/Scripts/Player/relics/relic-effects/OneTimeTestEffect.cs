public class OneTimeTestEffect : OneTimeRelicEffect
{
    public string text;

    public override void ApplyEffect()
    {
        print("One time effect " + text);
    }

    public override void RelicDestroyed()
    {
    }

    public override string GetDescription()
    {
        return "For test purposes one time test effect";
    }
}
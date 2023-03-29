public class OneTimeTestEffect : OneTimeRelicEffect
{
    public string text;

    public override void ObtainedEffect()
    {
        print("One time effect " + text);
    }

    public override void UsedEffect()
    {
    }

    public override string GetDescription()
    {
        return "For test purposes one time test effect";
    }
}
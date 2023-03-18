using UnityEngine;

public class TickTestEffect : TickBasedRelicEffect
{
    public override void TickEffect()
    {
        print("Time is " + Time.time);
    }

    public override string GetDescription()
    {
        return "For test purposes tick test effect";
    }
}
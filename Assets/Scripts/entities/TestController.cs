using UnityEngine;

public class TestController : MonoBehaviour
{
    [SerializeField] private AOEAbility ability;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ability.CastAbility(transform);
        }
    }
}
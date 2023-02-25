using UnityEngine;

public class TestController : MonoBehaviour
{
    [SerializeField] private AOEAbility ability;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ability.CastAbility(transform);
        }
    }
}
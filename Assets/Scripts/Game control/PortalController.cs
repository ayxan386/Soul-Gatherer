using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask playerLayer;

    void Update()
    {
        if (Physics.CheckSphere(transform.position, checkRadius, playerLayer))
        {
            var currentLevel = LevelLoader.Instance.GetCurrentLevel();
            if (!currentLevel.isLast)
            {
                PlayerDataManager.SaveData();
                LevelLoader.Instance.LevelComplete();
                IntermediateLevelLoader.LoadLevel(LevelLoader.Instance.GetCurrentLevel().levelName);
            }
            else
            {
                GameEndController.LoadEndScene(true);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
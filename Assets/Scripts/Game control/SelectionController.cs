using UnityEngine;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    // private void SelectionChanged()
    // {
    //     if (Selection.activeGameObject == null || !Selection.activeGameObject.activeSelf)
    //     {
    //         FindNextSelectable();
    //     }
    // }


    public static void FindNextSelectable()
    {
        foreach (var selectable in Selectable.allSelectablesArray)
        {
            if (selectable.IsInteractable())
            {
                selectable.Select();
                break;
            }
        }
    }
}
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    public static SelectionController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Selection.selectionChanged += SelectionChanged;
    }

    // private void SelectionChanged()
    // {
    //     if (Selection.activeGameObject == null || !Selection.activeGameObject.activeSelf)
    //     {
    //         FindNextSelectable();
    //     }
    // }


    public void FindNextSelectable()
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
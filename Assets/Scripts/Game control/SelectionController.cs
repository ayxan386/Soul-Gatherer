using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    private IEnumerator Start()
    {
        Application.focusChanged += ApplicationOnfocusChanged;
        while (true)
        {
            yield return new WaitUntil(() => EventSystem.current.currentSelectedGameObject == null);
            FindNextSelectable();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void ApplicationOnfocusChanged(bool obj)
    {
        if (obj) FindNextSelectable();
    }


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
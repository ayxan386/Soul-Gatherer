using UnityEngine;
using UnityEngine.UI;

public class SelectableElement : MonoBehaviour
{
    private Selectable selfRef;

    private void Awake()
    {
        TryGetComponent(out selfRef);
    }

    private void OnEnable()
    {
        selfRef.Select();
    }

    private void OnDisable()
    {
        SelectionController.FindNextSelectable();
    }
}
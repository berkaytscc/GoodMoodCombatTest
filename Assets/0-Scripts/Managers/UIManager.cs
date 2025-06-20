using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private readonly Dictionary<string, UIElement> _uiElements = new Dictionary<string, UIElement>();

    public void RegisterUIElement(UIElement element)
    {   // currently overrides existing UIElement of the same type.
        // if multiple UIElements of the same type registiration is wanted, we can change this.
        _uiElements[element.GetType().Name] = element;
    }

    public T GetUIElement<T>() where T : UIElement
    {
        string typeName = typeof(T).Name;
        if (_uiElements.TryGetValue(typeName, out UIElement element))
        {
            return element as T;
        }
        else
        {
            Debug.Log("UIElement of type " + typeName + " not found");
            return null;
        }
    }
}

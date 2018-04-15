using UnityEngine;
using UnityEngine.EventSystems;

public class OpenURLClickAction : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    string URL = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (null != URL)
            Application.OpenURL(URL);
    }

}
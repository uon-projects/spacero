using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MenuItemAudio : MonoBehaviour, IMoveHandler, ISubmitHandler, IPointerEnterHandler, IPointerDownHandler
{
    public void OnMove(AxisEventData eventData)
    {
        AudioHandler.Play("select");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioHandler.Play("select");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioHandler.Play("confirm");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioHandler.Play("confirm");
    }
}
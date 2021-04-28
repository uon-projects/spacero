using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour
{
    public EventSystem eventSystem;

    // Update is called once per frame
    void Update()
    {
        if ((InputExtensions.Pressed.Up || InputExtensions.Pressed.Down) &&
            eventSystem.currentSelectedGameObject == null &&
            eventSystem.firstSelectedGameObject != null)
        {
            eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
        }

        if (InputExtensions.Pressed.A &&
            eventSystem.currentSelectedGameObject != null)
        {
            ExecuteEvents.Execute<ISubmitHandler>(
                eventSystem.currentSelectedGameObject,
                new BaseEventData(EventSystem.current),
                ExecuteEvents.submitHandler
            );
        }
    }
}
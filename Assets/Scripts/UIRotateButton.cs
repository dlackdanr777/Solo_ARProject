using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerButton : MonoBehaviour
{
    [SerializeField] private EventTrigger _button;

    private GameObject _obj;

    private bool _isClicked;

    private event Action _onUpdateHanlder; 

    public void Init(Action action) 
    {
        EventTrigger.Entry addButtonDownEvent = new EventTrigger.Entry();
        EventTrigger.Entry addButtonUpEvent = new EventTrigger.Entry();

        addButtonDownEvent.eventID = EventTriggerType.PointerDown;
        addButtonUpEvent.eventID = EventTriggerType.PointerUp;

        addButtonDownEvent.callback.AddListener((eventData) => OnClickedButtonDown());
        addButtonUpEvent.callback.AddListener((eventData) => OnClickedButtonUp());

        _button.triggers.Add(addButtonDownEvent);
        _button.triggers.Add(addButtonUpEvent);

        _onUpdateHanlder += action;

        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (!_isClicked)
            return;

        _onUpdateHanlder?.Invoke();
    }


    private void OnClickedButtonDown()
    {
        _isClicked = true;
    }


    private void OnClickedButtonUp()
    {
        _isClicked = false;
    }
}

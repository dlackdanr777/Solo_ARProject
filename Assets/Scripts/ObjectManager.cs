using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] GameObject _indicator;
    [SerializeField] GameObject _showcaseObj;

    [SerializeField] float _rotMultiplier;
    ARRaycastManager _raycastManager;

    private void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
    }
    void Start()
    {
        _showcaseObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ARRaycastHit hitInfo = CastARRay();

        TouchScreen(hitInfo);
    }

    Vector3 _tempTouchPos;
    Vector3 _tempAngle;

    float _touchTimer;

    private void TouchScreen(ARRaycastHit hitInfo)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            _touchTimer += Time.deltaTime;

            if(2 < _touchTimer)
            {
                _tempTouchPos = touch.position;
                _tempAngle = _showcaseObj.transform.eulerAngles;

                if (hitInfo.trackable)
                {
                    _showcaseObj.SetActive(true);
                    _showcaseObj.transform.position = hitInfo.pose.position;
                }
                _touchTimer = 0;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                float targetRotate = _tempTouchPos.x - touch.position.x;
                _showcaseObj.transform.eulerAngles = _tempAngle + (Vector3.up * targetRotate);
            }
        }
        else
        {
            _touchTimer = 0;
        }
    }


    ARRaycastHit CastARRay()
    {
        Vector2 screenPoint = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        List<ARRaycastHit> hitInfo = new List<ARRaycastHit>();

        if (_raycastManager.Raycast(screenPoint, hitInfo, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {
            _indicator.SetActive(true);
            _indicator.transform.position = hitInfo[0].pose.position;
            _indicator.transform.rotation = hitInfo[0].pose.rotation;

            _indicator.transform.forward = Vector3.up;
        }
        else
        {
            _indicator.SetActive(false);
        }

        return hitInfo[0];
    }
}
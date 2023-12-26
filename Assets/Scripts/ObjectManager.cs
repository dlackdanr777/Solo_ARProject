using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


[RequireComponent(typeof(ARRaycastManager))]
public class ObjectManager : MonoBehaviour
{
    [SerializeField] private UIBuild _uiBuild;

    [SerializeField] private GameObject _indicator;

    [SerializeField] private float _rotMultiplier;

    private ARRaycastManager _raycastManager;

    private Dictionary<string, FurnitureData> _furnitureDataDic;
    public Dictionary<string, FurnitureData> FurnitureDataDic => _furnitureDataDic;


    private bool _isStart;

    private void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
        _furnitureDataDic = DatabaseManager.Instance.GetFurnitureDataDic();
    }
    void Start()
    {
        _uiBuild.Init(this);
    }

    // Update is called once per frame
    void Update()
    {
        StartBuild();
        CreateFurniture();

        //ARRaycastHit hitInfo = CastARRay();

        //TouchScreen(hitInfo);
    }

    public void StartBuild()
    {
        _isStart = true;
    }

    public void EndBuild()
    {
        _isStart = false;
    }


    Vector3 _tempTouchPos;
    Vector3 _tempAngle;

    float _touchTimer;

    /* private void TouchScreen(ARRaycastHit hitInfo)
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
     }*/

    public void CreateFurniture()
    {
        if (!_isStart)
            return;

        if (Input.touchCount == 0)
            return;


        Touch touch = Input.GetTouch(0);
        List<ARRaycastHit> hitInfo = new List<ARRaycastHit>();

        if (_raycastManager.Raycast(touch.position, hitInfo, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
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
    }


   /* ARRaycastHit CastARRay()
    {
        Vector2 HitPoint = new Vector2(Input.Touch);

        List<ARRaycastHit> hitInfo = new List<ARRaycastHit>();

        if (_raycastManager.Raycast(HitPoint, hitInfo, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
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
    }*/
}

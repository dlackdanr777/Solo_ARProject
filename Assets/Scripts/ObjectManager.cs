using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Muks.DataBind;
using UnityEngine.EventSystems;

public enum BuildState
{
    None,
    Build,
    Correction
}

[RequireComponent(typeof(ARRaycastManager))]
public class ObjectManager : MonoBehaviour
{


    [SerializeField] private BuildState _buildState;
    public BuildState BuildState => _buildState;

    [SerializeField] private UIBuild _uiBuild;

    [SerializeField] private GameObject _indicator;

    [SerializeField] private float _rotMultiplier;

    private ARRaycastManager _raycastManager;

    private Dictionary<string, FurnitureData> _furnitureDataDic;
    public Dictionary<string, FurnitureData> FurnitureDataDic => _furnitureDataDic;

    private FurnitureData _currentFurnitureData;

    private List<ARRaycastHit> _hitInfo = new List<ARRaycastHit>();

    private List<GameObject> _createdObjList = new List<GameObject>();

    private GameObject _correctionObj;

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
        ControllBuildState();
    }

    public void ChangeBuildState(FurnitureData furnitureData)
    {
        ChangedState(BuildState.Build);
        _currentFurnitureData = furnitureData;
        _uiBuild.ButtonSetActive(false);
        _uiBuild.ExitBuildButtonSetActive(true);
        DataBind.SetTextValue("BuildText", "[" + furnitureData.Name + "] 설치");
    }


    public void ChangeCorrectionState(GameObject obj)
    {
        _correctionObj = obj;
        ChangedState(BuildState.Correction);
        _uiBuild.CorrectionButtonsSerActive(true);
        _uiBuild.ButtonSetActive(false);
        DataBind.SetTextValue("BuildText", "수정");
    }


    public void ChangeNoneState()
    {
        ChangedState(BuildState.None);
        _uiBuild.ButtonSetActive(true);
        DataBind.SetTextValue("BuildText", "");
    }


    private void ChangedState(BuildState state)
    {
        _buildState = state;
        _indicator.SetActive(false);
        _uiBuild.BuildButtonSetActive(false);
        _uiBuild.ExitBuildButtonSetActive(false);
        _uiBuild.CorrectionButtonsSerActive(false);
    }


    public void ControllBuildState()
    {
        switch (_buildState)
        {
            case BuildState.None:
                StartCorrection();
                break;

            case BuildState.Build:
                BuildMode();
                break;

            case BuildState.Correction:
                break;
        }
    }


    public void BuildMode()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;

        if (_raycastManager.Raycast(touch.position, _hitInfo, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
        {
            _indicator.SetActive(true);

            _indicator.transform.position = _hitInfo[0].pose.position;
            _indicator.transform.rotation = _hitInfo[0].pose.rotation;

            _indicator.transform.forward = Vector3.up;

            _uiBuild.BuildButtonSetActive(true);
        }
        else
        {
            _indicator.SetActive(false);
            _uiBuild.BuildButtonSetActive(false);
        }
    }

    public void OnBuildButtonClicked()
    {
        GameObject obj = Instantiate(_currentFurnitureData.Prefab);
        obj.transform.position = _indicator.transform.position;
        obj.transform.rotation = _hitInfo[0].pose.rotation;

        _createdObjList.Add(obj);

        _uiBuild.BuildButtonSetActive(false);
        ChangeNoneState();
    }

    public void OnExitBuildButtonClicked()
    {
        ChangeNoneState();
    }


    private void StartCorrection()
    {
        if (Input.touchCount == 0)
            return;



        Touch touch = Input.GetTouch(0);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(touch.position);

        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;

        if (Physics.Raycast(ray, out hit))
        {
            foreach(GameObject obj in _createdObjList)
            {
                if (hit.collider.gameObject != obj)
                    continue;

                TestCanvas.Instance.SetText(obj.name + "발견");
                ChangeCorrectionState(obj);
                return;
            }
            
        }
        else
        {
            ChangeNoneState();
        } 
    }

    public void OnDeleteObjButtonClicked()
    {
        _createdObjList.Remove(_correctionObj);
        Destroy(_correctionObj);
        _correctionObj = null;

        ChangeNoneState();

    }

    public void OnRotateObjButtonClicked(int dir)
    {
        _correctionObj.transform.Rotate(Vector3.up * dir * _rotMultiplier * Time.deltaTime);
    }

    public void OnChangeNoneStateButtonClicked()
    {
        ChangeNoneState();
    }
}

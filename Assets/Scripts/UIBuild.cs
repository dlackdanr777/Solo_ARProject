using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuild : MonoBehaviour
{
    [SerializeField] private Transform _slotParent;

    [SerializeField] private UIBuildSlot _slotPrefab;

    [SerializeField] private GameObject _buildUIParent;

    [Space]
    [SerializeField] private Button _button;

    [SerializeField] private Button _buildButton;

    [SerializeField] private Button _exitBuildButton;

    [Space]
    [SerializeField] private EventTriggerButton _leftRotateButton;

    [SerializeField] private EventTriggerButton _rightRotateButton;

    [SerializeField] private Button _destoryButton;

    [SerializeField] private Button _exitButton;

    private ObjectManager _build;

    private List<UIBuildSlot> _slotList = new List<UIBuildSlot>();


    private void BuildButtonClicked()
    {
        _buildUIParent.SetActive(!_buildUIParent.activeSelf);
        _build.ChangeNoneState();
    }

    public void Init(ObjectManager build)
    {
        _build = build;
        InitSlot();

        _button.onClick.AddListener(BuildButtonClicked);
        _buildButton.onClick.AddListener(_build.OnBuildButtonClicked);
        _exitBuildButton.onClick.AddListener(_build.OnExitBuildButtonClicked);

        _leftRotateButton.Init(() => _build.OnRotateObjButtonClicked(-1));
        _rightRotateButton.Init(() => _build.OnRotateObjButtonClicked(1));

        _destoryButton.onClick.AddListener(_build.OnDeleteObjButtonClicked);
        _exitButton.onClick.AddListener(_build.OnChangeNoneStateButtonClicked);

        CorrectionButtonsSerActive(false);
        BuildButtonSetActive(false);
        ExitBuildButtonSetActive(false);

        _buildUIParent.SetActive(false);
    }


    private void InitSlot()
    {
        foreach (FurnitureData data in _build.FurnitureDataDic.Values)
        {
            UIBuildSlot slot = Instantiate(_slotPrefab, Vector3.zero, Quaternion.identity, _slotParent);
            slot.Init(this, data, OnSlotClicked);
        }
    }

    private void OnSlotClicked(FurnitureData data)
    {
        _buildUIParent.SetActive(false);
        _build.ChangeBuildState(data);
    }


    public void ButtonSetActive(bool value)
    {
        _button.gameObject.SetActive(value);
    }


    public void BuildButtonSetActive(bool value)
    {
        _buildButton.gameObject.SetActive(value);
    }

    public void ExitBuildButtonSetActive(bool value)
    {
        _exitBuildButton.gameObject.SetActive(value);
    }


    public void CorrectionButtonsSerActive(bool value)
    {
        _leftRotateButton.gameObject.SetActive(value);
        _rightRotateButton.gameObject.SetActive(value);
        _destoryButton.gameObject.SetActive(value);
        _exitButton.gameObject.SetActive(value);
    }
}

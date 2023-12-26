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

    private ObjectManager _build;

    private List<UIBuildSlot> _slotList = new List<UIBuildSlot>();


    private void BuildButtonClicked()
    {
        _buildUIParent.SetActive(!_buildUIParent.activeSelf);
    }

    public void Init(ObjectManager build)
    {
        _build = build;
        InitSlot();

        _button.onClick.AddListener(BuildButtonClicked);
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
    }
}

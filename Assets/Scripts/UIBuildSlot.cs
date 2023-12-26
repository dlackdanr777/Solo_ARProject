using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBuildSlot : MonoBehaviour
{
    [SerializeField] private Button _button;

    [SerializeField] private Image _image;

    private UIBuild _uiBuild;

    private FurnitureData _data;

    public void Init(UIBuild uiBuild, FurnitureData data, Action<FurnitureData> buttonClicked)
    {
        _uiBuild = uiBuild;
        _data = data;
        _image.sprite = _data.Sprite;
        _button.onClick.AddListener(() => buttonClicked?.Invoke(_data));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Furniture Sprite Database", menuName = "Scriptable Object/Furniture Sprite Database", order = int.MaxValue)]
public class FurnitureSpriteDatabase : ScriptableObject
{
    [SerializeField] private List<FurnitureSpriteData> _furnitureSpriteDataList;
    public List<FurnitureSpriteData> FurnitureSpriteDataList => _furnitureSpriteDataList;

    public Sprite GetSprite(string id)
    {
        foreach(FurnitureSpriteData data in _furnitureSpriteDataList)
        {
            if (data.Id == id)
                return data.Sprite;
        }

        Debug.Log("이미지를 찾을 수 없습니다.");
        return null;
    }
}

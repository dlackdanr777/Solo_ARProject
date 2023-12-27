using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FurnitureData
{
    private string _id;
    public string Id => _id;

    private string _name;
    public string Name => _name;

    private GameObject _prefab;
    public GameObject Prefab => _prefab;

    private Sprite _sprite;
    public Sprite Sprite => _sprite;


    public FurnitureData(string id, string name)
    {
        _id = id;
        _name = name;

        _prefab = (GameObject)Resources.Load("Furnitures/" + _name);
        _sprite = DatabaseManager.Instance.FurnitureSpriteDatabase.GetSprite(id);
    }
}

[Serializable]
public class FurnitureSpriteData
{
    [SerializeField] private string _id;
    public string Id => _id;

    [SerializeField] private  Sprite _sprite;
    public Sprite Sprite => _sprite;
}


public class DatabaseManager : SingletonHandler<DatabaseManager>
{
    [SerializeField] private FurnitureSpriteDatabase _furnitureSpriteDatabase;
    public FurnitureSpriteDatabase FurnitureSpriteDatabase => _furnitureSpriteDatabase;

    private FirebaseFirestore _firestore;

    private Dictionary<string, FurnitureData> _furnitureDataDic = new Dictionary<string, FurnitureData>();

    

    public override void Awake()
    {
        base.Awake();
    }


    public void Start()
    {
        _firestore = FirebaseFirestore.DefaultInstance;
        ReadFurnitureData();
    }


    public FurnitureData GetFurnitureDataByID(string id)
    {
        return _furnitureDataDic[id];
    }

    public Dictionary<string, FurnitureData> GetFurnitureDataDic()
    {
        return _furnitureDataDic;
    }


    private void ReadFurnitureData()
    {
        Debug.Log("Load Firestore...");
        TestCanvas.Instance.SetText("Load Firestore...");
        CollectionReference itemRef = _firestore.Collection("Furniture");

        itemRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {

            QuerySnapshot snapshots = task.Result;

            foreach (DocumentSnapshot document in snapshots.Documents)
            {

                Dictionary<string, object> dic = document.ToDictionary();

                string id = dic["id"].ToString();
                string name = dic["name"].ToString();

                FurnitureData data = new FurnitureData(id, name);
                _furnitureDataDic.Add(id, data);
            }

            TestCanvas.Instance.SetText("Load Complated!");
        });
    } 
}

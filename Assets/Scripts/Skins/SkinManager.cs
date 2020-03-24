using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static bool uFirst;
    public static string nick1;
    public static string nick2;
    [Header("First Player skins")]
    public static SkinItem envieronment1;
    public static SkinItem bird1;
    public static SkinItem crystal1;
    [Header("Second Player skins")]
    public static SkinItem envieronment2;
    public static SkinItem bird2;
    public static SkinItem crystal2;
    [Space]
    public List<SkinItem> skins;
    private void Start()
    {
        for (int i = 0; i < skins.Count; i++)
        {
            skins[i].own = false;
            skins[i].equiped = false;
        }
        RequestController.IncCount();
        new GameSparks.Api.Requests.LogEventRequest()
          .SetEventKey("getPlayerInventory")
          .Send((response) =>
          {
              RequestController.DecCount();
              string buff = JsonParcer.GetParamByName(response.JSONString, "skins");
              string skinEnvieronment = JsonParcer.GetParamByName(response.JSONString, "skinEnvieronment");
              string skinEnvieronment2 = JsonParcer.GetParamByName(response.JSONString, "skinEnvieronment2");
              string skinCrystal = JsonParcer.GetParamByName(response.JSONString, "skinCrystal");
              string skinBird = JsonParcer.GetParamByName(response.JSONString, "skinBird");
              string[] elements = buff.Split(';');
              foreach (var item in elements)
              {
                  SkinItem skinItem = skins.Find(u => u.name == item);
                  skinItem.own = true;
                  if (item == skinEnvieronment)
                  {
                      skinItem.equiped = true;
                      skinItem.priority = true;
                      Player.skinEnvieronment = skinItem;
                  }
                  if (item == skinEnvieronment2)
                  {
                      skinItem.equiped = true;
                      skinItem.priority = false;
                      Player.skinEnvieronment2 = skinItem;
                  }
                  if (item == skinCrystal)
                  {
                      skinItem.equiped = true;
                      Player.skinCrystal = skinItem;
                  }
                  if (item == skinBird)
                  {
                      skinItem.equiped = true;
                      Player.skinBird = skinItem;
                  }
              }
              RequestController.IncCount();
              RequestController.DecCount();
          });


        for (int i = 0; i < skins.Count; i++)
        {
            skins[i].GetDataFromServerByName();
        }
    }


    public void SortSkins()
    {
        skins.Sort((x1, x2) =>
            x1.type == x2.type ?
            x1.price.CompareTo(x2.price) :
            x1.type.CompareTo(x2.type));
    }
}

[System.Serializable]
public class SkinItem
{
    public int id;
    public SkinType type;
    public string name;
    public bool own;
    public bool equiped;
    public bool priority;
    public int price;
    public CurrencyType currency;
    public RarityType rarity;
    public GameObject prefab;
    public GameObject prefab1;
    public GameObject prefab4card;

    public void GetDataFromServerByName()
    {
        RequestController.IncCount();
        new GameSparks.Api.Requests.LogEventRequest()
           .SetEventKey("getSkinByName")
           .SetEventAttribute("name", name)
           .Send((response) =>
           {
               RequestController.DecCount();
               if (!response.HasErrors)
               {
                   Debug.Log("Setting Achieved: " + response.JSONString);
                   price = int.Parse(JsonParcer.GetParamByName(response.JSONString, "price"));
               }
               else
               {
                   GetDataFromServerByName();
                   Debug.Log("Error Getting Settings");
               }
           });
    }
}

public enum SkinType
{
    envieronment, bird, crystal
}

public enum CurrencyType
{
    gold, gem
}
public enum RarityType
{
    normal, rare, epic
}



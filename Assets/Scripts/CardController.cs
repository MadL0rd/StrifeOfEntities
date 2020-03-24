using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    private GameObject controller;
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
    }

    private SkinItem currentSkin;

    public Text nameText;
    public Text price;
    public GameObject currency;
    public GameObject item;
    public GameObject buyButton;
    public GameObject equipButton;
    public GameObject ownImage;
    public GameObject priority1;
    public GameObject priority2;

    public void SetContentShop(SkinItem skin, Image currencyImg)
    {
        currentSkin = skin;
        nameText.text = skin.name;
        if (!skin.own)
        {
            price.text = skin.price.ToString();
            Instantiate(currencyImg, currency.transform);
        }
        else
        {
            price.text = "";
        }
        Instantiate(skin.prefab4card, item.transform);
        equipButton.SetActive(false);
        buyButton.SetActive(!skin.own);
        ownImage.SetActive(skin.own);
    }
    public void SetContentInventory(SkinItem skin, Image currencyImg)
    {
        currentSkin = skin;
        nameText.text = skin.name;
        Instantiate(skin.prefab4card, item.transform);
        buyButton.SetActive(false);
        equipButton.SetActive(!skin.equiped);
        if (skin.type == SkinType.envieronment)
        {
            if (skin.priority)
            {
                priority1.SetActive(skin.equiped);
            }
            else
            {
                priority2.SetActive(skin.equiped);
            }
        }
        else
        {
            ownImage.SetActive(skin.equiped);
        }
        
        price.text = "";
    }

    public void SendPreviewSkin()
    {
        controller.GetComponent<MenuController>().PreviewSkin(currentSkin);
    }

    public void EquipSkin()
    {
        RequestController.IncCount();
        new GameSparks.Api.Requests.LogEventRequest()
          .SetEventKey("equipSkin")
          .SetEventAttribute("skinName", currentSkin.name)
          .Send((response) =>
          {
              
              string buff = JsonParcer.GetParamByName(response.JSONString, "status");
              if (buff == "completed")
              {
                  SkinItem prevSkin = new SkinItem();
                  switch (currentSkin.type)
                  {
                      case SkinType.envieronment:
                          prevSkin = Player.skinEnvieronment;
                          break;
                      case SkinType.bird:
                          prevSkin = Player.skinBird;
                          break;
                      case SkinType.crystal:
                          prevSkin = Player.skinCrystal;
                          break;
                  }
                  prevSkin = GameObject.FindWithTag("GameController").GetComponent<SkinManager>().skins.Find(u => u == prevSkin);
                  if (prevSkin.type == SkinType.envieronment)
                  {
                      Player.skinEnvieronment2.equiped = false;
                      Player.skinEnvieronment2 = prevSkin;
                      prevSkin.priority = false;
                  }
                  else
                  {
                      prevSkin.equiped = false;
                  }
                  



                  switch (currentSkin.type)
                  {
                      case SkinType.envieronment:
                          Player.skinEnvieronment = currentSkin;
                          break;
                      case SkinType.bird:
                          Player.skinBird = currentSkin;
                          break;
                      case SkinType.crystal:
                          Player.skinCrystal = currentSkin;
                          break;
                  }
                  currentSkin = GameObject.FindWithTag("GameController").GetComponent<SkinManager>().skins.Find(u => u == currentSkin);
                  currentSkin.equiped = true;
                  currentSkin.priority = true;
              }
              RequestController.DecCount();
          });
    }
    public void EquipSkin2()
    {
        switch (currentSkin.type)
        {
            case SkinType.envieronment:
                SkinManager.envieronment2 = currentSkin;
                break;
            case SkinType.bird:
                SkinManager.bird2 = currentSkin;
                break;
            case SkinType.crystal:
                SkinManager.crystal2 = currentSkin;
                break;
            default:
                break;
        }
    }

    public void BuySkin()
    {
        if (Player.gold >= currentSkin.price)
        {
            RequestController.IncCount();
            new GameSparks.Api.Requests.LogEventRequest()
              .SetEventKey("buySkin")
              .SetEventAttribute("skinName", currentSkin.name)
              .Send((response) =>
              {
                  
                  string buff = JsonParcer.GetParamByName(response.JSONString, "status");
                  if (buff == "completed")
                  {
                      GameObject.FindWithTag("GameController").GetComponent<SkinManager>().skins.Find(u => u == currentSkin).own = true;
                      Player.gold -= currentSkin.price;
                  }
                  RequestController.DecCount();
              });
        }
    }

}

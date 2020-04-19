using GameSparks.Api;
using GameSparks.Api.Messages;
using GameSparks.Core;
using GameSparksTutorials;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject MainMenu;
    public GameObject Shop;
    public GameObject Inventory;
    public GameObject BackButton;
    public GameObject RequestPanel;
    public GameObject OpponentFinding;
    public GameObject AcknowledgmentPanel;
    public Text nick;

    [Space]
    public Text Currency;
    private GameObject currentMenuPanel;

    [Space]
    [Header("Skin showing places")]
    public GameObject env;
    public GameObject bird;
    public GameObject crystal;

    [Space]
    [Header("Card spawn")]
    public GameObject SpawnPlaceShop;
    public GameObject SpawnPlaceInventory;
    public GameObject Card;
    public Image CurrencyImg;

    private List<SkinItem> skins;

    private bool timerOn;
    private float timer;
    private float timerBorder = 40;
    void Start()
    {
        UnityEngine.Debug.Log("Rot ebal!!!");
        timerOn = false;

        timer = 0;
        // RequestController.ResetCount();

        GSMessageHandler._AllMessages = HandleGameSparksMessageReceived;

        Player.GetDataFromGS();

        TriggerCheck = true;
        currentMenuPanel = MainMenu;
        ShowCurrent();

        if (skins == null)
        {
            skins = this.GetComponent<SkinManager>().skins;
        }


        firstRefrash = true;
    }

    private void CurrentGameCheck()
    {
        new GameSparks.Api.Requests.LogEventRequest()
           .SetEventKey("CurrentGameCheck")
           .Send((response) =>
           {
               if (response.HasErrors)
               {
                   CurrentGameCheck();
                   return;
               }

           });
    }

    private void GameWithBot()
    {
        new GameSparks.Api.Requests.LogEventRequest()
           .SetEventKey("BotFight")
           .Send((response) =>
           {
               if (response.HasErrors)
               {
                   GameWithBot();
                   return;
               }

           });
    }

    public void fightAcknowledgment()
    {
        new GameSparks.Api.Requests.LogEventRequest()
           .SetEventKey("fightAcknowledgment")
           .Send((response) =>
           { });
        OpponentFinding.SetActive(true);
    }
    void HandleGameSparksMessageReceived(GSMessage message)
    {
        string status = JsonParcer.GetParamByName(message.JSONString, "status");
        if (status == "opponentFinded")
        {
            OpponentFinding.SetActive(false);
            ChangeMenuPanel(AcknowledgmentPanel);
        }
        if (status == "StartFight")
        {
            string opponentEnvieronment = JsonParcer.GetParamByName(message.JSONString, "skinEnvieronment");
            string opponentCrystal = JsonParcer.GetParamByName(message.JSONString, "skinCrystal");
            string opponentBird = JsonParcer.GetParamByName(message.JSONString, "skinBird");
            string uFirst = JsonParcer.GetParamByName(message.JSONString, "uFirst");
            string oNick = JsonParcer.GetParamByName(message.JSONString, "nick");

            SkinItem oEnvieronment = skins.Find(u => u.name == opponentEnvieronment);
            SkinItem oCrystal = skins.Find(u => u.name == opponentCrystal);
            SkinItem oBird = skins.Find(u => u.name == opponentBird);

            if (uFirst == "true")
            {
                SkinManager.uFirst = true;

                SkinManager.envieronment1 = Player.skinEnvieronment;
                SkinManager.bird1 = Player.skinBird;
                SkinManager.crystal1 = Player.skinCrystal;
                SkinManager.nick1 = Player.displayName;

                SkinManager.envieronment2 = oEnvieronment;
                SkinManager.bird2 = oBird;
                SkinManager.crystal2 = oCrystal;
                SkinManager.nick2 = oNick;
            }
            else
            {
                SkinManager.uFirst = false;

                SkinManager.envieronment2 = Player.skinEnvieronment.name == oEnvieronment.name ? Player.skinEnvieronment2 : Player.skinEnvieronment;
                SkinManager.bird2 = Player.skinBird;
                SkinManager.crystal2 = Player.skinCrystal;
                SkinManager.nick2 = Player.displayName;

                SkinManager.envieronment1 = oEnvieronment;
                SkinManager.bird1 = oBird;
                SkinManager.crystal1 = oCrystal;
                SkinManager.nick1 = oNick;
            }

            GameObject.FindWithTag("SceneLoader").GetComponent<SceneLoading>().LoadGameScene();
        }
    }
    public void FindOpponent()
    {
        OpponentFinding.SetActive(true);
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("FindAnOpponent")
            .Send((response) =>
            { });

        timerOn = true;
    }

    public void ShowLoading()
    {
        RequestPanel.SetActive(true);
    }

    private bool firstRefrash = true;
    public void RefrashUI()
    {
        nick.text = Player.displayName;
        DestroyChilds(SpawnPlaceShop);
        DestroyChilds(SpawnPlaceInventory);
        Currency.text = Player.gold.ToString();
        this.GetComponent<SkinManager>().SortSkins();
        foreach (var item in skins)
        {
            Instantiate(Card, SpawnPlaceShop.transform).GetComponent<CardController>().SetContentShop(item, CurrencyImg);
            if (item.own == true)
            {
                Instantiate(Card, SpawnPlaceInventory.transform).GetComponent<CardController>().SetContentInventory(item, CurrencyImg);
            }
        }
        RequestPanel.SetActive(false);

        PreviewSkin(Player.skinBird);
        PreviewSkin(Player.skinCrystal);
        PreviewSkin(Player.skinEnvieronment);

        if (firstRefrash)
        {
            firstRefrash = false;
            CurrentGameCheck();
        }
    }

    public void ShowShop()
    {
        ChangeMenuPanel(Shop);
    }
    public void ShowInventory()
    {
        ChangeMenuPanel(Inventory);
    }
    public void Back2Main()
    {
        ChangeMenuPanel(MainMenu);
    }

    private void ChangeMenuPanel(GameObject target)
    {
        if (currentMenuPanel != null)
        {
            currentMenuPanel.GetComponent<Animator>().SetTrigger("Show");
        }
        currentMenuPanel = target;
        TriggerCheck = true;
        if (currentMenuPanel != MainMenu)
        {
            BackButton.GetComponent<Animator>().SetTrigger("Show");
        }
        else
        {
            BackButton.GetComponent<Animator>().SetTrigger("Show");
        }
    }

    private bool TriggerCheck;
    public void ShowCurrent()
    {
        if (TriggerCheck)
        {
            currentMenuPanel.GetComponent<Animator>().SetTrigger("Show");
            TriggerCheck = false;
        }
    }
    private void DestroyChilds(GameObject target)
    {
        foreach (Transform item in target.GetComponent<Transform>())
        {
            if (item != null || item != target.transform)
            {
                Destroy(item.gameObject);
            }
        }
    }
    public void PreviewSkin(SkinItem skin)
    {
        switch (skin.type)
        {
            case SkinType.envieronment:
                DestroyChilds(env);
                Instantiate(skin.prefab4card, env.transform);
                break;
            case SkinType.bird:
                DestroyChilds(bird);
                Instantiate(skin.prefab4card, bird.transform).GetComponent<BirdScript>().DisableRotation = true;
                break;
            case SkinType.crystal:
                DestroyChilds(crystal);
                Instantiate(skin.prefab, crystal.transform).GetComponent<Animator>().SetBool("Enabled", true);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (timerOn)
        {
            timer += Time.deltaTime;
            if (timer >= timerBorder)
            {
                timerOn = false;
                timer = 0;

                GameWithBot();
            }
        }
    }
}

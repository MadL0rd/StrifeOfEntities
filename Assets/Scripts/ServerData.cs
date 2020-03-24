using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using GameSparksTutorials;

public class Player:MonoBehaviour
{
    public static IList<string> achievements;
    public static GSData currencies;
    public static long? gold;
    public static long? gems;
    public static string displayName;
    public static GSData externalIds;
    public static AccountDetailsResponse._Location location;
    public static string userId;
    public static GSData virtualGoods;
    public static SkinItem skinEnvieronment;
    public static SkinItem skinEnvieronment2;
    public static SkinItem skinCrystal;
    public static SkinItem skinBird;

    public static void GetDataFromGS()
    {
        RequestController.IncCount();
        new AccountDetailsRequest()
        .Send((response) =>
        {
            RequestController.DecCount();
            if (!response.HasErrors)
            {
                achievements = response.Achievements;
                currencies = response.Currencies;
                gold = response.Currency1;
                gems = response.Currency2;
                displayName = response.DisplayName;
                externalIds = response.ExternalIds;
                location = response.Location;
                userId = response.UserId;
                virtualGoods = response.VirtualGoods;

                RequestController.IncCount();
                RequestController.DecCount();
            }
            else
            {
                GetDataFromGS();
                Debug.Log("Error Getting Settings");
            }
        });
    }
}



public class JsonParcer : MonoBehaviour
{
    public static string GetParamByName(string json, string param)
    {
        param = '\"' + param + '\"';
        if (json.Contains(param))
        {
            string buff = json.Substring(json.IndexOf(param) + param.Length + 1);
            if (buff.IndexOf("\"") == 0)
            {
                buff = buff.Substring(1);
            }
            int index = Int32.MaxValue;
            string format = "\",}\\";
            for (int i = 0; i < format.Length; i++)
            {
                int buffIndex = buff.IndexOf(format[i]);
                if (buffIndex != -1 && buffIndex < index) index = buffIndex;
            }
            buff = buff.Substring(0, index);
            return buff;
        }
        else
        {
            return null;
        }
    }
    public static string GetParamArrayByName(string json, string param)
    {
        param = '\"' + param + '\"';
        if (json.Contains(param))
        {
            string buff = json.Substring(json.IndexOf(param) + param.Length + 1);
            int index = Int32.MaxValue;
            string format = "\"}\\";
            for (int i = 0; i < format.Length; i++)
            {
                int buffIndex = buff.IndexOf(format[i]);
                if (buffIndex != -1 && buffIndex < index) index = buffIndex;
            }
            buff = buff.Substring(0, index + 1);
            return buff;
        }
        else
        {
            return null;
        }
    }
}

public static class RequestController
{
    private static int currentRequestsCount = 0;
    public static void ResetCount()
    {
        currentRequestsCount = 0;
    }
    public static void IncCount()
    {
        if (!HaveActiveRequests())
        {
            GameObject.FindWithTag("GameController").GetComponent<MenuController>().ShowLoading();
        }
        currentRequestsCount++;
    }
    public static void DecCount()
    {
        currentRequestsCount--;
        if (!HaveActiveRequests())
        {
            GameObject.FindWithTag("GameController").GetComponent<MenuController>().RefrashUI();
        }
    }
    public static bool HaveActiveRequests()
    {
        if (currentRequestsCount < 0)
        {
            currentRequestsCount = 0;
        }
        if (currentRequestsCount == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
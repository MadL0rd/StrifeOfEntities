using GameSparks.Api;
using GameSparks.Api.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameControlScript : MonoBehaviour {

    [Header("EnviromentSettings")]
    [Range(0, 10)]
    public float distanceBig;
    [Range(0, 10)]
    public float distanceSmall;
    [Range(0, 10)]
    public float birdHeight;
    [Range(0, 10)]
    public float birdRadius;
    [Space]
    public GameObject xСrystalPlace;
    public GameObject oСrystalPlace;
    [Space]
    public GameObject xBirdPlace;
    public GameObject oBirdPlace;
    [Space]
    public GameObject xSmallSkinPlace;
    public GameObject oSmallSkinPlace;
    [Space]
    [Header("Skins")]
    public bool SkinImport;
    [Space]
    [Header("EnvironmentSkins")]
    public GameObject environmentSmallSkin;
    public GameObject environmentBigSkin;
    [Space]
    [Header("PlayersSkins")]
    public GameObject xSmallSkin;
    public GameObject oSmallSkin;
    [Space]
    public GameObject xBigSkin;
    public GameObject oBigSkin;
    [Space]
    public GameObject xСrystalSkin;
    public GameObject oСrystalSkin;
    [Space]
    public GameObject xBirdSkin;
    public GameObject oBirdSkin;
    [Space]
    [Header("UI")]
    public Text Nick1;
    public Text Nick2;
    public GameObject menuButton;
    public List<GameObject> otherButtons;
    public GameObject surrenderPanel;
    public GameObject endGamePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject OpponentWaiting;


    private bool gameEnabled;
    private bool stepState;
    private bool uFirst;
    private GameObject currentTarget;
    private int nextStepPlane;

    private int bigIndexBuff;
    private int smallIndexBuff;

    [Space]
    [Header("Timer")]
    private float timer;
    private int timeout;
    public int StepMaxTime;
    public Text timerTextPlayer1;
    public Text timerTextPlayer2;

    private Animator xСrystalAnimator;
    private Animator oСrystalAnimator;

    private GameObject xBird;
    private GameObject oBird;

    private List<GameObject> Planes;

    private List<List<int>> xData = new List<List<int>>();
    private List<List<int>> oData = new List<List<int>>();

    private int[,] winCombinations = new int[8, 3] {
            {1, 2, 3},
            {1, 4, 7},
            {1, 5, 9},
            {2, 5, 8},
            {3, 6, 9},
            {3, 5, 7},
            {4, 5, 6},
            {7, 8, 9}
        };
    private bool WinCheck(List<int> currentList)
    {
        if (currentList.Count < 3)
        {
            return false;
        }

        for (int i = 0; i < 8; i++)
        {
            if (currentList.Contains(winCombinations[i, 0]) && 
                currentList.Contains(winCombinations[i, 1]) && 
                currentList.Contains(winCombinations[i, 2]))
            {
                return true;
            }
        }
        return false;
    }

    private void Start()
    {
        GSMessageHandler._AllMessages = HandleGameSparksMessageReceived;
        menuButton.GetComponent<Animator>().SetTrigger("Show");
        timer = 0;
        timeout = -1;
        //загрузка скинов
        if (SkinImport)
        {
            try
            {
                xSmallSkin = SkinManager.envieronment1.prefab1;
                oSmallSkin = SkinManager.envieronment2.prefab1;
                xBigSkin = SkinManager.envieronment1.prefab;
                oBigSkin = SkinManager.envieronment2.prefab;
                xСrystalSkin = SkinManager.crystal1.prefab;
                oСrystalSkin = SkinManager.crystal2.prefab;
                xBirdSkin = SkinManager.bird1.prefab;
                oBirdSkin = SkinManager.bird2.prefab;
                Nick1.text = SkinManager.nick1;
                Nick2.text = SkinManager.nick2;
                uFirst = SkinManager.uFirst;
                if (!uFirst)
                {
                    OpponentWaiting.SetActive(true);
                }

                Instantiate(SkinManager.envieronment1.prefab4card, xSmallSkinPlace.transform);
                Instantiate(SkinManager.envieronment2.prefab4card, oSmallSkinPlace.transform);
            }
            catch (System.Exception)
            {
                SkinImport = false;
            }
        }

        //заполнение полей
        gameEnabled = false;
        stepState = true;
        nextStepPlane = 0;
        for (int i = 0; i < 10; i++)
        {
            xData.Add(new List<int>());
            oData.Add(new List<int>());
        }

        //спавн объектов
        xСrystalAnimator = Instantiate(xСrystalSkin, xСrystalPlace.transform).GetComponent<Animator>();
        oСrystalAnimator = Instantiate(oСrystalSkin, oСrystalPlace.transform).GetComponent<Animator>();

        xBird = Instantiate(xBirdSkin, xBirdPlace.transform);
        oBird = Instantiate(oBirdSkin, oBirdPlace.transform);

        if (!SkinImport)
        {
            Instantiate(xSmallSkin, xSmallSkinPlace.transform);
            Instantiate(oSmallSkin, oSmallSkinPlace.transform);
        }

        Planes = new List<GameObject>();
        distanceSmall += environmentSmallSkin.transform.lossyScale.z;
        for (int i = 0; i < 9; i++)
        {
            GameObject planePlace = new GameObject((i+1)+ " PlanePlace");
            planePlace.transform.SetParent(this.transform);
            planePlace.transform.localScale = Vector3.one;
            GameObject planeBuff = Instantiate(environmentBigSkin, planePlace.transform);

            Planes.Add(planeBuff);
            planeBuff.name = (i + 1) + " Plane";
            Vector3 planePosition = this.transform.position;
            planePosition.x += (i % 3 - 1) * distanceBig;
            planePosition.z += (i / 3 - 1) * distanceBig;
            planePlace.transform.position = planePosition;
            planePlace.transform.rotation *= Quaternion.Euler(0, Random.Range(0, 3) * 90, 0);
            for (int j = 0; j < 9; j++)
            {
                GameObject smallBuff = Instantiate(environmentSmallSkin, planeBuff.transform);
                smallBuff.name = (i + 1) + " " + (j + 1) + " Square";
                Vector3 smallPosition = planePosition;
                smallPosition.x += (j % 3 - 1) * distanceSmall;
                smallPosition.z += (j / 3 - 1) * distanceSmall;
                smallBuff.transform.position = smallPosition;
                smallBuff.transform.rotation *= Quaternion.Euler(0, Random.Range(0, 3) * 90, 0);
            }
        }

    }

    //Server
    void HandleGameSparksMessageReceived(GSMessage message)
    {
        string status = JsonParcer.GetParamByName(message.JSONString, "status");
        if (status == "DoStep")
        {
            int bigIndex = int.Parse(JsonParcer.GetParamByName(message.JSONString, "bigIndex"));
            int smallIndex = int.Parse(JsonParcer.GetParamByName(message.JSONString, "smallIndex"));
            string timeoutStr = JsonParcer.GetParamByName(message.JSONString, "timeout");
            if (timeoutStr != null)
            {
                timeout = int.Parse(timeoutStr);
                bigIndexBuff = bigIndex;
                smallIndexBuff = smallIndex;
            }
            else DoStep(bigIndex, smallIndex);
        }
        if (status == "endGame")
        {
            gameEnabled = false;

            string win = JsonParcer.GetParamByName(message.JSONString, "win");
            endGamePanel.SetActive(true);
            if (win == "true")
            {
                winPanel.SetActive(true);
                losePanel.SetActive(false);
            }
            else
            {
                winPanel.SetActive(false);
                losePanel.SetActive(true);
            }
        }
    }
    private void StepRequest(int bigIndex, int smallIndex)
    {
        SetTarget(null);
        new GameSparks.Api.Requests.LogEventRequest()
          .SetEventKey("DoStep")
          .SetEventAttribute("bigIndex", bigIndex)
          .SetEventAttribute("smallIndex", smallIndex)
          .Send((response) =>
          {

          });
    }
    public void WinRequestTimer()
    {
        new GameSparks.Api.Requests.LogEventRequest()
          .SetEventKey("endGameTimer")
          .Send((response) =>
          {
              if (response.HasErrors)
              {
                  WinRequestTimer();
                  return;
              }
              string buff = JsonParcer.GetParamByName(response.JSONString, "status");
              if (buff == "canceled")
              {
                  WinRequestTimer();
              }
              if (buff == "completed")
              {
                  endGamePanel.SetActive(true);
                  winPanel.SetActive(true);
                  losePanel.SetActive(false);
              }
          });
    }


    //Game
    public void StartGame()
    {
        new GameSparks.Api.Requests.LogEventRequest()
         .SetEventKey("GetCurrentGame")
         .Send((response) =>
         {
             if (response.HasErrors)
             {
                 StartGame();
                 return;
             }

             string status = JsonParcer.GetParamByName(response.JSONString, "status");
             if (status == "HaveGame")
             {
                 string buff = JsonParcer.GetParamByName(response.JSONString, "stepState");

                 if (buff == "true") stepState = false; else stepState = true;

                 buff = JsonParcer.GetParamByName(response.JSONString, "nextStepField");
                 nextStepPlane = Convert.ToInt32(buff);

                 ChangeEnabledPlayer();

                 buff = JsonParcer.GetParamByName(response.JSONString, "timer");
                 timer = (float)Convert.ToInt32(buff) / 1000;

                 buff = JsonParcer.GetParamArrayByName(response.JSONString, "battlefieldX");
                 for (int i = 0, count = 0; count < 10 && i < buff.Length; i++)
                 {
                     if (buff[i] == ']')
                     {
                         count++;
                         continue;
                     }
                     if (buff[i] >= '1' && buff[i] <= '9')
                     {
                         int index = buff[i] - '0';
                         if (count == 0)
                         {
                             xData[0].Add(index);

                             Transform ground = Planes[index - 1].transform.GetChild(0);
                             Instantiate(xBigSkin, ground);
                             Planes[index - 1].GetComponent<Animator>().SetTrigger("Conquest");
                         }
                         else
                         {
                             string name = count + " " + index + " Square";
                             currentTarget = GameObject.Find(name);
                             currentTarget.GetComponent<BoxCollider>().enabled = false;
                             Instantiate(xSmallSkin, currentTarget.transform);
                             xData[count].Add(index);
                         }
                     }
                 }

                 buff = JsonParcer.GetParamArrayByName(response.JSONString, "battlefieldO");
                 for (int i = 0, count = 0; count < 10 && i < buff.Length; i++)
                 {
                     if (buff[i] == ']')
                     {
                         count++;
                         continue;
                     }
                     if (buff[i] >= '1' && buff[i] <= '9')
                     {
                         int index = buff[i] - '0';
                         if (count == 0)
                         {
                             oData[0].Add(index);

                             Transform ground = Planes[index - 1].transform.GetChild(0);
                             Instantiate(oBigSkin, ground);
                             Planes[index - 1].GetComponent<Animator>().SetTrigger("Conquest");
                         }
                         else
                         {
                             string name = count + " " + index + " Square";
                             currentTarget = GameObject.Find(name);
                             currentTarget.GetComponent<BoxCollider>().enabled = false;
                             Instantiate(oSmallSkin, currentTarget.transform);
                             oData[count].Add(index);
                         }
                     }
                 }
             }
             else
             {
                 xСrystalAnimator.SetBool("Enabled", stepState);
             }

             gameEnabled = true;

         });

    }
    public void SetTarget(GameObject target)
    {
        if (gameEnabled && uFirst == stepState)
        {
            if (target != null)
            {
                int bigIndex = int.Parse(target.name.Substring(0, 1));
                int smallIndex = int.Parse(target.name.Substring(2, 1));

                //проверка выбраного поля
                if (nextStepPlane != 0 && nextStepPlane != bigIndex)
                {
                    return;
                }

                //если нажали 2 раз на то же поле, то сделать ход
                if (currentTarget == target)
                {
                    StepRequest(bigIndex, smallIndex);
                    return;
                }
            }

            //если цель была, то отменить анимацию
            if (currentTarget != null)
            {
                currentTarget.GetComponent<Animator>().SetBool("IsTarget", false);
            }

            currentTarget = target;

            if (currentTarget != null)
            {
                currentTarget.GetComponent<Animator>().SetBool("IsTarget", true);
            }
        }
    }
    public void DoStep(int bigIndex, int smallIndex)
    {
        string name = bigIndex + " " + smallIndex + " Square";
        currentTarget = GameObject.Find(name);
        currentTarget.GetComponent<BoxCollider>().enabled = false;
        timer = 0;

        if (stepState)
        {
            Instantiate(xSmallSkin, currentTarget.transform);
            xData[bigIndex].Add(smallIndex);
            //если собрана линия и поле не было захвачено другим игроком
            if (WinCheck(xData[bigIndex]) && !oData[0].Contains(bigIndex))
            {
                xData[0].Add(bigIndex);

                Transform ground = Planes[bigIndex - 1].transform.GetChild(0);
                Instantiate(xBigSkin, ground);
                Planes[bigIndex - 1].GetComponent<Animator>().SetTrigger("Conquest");


                //проверка на полную победу
                if (WinCheck(xData[0]))
                {
                    EndOfGame();
                    return;
                }
            }
        }
        else
        {
            Instantiate(oSmallSkin, currentTarget.transform);
            oData[bigIndex].Add(smallIndex);
            //если собрана линия и поле не было захвачено другим игроком
            if (WinCheck(oData[bigIndex]) && !xData[0].Contains(bigIndex))
            {
                oData[0].Add(bigIndex);

                Transform ground = Planes[bigIndex - 1].transform.GetChild(0);
                Instantiate(oBigSkin, ground);
                Planes[bigIndex - 1].GetComponent<Animator>().SetTrigger("Conquest");

                //проверка на полную победу
                if (WinCheck(oData[0]))
                {
                    EndOfGame();
                    return;
                }
            }
        }

        nextStepPlane = smallIndex;
        if (xData[nextStepPlane].Count + oData[nextStepPlane].Count == 9)
        {
            nextStepPlane = 0;
        }

        SetTarget(null);

        ChangeEnabledPlayer();
    }
    private void ChangeEnabledPlayer()
    {
        stepState = !stepState;
        xСrystalAnimator.SetBool("Enabled", stepState);
        oСrystalAnimator.SetBool("Enabled", !stepState);
        if (nextStepPlane == 0)
        {
            oBird.GetComponent<BirdScript>().FlyHome();
            xBird.GetComponent<BirdScript>().FlyHome();
            return;
        }
        if (stepState)
        {
            xBird.GetComponent<BirdScript>().FlyCircle(Planes[nextStepPlane-1], birdRadius, birdHeight, false);
            oBird.GetComponent<BirdScript>().FlyHome();
        }
        else
        {
            oBird.GetComponent<BirdScript>().FlyCircle(Planes[nextStepPlane - 1], birdRadius, birdHeight, true);
            xBird.GetComponent<BirdScript>().FlyHome();
        }
        OpponentWaiting.SetActive(stepState != uFirst);
    }
    private void EndOfGame()
    {
        OpponentWaiting.SetActive(false);
        SetTarget(null);
        gameEnabled = false;
        oBird.GetComponent<BirdScript>().FlyHome();
        xBird.GetComponent<BirdScript>().FlyHome();

        //Включать только в сингл плеере
        //WinRequest();
    }
    private void Update()
    {
        if (gameEnabled)
        {
            timer += Time.deltaTime;
            if (timer >= StepMaxTime)
            {
                if (uFirst != stepState)
                {
                    WinRequestTimer();
                }

                if (stepState)
                {
                    timerTextPlayer1.text = "0";
                    timerTextPlayer2.text = "Wait";
                }
                else
                {
                    timerTextPlayer2.text = "0";
                    timerTextPlayer1.text = "Wait";
                }
            }
            else
            {
                if (stepState)
                {
                    timerTextPlayer1.text = (Math.Truncate(StepMaxTime - timer)).ToString();
                    timerTextPlayer2.text = "Wait";
                }
                else
                {
                    timerTextPlayer2.text = (Math.Truncate(StepMaxTime - timer)).ToString();
                    timerTextPlayer1.text = "Wait";
                }
                
            }
            if (timeout > 0 && timer>=timeout)
            {
                timeout = -1;
                DoStep(bigIndexBuff, smallIndexBuff);
            }
        }
    }

    //UI control
    public void ButtonsView()
    {
        for (int i = 0; i < otherButtons.Count; i++)
        {
            otherButtons[i].GetComponent<Animator>().SetTrigger("Show");
        }
    }
    public void ShowSurrenderPanel()
    {
        surrenderPanel.GetComponent<Animator>().SetTrigger("ShowUp");
    }
    public void Surrender()
    {
        ShowSurrenderPanel();
        new GameSparks.Api.Requests.LogEventRequest()
          .SetEventKey("surrender")
          .Send((response) =>
          {

          });
    }
}

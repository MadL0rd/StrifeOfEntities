using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLog : MonoBehaviour
{
    public Text infoText;

    public static void GlobalMessage(string msg)
    {
        foreach (var item in FindObjectsOfType<PlayerLog>())
        {
            item.NewLogMessage(msg);
        }
    }
    public void NewLogMessage(string msg)
    {
        Instantiate(infoText, this.transform).text = msg;
    }
}

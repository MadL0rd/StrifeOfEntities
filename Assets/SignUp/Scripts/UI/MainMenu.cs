using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSparksTutorials
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private Text displayName;

        void OnEnable()
        {
            displayName.text = DataController.GetValue<string>("displayName");
        }
    }
}
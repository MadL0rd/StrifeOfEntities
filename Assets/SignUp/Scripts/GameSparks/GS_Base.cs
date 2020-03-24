
using UnityEngine;

// GameSparks
using GameSparks.Core;

namespace GameSparksTutorials
{
    public class GS_Base : AMonoBehaviourSingleton<GS_Base>
    {
        public bool IsAvailable { get; private set; }

        public bool IsAuthenticated { get; private set; }

        private void Awake()
        {
            GS.GameSparksAvailable += OnGameSparksAvailable;
            GS.GameSparksAuthenticated += OnGameSparksAuthenticated;
        }

        private void OnGameSparksAvailable(bool available)
        {
            IsAvailable = available;
        }

        private void OnGameSparksAuthenticated(string authentication)
        {
            IsAuthenticated = true;
        }
    }
}
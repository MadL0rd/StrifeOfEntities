// <copyright file="AMonoBehaviourSingleton.cs">
//   Copyright (c) 2018 Ramón Janousch. All rights reserved.
// </copyright>

#region Usings

using System;

using JetBrains.Annotations;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#endregion

namespace GameSparksTutorials
{
    [Serializable]
    public abstract class AMonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// Locking ensures that one thread does not enter a critical section
        /// of code while another thread is in this critical section.
        /// </summary>
        [NotNull]
        private static readonly object Lock = new object();

        private static bool applicationIsQuitting;

        private static T instance;

        public static T Instance
        {
            get
            {
                lock (Lock)
                {
#if !UNITY_EDITOR
                    if (applicationIsQuitting)
                    {
                        instance = null;

                        Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                            ": Application is quitting and instance is already destroyed therefore returning null.");

                        return null;
                    }
#endif

                    if (instance != null)
                    {
                        return instance;
                    }

                    var sceneObjects = FindObjectsOfType<T>();

                    if (sceneObjects.Length == 0)
                    {
                        var singleton = new GameObject("[Singleton] " + typeof(T).Name, typeof(T));

                        DontDestroyOnLoad(singleton);
                        instance = singleton.GetComponent<T>();

                        Debug.Log("[Singleton] Instance '" + typeof(T) +
                            ": Initialized with new instance.");

                        return instance;
                    }

                    instance = sceneObjects[0];

                    if (sceneObjects.Length > 1)
                    {
                        Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                            ": More than one instance!");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Instance '" + typeof(T) +
                            ": Initialized with scene object.");
                    }

                    return instance;
                }
            }
        }

        private void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }
    }
}
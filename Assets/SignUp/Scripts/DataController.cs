
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

using UnityEngine;

namespace GameSparksTutorials
{
    public static class DataController
    {
        public static void SaveValue(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public static void SaveValue(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        }

        public static void SaveValue(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// T must be a string, float or int.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetValue<T>(string key) where T : IComparable
        {
            if (typeof(T) == typeof(string)) return (T)Convert.ChangeType(PlayerPrefs.GetString(key), typeof(T));
            if (typeof(T) == typeof(float)) return (T)Convert.ChangeType(PlayerPrefs.GetFloat(key), typeof(T));
            if (typeof(T) == typeof(int)) return (T)Convert.ChangeType(PlayerPrefs.GetInt(key), typeof(T));

            return default(T);
        }

        public static void DeleteValue(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameSparks
using GameSparks.Api.Requests;
using GameSparks.Core;

namespace GameSparksTutorials
{
    public enum ESignUpResponse
    {
        EMAILISTAKEN, USERNAMEISTAKEN, SUCCESS, ERROR
    }

    public class GS_Authentication : GS_Base
    {
        public static bool IsUserLoggedIn { get; private set; }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="eventName"></param>
        public static void Login(string username, string password, string eventName)
        {
            Debug.Log("Authentication...");

            var loginRequest = new AuthenticationRequest();

            loginRequest.SetUserName(username);
            loginRequest.SetPassword(password);

            loginRequest.Send(response =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Player authenticated! \n Name: " + response.DisplayName);

                    EventManager.TriggerEvent(eventName, response.DisplayName);

                    IsUserLoggedIn = true;
                } else
                {
                    Debug.Log("Error authenticating player... \n" + response.Errors.JSON.ToString());

                    EventManager.TriggerEvent(eventName, "");
                }
            });
        }

        /// <summary>
        /// Log the user in as a guest.
        /// </summary>
        /// <param name="eventName">The event will be called after the device authentication response.</param>
        public static void DeviceAuthentication(string eventName)
        {
            Debug.Log("Device authentication...");

            var deviceAuthenticationRequest = new DeviceAuthenticationRequest();

            deviceAuthenticationRequest.Send(response =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Player authenticated!\nName: " + response.DisplayName);

                    EventManager.TriggerEvent(eventName, response.DisplayName);

                    IsUserLoggedIn = true;
                } else
                {
                    Debug.Log("Error authenticating player... \n" + response.Errors.JSON.ToString());

                    EventManager.TriggerEvent(eventName, "");
                }
            });
        }

        /// <summary>
        /// Sign up a new player.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="displayName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="eventName"></param>
        public static void SignUp(string username, string displayName, string password, string email, string eventName)
        {
            Debug.Log("Sign up...");

            if (!IsUserLoggedIn)
            {
                var signUpRequest = new RegistrationRequest();

                signUpRequest.SetUserName(username);
                signUpRequest.SetDisplayName(displayName);
                signUpRequest.SetPassword(password);
                signUpRequest.SetSegments(new GSRequestData().AddString("email", email));

                signUpRequest.Send(response =>
                {
                    if (!response.HasErrors)
                    {
                        Debug.Log("Player registration successful!\n Name: " + response.DisplayName);

                        EventManager.TriggerEvent(eventName, response.DisplayName);
                    } else
                    {
                        Debug.Log("Error registrating player... \n" + response.Errors.JSON.ToString());

                        EventManager.TriggerEvent(eventName, "");
                    }
                });
            } else
            {
                UpgradeGuestUser(username, displayName, password, email, eventName);
            }
        }

        private static void UpgradeGuestUser(string username, string displayName, string password, string email, string eventName)
        {
            Debug.Log("Upgrade guest user...");

            var upgradeRequest = new LogEventRequest_upgradeGuestAccount();

            upgradeRequest.Set_username(username);
            upgradeRequest.Set_displayName(displayName);
            upgradeRequest.Set_password(password);
            upgradeRequest.Set_email(email);

            upgradeRequest.Send(response =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Guest upgrade successful!");

                    EventManager.TriggerEvent(eventName, displayName);
                }
                else
                {
                    Debug.Log("Error registrating player... \n" + response.Errors.JSON.ToString());

                    EventManager.TriggerEvent(eventName, "");
                }
            });
        }
    }
}
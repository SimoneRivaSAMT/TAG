using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PlayerPreferences
{
    public class PlayerPreference
    {
        #region SETTINGS
        public const string SETTINGS_GRAPHICS = "Graphic";
        public const string SETTINGS_DISPLAY = "Display";
        public const string SETTINGS_VOLUME = "Volume";
        public const string SETTINGS_SENS_X = "SensX";
        public const string SETTINGS_SENS_Y = "SensY";
        #endregion

        #region MENU
        public const string MENU_IS_FIRST_START = "FirstStart";
        public const string MENU_PRIVACY_ACCEPTED = "PrivacyAccepted";
        #endregion

        #region LOGIN/SIGNUP
        public const string USER_UNAME = "Login_UserName";
        public const string USER_EMAIL = "Login_UserEmail";
        public const string USER_PASS = "Login_UserPass";
        public const string USER_ID = "Login_UserId";
        #endregion
    }
}

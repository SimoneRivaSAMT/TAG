using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.GraphicsQuality {
    public class QualityManager : MonoBehaviour
    {

        [Header("Graphics Settings")]
        public QualityLevel defaultQuiality;

        public static void SetQualityLevel(int qualityIndex)
        {
            PlayerPrefs.SetInt("qualityLevel", qualityIndex);
        }

        private void Start()
        {
            Volume[] qualities = GetComponentsInChildren<Volume>();
            if (!PlayerPrefs.HasKey("qualityLevel"))
            {
                PlayerPrefs.SetInt("qualityLevel", (int)defaultQuiality);
            }
            int quality = PlayerPrefs.GetInt("qualityLevel");
            if(quality < 0 && quality > 5)
            {
                PlayerPrefs.SetInt("qualityLevel", (int)defaultQuiality);
                quality = (int)defaultQuiality;
            }
            for (int i = 0; i <= (int)QualityLevel.ExtraLow; i++)
            {
                if(i == quality)
                {
                    qualities[i].enabled = true;
                }
                else
                {
                    qualities[i].enabled = false;
                }
                
            }
        }
        public enum QualityLevel
        {
            UltraRtx,
            HighRTX,
            High,
            Medium,
            Low,
            Lowest,
            ExtraLow
        }
    }

    
}
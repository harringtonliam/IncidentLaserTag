using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  RPG.GameTime
{
    public class SunIntensityController : MonoBehaviour
    {
        [SerializeField] GameTimeContoller gameTimeController;
        [SerializeField] Light sun;
        [SerializeField] int sunriseHour = 6;
        [SerializeField] int sunsetHour = 18;
        [SerializeField] float dayTimeIntensity = 1f;
        [SerializeField] float dayTimeShadowStrenght = 1f;
        [SerializeField] float nightTimeIntensity = 0.1f;
        [SerializeField] float nightTimeShadowStrenght = 0.1f;
        [SerializeField] float dayTimeEnvironmnetLightingIntensityMultiplier = 1f;
        [SerializeField] float nighTimeEnvironmnetLightingIntensityMultiplier = 0.05f;
        [SerializeField] float duskFraction = 0.5f;




        // Start is called before the first frame update
        void Start()
        {
            gameTimeController.hourHasPassed += SetSunProperties;
        }
        
        private void SetSunProperties()
        {
            if(IsDusk())
            {

            }
            else if (IsDayTime())
            {
                SetDayTimeProperties();
            }
            else
            {
                SetNightTimeProperties();
            }


        }

        private void SetNightTimeProperties()
        {
            sun.intensity = nightTimeIntensity;
            sun.shadowStrength = nightTimeShadowStrenght;
            RenderSettings.ambientIntensity = nighTimeEnvironmnetLightingIntensityMultiplier;


        }

        private void SetDayTimeProperties()
        {
            sun.intensity = dayTimeIntensity;
            sun.shadowStrength = dayTimeShadowStrenght;
            RenderSettings.ambientIntensity = dayTimeEnvironmnetLightingIntensityMultiplier;
        }

        private void SetDuskProperties()
        {
            sun.intensity = dayTimeIntensity * duskFraction;
            sun.shadowStrength = dayTimeShadowStrenght * duskFraction;
            RenderSettings.ambientIntensity = dayTimeEnvironmnetLightingIntensityMultiplier * duskFraction;
        }

        private bool IsDayTime()
        {
            return gameTimeController.CurrentHour > sunriseHour && gameTimeController.CurrentHour < sunsetHour;
        }

        private bool IsDusk()
        {
            return gameTimeController.CurrentHour == sunriseHour || gameTimeController.CurrentHour == sunsetHour;
        }

    }

}


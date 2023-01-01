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




        // Start is called before the first frame update
        void Start()
        {
            gameTimeController.hourHasPassed += SetSunProperties;
        }
        
        private void SetSunProperties()
        {
            if (IsDayTime())
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
        }

        private void SetDayTimeProperties()
        {
            sun.intensity = dayTimeIntensity;
            sun.shadowStrength = dayTimeShadowStrenght;
        }

        private bool IsDayTime()
        {
            return gameTimeController.CurrentHour >= sunriseHour && gameTimeController.CurrentHour <= sunsetHour;
        }

    }

}


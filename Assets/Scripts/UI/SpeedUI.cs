using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;

namespace RPG.UI
{
    public class SpeedUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;

        GameObject player;
        FirstPersonController firstPersonController;
        
        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            firstPersonController = player.GetComponent<FirstPersonController>();
        }

        // Update is called once per frame
        void Update()
        {
            text.text = firstPersonController.Speed.ToString() + " : " + firstPersonController.InputDirection.ToString();

        }
    }

}




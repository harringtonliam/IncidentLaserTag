using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SC.Core
{


    public class VersionNumber : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI versionNumber;

        // Start is called before the first frame update
        void Start()
        {
            versionNumber.text = "Version " + Application.version;
        }

    }

}



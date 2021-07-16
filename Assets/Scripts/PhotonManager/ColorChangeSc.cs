using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photonmanager{
    public class ColorChangeSc : MonoBehaviour
    {
        //[ColorUsage(false, true)] private Color colorHDR;
        CustomPropertiesList _customPropertiesList = new CustomPropertiesList();
        private void Awake()
        {
            float[] colorArray = (float[])GetComponent<PhotonView>().Owner.CustomProperties[_customPropertiesList.colorKey];

            Renderer renderer = GetComponent<Renderer>();
            Debug.Log("renderer" + renderer);
            renderer.material.color = new Color(colorArray[0], colorArray[1], colorArray[2]);

            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", new Color(colorArray[0] * colorArray[3], colorArray[1] * colorArray[3], colorArray[2] * colorArray[3]));


        }



    }
}

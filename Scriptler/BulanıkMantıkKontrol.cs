using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulanýkMantýkKontrol : MonoBehaviour
{
    //grafiklerin tanýmlanmasý
    public AnimationCurve close;
    public AnimationCurve midClose;
    public AnimationCurve middle;
    public AnimationCurve midFar;
    public AnimationCurve far;
    
    public GameObject car;//arabanýn tanýmlanmasý

    public float x;//bulanýk mantýk girdi deðeri

    public float fuzzyValue;//bulanýk mantýk çýkýþ deðeri
    public string activatedPart;//bulanýk mantýk çýkýþ sözel deðeri



    // Update is called once per frame
    void Update()
    {
        // girdi olarak verilen x deðiþkenin deðerinin okunmasý ve çýktýlarý
        float closeLabel = close.Evaluate(x);
        float middleLabel = middle.Evaluate(x);
        float farLabel = far.Evaluate(x);
        float midFarLabel = midFar.Evaluate(x);
        float midCloseLabel = midClose.Evaluate(x);

        //çýktýlara göre sözel deðerin döndürülmesi
        if (closeLabel > 0)
        {
            fuzzyValue = closeLabel;
            activatedPart = "close";
        }
        else if (middleLabel > 0) {
            fuzzyValue = middleLabel;
            activatedPart = "middle";
        }
        if (farLabel > 0) {
            fuzzyValue = farLabel;
            activatedPart = "far";
        }
        if (midFarLabel > 0)
        {
            fuzzyValue = midFarLabel;
            activatedPart = "midFar";
        }

        if (midCloseLabel > 0)
        {
            fuzzyValue = midCloseLabel;
            activatedPart = "midClose";
        }


    }
}

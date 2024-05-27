using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulan�kMant�kKontrol : MonoBehaviour
{
    //grafiklerin tan�mlanmas�
    public AnimationCurve close;
    public AnimationCurve midClose;
    public AnimationCurve middle;
    public AnimationCurve midFar;
    public AnimationCurve far;
    
    public GameObject car;//araban�n tan�mlanmas�

    public float x;//bulan�k mant�k girdi de�eri

    public float fuzzyValue;//bulan�k mant�k ��k�� de�eri
    public string activatedPart;//bulan�k mant�k ��k�� s�zel de�eri



    // Update is called once per frame
    void Update()
    {
        // girdi olarak verilen x de�i�kenin de�erinin okunmas� ve ��kt�lar�
        float closeLabel = close.Evaluate(x);
        float middleLabel = middle.Evaluate(x);
        float farLabel = far.Evaluate(x);
        float midFarLabel = midFar.Evaluate(x);
        float midCloseLabel = midClose.Evaluate(x);

        //��kt�lara g�re s�zel de�erin d�nd�r�lmesi
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

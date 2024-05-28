using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArabaKontrol : MonoBehaviour
{
    public float MotorForce, SteerForce, BrakeForce;//araç motor, yön ve fren gücünün tanımlanması
    public WheelCollider FR_L_Wheel, FR_R_Wheel, RE_L_WHeel, RE_R_Wheel;//tekerlerin tanımlanması

    
    //bulanık mantık işlemlerinin yapıldığı objelerin tanımlanması
    public GameObject frontRayCurve;
    public GameObject rightRayCurve;
    public GameObject leftRayCurve;

    private float frontM, rightM, leftM;//çıktı değişkenlerinin tanımlanması

    private string frontA, rightA, leftA;//sözel değerlerin tutulacağı değişkenler

    public float value = 50;//yön vermek için alınan referans değerin tanımlanması

    public float hitDist;//sağ ve sol ışınların çizim mesafesi
    public float frontHitDist;//ön ışının çizim mesafesi
    private float lastTurn = 0.0f;

    void Calc(Vector3 dist)
    {
       

        SteerForce = 0.0f;//tekerlerin dönüşü için uygulanan kuvvet(açı)
        
        float deFuzzyV = 30.0f;//dönüş için verilen temel açı değeri
        
        frontRayCurve.GetComponent<BulanıkMantıkKontrol>().x = dist.x;//ön ışının girdi değeri
        rightRayCurve.GetComponent<BulanıkMantıkKontrol>().x = dist.y;//sağ ışının girdi değeri
        leftRayCurve.GetComponent<BulanıkMantıkKontrol>().x = dist.z;//sol ışının girdi değeri

        this.frontM = frontRayCurve.GetComponent<BulanıkMantıkKontrol>().fuzzyValue;//ön ışının sayısal çıktı değeri
        this.rightM = rightRayCurve.GetComponent<BulanıkMantıkKontrol>().fuzzyValue;//sağ ışının sayısal çıktı değeri
        this.leftM = leftRayCurve.GetComponent<BulanıkMantıkKontrol>().fuzzyValue;//sol ışının sayısal çıktı değeri

        //ışın çıktılarının sözel karşılığı (close-midClose-middle-midFar-far)
        this.frontA = frontRayCurve.GetComponent<BulanıkMantıkKontrol>().activatedPart;
        this.rightA = rightRayCurve.GetComponent<BulanıkMantıkKontrol>().activatedPart;
        this.leftA = leftRayCurve.GetComponent<BulanıkMantıkKontrol>().activatedPart;

        //// aracın tekerlerine verdiğimiz güç 100 ü geçerse sıfırla
        if (RE_L_WHeel.rpm > 100)
            RE_L_WHeel.motorTorque = 0;

        if (RE_R_Wheel.rpm > 100)
            RE_R_Wheel.motorTorque = 0;

        value = Mathf.Min(frontM, Mathf.Min(rightM, leftM));//bulanık mantığa göre kesişen noktaların minimum değeri alınıyor

        print("Front ray: " + frontA + " Right ray: " + rightA + " Left ray: " + leftA);

        // bulanık mantık kuralları, araç sağ sol ve ön ışınlardan gelen değerlere göre aksiyon alıyor
        if (frontA == "far" || frontA == "midFar")
        {
            if (rightA == "far" || rightA == "midFar")
            {
                if (leftA == "far" || leftA == "midFar")
                {
                    if (rightA=="far"&&leftA== "midFar")
                    {
                        SteerForce += value * deFuzzyV;
                    }
                    else if (rightA=="midFar"&&leftA=="far") 
                    {
                        SteerForce += value * deFuzzyV*-1;
                    }
                    
                    RE_R_Wheel.motorTorque = MotorForce;
                    RE_L_WHeel.motorTorque = MotorForce;

                }
                else if (leftA == "close" || leftA == "midClose" || leftA == "middle")
                {
                    SteerForce += value * deFuzzyV;
                }
            }
            else if (rightA == "middle")
            {
                if (leftA == "close" || leftA == "midClose")
                {
                    SteerForce += value * deFuzzyV;
                }
                if (leftA == "far"|| leftA == "midFar")
                {
                    SteerForce += value * deFuzzyV*-1;
                    RE_R_Wheel.motorTorque = MotorForce;
                    RE_L_WHeel.motorTorque = MotorForce;
                }

            }
            else if (rightA == "close" || rightA == "midClose")
            {
                if ((leftA == "close" &&rightA=="close")||(leftA== "midClose"&& rightA == "midClose"))
                    return;
               
                SteerForce += value * deFuzzyV * -1;
                RE_R_Wheel.motorTorque = MotorForce;
                RE_L_WHeel.motorTorque = MotorForce;

            }


        }
        if (frontA == "close")
        {
            if (rightA == "close" || rightA == "midClose" || rightA == "middle")
            {
                if(rightA == "close"&&leftA== "close") 
                {
                    RE_R_Wheel.brakeTorque = 10000;
                    RE_L_WHeel.brakeTorque = 10000;
                    return;
                }
                if (leftA == "midClose" || leftA == "close")
                {
                    lastTurn = value * deFuzzyV * -1;
                    RE_R_Wheel.brakeTorque = 1000;
                    RE_L_WHeel.brakeTorque = 1000;
                }
                else
                {
                    lastTurn = value * deFuzzyV * -1;
                    RE_R_Wheel.motorTorque = MotorForce;
                    RE_L_WHeel.motorTorque = MotorForce;
                }

            }

            else if (leftA == "close" || leftA == "midClose" || leftA == "middle")
            {
                if (rightA == "midClose" || rightA == "close")
                {
                    lastTurn = value * deFuzzyV;
                    RE_R_Wheel.brakeTorque = 1000;
                    RE_L_WHeel.brakeTorque = 1000;
                }
                else
                {
                    lastTurn = value * deFuzzyV;
                    RE_R_Wheel.motorTorque = MotorForce;
                    RE_L_WHeel.motorTorque = MotorForce;
                }
            }

            if (rightA == "far" || rightA == "midFar") 
            {

                SteerForce += value * deFuzzyV;
               
                RE_R_Wheel.motorTorque = MotorForce;
                RE_L_WHeel.motorTorque = MotorForce;
                
            }

        }
        if (frontA == "midClose" || frontA == "middle")
        {
            if (leftA == "midClose" || leftA == "close")
            {

                if ((rightA == "midClose"||rightA=="midFar"||rightA=="middle" )&& leftA == "close")
                {
                    SteerForce += value * deFuzzyV;
                    RE_R_Wheel.motorTorque = MotorForce;
                    RE_L_WHeel.motorTorque = MotorForce;
                }
                if (leftA == "midClose" &&rightA == "close")
                {
                    SteerForce += value * deFuzzyV*-1;
                    RE_R_Wheel.motorTorque = MotorForce;
                    RE_L_WHeel.motorTorque = MotorForce;
                }

                

            }
            else if (rightA == "midClose" || rightA == "close")
            {


                SteerForce += value * deFuzzyV * -1;
                RE_R_Wheel.motorTorque = MotorForce;
                RE_L_WHeel.motorTorque = MotorForce;
            }
            
            if ((leftA == "middle" || leftA == "close" || leftA == "midClose") && (rightA == "far" || rightA == "midFar"))
            {
                SteerForce += value * deFuzzyV * 1.5f;
                RE_R_Wheel.motorTorque = MotorForce;
                RE_L_WHeel.motorTorque = MotorForce;
                
            }
            else if (rightA == "far" || rightA == "midFar")
            {
                if (leftA == "middle")
                {
                    SteerForce += value * deFuzzyV;
                    RE_R_Wheel.motorTorque = MotorForce;
                    RE_L_WHeel.motorTorque = MotorForce;
                    
                }
                if (leftA == "midFar"&& rightA == "far") 
                {
                    SteerForce += value * deFuzzyV;
                    RE_R_Wheel.motorTorque = MotorForce;
                    RE_L_WHeel.motorTorque = MotorForce;
                    
                }


            }
            
            if ((leftA == "far" || leftA == "midFar") && (rightA == "middle" || rightA == "midFar" || rightA == "far"))
            {
                if (rightA == "middle" && leftA == "midFar")
                {
                    SteerForce += value * deFuzzyV*-1;
                    RE_R_Wheel.motorTorque = MotorForce;
                    RE_L_WHeel.motorTorque = MotorForce;
                }
                else
                {

                    SteerForce += value * deFuzzyV ;
                    RE_R_Wheel.motorTorque = MotorForce;
                    RE_L_WHeel.motorTorque = MotorForce;
                }
            }
            
           

        }


    }

    Vector3 DetectColision()//ışınların unity içerisinde oluşturulması ve mesafe ölçülmesi
    {
        int layerMask = 1 << 8;
       
        layerMask = ~layerMask;
        Vector3 dist = new Vector3(10000.0f, 10000.0f, 10000.0f);//x:ön y:sağ z:sol
        RaycastHit hit;
        Vector3 rayPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);//ışınların 0 yani başlangıç noktası

        //ışınların oluşturulması ve ölçtükleri mesafeyi kaydetmesi
        if (Physics.Raycast(rayPos, transform.TransformDirection(Vector3.forward), out hit, frontHitDist, layerMask))
        {
            Debug.DrawRay(rayPos, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            dist.x = hit.distance / hitDist;
        }
        else
        {
            Debug.DrawRay(rayPos, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }

        if (Physics.Raycast(rayPos, transform.TransformDirection(new Vector3(1.0f, 0.0f, 2f)), out hit, hitDist, layerMask))
        {
            Debug.DrawRay(rayPos, transform.TransformDirection(new Vector3(1.0f, 0.0f, 2f)) * hit.distance, Color.green);
            dist.y = hit.distance / hitDist;
        }
        else
        {
            Debug.DrawRay(rayPos, transform.TransformDirection(new Vector3(1.0f, 0.0f, 2f)) * 1000, Color.white);
        }

        if (Physics.Raycast(rayPos, transform.TransformDirection(new Vector3(-1.0f, 0.0f, 2f)), out hit, hitDist, layerMask))
        {
            Debug.DrawRay(rayPos, transform.TransformDirection(new Vector3(-1.0f, 0.0f, 2f)) * hit.distance, Color.blue);
            dist.z = hit.distance / hitDist;
        }
        else
        {
            Debug.DrawRay(rayPos, transform.TransformDirection(new Vector3(-1.0f, 0.0f, 2f)) * 1000, Color.white);
        }
        return dist;//ölçülen ön,sağ ve sol ışınların ölçtüğü uzaklıkların geri döndürülmesi
    }


    void Start()
    {
        lastTurn = 0.0f;
    }
    void Update()//her frame de çalıştırılarak oyun döngüsünün oluşturulması
    {
        Vector3 dist = DetectColision();//ışınların oluşturulması ve ölçülen uzakların değişkene atanması
        Calc(dist);//ölçülen uzaklıkların Calc() methodu ile bulanık mantık sınıflandırması yapılarak arabanın engellere çarpmadan açı ile sağ sol yapmasını sağlar
       

        // araç hızı tanımlamaları
        float v = MotorForce;
        float h = SteerForce;
        RE_R_Wheel.motorTorque = v;
        RE_L_WHeel.motorTorque = v;
        FR_L_Wheel.steerAngle = h;
        FR_R_Wheel.steerAngle = h;

       

    }
}

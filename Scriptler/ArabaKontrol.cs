using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArabaKontrol : MonoBehaviour
{
    public float MotorForce, SteerForce, BrakeForce;//ara� motor, y�n ve fren g�c�n�n tan�mlanmas�
    public WheelCollider FR_L_Wheel, FR_R_Wheel, RE_L_WHeel, RE_R_Wheel;//tekerlerin tan�mlanmas�

    
    //bulan�k mant�k i�lemlerinin yap�ld��� objelerin tan�mlanmas�
    public GameObject frontRayCurve;
    public GameObject rightRayCurve;
    public GameObject leftRayCurve;

    private float frontM, rightM, leftM;//��kt� de�i�kenlerinin tan�mlanmas�

    private string frontA, rightA, leftA;//s�zel de�erlerin tutulaca�� de�i�kenler

    public float value = 50;//y�n vermek i�in al�nan referans de�erin tan�mlanmas�

    public float hitDist;//sa� ve sol ���nlar�n �izim mesafesi
    public float frontHitDist;//�n ���n�n �izim mesafesi
    private float lastTurn = 0.0f;

    void Calc(Vector3 dist)
    {
       

        SteerForce = 0.0f;//tekerlerin d�n��� i�in uygulanan kuvvet(a��)
        
        float deFuzzyV = 30.0f;//d�n�� i�in verilen temel a�� de�eri
        
        frontRayCurve.GetComponent<Bulan�kMant�kKontrol>().x = dist.x;//�n ���n�n girdi de�eri
        rightRayCurve.GetComponent<Bulan�kMant�kKontrol>().x = dist.y;//sa� ���n�n girdi de�eri
        leftRayCurve.GetComponent<Bulan�kMant�kKontrol>().x = dist.z;//sol ���n�n girdi de�eri

        this.frontM = frontRayCurve.GetComponent<Bulan�kMant�kKontrol>().fuzzyValue;//�n ���n�n say�sal ��kt� de�eri
        this.rightM = rightRayCurve.GetComponent<Bulan�kMant�kKontrol>().fuzzyValue;//sa� ���n�n say�sal ��kt� de�eri
        this.leftM = leftRayCurve.GetComponent<Bulan�kMant�kKontrol>().fuzzyValue;//sol ���n�n say�sal ��kt� de�eri

        //���n ��kt�lar�n�n s�zel kar��l��� (close-midClose-middle-midFar-far)
        this.frontA = frontRayCurve.GetComponent<Bulan�kMant�kKontrol>().activatedPart;
        this.rightA = rightRayCurve.GetComponent<Bulan�kMant�kKontrol>().activatedPart;
        this.leftA = leftRayCurve.GetComponent<Bulan�kMant�kKontrol>().activatedPart;

        //// arac�n tekerlerine verdi�imiz g�� 100 � ge�erse s�f�rla
        if (RE_L_WHeel.rpm > 100)
            RE_L_WHeel.motorTorque = 0;

        if (RE_R_Wheel.rpm > 100)
            RE_R_Wheel.motorTorque = 0;

        value = Mathf.Min(frontM, Mathf.Min(rightM, leftM));//bulan�k mant��a g�re kesi�en noktalar�n minimum de�eri al�n�yor

        print("Front ray: " + frontA + " Right ray: " + rightA + " Left ray: " + leftA);

        // bulan�k mant�k kurallar�, ara� sa� sol ve �n ���nlardan gelen de�erlere g�re aksiyon al�yor
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

    Vector3 DetectColision()//���nlar�n unity i�erisinde olu�turulmas� ve mesafe �l��lmesi
    {
        int layerMask = 1 << 8;
       
        layerMask = ~layerMask;
        Vector3 dist = new Vector3(10000.0f, 10000.0f, 10000.0f);//x:�n y:sa� z:sol
        RaycastHit hit;
        Vector3 rayPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);//���nlar�n 0 yani ba�lang�� noktas�

        //���nlar�n olu�turulmas� ve �l�t�kleri mesafeyi kaydetmesi
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
        return dist;//�l��len �n,sa� ve sol ���nlar�n �l�t��� uzakl�klar�n geri d�nd�r�lmesi
    }


    void Start()
    {
        lastTurn = 0.0f;
    }
    void Update()//her frame de �al��t�r�larak oyun d�ng�s�n�n olu�turulmas�
    {
        Vector3 dist = DetectColision();//���nlar�n olu�turulmas� ve �l��len uzaklar�n de�i�kene atanmas�
        Calc(dist);//�l��len uzakl�klar�n Calc() methodu ile bulan�k mant�k s�n�fland�rmas� yap�larak araban�n engellere �arpmadan a�� ile sa� sol yapmas�n� sa�lar
       

        // ara� h�z� tan�mlamalar�
        float v = MotorForce;
        float h = SteerForce;
        RE_R_Wheel.motorTorque = v;
        RE_L_WHeel.motorTorque = v;
        FR_L_Wheel.steerAngle = h;
        FR_R_Wheel.steerAngle = h;

       

    }
}

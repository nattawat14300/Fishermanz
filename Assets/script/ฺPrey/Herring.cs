using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herring : Prey
{
    void Awake() 
    {
        // กำหนดค่าเริ่มต้นของชื่อปลา (ถ้ายังไม่ได้กำหนดใน Inspector)
        if (fishName == "Prey") 
        {
            fishName = "ทูน่าตัวใหญ่";
        }
        if (fishDescription == "รายละเอียดเหยื่อเริ่มต้น")
        {
            fishDescription = "ปลาทูน่าให้พลังงานสูง";
        }
        // *ไม่แนะนำให้เรียก base.Awake() ในกรณีนี้*
    }
}

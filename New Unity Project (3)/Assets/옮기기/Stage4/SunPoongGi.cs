using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunPoongGi : MonoBehaviour
{
    public GameObject HitRegion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)//충돌한 오브젝트가 총알일 경우
    {
        Debug.Log("총알 반사");
        if(other.transform.tag == "Bullet")
        {
            other.transform.localEulerAngles = new Vector3(other.transform.localEulerAngles.x - 180, other.transform.localEulerAngles.y, other.transform.localEulerAngles.z);
            other.GetComponent<Bullet>().isCanHitPlayer = true;
        }
    }
}

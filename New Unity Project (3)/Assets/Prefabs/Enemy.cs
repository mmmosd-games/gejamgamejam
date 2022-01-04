using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool Enemy_isReversed = false;
    public ConstantForce Enemy_ConstantForce;
    // Start is called before the first frame update
    void Start()
    {
        Enemy_ConstantForce = this.GetComponent<ConstantForce>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyReverse();
    }
    public void EnemyReverse()
    {
        if (!GravityObject.instance.isReversed)//맵이 뒤집어져 있지 않을 때
        {
            if (!Enemy_isReversed)//적(이 오브젝트)이 뒤집어져 있지 않을 때
            {
                Enemy_ConstantForce.force = new Vector3(0, 0, 0);
            }
            else//적(이 오브젝트)이 뒤집어져 있을 때
            {
                Enemy_ConstantForce.force = new Vector3(0, 19.6f, 0);
            }
        }
        else//맵이 뒤집어져 있을 때
        {
            if (!Enemy_isReversed)//적(이 오브젝트)이 뒤집어져 있지 않을 때
            {
                Enemy_ConstantForce.force = new Vector3(0, 0, 0);
            }
            else//적(이 오브젝트)이 뒤집어져 있을 때
            {
                Enemy_ConstantForce.force = new Vector3(0, -19.6f, 0);
            }
        }
    }
}

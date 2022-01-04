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
        if (!GravityObject.instance.isReversed)//���� �������� ���� ���� ��
        {
            if (!Enemy_isReversed)//��(�� ������Ʈ)�� �������� ���� ���� ��
            {
                Enemy_ConstantForce.force = new Vector3(0, 0, 0);
            }
            else//��(�� ������Ʈ)�� �������� ���� ��
            {
                Enemy_ConstantForce.force = new Vector3(0, 19.6f, 0);
            }
        }
        else//���� �������� ���� ��
        {
            if (!Enemy_isReversed)//��(�� ������Ʈ)�� �������� ���� ���� ��
            {
                Enemy_ConstantForce.force = new Vector3(0, 0, 0);
            }
            else//��(�� ������Ʈ)�� �������� ���� ��
            {
                Enemy_ConstantForce.force = new Vector3(0, -19.6f, 0);
            }
        }
    }
}

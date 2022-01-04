using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject Player;//�÷��̾� ������Ʈ�� �̱������� ������

    public float BulletSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Player = GravityObject.instance.gameObject;//�÷��̾� ������Ʈ�� �̱������� ������
    }

    // Update is called once per frame
    void Update()
    {
        a();
    }

    void a()
    {
        this.transform.Translate(Vector3.forward * BulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {//�Ѿ��� ���� �� ȣ��
        if (other.transform.tag == "Wall")
        {
            GravityObject.instance.GravityCtrl();//���� ��ü�� ���̸� �߷� ����
            Destroy(this.gameObject);
        }
        else if (other.transform.tag == "Enemy")
        {
            if (other.GetComponent<Enemy>().Enemy_isReversed == false)//���� �����Ǿ� ���� ������
            {
                other.GetComponent<Enemy>().Enemy_isReversed = true;
            }
            else//���� �����Ǿ� ������
            {
                other.GetComponent<Enemy>().Enemy_isReversed = false;
            }//���̸� ���� �߷� ����
            Destroy(this.gameObject);
        }//�Ѿ� �⵹ �� �Ѿ� ����
    }
}

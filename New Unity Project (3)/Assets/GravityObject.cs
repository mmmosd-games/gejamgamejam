using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    #region �̱���
    public static GravityObject instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
    }
    #endregion
    [Header("�̵�")]
    public float rotSpeed = 3.0f;

    public Camera fpsCam;
    public float currentCameraRotationX;

    [Header("�߷� ����(��)")]
    public GameObject Gun;
    public GameObject Bullet;
    public GameObject FirePoint;
    public bool isReversed = false;
    public bool isRolling = false;
    public int Ammo = 0;
    // Update is called once per frame

    void Update()
    {
        Rotctrl();
        ShootFire();
    }
    private void LateUpdate()
    {
        
    }
    void Rotctrl()
    {
        if (!isRolling)
        {
            // float _xRotation = Input.GetAxisRaw("Mouse Y");
            // float _cameraRotationX = _xRotation * rotSpeed;

            // currentCameraRotationX -= _cameraRotationX;
            // currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -80, 80);

            // fpsCam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
            // #region �÷��̾� ������
            // float rotY = Input.GetAxis("Mouse X") * rotSpeed;
            // this.transform.localRotation *= Quaternion.Euler(0, rotY, 0);
            // #endregion
        }
    }

    #region �߷� ���� + ��
    public void ShootFire()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))//�� �߻縦 �Է��ϸ�
        {
            Vector3 FireStart = FirePoint.transform.position;
            GameObject NewBullet = Instantiate(Bullet);//�Ѿ��� �߻� ��ġ�� ����
            NewBullet.transform.position = FireStart;//�Ѿ��� ��ġ ����
            NewBullet.transform.rotation = fpsCam.transform.rotation;//�Ѿ��� ���� ����
            NewBullet.transform.SetParent(null);
            NewBullet.GetComponent<Bullet>().BulletSpeed = 50f;
        }
    }
    public void GravityCtrl()//�÷��̾ ������ �߷��� �����Ǿ��� �� ȣ��(���� �Ѿ� ��ũ��Ʈ���� ȣ�� ����)
    {
        GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(Enemys);
        StartCoroutine(GravityCtrl_Cor());
        // if (!isReversed)
        // {//�߷�->���߷�
        //    Physics.gravity = new Vector3(0, 9.8f);
        //    this.transform.localRotation = Quaternion.Euler(180, this.transform.localEulerAngles.y + 180, 0);
        //    isReversed = true;
        // }
        // else
        // {//���߷�->�߷�
        //    Physics.gravity = new Vector3(0, -9.8f);
        //    this.transform.localRotation = Quaternion.Euler(0, this.transform.localEulerAngles.y, 0);
        //    isReversed = false;
        // }
    }
    IEnumerator GravityCtrl_Cor()
    {
        yield return null;
        if (!isReversed)
        {//�߷�->���߷�
            Physics.gravity = new Vector3(0, 9.8f);
            isReversed = true;
            Vector3 StartRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
            Vector3 EndRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 180);
            if (!isRolling)
            {//�������� ������
                isRolling = true;//�߷� ���� ȸ���� ����
                while (Vector3.Distance(StartRot, EndRot) > 1.5f)
                {
                    yield return null;
                    StartRot = (Vector3.Lerp(StartRot, EndRot, 5f * Time.deltaTime));
                    transform.localEulerAngles = Vector3.Lerp(StartRot, EndRot, 5f * Time.deltaTime);
                }//��ǥġ�� ȸ��
            }
            transform.localEulerAngles = EndRot;//����� ���� ȸ���� ����
        }
        else
        {//���߷�->�߷�
            Physics.gravity = new Vector3(0, -9.8f);
            isReversed = false;
            Vector3 StartRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 180);
            Vector3 EndRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
            if (!isRolling)
            {
                isRolling = true;//�߷� ���� ȸ���� ����
                while (Vector3.Distance(transform.localEulerAngles, EndRot) > 1.5f)
                {
                    yield return null;
                    StartRot = (Vector3.Lerp(StartRot, EndRot, 5f * Time.deltaTime));
                    transform.localEulerAngles = Vector3.Lerp(StartRot, EndRot, 5f * Time.deltaTime);
                }//��ǥġ�� ȸ��
            }
            transform.localEulerAngles = EndRot;//����� ���� ȸ���� ����
        }
        isRolling = false;//�߷� ���� ȸ���� ��
        #region ȸ�� üũ
        if (!isReversed)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180);
        }
        #endregion
    }
    #endregion
}
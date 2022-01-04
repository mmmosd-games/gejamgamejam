using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    #region 싱글톤
    public static NewBehaviourScript instance = null;

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
    [Header("이동")]
    public float rotSpeed = 3.0f;
    public float moveSpeed = 2.0f;
    public float Runspeed = 6.0f;

    public Camera fpsCam;
    public float currentCameraRotationX;

    [Header("중력 조작(총)")]
    public GameObject Gun;
    public GameObject Bullet;//총에서 발사되는 총알 오브젝트
    public GameObject FirePoint;//총알이 발사되는 지점
    public bool isReversed = false;//반전 상태(맵)
    private bool isRolling = false;//지금 돌고있는가?(중력조작중)

    public bool isCanFire;//현재 총알을 쏠 수 있는가
    public bool isReloading;//현재 재장전을 하는 중인가
    public int ReloadedAmmo = 0;//중력총을 쏠 수 있는 장탄수(아직 사용 안됨)
    public int CanReloadAmmo = 0;//최대로 재장전 할 수 있는 수
    public int MaxAmmo = 0;//현재 가진 탄 수
    // Update is called once per frame

    void Update()
    {
        SetCanFire();
        ReloadAmmo();
        Rotctrl();//플레이어 회전 조작
        ShootFire();//발사 조작
    }
    private void LateUpdate()
    {
        MoveCtrl();//플레이어의 이동을 조작
    }
    void Rotctrl()
    {
        if (!isRolling)
        {
            float _xRotation = Input.GetAxisRaw("Mouse Y");
            float _cameraRotationX = _xRotation * rotSpeed;

            currentCameraRotationX -= _cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -80, 80);

            fpsCam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
            #region 플레이어 움직임
            float rotY = Input.GetAxis("Mouse X") * rotSpeed;
            this.transform.localRotation *= Quaternion.Euler(0, rotY, 0);
            #endregion
        }
    }
    void MoveCtrl()
    {
        Vector3 MoveForward = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)){ MoveForward += Vector3.forward; }
        if (Input.GetKey(KeyCode.S)) { MoveForward += Vector3.back; ; }
        if (Input.GetKey(KeyCode.A)) { MoveForward += Vector3.left; }
        if (Input.GetKey(KeyCode.D)) { MoveForward += Vector3.right; }//이동값 추가하기

        if(!Input.GetKey(KeyCode.LeftShift))//걸어다니기
            this.transform.Translate(MoveForward.normalized * Time.deltaTime * moveSpeed);
        else//달려다니기
            this.transform.Translate(MoveForward.normalized * Time.deltaTime * Runspeed);
    }
    #region 중력 관련 + 총
    public void ShootFire()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isCanFire)//총 발사를 입력하면
        {
            Vector3 FireStart = FirePoint.transform.position;
            #region 총알 오브젝트 생성
            GameObject NewBullet = Instantiate(Bullet);//총알을 발사 위치에 생성
            NewBullet.transform.position = FireStart;//총알의 위치 성정
            NewBullet.transform.rotation = fpsCam.transform.rotation;//총알의 방향 설정
            NewBullet.transform.SetParent(null);
            NewBullet.GetComponent<Bullet>().BulletSpeed = 10f;
            #endregion
            ReloadedAmmo -= 1;//플레이어가 소지중인 탄환을 한개 제거
        }
    }
    public void ReloadAmmo()
    {
        if (Input.GetKeyDown(KeyCode.R) && MaxAmmo >= 1 && ReloadedAmmo < CanReloadAmmo)//현재 총알이 있고 최대 장전 수보다 현재 장전된 총알이 적을 때
        {
            StartCoroutine(ReloadAmmo_Cor());
        }
    }
    IEnumerator ReloadAmmo_Cor()
    {
        isReloading = true;
        yield return null;//재장전 시간이 필요할 시 나중에 설정
        int WantToReload = CanReloadAmmo - ReloadedAmmo;//최대로 재장전 할 수 있는 수 설정
        if(MaxAmmo >= WantToReload)//현재 소지 총알 수가 충분할 때
        {
            ReloadedAmmo = CanReloadAmmo;//최대 수로 장전
            MaxAmmo -= WantToReload;//장전한 만큼 소지 탄 감소
        }
        else//현재 소지 총알 수가 부족할 때
        {
            ReloadedAmmo += MaxAmmo;
            MaxAmmo = 0;
        }
        isReloading = false;
    }
    public void SetCanFire()
    {
        if(ReloadedAmmo <= 0 || isReloading)//총을 쏘지 못하는 상황들
        {
            isCanFire = false;
        }
        else
        {
            isCanFire = true;
        }
    }
    #region 중력 조작
    public void GravityCtrl()//플레이어를 포합한 중력이 반전되었을 때 호출(현재 총알 스크립트에서 호출 가능)
    {
        GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(Enemys);
        StartCoroutine(GravityCtrl_Cor());
    }
    IEnumerator GravityCtrl_Cor()
    {
        yield return null;
        if (!isReversed)
        {//중력->반중력
            Physics.gravity = new Vector3(0, 9.8f);
            isReversed = true;
            Vector3 StartRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
            Vector3 EndRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 180);
            if (!isRolling)
            {//돌고있지 않으면
                isRolling = true;//중력 반전 회전을 시작
                while (Vector3.Distance(StartRot, EndRot) > 1.5f)
                {
                    yield return null;
                    StartRot = (Vector3.Lerp(StartRot, EndRot, 5f * Time.deltaTime));
                    transform.localEulerAngles = Vector3.Lerp(StartRot, EndRot, 5f * Time.deltaTime);
                }//목표치로 회전
            }
            transform.localEulerAngles = EndRot;//충분히 돌면 회전을 끝냄
        }
        else
        {//반중력->중력
            Physics.gravity = new Vector3(0, -9.8f);
            isReversed = false;
            Vector3 StartRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 180);
            Vector3 EndRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
            if (!isRolling)
            {
                isRolling = true;//중력 반전 회전을 시작
                while (Vector3.Distance(transform.localEulerAngles, EndRot) > 1.5f)
                {
                    yield return null;
                    StartRot = (Vector3.Lerp(StartRot, EndRot, 5f * Time.deltaTime));
                    transform.localEulerAngles = Vector3.Lerp(StartRot, EndRot, 5f * Time.deltaTime);
                }//목표치로 회전
            }
            transform.localEulerAngles = EndRot;//충분히 돌면 회전을 끝냄
        }
        isRolling = false;//중력 반전 회전을 끝
        #region 회전 체크
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
    #endregion
}
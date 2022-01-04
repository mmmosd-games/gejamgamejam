using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_GravityGun : MonoBehaviour
{
    [Header("중력총 발사")]
    public bool isCanFire = false;
    public Transform StartPoint;
    public float MaxReloadTime;
    public float ReloadTime;
    public GameObject Bullet;
    // Update is called once per frame
    void Update()
    {
        ShootGravityGun();
    }
    void ShootGravityGun()
    {//스테이지 1 기믹_ 중력총이 일정시간마다 발사
        ReloadTime -= Time.deltaTime;
        if(ReloadTime <= 0 && isCanFire)
        {//일정 시간마다 총알을 생성
            GameObject NewBullet = Instantiate(Bullet);//총알을 발사 위치에 생성
            NewBullet.transform.SetParent(null);
            NewBullet.transform.position = StartPoint.position;//총알의 위치 성정
            NewBullet.transform.rotation = StartPoint.transform.rotation;//총알의 방향 설정
            NewBullet.GetComponent<Bullet>().BulletSpeed = 10f;//총알의 속도 설정
            NewBullet.GetComponent<Bullet>().isCanHitPlayer = true;//총알의 속도 설정
            ReloadTime = MaxReloadTime;
        }
    }
}

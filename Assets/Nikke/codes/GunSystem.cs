using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor;

public class GunSystem : MonoBehaviour
{
    [SerializeField]
    private CrossHairManager crossHairManager;

    // Gun Stats
    public int damage;
    public float fireRate, spread, range, reloadTime;
    public int magazineSize, bulletsPerTaps;
    public bool allowButtonHold;
    public int bulletsLeft;

    // bools
    bool shooting, readyToShoot, reloading, playerShooting;

    // References
    public Camera tpsCam;
    public Transform gunModel;
    public Transform attackPoint;
    public Transform pivotPoint;
    public RaycastHit rayHit, spreadHit;
    public LayerMask whattIsEnemy;
    public GameObject targetEnemy;
    public GameObject AimPoint;

    // Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI text;
    public TrailRenderer bulletTrail;


    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    public void MyInput()
    {
        // Shooting state
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Reloading
        if (bulletsLeft == 0 && !reloading) Reload();


        // Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            crossHairManager.SetAim();
            Debug.Log("여기서 발사했는데");
            Shoot();
        }
    }

    public void AiInput(bool isFocused)
    {
        if (isFocused)
        {
            if (allowButtonHold) playerShooting = Input.GetKey(KeyCode.Mouse0);
            else playerShooting = Input.GetKeyDown(KeyCode.Mouse0);

            if (bulletsLeft == 0 && !reloading) Reload();

            if (playerShooting && readyToShoot && !reloading && bulletsLeft > 0)
            {
                crossHairManager.SetAim();
                Shoot();
                if (targetEnemy)
                {
                    targetEnemy = null;
                }
            }
            else
            {

                // Shoot
                if (readyToShoot && !reloading && bulletsLeft > 0)
                {
                    if (targetEnemy == null)
                    {
                        FindEnemy();
                    }
                    else
                    {
                        crossHairManager.FollowEnemy(tpsCam, targetEnemy);
                    }

                    Shoot();
                }
            }
        }
        else
        {
            if (bulletsLeft == 0 && !reloading) Reload();

            if (readyToShoot && !reloading && bulletsLeft > 0)
            {
                if (targetEnemy == null)
                {
                    FindEnemy();
                }
                else
                {
                    crossHairManager.FollowEnemy(tpsCam, targetEnemy);
                }

                Shoot();
            }
        }
    }


    public void Shoot()
    {
        readyToShoot = false;

        Vector2 aimPos = tpsCam.WorldToScreenPoint(AimPoint.transform.position);

        // Debug.Log(aimPos);
        // 마우스 위치
        Ray ray = tpsCam.ScreenPointToRay(aimPos);

        // RayCast 마우스가 보고 있는 물체에 바로 쏘아줌
        Physics.Raycast(ray, out rayHit, range, whattIsEnemy);

        // Debug.Log(rayHit.point);

        // 총 회전 - rayHit를 사용하는 이유는 스프레드가 직접적으로 총구 방향을 결정짓는 것은 아니기 때문임. 같은 곳을 보고 쏜다면 같은 곳을 가리켜야 함.
        Vector3 pivotToHit = rayHit.point - pivotPoint.position;
        Vector3 newDirection = pivotToHit.normalized;
        gunModel.rotation = Quaternion.LookRotation(newDirection, Vector3.up);

        if (rayHit.collider.CompareTag("Enemy"))
        {
            // 원형 spread
            float angle = Random.Range(0f, 2f * Mathf.PI);
            float radius = Random.Range(0f, spread);

            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);

            // 스프레드 적용된 탄착점
            Ray spreadRay = tpsCam.ScreenPointToRay(aimPos + new Vector2(x, y));
            Physics.Raycast(spreadRay, out spreadHit, range);

            // Trail 생성
            TrailRenderer trail = Instantiate(bulletTrail, attackPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, spreadHit));

            // 총구 화염 생성 및 delayt 후 파괴
            GameObject instantiatedMuzzleFlash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
            Destroy(instantiatedMuzzleFlash, 0.1f);

            // 적에게 공격이 닿았을 때만 데미지를 줌.
            if (spreadHit.collider.CompareTag("Enemy"))
            {
                spreadHit.collider.GetComponent<EnemyAction>().TakeDamage(damage);
            }

            bulletsLeft--;

        }
        Invoke("ResetShot", fireRate);

    }

    // 적을 자동으로 찾는 코드
    public void FindEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector2 aimPos = tpsCam.WorldToScreenPoint(AimPoint.transform.position);



        foreach (GameObject enemy in enemies)
        {
            Vector3 screenPoint = tpsCam.WorldToScreenPoint(enemy.transform.position);
            float distanceToCenter = Vector3.Distance(screenPoint, aimPos);

            if (distanceToCenter < closestDistance)
            {
                closestDistance = distanceToCenter;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            // 가장 가까운 적을 목표로 설정
            targetEnemy = closestEnemy;
            // Debug.Log(closestEnemy.transform.position);
        }


    }














    // Trail 생성 함수
    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit rayHit)
    {
        float time = 0;
        Vector3 startPos = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPos, rayHit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = rayHit.point;

        Destroy(trail.gameObject, trail.time);
    }

    private void ResetShot()
    {
        readyToShoot = true;


    }

    private void Reload()
    {
        reloading = true;
        gunModel.rotation = Quaternion.identity;
        Invoke("ReloadFinished", reloadTime);

    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}

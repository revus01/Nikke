using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using TMPro;
using UnityEditor.ShaderGraph.Internal;

public class GunSystem : MonoBehaviour
{

    // Gun Stats
    public int damage;
    public float fireRate, spread, range, reloadTime;
    public int magazineSize, bulletsPerTaps;
    public bool allowButtonHold;
    int bulletsLeft;

    // bools
    bool shooting, readyToShoot, reloading;

    // References
    public Camera tpsCam;
    public Transform gunModel;
    public Transform attackPoint;
    public Transform pivotPoint;
    public RaycastHit rayHit;
    public LayerMask whattIsEnemy;

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
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        // Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            Shoot();
        }

    }

    public void Shoot()
    {
        readyToShoot = false;

        // 원형 spread
        float angle = Random.Range(0f, 2f * Mathf.PI); // 0에서 2파이(360도) 사이의 무작위 각도를 선택합니다.
        float radius = Random.Range(0f, spread); // 0에서 spread 사이의 무작위 반지름을 선택합니다.

        float x = radius * Mathf.Cos(angle);
        float y = radius * Mathf.Sin(angle);



        // 마우스 위치 + 스프레드 보정
        Ray ray = tpsCam.ScreenPointToRay(Input.mousePosition + new Vector3(x, y, 0));

        // RayCast 마우스가 보고 있는 물체에 바로 쏘아줌
        if (Physics.Raycast(ray, out rayHit, range, whattIsEnemy))
        {

            // 총 회전
            Vector3 pivotToHit = rayHit.point - pivotPoint.position;
            Vector3 newDirection = pivotToHit.normalized;
            gunModel.rotation = Quaternion.LookRotation(newDirection, Vector3.up);


            Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
            {
                TrailRenderer trail = Instantiate(bulletTrail, attackPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, rayHit));
                rayHit.collider.GetComponent<EnemyAction>().TakeDamage(damage);
                GameObject instantiatedMuzzleFlash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

                bulletsLeft--;
                Destroy(instantiatedMuzzleFlash, 0.1f);
            }

            // Debug.DrawRay(attackPoint.position, rayHit.point - attackPoint.position, Color.red, 20f);

        }


        Invoke("ResetShot", fireRate);




    }

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

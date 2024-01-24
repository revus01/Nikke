using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{

    // Gun Stats
    public int damage;
    public float fireRate, spread, range, reloadTime;
    public int magazineSize, bulletsPerTaps;
    public bool allowButtonHold;
    int bulletsLeft, bulletShot;

    // bools
    bool shooting, readyToShoot, reloading;

    // References
    public Camera tpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whattIsEnemy;

    // Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI text;


    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        // SetText
        // text.SetText(bulletsLeft + "/" + magazineSize);
    }

    private void MyInput()
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

    private void Shoot()
    {
        readyToShoot = false;

        // Spread
        float x = Random.Range(-range, range);
        float y = Random.Range(-range, range);

        // Direction with spread
        // Vector3 mousePos = Input.mousePosition;
        // mousePos.z = 10f;
        // Debug.Log(mousePos);
        // // Vector3 direction = tpsCam.transform.forward + new Vector3(x, y, 0);
        // mousePos = tpsCam.ScreenToWorldPoint(mousePos);
        // Vector3 direction = (mousePos - attackPoint.position).normalized;
        // // Debug.DrawRay(attackPoint.position, mousePos - transform.position, Color.red, 20f);
        // // Debug.DrawRay(attackPoint.position, direction * 20f, Color.red, 20f);
        // Debug.Log(mousePos);



        // RayCast 마우스가 보고 있는 물체에 바로 쏘아줌
        Ray ray = tpsCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out rayHit, range, whattIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

            Debug.Log(rayHit.point);
            Debug.DrawRay(attackPoint.position, rayHit.point - attackPoint.position, Color.red, 20f);

            // if (rayHit.collider.CompareTag("Enemy"))
            // {
            //     rayHit.collider.GetComponent<ShootingAi>().TakeDame(damage);
            // }
        }


        GameObject instantiatedMuzzleFlash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        Invoke("ResetShot", fireRate);

        Destroy(instantiatedMuzzleFlash, 0.1f);



    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}

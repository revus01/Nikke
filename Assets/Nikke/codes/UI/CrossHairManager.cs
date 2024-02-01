using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CrossHairManager : MonoBehaviour
{
    private RectTransform canvasRect;

    public GameManager gameManager;

    public CameraManager cameraManager;

    public float followSpeed;

    bool isAutoFire;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        cameraManager = FindAnyObjectByType<CameraManager>();
    }

    void Start()
    {
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    // 수동 조작
    public void SetAim()
    {
        Vector2 mousePos = Input.mousePosition;
        Debug.Log(mousePos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mousePos, cameraManager.focusedCamera, out mousePos);
        transform.localPosition = mousePos;
        Debug.Log(mousePos);

    }

    // 자동 조준
    public void FollowEnemy(Camera cam, GameObject enemy)
    {

        Vector2 enemyPos = cam.WorldToScreenPoint(enemy.transform.position);
        Vector2 aimPos = cam.WorldToScreenPoint(transform.position);



        aimPos = Vector2.MoveTowards(aimPos, enemyPos, Time.deltaTime * followSpeed);

        // 따라가는 위치를 다시 월드 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, aimPos, cam, out aimPos);
        // aim 위치 업데이트
        transform.localPosition = aimPos;
    }
}

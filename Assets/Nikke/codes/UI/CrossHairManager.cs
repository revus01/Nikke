using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CrossHairManager : MonoBehaviour
{
    private RectTransform canvasRect;

    public GameManager gameManager;

    public float followSpeed;

    bool isAutoFire;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Start()
    {
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    // 수동 조작
    public void SetAim()
    {
        Vector2 mousePos = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mousePos, Camera.main, out mousePos);
        transform.localPosition = mousePos;
    }

    // 자동 조준
    public void FollowEnemy(Camera cam, GameObject enemy)
    {

        Vector2 enemyPos = cam.WorldToScreenPoint(enemy.transform.position);
        Vector2 aimPos = cam.WorldToScreenPoint(transform.position);

        Debug.Log(enemyPos);
        Debug.Log(enemyPos);

        aimPos = Vector2.MoveTowards(aimPos, enemyPos, Time.deltaTime * followSpeed);

        // 따라가는 위치를 다시 월드 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, aimPos, cam, out aimPos);
        // aim 위치 업데이트
        transform.localPosition = aimPos;
    }
}

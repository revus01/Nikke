using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;
    [SerializeField]
    private CameraManager cameraManager;

    // reference
    public bool isAutoFire;
    public bool isAutoBurst;

    public int focusedNikke;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        focusedNikke = 1;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 조작 니케 변경 요청을 받음.
    public void RequestChangeNikke(int num)
    {
        ChangeFocusedNikke(num);
    }

    // 조작 니케를 실제로 변경하는 코드
    private void ChangeFocusedNikke(int num)
    {
        // 니케가 활동 불가면 전환되지 않음.
        focusedNikke = num;

        // 카메라 포커스를 이동함.
        cameraManager.RequestChangeCamera(focusedNikke);
    }
}

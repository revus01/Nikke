using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NikkeAction : MonoBehaviour
{
    [SerializeField]
    private GunSystem gun;

    public GameManager gameManager;

    bool isAutoFire;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

    }

    private void Update()
    {
        isAutoFire = gameManager.isAutoFire;

        if (isAutoFire)
        {
            gun.AiInput();

        }
        else
        {
            gun.MyInput();
        }
    }

}

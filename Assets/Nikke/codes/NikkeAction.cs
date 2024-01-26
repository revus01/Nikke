using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NikkeAction : MonoBehaviour
{
    [SerializeField]
    private GunSystem gun;

    private void Update()
    {
        gun.MyInput();
    }

}

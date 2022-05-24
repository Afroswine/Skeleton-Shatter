using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Canvas))]
public class PlayerLose : MonoBehaviour
{
    //[SerializeField] private Canvas _canvas;

    //[SerializeField] PlayerHealth _playerHealth;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] MouseLook _mouseLook;
    [SerializeField] FireWeapon _weapon;
    //[Tooltip("Requires Components: PlayerHealth, PlayerMovement, MouseLook, FireWeapon")]
    //[SerializeField] GameObject _player;

    private Canvas _CANTVAS;

    private void Awake()
    {
        // assign _canvas to this gameObject's canvas and disable it
        _CANTVAS = GetComponent<Canvas>();
        _CANTVAS.enabled = false;
    }

    /*
    private void OnEnable()
    {
        _playerHealth.Died.AddListener(Lose);
    }

    private void OnDisable()
    {
        _playerHealth.Died.RemoveListener(Lose);
    }
    */

    private void Lose()
    {
        _CANTVAS.enabled = true;

        _playerMovement.enabled = false;
        _mouseLook.enabled = false;
        _weapon.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        /*
        if (TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            playerMovement.enabled = false;
            Debug.Log("playerMovement disabled");
        }
        if (GetComponentInChildren<Camera>().TryGetComponent<MouseLook>(out MouseLook mouseLook))
        {
            mouseLook.enabled = false;
            Debug.Log("mouseLook disabled");
        }
        */

    }
}

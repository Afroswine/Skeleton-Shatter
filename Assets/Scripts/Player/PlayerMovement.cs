using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
#if UNITY_EDITOR
using UnityEditor;
#endif
*/

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float walkSpeed = 12f;
    public float sprintSpeed = 24f;

    public float gravity = -50f;
    public float maxFallSpeed = -100f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    float speed;

    #region Editor
    /*
#if UNITY_EDITOR


    [CustomEditor(typeof(PlayerMovement))]
    public class PlayerMovementInspectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PlayerMovement playerMovement = (PlayerMovement)target;

            DrawMovementSpeed(playerMovement);
        }

        private static void DrawMovementSpeed(PlayerMovement playerMovement)
        {
            
            
            EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField("Movement Speed", EditorStyles.boldLabel, GUILayout.MaxWidth(200));
            EditorGUILayout.LabelField("Movement Speed", EditorStyles.boldLabel, GUILayout.MaxWidth(200));

            EditorGUILayout.LabelField("Walking", GUILayout.MaxWidth(75));
            playerMovement.walkSpeed = EditorGUILayout.FloatField(playerMovement.walkSpeed);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Sprinting", GUILayout.MaxWidth(75));
            playerMovement.sprintSpeed = EditorGUILayout.FloatField(playerMovement.sprintSpeed);

            EditorGUILayout.EndHorizontal();
        }
    }


#endif
    */
    #endregion

    // Update is called once per frame
    void Update()
    {
        #region Gravity
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if(!isGrounded && velocity.y <= maxFallSpeed)
        {
            velocity.y = maxFallSpeed;
            Debug.Log("At MaxFall, velocity.y = " + velocity.y.ToString());
        }

        velocity.y += gravity * Time.deltaTime;
        #endregion

        #region Input
        // W, A, S, D
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        // jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        #endregion

        // apply transforms
        controller.Move(move * speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

}

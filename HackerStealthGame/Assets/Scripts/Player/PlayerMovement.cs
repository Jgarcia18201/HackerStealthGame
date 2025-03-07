using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float rayLength = 1f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallHugOffset = 0.3f; // Distance from wall

    private CharacterController controller;
    private Animator animator;
    
    private bool isHuggingWall = false;
    private Vector3 wallNormal;
    private Vector3 targetWallPosition;
    private Quaternion targetRotation;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isHuggingWall)
        {
            HandleWallHugging();
        }
        else
        {
            HandleMovement();
            DetectWall();
        }
    }

    void HandleMovement()
    {
        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 move = new Vector3(moveHorizontal, 0f, moveVertical);

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        controller.Move(move * speed * Time.deltaTime);

        animator.SetFloat("Horizontal", moveHorizontal);
        animator.SetFloat("Vertical", moveVertical);
    }

    void DetectWall()
    {
        RaycastHit hit;
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (Physics.Raycast(transform.position, moveDirection, out hit, rayLength, wallLayer))
        {
            float dot = Vector3.Dot(hit.normal, moveDirection);
            if (dot < -0.7f) // Ensures the player is moving against the wall
            {
                isHuggingWall = true;
                wallNormal = hit.normal;

                // Set target position slightly away from the wall
                targetWallPosition = hit.point - wallNormal * wallHugOffset;

                // Rotate player's BACK to face the wall
                targetRotation = Quaternion.LookRotation(-wallNormal);

                animator.SetBool("isHugging", true);
            }
        }
    }

    void HandleWallHugging()
    {
        // Smoothly move to the wall-hugging position
        transform.position = Vector3.Lerp(transform.position, targetWallPosition, Time.deltaTime * 10f);

        // Smoothly rotate player so their back is against the wall
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 moveDirection = Vector3.Cross(wallNormal, Vector3.up) * moveHorizontal;

        controller.Move(moveDirection * speed * Time.deltaTime);
        animator.SetFloat("Horizontal", moveHorizontal);

        // Exit wall hug when player moves away from the wall
        if (Vector3.Dot(wallNormal, new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))) > -0.1f)
        {
            isHuggingWall = false;
            animator.SetBool("isHugging", false);
        }

        Debug.Log("Wall Normal: " + wallNormal);
        Debug.Log("Target Rotation: " + targetRotation.eulerAngles);
        Debug.Log("Current Rotation: " + transform.rotation.eulerAngles);

    }
}

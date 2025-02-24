using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float swimSpeed = 1.0f;
    private Vector2 swimDirection;
    private bool canFlip = true;
    void Start()
    {
        // Generate random movement
        swimDirection = Random.insideUnitCircle.normalized;
        if (swimDirection.x > 0) {
            // 设置物体的旋转为绕Y轴旋转0度，保持Z轴旋转不变
            transform.rotation = Quaternion.Euler(0, 0, -45);
        } else {
            // 设置物体的旋转为绕Y轴旋转180度，保持Z轴旋转不变
            transform.rotation = Quaternion.Euler(0, 180, -45);
        }
    }

    // Update is called once per frame
    void Update()
    {
        canFlip = true;
        // Move the fish
        if (Random.value < 0.3f * Time.deltaTime)
        {
            // Generate new random movement
            if (canFlip)
                swimDirection.x = -swimDirection.x;
        }
        if (Random.value < 0.8f * Time.deltaTime)
        {
            // Generate new random movement
            if (canFlip)
                swimDirection.y = -swimDirection.y;
        }
        Camera mainCamera = Camera.main;
        float horizontalSize = mainCamera.orthographicSize * mainCamera.aspect;
        float verticalSize = mainCamera.orthographicSize;
        if (transform.position.x < -horizontalSize) transform.position = new Vector3(-horizontalSize, transform.position.y, transform.position.z);
        if (transform.position.x > horizontalSize) transform.position = new Vector3(horizontalSize, transform.position.y, transform.position.z);
        if (transform.position.y < -verticalSize) transform.position = new Vector3(transform.position.x, -verticalSize, transform.position.z);
        if (transform.position.y > verticalSize) transform.position = new Vector3(transform.position.x, verticalSize, transform.position.z);
        Collider2D fishCollider = GetComponent<Collider2D>();

        float halfWidth = fishCollider.bounds.extents.x;
        float halfHeight = fishCollider.bounds.extents.y;
        if (transform.position.x + halfWidth < -horizontalSize || transform.position.x - halfWidth > horizontalSize ||
            transform.position.y + halfHeight < -verticalSize || transform.position.y - halfHeight > verticalSize)
        {
            // 鱼超出了边界，改变方向
            swimDirection = -swimDirection;
            canFlip = false;
            StartCoroutine(ReverseDIrectionWithDelay());
        }
        // change the model flip
        if (swimDirection.x > 0) {
            // 设置物体的旋转为绕Y轴旋转0度，保持Z轴旋转不变
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z), Time.deltaTime * 5f);
        } else {
            // 设置物体的旋转为绕Y轴旋转180度，保持Z轴旋转不变
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180, transform.rotation.eulerAngles.z), Time.deltaTime * 5f);
        }
        // Debug.Log("swimming direction is:" + swimDirection.x + "," + swimDirection.y);
        // Debug.Log("swimming speed is:" + swimSpeed);
        // Debug.Log("rotation direction is:" + transform.rotation.eulerAngles.y);
        transform.Translate(swimSpeed * Time.deltaTime * swimDirection, Space.World);
    }

    IEnumerator ReverseDIrectionWithDelay(float delay = 2.0f) {
        yield return new WaitForSeconds(delay);
        // swimDirection = -swimDirection;
        canFlip = true;
    }
}

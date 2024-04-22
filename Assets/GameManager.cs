using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform[] playerCheckpoints;
    Player player;
    Camera mainCamera;
    public Transform cameraContainer;
    public Vector3 cameraOffset;
    Vector2 angles;
    public float sensitivity;
    [Range(0f, 90f)] public float yRotationLimit;
    int lastCheckpoint;
    public TextMeshProUGUI dashesLeftText;
    public TextMeshProUGUI timerText;
    public static float timer;
    public bool reachedEnd;
    void Start()
    {
        GameObject playerClone = Instantiate(playerPrefab);
        player = playerClone.GetComponent<Player>();
        lastCheckpoint = 0;
        ResetPlayerToLastCheckpoint();
        mainCamera = Camera.main;
        InputManager.RegisterMouseInputCallback(MouseInputHandler);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        timer += Time.deltaTime;
        cameraContainer.position = player.transform.position;
        mainCamera.transform.localPosition = cameraOffset;
        dashesLeftText.text = $"Dashes Left = {player.dashesLeft}";
        timerText.text = $"Time: {timer}";
    }
    void MouseInputHandler(Vector2 mouseScreenPos, Vector2 mouseWorldPos, Vector2 mouseDelta)
    {
        angles.x += mouseDelta.x * sensitivity;
        angles.y += mouseDelta.y * sensitivity;
        angles.y = Mathf.Clamp(angles.y, -yRotationLimit, yRotationLimit);
        Quaternion xRotation = Quaternion.AngleAxis(angles.x, Vector3.up);
        Quaternion yRotation = Quaternion.AngleAxis(angles.y, Vector3.left);
        cameraContainer.transform.localRotation = xRotation * yRotation;
    }
    public void ResetPlayerToLastCheckpoint()
    {
        player.Reset();
        player.transform.position = playerCheckpoints[lastCheckpoint].position;
        timer = 0f;
    }
}

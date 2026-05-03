using UnityEngine;
using Unity.Cinemachine;

public class CameraFollowSetter : MonoBehaviour
{
    private void Start()
    {
        // 플레이어 찾기 (DontDestroyOnLoad에 있어도 찾힘)
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Player 없음");
            return;
        }

        // 현재 씬의 CinemachineCamera 찾기
        CinemachineCamera cam =
            FindFirstObjectByType<CinemachineCamera>();

        if (cam != null)
        {
            
            cam.Target.TrackingTarget = player.transform;
        }
    }
}
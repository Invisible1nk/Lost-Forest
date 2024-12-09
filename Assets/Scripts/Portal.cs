using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public int targetMapIndex; // 이동할 맵 번호
    public string targetPortal; // 이동할 포탈 ID

    private bool isPlayerInPortal = false; // 포탈 위에 플레이어가 있는지 확인

    private void Update()
    {
        if (isPlayerInPortal && Input.GetKeyDown(KeyCode.F))
        {
            TransportPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInPortal = true;
            ShowPortalMessage(true); // 메시지 표시
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInPortal = false;
            ShowPortalMessage(false); // 메시지 숨김
        }
    }

    private void TransportPlayer()
    {
        // 현재 활성화된 맵을 비활성화
        GameObject currentMap = GetCurrentMap();
        if (currentMap != null)
        {
            currentMap.SetActive(false);
        }

        // 타겟 맵 활성화
        GameObject targetMap = GetMapByIndex(targetMapIndex);
        if (targetMap != null)
        {
            targetMap.SetActive(true);

            // 타겟 포탈 찾기
            Portal targetPortalObj = FindTargetPortal(targetMap, targetPortal);
            if (targetPortalObj != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = targetPortalObj.transform.position;
            }
        }
    }

    private GameObject GetCurrentMap()
    {
        // Map 상위 오브젝트 아래의 활성화된 맵을 찾음
        Transform mapParent = GameObject.Find("Map").transform;
        foreach (Transform map in mapParent)
        {
            if (map.gameObject.activeSelf)
            {
                return map.gameObject;
            }
        }
        return null;
    }

    private GameObject GetMapByIndex(int mapIndex)
    {
        // Map 상위 오브젝트 아래에서 특정 인덱스의 맵을 찾음
        Transform mapParent = GameObject.Find("Map").transform;
        string mapName = "map" + mapIndex; // map0, map1 등의 이름으로 찾음
        foreach (Transform map in mapParent)
        {
            if (map.name == mapName)
            {
                return map.gameObject;
            }
        }
        Debug.LogError($"맵 {mapName}을 찾을 수 없습니다.");
        return null;
    }

    private Portal FindTargetPortal(GameObject map, string portalID)
    {
        // 활성화된 맵 내에서 타겟 포탈 찾기
        Portal[] portals = map.GetComponentsInChildren<Portal>();
        foreach (Portal portal in portals)
        {
            if (portal.name == portalID)
            {
                return portal;
            }
        }
        Debug.LogError($"타겟 포탈 {portalID}을 찾을 수 없습니다.");
        return null;
    }

    private void ShowPortalMessage(bool show)
    {
        // UI 메시지를 표시하거나 숨깁니다.
        if (show)
        {
            Debug.Log("Press 'F' to use the portal.");
        }
    }
}
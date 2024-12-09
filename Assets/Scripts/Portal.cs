using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public int targetMapIndex; // �̵��� �� ��ȣ
    public string targetPortal; // �̵��� ��Ż ID

    private bool isPlayerInPortal = false; // ��Ż ���� �÷��̾ �ִ��� Ȯ��

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
            ShowPortalMessage(true); // �޽��� ǥ��
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInPortal = false;
            ShowPortalMessage(false); // �޽��� ����
        }
    }

    private void TransportPlayer()
    {
        // ���� Ȱ��ȭ�� ���� ��Ȱ��ȭ
        GameObject currentMap = GetCurrentMap();
        if (currentMap != null)
        {
            currentMap.SetActive(false);
        }

        // Ÿ�� �� Ȱ��ȭ
        GameObject targetMap = GetMapByIndex(targetMapIndex);
        if (targetMap != null)
        {
            targetMap.SetActive(true);

            // Ÿ�� ��Ż ã��
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
        // Map ���� ������Ʈ �Ʒ��� Ȱ��ȭ�� ���� ã��
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
        // Map ���� ������Ʈ �Ʒ����� Ư�� �ε����� ���� ã��
        Transform mapParent = GameObject.Find("Map").transform;
        string mapName = "map" + mapIndex; // map0, map1 ���� �̸����� ã��
        foreach (Transform map in mapParent)
        {
            if (map.name == mapName)
            {
                return map.gameObject;
            }
        }
        Debug.LogError($"�� {mapName}�� ã�� �� �����ϴ�.");
        return null;
    }

    private Portal FindTargetPortal(GameObject map, string portalID)
    {
        // Ȱ��ȭ�� �� ������ Ÿ�� ��Ż ã��
        Portal[] portals = map.GetComponentsInChildren<Portal>();
        foreach (Portal portal in portals)
        {
            if (portal.name == portalID)
            {
                return portal;
            }
        }
        Debug.LogError($"Ÿ�� ��Ż {portalID}�� ã�� �� �����ϴ�.");
        return null;
    }

    private void ShowPortalMessage(bool show)
    {
        // UI �޽����� ǥ���ϰų� ����ϴ�.
        if (show)
        {
            Debug.Log("Press 'F' to use the portal.");
        }
    }
}
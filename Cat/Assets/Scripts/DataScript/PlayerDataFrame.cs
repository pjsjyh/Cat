using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataFrame : MonoBehaviour
{
    //�÷��̾ ������ ������ �Ѱ� ����
    [System.Serializable]
    public class PlayerData
    {
        public PlayerPersonalData playerPersonalData;
        public PlayerRoomData roomData;
        public PlayerCatData catData;

        public PlayerData()
        {
            playerPersonalData = new PlayerPersonalData
            {
                playerName = "newName222",
                playerCoin = 0,
                playerCash = 0
            };

            roomData = new PlayerRoomData(); // �ʿ��ϸ� ���� �ʱ�ȭ �߰�
            catData = new PlayerCatData();   // List �ʱ�ȭ ����
        }
    }
    [System.Serializable]
    public class PlayerPersonalData
    {
        public string playerName;
        public int playerCoin;
        public int playerCash;
    }
    [System.Serializable]
    public class PlayerRoomData
    {
        List<Furniture> furnitureList = new List<Furniture>();
    }

    [System.Serializable]
    public class PlayerCatData
    {
        List<Cat> catDataList = new List<Cat>();
    }
}

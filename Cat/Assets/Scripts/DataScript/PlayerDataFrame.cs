using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataFrame : MonoBehaviour
{
    //�÷��̾ ������ ������ �Ѱ� ����

    public class PlayerData
    {
        public PlayerPersonalData playerPersonalData;
        public PlayerRoomData roomData;
        public PlayerCatData catData;

        public PlayerData()
        {
            playerPersonalData = new PlayerPersonalData
            {
                playerName = "newName",
                playerCoin = 0,
                playerCash = 0
            };

            roomData = new PlayerRoomData(); // �ʿ��ϸ� ���� �ʱ�ȭ �߰�
            catData = new PlayerCatData();   // List �ʱ�ȭ ����
        }
    }
    public class PlayerPersonalData
    {
        public string playerName;
        public int playerCoin;
        public int playerCash;
    }
    public class PlayerRoomData
    {

    }
    public class Furniture
    {
        //��ġ, �̸�, ��ġ����
    }

    public class PlayerCatData
    {
        List<CatData> catDataList = new List<CatData>();
    }

    public class CatData
    {
        //�� ����� �ִϸ��̼�, �ڽ�Ƭ, ���
        public float health;
        public string name;
        public float jump;
        public string currentRoomNum;
    }
}

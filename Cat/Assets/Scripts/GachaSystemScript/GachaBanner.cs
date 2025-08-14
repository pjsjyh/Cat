using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GachaBanner", menuName = "Gacha/Banner")]
public class GachaBanner : ScriptableObject
{
    public GachaPool pool;

    [Tooltip("���� ��(�Ǵ� ��ȭ) �ܰ躰 �ʿ� ���� ��. �ε��� = ���� ����(0����).")]
    public List<int> shardCostsByLevel = new() { 3, 5, 10, 18, 30 };

    // ��޺� ���� ���� �� ����(�����̸� (3,3)ó��)
    public List<RarityShardRange> shardDrops = new()
    {
         new RarityShardRange{ rarity = Rarity.Legendary, range = new Vector2Int(1,2) },
    new RarityShardRange{ rarity = Rarity.Epic,      range = new Vector2Int(1,3) },
    new RarityShardRange{ rarity = Rarity.Rare,      range = new Vector2Int(2,4) },
    new RarityShardRange{ rarity = Rarity.Common,    range = new Vector2Int(3,5) },
    };

    // ���������� ��� Ȯ�� (minTotalRolls ���� �������� ���� ����)
    public List<StageConfig> stages = new()
    {
        new StageConfig{ stageId = "LV1", minTotalRolls = 0  },
        new StageConfig{ stageId = "LV2", minTotalRolls = 3  },
        new StageConfig{ stageId = "LV3", minTotalRolls = 10 },
    };

    public int costPerRoll = 300;  // ��ȭ �Ҹ�(���ϸ� 10��/35�� ���� ��Ģ �߰�)
}
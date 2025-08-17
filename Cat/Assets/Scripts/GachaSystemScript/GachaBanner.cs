using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GachaBanner", menuName = "Gacha/Banner")]
public class GachaBanner : ScriptableObject
{
    public GachaPool pool;

    [Tooltip("레벨 업(또는 진화) 단계별 필요 조각 수. 인덱스 = 현재 레벨(0부터).")]
    public List<int> shardCostsByLevel = new() { 3, 5, 10, 18, 30 };

    // 등급별 지급 조각 수 범위(고정이면 (3,3)처럼)
    public List<RarityShardRange> shardDrops = new()
    {
         new RarityShardRange{ rarity = Rarity.Legendary, range = new Vector2Int(1,2) },
    new RarityShardRange{ rarity = Rarity.Epic,      range = new Vector2Int(1,3) },
    new RarityShardRange{ rarity = Rarity.Rare,      range = new Vector2Int(2,4) },
    new RarityShardRange{ rarity = Rarity.Common,    range = new Vector2Int(3,5) },
    };

    // 스테이지별 레어도 확률 (minTotalRolls 기준 오름차순 정렬 권장)
    public List<StageConfig> stages = new()
    {
        new StageConfig{ stageId = "LV1", minTotalRolls = 0  },
        new StageConfig{ stageId = "LV2", minTotalRolls = 3  },
        new StageConfig{ stageId = "LV3", minTotalRolls = 10 },
    };

    public int costPerRoll = 300;  // 재화 소모(원하면 10연/35연 할인 규칙 추가)
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GachaSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject objListPanel;
    [SerializeField]
    private GameObject gachaBox;
    [SerializeField]
    private GameObject gachaPanel;

    [Header("Assign in Inspector")]
    public GachaBanner banner;

    [Header("Runtime State")]
    public int currency = 30000;   // 보유 재화
    public int totalGachaCount = 0;     // 누적 뽑기 수 (스테이지 판정용)

    // 아이템별 조각 수 (플레이어 상태)
    private readonly Dictionary<string, int> shards = new Dictionary<string, int>();

    [Serializable]
    public struct RollResult
    {
        public string stageId;
        public Furniture item;
        public Rarity rarity;
       // public int shardGain;
      //  public int shardAfter;     // 지급 후 남은 조각
       // public bool leveledUp;     // 이번 지급으로 nowOwned가 올라갔는지
       // public int ownedAfter;     // 상승 후 nowOwned
    }

    // === Public APIs ===
    public List<RollResult> Roll10() => RollMulti(10);
    public List<RollResult> Roll35() => RollMulti(35);
    public List<RollResult> RollMulti(int count)
    {
        ClearObjListPanel();
        var list = new List<RollResult>(count);
        for (int i = 0; i < count; i++)
            list.Add(RollOnce());
        return list;
    }
    public void ClearObjListPanel()
    {
        //리스트 ui삭제
        for (int i = objListPanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(objListPanel.transform.GetChild(i).gameObject);
        }
    }

    public RollResult RollOnce()
    {
        if (!banner) { Debug.LogWarning("Banner not assigned."); return default; }
        //if (currency < banner.costPerRoll) { Debug.LogWarning("Not enough currency."); return default; }

        // 1) 스테이지 & 레어도
        var stage = GetStageFor(totalGachaCount);
        var rarity = DecideRarity(stage);

        // 2) 아이템 선택(해당 레어도 + 가중치)
        var item = PickItemByWeight(banner.pool, rarity);
        if (!item) { Debug.LogWarning("No item candidate."); return default; }

        GameObject pickObj = Instantiate(gachaBox, objListPanel.transform);
        pickObj.GetComponent<GachaBoxItem>().ChangeImage(item.FurnitureThumbnail);

        var res = new RollResult
        {
            stageId = stage.stageId,
            item = item,
            rarity = rarity,
           // shardGain = gain,
            //shardAfter = curShards,
            //leveledUp = (item.nowOwned > beforeLevel),
            //ownedAfter = item.nowOwned
        };

        // 3) 조각 지급
        //int gain = GetShardGain(rarity);
        AddShards(item.furnitureId, 1);
        return res;
        //        // 4) 레벨업(진화) 처리: 현재 nowOwned 기준으로 비용 차감
        //        int beforeLevel = item.nowOwned;
        //        while (item.nowOwned < item.maxOwned)
        //        {
        //            int need = CostForLevel(item.nowOwned);
        //            if (curShards < need) break;
        //            curShards -= need;
        //            item.nowOwned += 1;      // ★ 네가 SO의 nowOwned를 쓰는 구조 그대로
        //        }
        //        shards[item.furnitureId] = curShards;

        //        // 5) 자원/누적
        //        currency -= banner.costPerRoll;
        //        totalGachaCount += 1;

        //        var res = new RollResult
        //        {
        //            stageId = stage.stageId,
        //            item = item,
        //            rarity = rarity,
        //            shardGain = gain,
        //            shardAfter = curShards,
        //            leveledUp = (item.nowOwned > beforeLevel),
        //            ownedAfter = item.nowOwned
        //        };

        //#if UNITY_EDITOR
        //        Debug.Log($"[{res.stageId}] {item.furnitureId} ({res.rarity}) +{res.shardGain} → shards:{res.shardAfter}, owned:{res.ownedAfter}");
        //#endif
        //        return res;
    }
    public void OnClickGachaBtn(int cash, int boxnum)
    {
        //클릭해서 가챠
        if (PlayerDataManager.Instance.ReturnPlayerCash() >= cash)
        {
            gachaPanel.SetActive(true);
            var r = RollMulti(boxnum);
            foreach (var kvp in shards)
            {
                FurnitureManager.Instance.AddPieces(kvp.Key, kvp.Value);
            }
            FurnitureManager.Instance.DataUpdateFurniture();

            totalGachaCount += 1; //가챠횟수 증가
        }
     
    }
    StageConfig GetStageFor(int totalRollsBefore)
    {
        //몇번째 스테이지(가챠레벨)인지 검사
        StageConfig pick = null;
        foreach (var s in banner.stages)
            if (totalRollsBefore >= s.minTotalRolls) pick = s;
        return pick ?? banner.stages[0];
    }

    Rarity DecideRarity(StageConfig stage)
    {
        // 합이 1.0이 아닐 수도 있으니 정규화
        float sum = 0f;
        for (int i = 0; i < stage.rates.Count; i++) sum += stage.rates[i].rate;
        if (sum <= 0f) return Rarity.Common;

        float r = UnityEngine.Random.value;
        float acc = 0f;
        for (int i = 0; i < stage.rates.Count; i++)
        {
            acc += stage.rates[i].rate / sum;
            if (r < acc) return stage.rates[i].rarity;
        }
        return stage.rates[^1].rarity;
    }

    Furniture PickItemByWeight(GachaPool pool, Rarity rarity)
    {
        //뽑을 아이템 리스트 셋팅용
        var list = new List<(Furniture f, int w)>();
        int total = 0;

        foreach (var e in pool.entries)
        {
            //내가 찾는 레어도인지, 가챠템이 맞는지 검사
            if (!e.item || !e.item.gachaEligible) continue;
            if (e.item.rarity != rarity) continue;

            int w = Mathf.Max(1, e.weight);
            list.Add((e.item, w));
            total += w;
        }

        // 해당 레어도에 후보가 없으면 전체에서 폴백 (선택)
        if (list.Count == 0)
        {
            foreach (var e in pool.entries)
            {
                if (!e.item || !e.item.gachaEligible) continue;
                int w = Mathf.Max(1, e.weight);
                list.Add((e.item, w));
                total += w;
            }
            if (list.Count == 0) return null;
        }
        //누적합으로 뽑을 아이템 선정
        int r = UnityEngine.Random.Range(0, total);
        int acc = 0;
        foreach (var (f, w) in list)
        {
            acc += w;
            if (r < acc) return f;
        }
        return list[0].f;
    }
    public void AddPiece(string furnitureId, int amount)
    {
        // 저장된 가구 목록 불러오기
        var list = PlayerDataManager.Instance.playerData.roomData.furnitureList;

        foreach (var data in list)
        {
            if (data.id == furnitureId)
            {
                data.nowPeice += amount;
                break;
            }
        }

        // 필요하면 저장
        PlayerDataManager.Instance.SaveData();
    }
    int GetShardGain(Rarity rarity)
    {
        for (int i = 0; i < banner.shardDrops.Count; i++)
            if (banner.shardDrops[i].rarity == rarity)
                return UnityEngine.Random.Range(banner.shardDrops[i].range.x, banner.shardDrops[i].range.y + 1);
        return 1;
    }

    int CostForLevel(int level)
    {
        if (banner.shardCostsByLevel == null || banner.shardCostsByLevel.Count == 0) return int.MaxValue;
        int idx = Mathf.Clamp(level, 0, banner.shardCostsByLevel.Count - 1);
        return Mathf.Max(1, banner.shardCostsByLevel[idx]);
    }

    int GetShards(string id) => shards.TryGetValue(id, out var n) ? n : 0;
    void AddShards(string id, int add) => shards[id] = GetShards(id) + add;
}

[System.Serializable] 
public class RarityRate 
{
    //각 가챠 등급 별 확률
    public Rarity rarity; 
    public float rate; 
}
[System.Serializable]
public class StageConfig
{
    public string stageId = "LV1";
    public int minTotalRolls = 0;   
    // 이 값 이상이면 이 스테이지 적용 (가장 큰 min 이하 중 최댓값 채택) 0~2 = 1단계 / 3~7 = 2단계
    public List<RarityRate> rates = new()
    {
       new RarityRate{ rarity = Rarity.Legendary, rate = 0.01f },
    new RarityRate{ rarity = Rarity.Epic,      rate = 0.04f },
    new RarityRate{ rarity = Rarity.Rare,      rate = 0.20f },
    new RarityRate{ rarity = Rarity.Common,    rate = 0.75f },
    };
}


[System.Serializable]
public class GachaEntry
{
    //이번 뽑기에 나올 수 있는 아이템 목록
    //같은 레어도 내의 각 아이템의 비중(클수록 잘뽑힘)
    public Furniture item;
    public int weight = 100;
}




[System.Serializable] public class RarityShardRange { public Rarity rarity; public Vector2Int range = new(3, 3); }


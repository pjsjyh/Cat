using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GachaPool", menuName = "Gacha/Pool")]
public class GachaPool : ScriptableObject
{
    //���� �̱� ����Ʈ
    public List<GachaEntry> entries = new List<GachaEntry>();
}

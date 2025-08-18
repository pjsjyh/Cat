using UnityEngine;
using System.Collections.Generic;
using System.Linq;
[CreateAssetMenu(menuName = "TrickGame/Level")]
public class TrickGame : ScriptableObject
{
    [Min(1)] public int level = 1;   // 레벨 번호(표시/식별용)
    [Min(2)] public int cupCount = 3;
    [Min(1)] public int swapCount = 8;
}

[CreateAssetMenu(menuName = "TrickGame/Level Set")]
public class TrickGameLevelSet : ScriptableObject
{
    public List<TrickGame> levels = new List<TrickGame>();

    public TrickGame GetByLevelNumber(int levelNumber)
        => levels.FirstOrDefault(l => l && l.level == levelNumber);

    public TrickGame GetByIndex(int index)
        => (index >= 0 && index < levels.Count) ? levels[index] : null;
}
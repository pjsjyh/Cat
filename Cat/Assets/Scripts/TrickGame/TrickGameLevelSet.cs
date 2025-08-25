using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(menuName = "TrickGame/Level Set")]
public class TrickGameLevelSet : ScriptableObject
{
    public List<TrickGame> levels = new List<TrickGame>();

    public TrickGame GetByLevelNumber(int levelNumber)
        => levels.FirstOrDefault(l => l && l.level == levelNumber);

    public TrickGame GetByIndex(int index)
        => (index >= 0 && index < levels.Count) ? levels[index] : null;
}

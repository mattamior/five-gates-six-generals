using UnityEngine;

namespace SanguoLegend.Data
{
    /// <summary>Authoring data for one pass. Kept out of scenes so levels can be reordered or regenerated.</summary>
    [CreateAssetMenu(menuName = "Sanguo/Level Definition", fileName = "Level_")]
    public sealed class LevelDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string levelId = "dongling";
        public string chapterTitle = "第一关 · 东岭关";
        public string chapterSubtitle = "过五关斩六将";

        [Header("Encounter")]
        public string generalName = "孔秀";
        public int soldierCount = 12;
        public float generalHealth = 160f;

        [Header("Narrative")]
        [TextArea(3, 6)]
        public string intro = "关羽辞别曹操，护送二嫂北上。东岭关守将孔秀索要文凭，关羽无以为对。";

        public string sceneName = "Graybox_Dongling";
    }
}

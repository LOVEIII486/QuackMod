using System.Collections.Generic;
using FastModdingLib;
using Duckov.Quests;

namespace QuackItem.Quests
{
    /// <summary>
    /// 存储所有自定义任务的静态定义
    /// </summary>
    public static class QuestDefinitions
    {
        public static readonly QuestData Quest_GoldenCarrot = new QuestData
        {
            ID = 777001,
            displayName = "金色的期许",
            description = "据传有一种胡萝卜通体金黄，蕴含神秘的力量。如果你能证明你的实力，我将把它交给你。",
            questGiver = QuestGiverID.Jeff,
            requireLevel = 1,
            tasks = new List<TaskData>
            {
                new TaskRequireMoney { id = 1, money = 5000 },
                new TaskKillCount { id = 2, requireAmount = 5, requireEnemy = "Cname_Scav" }
            },
            rewards = new List<RewardData>
            {
                new RewardGiveItem { id = 1, itemTypeID = 777001, amount = 1 },
                new RewardEXP { id = 2, amount = 500 }
            }
        };

        public static readonly List<QuestData> AllQuests = new List<QuestData>
        {
            Quest_GoldenCarrot
        };
        
        // 3. 任务关系定义 (ID, before, after)
        // 这里的 Tuple 代表 (当前任务ID, 前置ID, 后置ID)
        public static readonly List<(int id, int before, int after)> AllRelations = new List<(int, int, int)>
        {
            (777001, -1, -1)
        };
    }
}
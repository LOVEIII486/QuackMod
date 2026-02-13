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
        public static readonly QuestData Quest_GoldenCarrot2 = new QuestData
        {
            ID = 777001,
            displayName = "Quest_GoldenCarrot2_Name",
            description = "Quest_GoldenCarrot2_Desc",
            questGiver = QuestGiverID.Fo,
            requireLevel = 1,
            tasks = new List<TaskData>
            {
                new TaskRequireMoney { id = 1, money = 2000 },
                new TaskKillCount { id = 2, requireAmount = 3, requireEnemy = "Cname_RaiderIce" }
            },
            rewards = new List<RewardData>
            {
                new RewardGiveItem { id = 1, itemTypeID = 777002, amount = 3 },
                new RewardEXP { id = 2, amount = 500 },
                new RewardUnlockItem { id = 3, itemTypeID = 777002 }
            }
        };
        
        public static readonly QuestData Quest_LifeFruit = new QuestData
        {
            ID = 777002,
            displayName = "Quest_LifeFruit_Name",
            description = "Quest_LifeFruit_Desc",
            questGiver = QuestGiverID.Fo,
            requireLevel = 1,
            tasks = new List<TaskData>
            {
                new TaskRequireItem { id = 1, itemTypeID = 1109, requiredAmount = 2},
                new TaskRequireItem { id = 2, itemTypeID = 1011, requiredAmount = 2},
                new TaskRequireItem { id = 3, itemTypeID = 888, requiredAmount = 2},
            },
            rewards = new List<RewardData>
            {
                new RewardGiveItem { id = 1, itemTypeID = 777003, amount = 2 },
                new RewardEXP { id = 2, amount = 800 },
                new RewardUnlockItem { id = 3, itemTypeID = 777003 }
            }
        };

        public static readonly List<QuestData> AllQuests = new List<QuestData>
        {
            Quest_GoldenCarrot2,
            Quest_LifeFruit
        };
        
        // 任务关系定义 (ID, before, after)
        // 这里的 Tuple 代表 (当前任务ID, 前置ID, 后置ID)
        public static readonly List<(int id, int before, int after)> AllRelations = new List<(int, int, int)>
        {
            (777001, -1, -1),
            (777002, -1, -1)
        };
    }
}
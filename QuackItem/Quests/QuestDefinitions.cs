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
            requireScene = "Level_SnowMilitaryBase_Main",
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
        
        public static readonly QuestData Quest_AmmoCase = new QuestData
        {
            ID = 777003,
            displayName = "Quest_AmmoCase_Name",
            description = "Quest_AmmoCase_Desc",
            questGiver = QuestGiverID.Xavier,
            requireLevel = 1,
            requireScene = "Level_HiddenWarehouse_Main",
            tasks = new List<TaskData>
            {
                new TaskKillCount { id = 1, requireAmount = 5, requireEnemy = "Cname_Usec" },
                new TaskRequireItem { id = 2, itemTypeID = 91, requiredAmount = 1 },
                new TaskRequireItem { id = 3, itemTypeID = 332, requiredAmount = 5 },
                new TaskRequireItem { id = 3, itemTypeID = 367, requiredAmount = 3 }
            },
            rewards = new List<RewardData>
            {
                new RewardEXP { id = 1, amount = 1500 },
                new RewardUnlockItem { id = 2, itemTypeID = 777007 }
            }
        };
        
        public static readonly QuestData Quest_AGDumbbell = new QuestData
        {
            ID = 777004,
            displayName = "Quest_AGDumbbell_Name",
            description = "Quest_AGDumbbell_Desc",
            questGiver = QuestGiverID.Mud,
            requireLevel = 1,
            requireScene = "Level_SnowMilitaryBase_Main",
            tasks = new List<TaskData>
            {
                new TaskKillCount { id = 1, requireAmount = 1, requireEnemy = "Cname_Tagilla" },
                new TaskRequireItem { id = 2, itemTypeID = 1263, requiredAmount = 5 },
            },
            rewards = new List<RewardData>
            {
                new RewardEXP { id = 1, amount = 3000 },
                new RewardGiveItem { id = 2, itemTypeID = 777009, amount = 1 },
                new RewardUnlockItem { id = 3, itemTypeID = 777009 }
            }
        };
        
        public static readonly List<QuestData> AllQuests = new List<QuestData>
        {
            Quest_GoldenCarrot2,
            Quest_LifeFruit,
            Quest_AmmoCase,
            Quest_AGDumbbell
        };
        
        // 任务关系定义 (ID, before, after)
        // 这里的 Tuple 代表 (当前任务ID, 前置ID, 后置ID)
        public static readonly List<(int id, int before, int after)> AllRelations = new List<(int, int, int)>
        {
            (777001, -1, -1),
            (777002, -1, -1),
            (777003, 48, -1),
            (777004, 642, -1)
        };
    }
}
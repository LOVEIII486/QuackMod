using ItemStatsSystem;
using FastModdingLib;

namespace QuackItem.Behaviors
{
    /// <summary>
    /// 用于在 Items.cs 中配置的桥梁类
    /// </summary>
    public class QuackCookieData : UsageBehaviorData
    {
        public int buffId;
        public string popMessage;

        /// <summary>
        /// FML 内部在创建物品时会调用此方法
        /// </summary>
        public override UsageBehavior GetBehavior(Item item)
        {
            // 1. 将逻辑组件挂载到物品实例上
            QuackCookieBehavior behavior = item.gameObject.AddComponent<QuackCookieBehavior>();

            // 2. 将配置参数从 Data 传递给 Runtime
            behavior.BuffID = this.buffId;
            behavior.Message = this.popMessage;

            return behavior;
        }
    }
}
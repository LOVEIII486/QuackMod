using Duckov.Buffs;

namespace QuackCore.BuffSystem.Logic
{
    /// <summary>
    /// 免疫逻辑：在 Buff 存续期间持续通过 ID 移除指定的 Buff 列表
    /// </summary>
    public class BuffImmunityLogic : IQuackBuffLogic
    {
        private readonly int[] _immuneBuffIDs;
        private readonly bool _removeOneLayer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="removeOneLayer">是否仅移除一层</param>
        /// <param name="buffIDs">需要免疫/移除的 Buff ID 列表</param>
        public BuffImmunityLogic(bool removeOneLayer, params int[] buffIDs)
        {
            _removeOneLayer = removeOneLayer;
            _immuneBuffIDs = buffIDs;
        }

        public void OnSetup(Buff buff, CharacterMainControl target)
        {
            RemoveTargetBuffs(target);
        }

        public void OnUpdate(Buff buff, CharacterMainControl target)
        {
            RemoveTargetBuffs(target);
        }

        public void OnDestroy(Buff buff, CharacterMainControl target)
        {
        }

        private void RemoveTargetBuffs(CharacterMainControl target)
        {
            if (target == null || _immuneBuffIDs == null) return;

            foreach (var id in _immuneBuffIDs)
            {
                target.RemoveBuff(id, _removeOneLayer);
            }
        }
    }
}
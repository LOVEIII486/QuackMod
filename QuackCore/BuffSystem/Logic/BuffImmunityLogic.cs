using Duckov.Buffs;

namespace QuackCore.BuffSystem.Logic
{
    /// <summary>
    /// 免疫逻辑
    /// </summary>
    public class BuffImmunityLogic : IQuackBuffLogic
    {
        private readonly int[] _immuneBuffIDs;
        
        public BuffImmunityLogic(bool removeOneLayer, params int[] buffIDs)
        {
            _immuneBuffIDs = buffIDs;
        }

        public void OnSetup(Buff buff, CharacterMainControl target)
        {
            // 注册免疫 ID
            QuackImmunityHandler.RegisterImmunity(target, _immuneBuffIDs);
            
            if (_immuneBuffIDs != null)
            {
                foreach (var id in _immuneBuffIDs)
                    target.RemoveBuff(id, false);
            }
        }

        public void OnUpdate(Buff buff, CharacterMainControl target)
        {
        }

        public void OnDestroy(Buff buff, CharacterMainControl target)
        {
            QuackImmunityHandler.UnregisterImmunity(target, _immuneBuffIDs);
        }
    }
}
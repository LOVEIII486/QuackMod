using Duckov.Buffs;

namespace QuackCore.BuffSystem.Effects
{
    public class DamageEffect : IQuackBuffEffect
    {
        private readonly float _damage;
        private readonly bool _ignoreArmor;
        private readonly DamageTypes _type;
        
        /// <summary>
        /// 伤害效果
        /// </summary>
        /// <param name="damage">伤害数值</param>
        /// <param name="ignoreArmor">是否无视护甲</param>
        /// <param name="type">伤害类型</param>
        public DamageEffect(float damage, bool ignoreArmor = true, DamageTypes type = DamageTypes.normal)
        {
            _damage = damage;
            _ignoreArmor = ignoreArmor;
            _type = type;
        }

        public void OnApplied(Buff buff, CharacterMainControl target)
        {
            if (target == null || target.mainDamageReceiver == null) return;

            DamageInfo info = new DamageInfo(buff.fromWho);
            info.damageValue = _damage;
            info.isFromBuffOrEffect = true;
            info.ignoreArmor = _ignoreArmor;
            info.ignoreDifficulty = true;
            info.damageType = _type;
            info.buff = buff;
            target.mainDamageReceiver.Hurt(info);
            
            ModLogger.LogDebug($"[DamageEffect] 对 {target.name} 造成了 {_damage} 点官方伤害 (忽略护甲: {_ignoreArmor})");
        }

        public void OnRemoved(Buff buff, CharacterMainControl target) { }
    }
}
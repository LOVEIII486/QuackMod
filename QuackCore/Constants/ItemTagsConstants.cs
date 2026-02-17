namespace QuackCore.Constants
{
    /// <summary>
    /// 物品标签(Tags)常量定义。
    /// 包含所有原版及扩展系统使用的标签 Key。
    /// </summary>
    public static class ItemTagsConstants
    {
        #region 1. 枪械专用配件 (Weapon Specific Accessories)
        public const string Acc_AHBow = "Acc_AHBow"; // 反曲弓专用
        public const string Acc_ArcadeGun = "Acc_ArcadeGun"; // 玩具枪专用
        public const string Acc_Aug = "Acc_Aug"; // StG 77专用
        public const string Acc_Candy = "Acc_Candy"; // 糖果枪专用
        public const string Acc_CubeGun = "Acc_CubeGun"; // 能量枪专用
        public const string Acc_Glick = "Acc_Glick"; // 格力克专用
        public const string Acc_M14 = "Acc_M14"; // M14专用
        public const string Acc_Mosin = "Acc_Mosin"; // Mosin-Nagant专用
        public const string Acc_PP19Bizon = "Acc_PP19Bizon"; // Bizon-2专用
        public const string Acc_RPD = "Acc_RPD"; // RPD专用
        public const string Acc_SKS = "Acc_SKS"; // SKS-45专用
        public const string Acc_Balett = "Acc_Balett"; // M107专属
        public const string Acc_Hand = "Acc_Hand"; // 土制猎枪专属
        public const string Acc_LightSaber = "Acc_LightSaber"; // 伞柄军刀专属
        public const string Acc_M700 = "Acc_M700"; // M700专属
        public const string Acc_NMGun = "Acc_NMGun"; // 纳米枪专属
        public const string Acc_ShitGun = "Acc_ShitGun"; // 粑粑枪专用
        public const string Acc_SnowBoss = "Acc_SnowBoss"; // BSG专属
        public const string Acc_SnowGun = "Acc_SnowGun"; // 雪球枪专用
        public const string Acc_TecSniper = "Acc_TecSniper"; // TS-128专属
        public const string Acc_UZI = "Acc_UZI"; // UZI专属
        #endregion

        #region 2. 装备与护甲 (Equipment & Armor)
        public const string Accessory = "Accessory"; // 配件
        public const string SpecialAcc = "SpecialAcc"; // 特殊配件
        public const string Armor = "Armor"; // 身体护甲
        public const string Backpack = "Backpack"; // 背包
        public const string FaceMask = "FaceMask"; // 面部
        public const string Helmat = "Helmat"; // 头部
        public const string Headset = "Headset"; // 耳机
        public const string Equipment = "Equipment"; // 装备
        public const string DecorateEquipment = "DecorateEquipment"; // 装饰性装备
        public const string Repairable = "Repairable"; // 可维修
        public const string DontDropOnDeadInSlot = "DontDropOnDeadInSlot"; // 绑定装备
        #endregion

        #region 3. 武器与枪械分类 (Weapons & Gun Types)
        public const string Weapon = "Weapon"; // 武器
        public const string Weapon_LV1 = "Weapon_LV1"; // 低级武器
        public const string Gun = "Gun"; // 枪械
        public const string MeleeWeapon = "MeleeWeapon"; // 近战武器
        public const string Explosive = "Explosive"; // 爆炸物
        public const string GunType_AR = "GunType_AR"; // 步枪
        public const string GunType_ARR = "GunType_ARR"; // 弓箭
        public const string GunType_BR = "GunType_BR"; // 战斗步枪
        public const string GunType_MAG = "GunType_MAG"; // 马格南
        public const string GunType_PST = "GunType_PST"; // 手枪
        public const string GunType_PWS = "GunType_PWS"; // 小能量枪
        public const string GunType_Rifle = "GunType_Rifle"; // 步枪
        public const string GunType_Rocket = "GunType_Rocket"; // 火箭筒
        public const string GunType_Shot = "GunType_Shot"; // 霰弹枪
        public const string GunType_SHT = "GunType_SHT"; // 霰弹枪
        public const string GunType_SMG = "GunType_SMG"; // 冲锋枪
        public const string GunType_Sniper = "GunType_Sniper"; // 狙击枪
        public const string GunType_SNP = "GunType_SNP"; // 狙击枪
        #endregion

        #region 4. 改装组件 (Gun Parts)
        public const string Grip = "Grip"; // 握把
        public const string Muzzle = "Muzzle"; // 枪口
        public const string Magazine = "Magazine"; // 弹夹
        public const string Scope = "Scope"; // 瞄具
        public const string Stock = "Stock"; // 枪托
        public const string TecEquip = "TecEquip"; // 战术装备
        #endregion

        #region 5. 消耗品与医疗 (Consumables & Medical)
        public const string Food = "Food"; // 食物
        public const string Drink = "Drink"; // 饮品
        public const string Medic = "Medic"; // 医疗用品
        public const string Healing = "Healing"; // 治疗
        public const string Injector = "Injector"; // 注射器
        public const string Bullet = "Bullet"; // 子弹
        #endregion

        #region 6. 钓鱼与生物 (Fishing & Biology)
        public const string Fish = "Fish"; // 鱼
        public const string Bait = "Bait"; // 鱼饵
        public const string Earthworm = "Earthworm"; // 蚯蚓
        public const string Fish_OnlyDay = "Fish_OnlyDay"; // 仅白天可钓
        public const string Fish_OnlyNight = "Fish_OnlyNight"; // 仅晚上可钓
        public const string Fish_OnlyRainDay = "Fish_OnlyRainDay"; // 仅雨天可钓
        public const string Fish_OnlyStorm = "Fish_OnlyStorm"; // 仅风暴可钓
        public const string Fish_OnlySunDay = "Fish_OnlySunDay"; // 仅晴天可钓
        public const string Fish_Other = "Fish_Other"; // 其他鱼类
        public const string Fish_Special = "Fish_Special"; // 特殊鱼类
        public const string Pelt = "Pelt"; // 毛皮
        #endregion

        #region 7. 配方与功能 (Formulas & Function)
        public const string Formula = "Formula"; // 配方
        public const string Formula_Blueprint = "Formula_Blueprint"; // 高级蓝图
        public const string Formula_Cook = "Formula_Cook"; // 菜谱
        public const string Formula_Medic = "Formula_Medic"; // 医疗配方
        public const string Formula_Normal = "Formula_Normal"; // 工作台配方
        public const string Formula_Printer = "Formula_Printer"; // 3D打印设计图
        public const string Seed = "Seed"; // 种子
        public const string Crop = "Crop"; // 作物
        public const string Tool = "Tool"; // 工具
        #endregion

        #region 8. 电子产品与电脑 (Electronics & Computer)
        public const string Electric = "Electric"; // 电子产品
        public const string Computer = "Computer"; // 电脑配件
        public const string ComputerParts_GPU = "ComputerParts_GPU"; // 显卡
        public const string GamingConsole = "GamingConsole"; // 游戏主机
        public const string Cartridge = "Cartridge"; // 游戏卡带
        public const string Monitor = "Monitor"; // 显示器
        public const string FcController = "FcController"; // 控制器
        #endregion

        #region 9. 系统逻辑与特殊 (System & Logic)
        public const string AdvancedDebuffMode = "AdvancedDebuffMode"; // 高级Debuff模式
        public const string Cash = "Cash"; // 现金
        public const string Character = "Character"; // 角色
        public const string Continer = "Continer"; // 容器
        public const string DestroyInBase = "DestroyInBase"; // 无法带出
        public const string DestroyOnLootBox = "DestroyOnLootBox"; // 玩家不可见
        public const string DogTag = "DogTag"; // 狗牌
        public const string Information = "Information"; // 情报物品
        public const string Key = "Key"; // 钥匙
        public const string SpecialKey = "SpecialKey"; // 特殊钥匙(无法录入)
        public const string LockInDemo = "LockInDemo"; // Demo锁定
        public const string NotForSell = "NotForSell"; // 不可出售
        public const string NotSellable = "NotSellable"; // 不可出售
        public const string NotNested = "NotNested"; // 不可嵌套
        public const string Quest = "Quest"; // 任务物品
        public const string Sticky = "Sticky"; // 受保护(绑定)
        public const string Totem = "Totem"; // 图腾
        public const string SnowLand = "SnowLand"; // 雪地
        public const string Western = "Western"; // 西方
        public const string MiniGame = "MiniGame"; // 小游戏
        public const string JLab = "JLab"; // J实验室
        #endregion

        #region 10. 杂物与收藏 (Misc & Collections)
        public const string Daily = "Daily"; // 日常用品
        public const string Luxury = "Luxury"; // 奢侈品
        public const string Material = "Material"; // 材料
        public const string Misc = "Misc"; // 杂物
        public const string Collection = "Collection"; // 收藏品
        public const string ColorCard = "ColorCard"; // 颜色卡
        public const string Gem = "Gem"; // 宝石
        public const string Gem_Armor_Fleeze = "Gem_Armor_Fleeze"; // 蓝色圆球
        public const string Gem_Armor_Igny = "Gem_Armor_Igny"; // 红色圆球
        public const string SoulCube = "SoulCube"; // 蓝色方块
        public const string Special = "Special"; // 特殊
        public const string Shit = "Shit"; // 粑粑
        public const string SnowBall = "SnowBall"; // 雪球
        public const string Pelt_Pelt = "Pelt"; // 皮毛
        public const string ShowCase = "ShowCase"; // 展示品
        #endregion

        #region 11. 基地与装修 (Base & Deco)
        public const string Base_Deco = "Base_Deco"; // 基地装饰
        public const string Base_WallPaper = "Base_WallPaper"; // 基地墙纸
        #endregion
    }
}
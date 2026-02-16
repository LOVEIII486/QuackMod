using System;
using System.Reflection;
using UnityEngine;

namespace QuackItem.Utils
{
    public interface IModelReplacer
    {
        void ApplyModel(CharacterMainControl target, CharacterMainControl source);
    }

    public class ModelReplacer : IModelReplacer
    {
        private static bool _inited = false;
        private static bool _hasDcm = false;
        private static Type _modelHandlerType, _bundleType, _infoType;
        private static FieldInfo _bundleInfoField;
        private static PropertyInfo _modelInfoProperty;
        private static MethodInfo _initMethod, _loadMethod;

        private static void InitReflection()
        {
            if (_inited) return;
            _inited = true;
            try
            {
                _modelHandlerType = Type.GetType("DuckovCustomModel.MonoBehaviours.ModelHandler, DuckovCustomModel.GameModules");
                _bundleType = Type.GetType("DuckovCustomModel.Core.Data.ModelBundleInfo, DuckovCustomModel.Core");
                _infoType = Type.GetType("DuckovCustomModel.Core.Data.ModelInfo, DuckovCustomModel.Core");
                if (_modelHandlerType == null)
                {
                    _hasDcm = false;
                    return;
                }

                _bundleInfoField = _modelHandlerType.GetField("_currentModelBundleInfo", BindingFlags.NonPublic | BindingFlags.Instance);
                _modelInfoProperty = _modelHandlerType.GetProperty("CurrentModelInfo", BindingFlags.Public | BindingFlags.Instance);
                _initMethod = _modelHandlerType.GetMethod("Initialize", new Type[] { typeof(CharacterMainControl), typeof(string) });
                _loadMethod = _modelHandlerType.GetMethod("InitializeCustomModel", new Type[] { _bundleType, _infoType });
                _hasDcm = _loadMethod != null;
            }
            catch
            {
                _hasDcm = false;
            }
        }

        public void ApplyModel(CharacterMainControl target, CharacterMainControl source)
        {
            if (target == null || source == null) return;

            InitReflection();

            if (_hasDcm)
            {
                try
                {
                    var srcHandler = source.GetComponent(_modelHandlerType);
                    if (srcHandler != null)
                    {
                        object bundleInfo = _bundleInfoField.GetValue(srcHandler);
                        object modelInfo = _modelInfoProperty.GetValue(srcHandler);

                        Component targetHandler = target.GetComponent(_modelHandlerType) ?? (Component)target.gameObject.AddComponent(_modelHandlerType);
                        _initMethod.Invoke(targetHandler, new object[] { target, "AllAICharacters" });
                        _loadMethod.Invoke(targetHandler, new object[] { bundleInfo, modelInfo });

                        //HideEquipmentVisuals(target);
                        return;
                    }
                }
                catch
                {
                    // 忽略
                }
            }

            CopyVanillaFace(target, source);
            //HideEquipmentVisuals(target);
        }

        private void CopyVanillaFace(CharacterMainControl enemy, CharacterMainControl player)
        {
            if (enemy?.characterModel?.CustomFace == null || player?.characterModel?.CustomFace == null) return;
            enemy.characterModel.SetFaceFromData(player.characterModel.CustomFace.ConvertToSaveData());
        }

        private static void HideEquipmentVisuals(CharacterMainControl character)
        {
            if (character?.characterModel == null) return;
            Transform[] sockets = { character.characterModel.HelmatSocket, character.characterModel.ArmorSocket };
            foreach (var s in sockets)
            {
                if (s == null) continue;
                foreach (var r in s.GetComponentsInChildren<Renderer>(true)) r.enabled = false;
            }
        }
    }
}


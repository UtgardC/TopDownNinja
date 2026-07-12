#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace TopDownNinja.Editor
{
    public static class TopDownNinjaContentBuilder
    {
        private const string TutorialScenePath = "Assets/Scenes/Tutorial.unity";
        private const string LevelScenePath = "Assets/Scenes/Level1.unity";
        private const string PlayerPrefabPath = "Assets/Prefabs/Player.prefab";
        private const string AutoRootName = "__AUTO_TUTORIAL_CONTENT__";

        private const string FireProjectilePath = "Assets/Prefabs/Proyectiles/FireAbilty.prefab";
        private const string EnemyProjectilePath = "Assets/Prefabs/Proyectiles/Projectile.prefab";
        private const string BossProjectilePath = "Assets/Prefabs/Proyectiles/ProyectilBoss.prefab";
        private const string RockEffectPath = "Assets/Prefabs/FX/RockEffect.prefab";
        private const string FireScrollPath = "Assets/Prefabs/Coleccionables/FireScroll.prefab";
        private const string RockScrollPath = "Assets/Prefabs/Coleccionables/RockScroll.prefab";
        private const string DummyPrefabPath = "Assets/Prefabs/Tutorial/TrainingDummy.prefab";

        private const string MeleePrefabPath = "Assets/Prefabs/Enemigos/MeleeEnemy.prefab";
        private const string RangedPrefabPath = "Assets/Prefabs/Enemigos/RangedEnemy.prefab";
        private const string BossPrefabPath = "Assets/Prefabs/Enemigos/Boss.prefab";

        private static int PlayerLayer => RequireLayer("Player");
        private static int EnemyLayer => RequireLayer("Enemy");
        private static int PlayerProjectileLayer => RequireLayer("PlayerProjectile");
        private static int EnemyProjectileLayer => RequireLayer("EnemyProjectile");
        private static int CollectibleLayer => RequireLayer("Collectible");

        [MenuItem("Tools/TopDown Ninja/Build Gameplay Prefabs and Tutorial")]
        public static void BuildAll()
        {
            try
            {
                if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

                ValidateRequiredLayers();
                BackupImportantAssets();
                EnsureFolders();
                NormalizeUsedSpriteImports();

                RuntimeAnimatorController playerController = BuildPlayerAnimations();
                RuntimeAnimatorController slimeController = BuildDirectionalEnemyAnimations(
                    "Slime", "Assets/NinjaAssetPack/Actor/Monster/Slime/Slime.png");
                RuntimeAnimatorController skullController = BuildDirectionalEnemyAnimations(
                    "Skull", "Assets/NinjaAssetPack/Actor/Monster/Skull/SpriteSheet.png");
                RuntimeAnimatorController bossController = BuildBossAnimations();

                BuildProjectilePrefabs();
                BuildRockEffect();
                BuildScrollPrefabs();
                RepairStandardCollectibles();
                BuildTrainingDummy();
                RepairEnemyPrefabs(slimeController, skullController, bossController);
                RepairPlayerPrefab(playerController);
                BuildTutorialScene();
                EnsureBuildScenes();

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("[TopDown Ninja] Prefabs y Tutorial construidos correctamente.");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                if (Application.isBatchMode) EditorApplication.Exit(1);
                throw;
            }
        }

        private static void ValidateRequiredLayers()
        {
            _ = PlayerLayer;
            _ = EnemyLayer;
            _ = PlayerProjectileLayer;
            _ = EnemyProjectileLayer;
            _ = CollectibleLayer;
        }

        private static int RequireLayer(string layerName)
        {
            int layer = LayerMask.NameToLayer(layerName);
            if (layer < 0) throw new InvalidOperationException("Falta la Layer requerida: " + layerName);
            return layer;
        }

        private static void BackupImportantAssets()
        {
            string backupDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Library", "TopDownNinjaSetupBackups");
            Directory.CreateDirectory(backupDirectory);
            string stamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            File.Copy(TutorialScenePath, Path.Combine(backupDirectory, "Tutorial.before-builder-" + stamp + ".unity"), true);
            File.Copy(PlayerPrefabPath, Path.Combine(backupDirectory, "Player.before-builder-" + stamp + ".prefab"), true);
        }

        private static void EnsureFolders()
        {
            EnsureFolder("Assets/Prefabs/FX");
            EnsureFolder("Assets/Prefabs/Tutorial");
            EnsureFolder("Assets/Animations/Generated");
            EnsureFolder("Assets/Animations/Generated/Player");
            EnsureFolder("Assets/Animations/Generated/Enemies");
            EnsureFolder("Assets/Animations/Generated/FX");
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            string parent = Path.GetDirectoryName(path)?.Replace('\\', '/');
            string name = Path.GetFileName(path);
            if (!string.IsNullOrEmpty(parent)) EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, name);
        }

        private static void NormalizeUsedSpriteImports()
        {
            string[] paths =
            {
                "Assets/NinjaAssetPack/Items/Scroll/ScrollFire.png",
                "Assets/NinjaAssetPack/Items/Scroll/ScrollRock.png",
                "Assets/NinjaAssetPack/FX/Projectile/Fireball.png",
                "Assets/NinjaAssetPack/FX/Projectile/CanonBall.png",
                "Assets/NinjaAssetPack/FX/Projectile/EnergyBall.png",
                "Assets/NinjaAssetPack/FX/Projectile/SpriteSheetRock.png",
                "Assets/NinjaAssetPack/Actor/Character/GoldStatue/SeparateAnim/Idle.png"
            };

            foreach (string path in paths) ConfigurePixelArtImporter(path);
        }

        private static void ConfigurePixelArtImporter(string path)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null) return;

            bool changed = importer.spritePixelsPerUnit != 16f ||
                           importer.filterMode != FilterMode.Point ||
                           importer.textureCompression != TextureImporterCompression.Uncompressed;
            if (!changed) return;

            importer.spritePixelsPerUnit = 16f;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }

        private static RuntimeAnimatorController BuildPlayerAnimations()
        {
            const string baseFolder = "Assets/Animations/Generated/Player";
            Sprite[] idle = LoadSprites("Assets/NinjaAssetPack/Actor/CharacterAnimated/NinjaGreen/Separate/Idle.png");
            Sprite[] walk = LoadSprites("Assets/NinjaAssetPack/Actor/CharacterAnimated/NinjaGreen/Separate/Walk.png");
            Sprite[] attack = LoadSprites("Assets/NinjaAssetPack/Actor/CharacterAnimated/NinjaGreen/Separate/Attack.png");
            Sprite[] hit = LoadSprites("Assets/NinjaAssetPack/Actor/CharacterAnimated/NinjaGreen/Separate/Hit.png");
            Sprite[] dead = LoadSprites("Assets/NinjaAssetPack/Actor/CharacterAnimated/NinjaGreen/Separate/Dead.png");

            DirectionalClips idleClips = CreateDirectionalClips(baseFolder, "Player_Idle", idle, true, 8f);
            DirectionalClips walkClips = CreateDirectionalClips(baseFolder, "Player_Walk", walk, true, 10f);
            DirectionalClips attackClips = CreateDirectionalClips(baseFolder, "Player_Attack", attack, false, 12f);
            DirectionalClips hitClips = CreateDirectionalClips(baseFolder, "Player_Hit", hit, false, 10f);
            AnimationClip deadClip = CreateClip(baseFolder + "/Player_Dead.anim", dead, false, 8f);

            return CreateDirectionalController(baseFolder + "/Player.controller", idleClips, walkClips, attackClips, hitClips, deadClip);
        }

        private static RuntimeAnimatorController BuildDirectionalEnemyAnimations(string name, string spritePath)
        {
            string baseFolder = "Assets/Animations/Generated/Enemies/" + name;
            EnsureFolder(baseFolder);
            Sprite[] sprites = LoadSprites(spritePath);

            DirectionalClips idle = CreateDirectionalClips(baseFolder, name + "_Idle", sprites, true, 6f);
            DirectionalClips walk = CreateDirectionalClips(baseFolder, name + "_Walk", sprites, true, 9f);
            DirectionalClips attack = CreateDirectionalClips(baseFolder, name + "_Attack", sprites, false, 10f);
            DirectionalClips hit = CreateDirectionalClips(baseFolder, name + "_Hit", sprites, false, 10f);
            AnimationClip dead = CreateClip(baseFolder + "/" + name + "_Dead.anim", new[] { sprites[0] }, false, 8f);
            return CreateDirectionalController(baseFolder + "/" + name + ".controller", idle, walk, attack, hit, dead);
        }

        private static RuntimeAnimatorController BuildBossAnimations()
        {
            const string folder = "Assets/Animations/Generated/Enemies/Boss";
            EnsureFolder(folder);
            AnimationClip idle = CreateClip(folder + "/Boss_Idle.anim", LoadSprites("Assets/NinjaAssetPack/Actor/Boss/TenguRed/Idle.png"), true, 7f);
            AnimationClip walk = CreateClip(folder + "/Boss_Walk.anim", LoadSprites("Assets/NinjaAssetPack/Actor/Boss/TenguRed/Walk.png"), true, 9f);
            AnimationClip attack = CreateClip(folder + "/Boss_Attack.anim", LoadSprites("Assets/NinjaAssetPack/Actor/Boss/TenguRed/Attack.png"), false, 12f);
            AnimationClip hit = CreateClip(folder + "/Boss_Hit.anim", LoadSprites("Assets/NinjaAssetPack/Actor/Boss/TenguRed/Hit.png"), false, 10f);
            AnimationClip transition = CreateClip(folder + "/Boss_Special.anim", LoadSprites("Assets/NinjaAssetPack/Actor/Boss/TenguRed/Trans.png"), false, 10f);

            string controllerPath = folder + "/Boss.controller";
            AssetDatabase.DeleteAsset(controllerPath);
            AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            AddEnemyParameters(controller, true);
            AnimatorStateMachine sm = controller.layers[0].stateMachine;
            AnimatorState idleState = sm.AddState("Idle"); idleState.motion = idle;
            AnimatorState walkState = sm.AddState("Walk"); walkState.motion = walk;
            AnimatorState attackState = sm.AddState("Attack"); attackState.motion = attack;
            AnimatorState hitState = sm.AddState("Hit"); hitState.motion = hit;
            AnimatorState specialState = sm.AddState("Special"); specialState.motion = transition;
            AnimatorState deadState = sm.AddState("Dead"); deadState.motion = transition;
            sm.defaultState = idleState;

            AddTransition(idleState, walkState, AnimatorConditionMode.Greater, 0.01f, "Speed", false);
            AddTransition(walkState, idleState, AnimatorConditionMode.Less, 0.01f, "Speed", false);
            AddAnyTrigger(sm, attackState, "Attack");
            AddAnyTrigger(sm, hitState, "Hit");
            AddAnyTrigger(sm, specialState, "Special");
            AddAnyBool(sm, deadState, "Dead");
            AddExitTransition(attackState, idleState);
            AddExitTransition(hitState, idleState);
            AddExitTransition(specialState, idleState);
            EditorUtility.SetDirty(controller);
            return controller;
        }

        private static DirectionalClips CreateDirectionalClips(string folder, string prefix, Sprite[] sprites, bool loop, float frameRate)
        {
            if (sprites.Length < 4) throw new InvalidOperationException(prefix + " necesita al menos cuatro sprites.");
            List<Sprite>[] directions = { new List<Sprite>(), new List<Sprite>(), new List<Sprite>(), new List<Sprite>() };
            for (int i = 0; i < sprites.Length; i++) directions[i % 4].Add(sprites[i]);

            return new DirectionalClips
            {
                Down = CreateClip(folder + "/" + prefix + "_Down.anim", directions[0].ToArray(), loop, frameRate),
                Right = CreateClip(folder + "/" + prefix + "_Right.anim", directions[1].ToArray(), loop, frameRate),
                Up = CreateClip(folder + "/" + prefix + "_Up.anim", directions[2].ToArray(), loop, frameRate),
                Left = CreateClip(folder + "/" + prefix + "_Left.anim", directions[3].ToArray(), loop, frameRate)
            };
        }

        private static AnimationClip CreateClip(string path, Sprite[] sprites, bool loop, float frameRate)
        {
            if (sprites == null || sprites.Length == 0) throw new InvalidOperationException("No hay sprites para " + path);
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            if (clip == null)
            {
                clip = new AnimationClip();
                AssetDatabase.CreateAsset(clip, path);
            }

            clip.frameRate = frameRate;
            EditorCurveBinding binding = new EditorCurveBinding { type = typeof(SpriteRenderer), path = string.Empty, propertyName = "m_Sprite" };
            ObjectReferenceKeyframe[] keys = new ObjectReferenceKeyframe[sprites.Length];
            for (int i = 0; i < sprites.Length; i++) keys[i] = new ObjectReferenceKeyframe { time = i / frameRate, value = sprites[i] };
            AnimationUtility.SetObjectReferenceCurve(clip, binding, keys);
            AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = loop;
            AnimationUtility.SetAnimationClipSettings(clip, settings);
            EditorUtility.SetDirty(clip);
            return clip;
        }

        private static RuntimeAnimatorController CreateDirectionalController(
            string path, DirectionalClips idle, DirectionalClips walk, DirectionalClips attack, DirectionalClips hit, AnimationClip dead)
        {
            AssetDatabase.DeleteAsset(path);
            AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(path);
            AddEnemyParameters(controller, false);
            AnimatorStateMachine sm = controller.layers[0].stateMachine;

            AnimatorState idleState = AddDirectionalState(controller, sm, "Idle", idle);
            AnimatorState walkState = AddDirectionalState(controller, sm, "Walk", walk);
            AnimatorState attackState = AddDirectionalState(controller, sm, "Attack", attack);
            AnimatorState hitState = AddDirectionalState(controller, sm, "Hit", hit);
            AnimatorState deadState = sm.AddState("Dead"); deadState.motion = dead;
            sm.defaultState = idleState;

            AddTransition(idleState, walkState, AnimatorConditionMode.Greater, 0.01f, "Speed", false);
            AddTransition(walkState, idleState, AnimatorConditionMode.Less, 0.01f, "Speed", false);
            AddAnyTrigger(sm, attackState, "Attack");
            AddAnyTrigger(sm, hitState, "Hit");
            AddAnyBool(sm, deadState, "Dead");
            AddExitTransition(attackState, idleState);
            AddExitTransition(hitState, idleState);
            EditorUtility.SetDirty(controller);
            return controller;
        }

        private static void AddEnemyParameters(AnimatorController controller, bool includeSpecial)
        {
            controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
            controller.AddParameter("MoveX", AnimatorControllerParameterType.Float);
            controller.AddParameter("MoveY", AnimatorControllerParameterType.Float);
            controller.AddParameter("Attack", AnimatorControllerParameterType.Trigger);
            controller.AddParameter("Hit", AnimatorControllerParameterType.Trigger);
            controller.AddParameter("Dead", AnimatorControllerParameterType.Bool);
            if (includeSpecial) controller.AddParameter("Special", AnimatorControllerParameterType.Trigger);
        }

        private static AnimatorState AddDirectionalState(AnimatorController controller, AnimatorStateMachine sm, string name, DirectionalClips clips)
        {
            AnimatorState state = sm.AddState(name);
            BlendTree tree = new BlendTree
            {
                name = name + " Blend Tree",
                blendType = BlendTreeType.SimpleDirectional2D,
                blendParameter = "MoveX",
                blendParameterY = "MoveY",
                useAutomaticThresholds = false
            };
            AssetDatabase.AddObjectToAsset(tree, controller);
            tree.AddChild(clips.Down, new Vector2(0f, -1f));
            tree.AddChild(clips.Right, new Vector2(1f, 0f));
            tree.AddChild(clips.Up, new Vector2(0f, 1f));
            tree.AddChild(clips.Left, new Vector2(-1f, 0f));
            state.motion = tree;
            return state;
        }

        private static void AddTransition(AnimatorState from, AnimatorState to, AnimatorConditionMode mode, float threshold, string parameter, bool exitTime)
        {
            AnimatorStateTransition transition = from.AddTransition(to);
            transition.hasExitTime = exitTime;
            transition.duration = 0f;
            transition.AddCondition(mode, threshold, parameter);
        }

        private static void AddAnyTrigger(AnimatorStateMachine sm, AnimatorState state, string parameter)
        {
            AnimatorStateTransition transition = sm.AddAnyStateTransition(state);
            transition.hasExitTime = false;
            transition.duration = 0f;
            transition.canTransitionToSelf = false;
            transition.AddCondition(AnimatorConditionMode.If, 0f, parameter);
        }

        private static void AddAnyBool(AnimatorStateMachine sm, AnimatorState state, string parameter)
        {
            AnimatorStateTransition transition = sm.AddAnyStateTransition(state);
            transition.hasExitTime = false;
            transition.duration = 0f;
            transition.canTransitionToSelf = false;
            transition.AddCondition(AnimatorConditionMode.If, 0f, parameter);
        }

        private static void AddExitTransition(AnimatorState from, AnimatorState to)
        {
            AnimatorStateTransition transition = from.AddTransition(to);
            transition.hasExitTime = true;
            transition.exitTime = 0.95f;
            transition.duration = 0f;
        }

        private static Sprite[] LoadSprites(string path)
        {
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().OrderBy(sprite => NumericSuffix(sprite.name)).ToArray();
            if (sprites.Length == 0) throw new InvalidOperationException("No se encontraron sprites en " + path);
            return sprites;
        }

        private static int NumericSuffix(string name)
        {
            int separator = name.LastIndexOf('_');
            return separator >= 0 && int.TryParse(name.Substring(separator + 1), out int result) ? result : 0;
        }

        private static Sprite FirstSprite(string path) => LoadSprites(path)[0];

        private static void BuildProjectilePrefabs()
        {
            CreateProjectilePrefab(FireProjectilePath, "FireProjectile",
                FirstSprite("Assets/NinjaAssetPack/FX/Projectile/Fireball.png"), PlayerProjectileLayer,
                LayerMask.GetMask("Enemy"), 7f, 0.22f);
            CreateProjectilePrefab(EnemyProjectilePath, "EnemyProjectile",
                FirstSprite("Assets/NinjaAssetPack/FX/Projectile/CanonBall.png"), EnemyProjectileLayer,
                LayerMask.GetMask("Player"), 6f, 0.2f);
            CreateProjectilePrefab(BossProjectilePath, "BossProjectile",
                FirstSprite("Assets/NinjaAssetPack/FX/Projectile/EnergyBall.png"), EnemyProjectileLayer,
                LayerMask.GetMask("Player"), 7f, 0.3f);
        }

        private static void CreateProjectilePrefab(string path, string name, Sprite sprite, int layer, int targets, float speed, float radius)
        {
            GameObject root = new GameObject(name) { layer = layer };
            try
            {
                SpriteRenderer renderer = root.AddComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                renderer.sortingLayerName = "FX";

                Rigidbody2D body = root.AddComponent<Rigidbody2D>();
                ConfigureTopDownBody(body, true);
                CircleCollider2D collider = root.AddComponent<CircleCollider2D>();
                collider.isTrigger = true;
                collider.radius = radius;
                Projectile projectile = root.AddComponent<Projectile>();
                SetSerialized(projectile, "speed", speed);
                SetSerialized(projectile, "targetLayers", targets);
                SetSerialized(projectile, "blockingLayers", LayerMask.GetMask("Obstacle"));
                SetSerialized(projectile, "lifetime", 4f);
                SavePrefab(root, path);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        private static void BuildRockEffect()
        {
            Sprite[] sprites = LoadSprites("Assets/NinjaAssetPack/FX/Projectile/SpriteSheetRock.png");
            AnimationClip clip = CreateClip("Assets/Animations/Generated/FX/RockEffect.anim", sprites, false, 12f);
            string controllerPath = "Assets/Animations/Generated/FX/RockEffect.controller";
            AssetDatabase.DeleteAsset(controllerPath);
            AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            controller.layers[0].stateMachine.AddState("Rock").motion = clip;

            GameObject root = new GameObject("RockEffect");
            try
            {
                SpriteRenderer renderer = root.AddComponent<SpriteRenderer>();
                renderer.sprite = sprites[0];
                renderer.sortingLayerName = "FX";
                Animator animator = root.AddComponent<Animator>();
                animator.runtimeAnimatorController = controller;
                SavePrefab(root, RockEffectPath);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        private static void BuildScrollPrefabs()
        {
            Sprite fire = FirstSprite("Assets/NinjaAssetPack/Items/Scroll/ScrollFire.png");
            Sprite rock = FirstSprite("Assets/NinjaAssetPack/Items/Scroll/ScrollRock.png");
            CreateScrollPrefab(FireScrollPath, "FireScroll", ScrollType.Fire, fire, fire, rock);
            CreateScrollPrefab(RockScrollPath, "RockScroll", ScrollType.Rock, rock, fire, rock);
        }

        private static void CreateScrollPrefab(string path, string name, ScrollType type, Sprite displayed, Sprite fire, Sprite rock)
        {
            GameObject root = new GameObject(name) { layer = CollectibleLayer };
            try
            {
                SpriteRenderer renderer = root.AddComponent<SpriteRenderer>();
                renderer.sprite = displayed;
                renderer.sortingLayerName = "World";
                CircleCollider2D collider = root.AddComponent<CircleCollider2D>();
                collider.isTrigger = true;
                collider.radius = 0.42f;
                ScrollCollectible collectible = root.AddComponent<ScrollCollectible>();
                SetSerialized(collectible, "scrollType", (int)type);
                SetSerialized(collectible, "fireSprite", fire);
                SetSerialized(collectible, "rockSprite", rock);
                SetSerialized(collectible, "spriteRenderer", renderer);
                SavePrefab(root, path);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        private static void RepairStandardCollectibles()
        {
            string[] paths =
            {
                "Assets/Prefabs/Coleccionables/Coin.prefab",
                "Assets/Prefabs/Coleccionables/Food.prefab",
                "Assets/Prefabs/Coleccionables/SpeedBuff.prefab",
                "Assets/Prefabs/Coleccionables/DamageBuff.prefab",
                "Assets/Prefabs/Coleccionables/AttackSpeedBuff.prefab"
            };

            foreach (string path in paths)
            {
                EditPrefab(path, root =>
                {
                    root.layer = CollectibleLayer;
                    root.transform.localPosition = Vector3.zero;
                    root.transform.localScale = Vector3.one;
                    SpriteRenderer renderer = root.GetComponent<SpriteRenderer>();
                    if (renderer != null) renderer.sortingLayerName = "World";
                    BoxCollider2D collider = GetOrAdd<BoxCollider2D>(root);
                    collider.isTrigger = true;
                    collider.size = new Vector2(0.7f, 0.7f);
                    collider.offset = Vector2.zero;
                });
            }
        }

        private static void BuildTrainingDummy()
        {
            GameObject root = new GameObject("TrainingDummy") { layer = EnemyLayer };
            try
            {
                SpriteRenderer renderer = root.AddComponent<SpriteRenderer>();
                renderer.sprite = FirstSprite("Assets/NinjaAssetPack/Actor/Character/GoldStatue/SeparateAnim/Idle.png");
                renderer.sortingLayerName = "Characters";
                BoxCollider2D collider = root.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(0.75f, 0.85f);
                Health health = root.AddComponent<Health>();
                SetSerialized(health, "maxHealth", 40);
                TrainingDummy dummy = root.AddComponent<TrainingDummy>();
                SetSerialized(dummy, "health", health);
                SetSerialized(dummy, "spriteRenderer", renderer);
                SetSerialized(dummy, "resetDelay", 1f);
                SavePrefab(root, DummyPrefabPath);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        private static void RepairEnemyPrefabs(RuntimeAnimatorController meleeController, RuntimeAnimatorController rangedController, RuntimeAnimatorController bossController)
        {
            RepairEnemyPrefab<MeleeEnemy>(MeleePrefabPath, 35, 8f, meleeController, false);
            RepairEnemyPrefab<RangedEnemy>(RangedPrefabPath, 30, 9f, rangedController, false);
            RepairEnemyPrefab<BossEnemy>(BossPrefabPath, 300, 20f, bossController, true);

            GameObject projectile = AssetDatabase.LoadAssetAtPath<GameObject>(EnemyProjectilePath);
            EditPrefab(RangedPrefabPath, root => SetSerialized(root.GetComponent<RangedEnemy>(), "projectilePrefab", projectile));
        }

        private static void RepairEnemyPrefab<T>(string path, int healthValue, float detectionRange, RuntimeAnimatorController controller, bool isBoss)
            where T : EnemyBase
        {
            EditPrefab(path, root =>
            {
                root.layer = EnemyLayer;
                root.transform.localPosition = Vector3.zero;
                root.transform.localRotation = Quaternion.identity;
                root.transform.localScale = Vector3.one;

                Rigidbody2D body = GetOrAdd<Rigidbody2D>(root);
                ConfigureTopDownBody(body, isBoss);
                Collider2D bodyCollider = root.GetComponent<Collider2D>();
                if (bodyCollider == null) bodyCollider = root.AddComponent<BoxCollider2D>();
                bodyCollider.isTrigger = false;
                if (bodyCollider is BoxCollider2D box) box.size = isBoss ? new Vector2(1.4f, 1.4f) : new Vector2(0.8f, 0.8f);
                if (bodyCollider is CircleCollider2D circle) circle.radius = isBoss ? 0.7f : 0.4f;

                Health health = GetOrAdd<Health>(root);
                SetSerialized(health, "maxHealth", healthValue);
                T enemy = GetOrAdd<T>(root);
                SetSerialized(enemy, "health", health);
                SetSerialized(enemy, "target", (UnityEngine.Object)null);
                SetSerialized(enemy, "detectionRange", detectionRange);
                SetSerialized(enemy, "deathDisableDelay", 0.75f);

                if (enemy is MeleeEnemy melee) SetSerialized(melee, "playerLayer", LayerMask.GetMask("Player"));
                if (enemy is BossEnemy boss)
                {
                    SetSerialized(boss, "playerLayer", LayerMask.GetMask("Player"));
                    SetSerialized(boss, "chargeHitRadius", 0.9f);
                }

                SpriteRenderer renderer = EnsureVisualRenderer(root);
                renderer.sortingLayerName = "Characters";
                if (isBoss) renderer.transform.localScale = Vector3.one * 0.5f;

                Animator animator = GetOrAdd<Animator>(renderer.gameObject);
                animator.runtimeAnimatorController = controller;
                EnemyAnimator bridge = GetOrAdd<EnemyAnimator>(renderer.gameObject);
                SetSerialized(bridge, "enemy", enemy);
                SetSerialized(bridge, "health", health);
                SetSerialized(bridge, "animator", animator);
            });
        }

        private static void RepairPlayerPrefab(RuntimeAnimatorController playerController)
        {
            GameObject fireProjectile = AssetDatabase.LoadAssetAtPath<GameObject>(FireProjectilePath);
            GameObject rockEffect = AssetDatabase.LoadAssetAtPath<GameObject>(RockEffectPath);
            InputActionAsset actions = AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/InputSystem_Actions.inputactions");

            EditPrefab(PlayerPrefabPath, root =>
            {
                root.name = "Player";
                root.layer = PlayerLayer;
                root.tag = "Player";

                Rigidbody2D body = GetOrAdd<Rigidbody2D>(root);
                ConfigureTopDownBody(body, true);
                BoxCollider2D bodyCollider = GetOrAdd<BoxCollider2D>(root);
                bodyCollider.isTrigger = false;
                bodyCollider.size = new Vector2(0.81f, 0.8f);

                Health health = GetOrAdd<Health>(root);
                PlayerStats stats = GetOrAdd<PlayerStats>(root);
                PlayerMovement movement = GetOrAdd<PlayerMovement>(root);
                PlayerAttack attack = GetOrAdd<PlayerAttack>(root);
                PlayerCollector collector = GetOrAdd<PlayerCollector>(root);
                TemporaryPowerUpController powerUps = GetOrAdd<TemporaryPowerUpController>(root);
                ScoreTracker score = GetOrAdd<ScoreTracker>(root);
                ScrollLoadout loadout = GetOrAdd<ScrollLoadout>(root);
                FireAbility fire = GetOrAdd<FireAbility>(root);
                RockAbility rock = GetOrAdd<RockAbility>(root);

                Transform attackOrigin = FindOrCreateChild(root.transform, "AttackOrigin");
                SpriteRenderer renderer = root.GetComponentInChildren<SpriteRenderer>(true);
                if (renderer == null)
                {
                    Transform visual = FindOrCreateChild(root.transform, "Visual");
                    renderer = visual.gameObject.AddComponent<SpriteRenderer>();
                    renderer.sprite = FirstSprite("Assets/NinjaAssetPack/Actor/CharacterAnimated/NinjaGreen/Separate/Idle.png");
                }
                renderer.sortingLayerName = "Characters";
                Animator animator = GetOrAdd<Animator>(renderer.gameObject);
                animator.runtimeAnimatorController = playerController;

                foreach (Transform child in root.GetComponentsInChildren<Transform>(true).ToArray())
                {
                    if (child == root.transform || child == renderer.transform) continue;
                    if (child.name == "Animator" && child.GetComponents<Component>().Length == 1)
                        UnityEngine.Object.DestroyImmediate(child.gameObject);
                }

                PlayerAnimator playerAnimator = GetOrAdd<PlayerAnimator>(root);
                PlayerInput playerInput = GetOrAdd<PlayerInput>(root);
                playerInput.actions = actions;
                playerInput.defaultActionMap = "Player";
                playerInput.notificationBehavior = PlayerNotifications.SendMessages;

                SetSerialized(movement, "stats", stats);
                SetSerialized(attack, "stats", stats);
                SetSerialized(attack, "movement", movement);
                SetSerialized(attack, "attackOrigin", attackOrigin);
                SetSerialized(attack, "enemyLayer", LayerMask.GetMask("Enemy"));
                SetSerialized(collector, "health", health);
                SetSerialized(collector, "scoreTracker", score);
                SetSerialized(collector, "powerUpController", powerUps);
                SetSerialized(collector, "scrollLoadout", loadout);
                SetSerialized(loadout, "movement", movement);
                SetSerialized(loadout, "equippedAbility", fire);
                SetSerializedArray(loadout, "availableAbilities", new UnityEngine.Object[] { fire, rock });
                SetSerialized(fire, "fireProjectilePrefab", fireProjectile);
                SetSerialized(fire, "targetLayers", LayerMask.GetMask("Enemy"));
                SetSerialized(rock, "rockEffectPrefab", rockEffect);
                SetSerialized(rock, "targetLayers", LayerMask.GetMask("Enemy"));
                SetSerialized(rock, "damage", 30);
                SetSerialized(rock, "cooldown", 2.5f);
                SetSerialized(playerAnimator, "movement", movement);
                SetSerialized(playerAnimator, "playerAttack", attack);
                SetSerialized(playerAnimator, "health", health);
                SetSerialized(playerAnimator, "animator", animator);
            });
        }

        private static void BuildTutorialScene()
        {
            Scene scene = EditorSceneManager.OpenScene(TutorialScenePath, OpenSceneMode.Single);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) throw new InvalidOperationException("Tutorial no contiene una instancia con Tag Player.");

            RemoveDuplicateAddedOverrides<PlayerAnimator>(player);
            RemoveDuplicateAddedOverrides<RockAbility>(player);

            Health health = player.GetComponent<Health>();
            ScoreTracker score = player.GetComponent<ScoreTracker>();
            TemporaryPowerUpController powerUps = player.GetComponent<TemporaryPowerUpController>();
            ScrollLoadout loadout = player.GetComponent<ScrollLoadout>();
            PlayerCollector collector = player.GetComponent<PlayerCollector>();
            FireAbility fire = player.GetComponent<FireAbility>();
            RockAbility rock = player.GetComponent<RockAbility>();
            SetSerialized(loadout, "equippedAbility", (UnityEngine.Object)null);
            SetSerializedArray(loadout, "availableAbilities", new UnityEngine.Object[] { fire, rock });
            SetSerialized(collector, "scrollLoadout", loadout);

            HUDController hud = UnityEngine.Object.FindAnyObjectByType<HUDController>();
            if (hud != null)
            {
                SetSerialized(hud, "playerHealth", health);
                SetSerialized(hud, "scoreTracker", score);
                SetSerialized(hud, "powerUpController", powerUps);
            }

            LevelFlowController levelFlow = UnityEngine.Object.FindAnyObjectByType<LevelFlowController>();
            GameResultController result = UnityEngine.Object.FindAnyObjectByType<GameResultController>();
            if (result != null)
            {
                SetSerialized(result, "playerHealth", health);
                SetSerialized(result, "objectiveTracker", (UnityEngine.Object)null);
                SetSerialized(result, "levelFlow", levelFlow);
            }

            ObjectiveTracker objective = UnityEngine.Object.FindAnyObjectByType<ObjectiveTracker>();
            if (objective != null)
            {
                SetSerialized(objective, "boss", (UnityEngine.Object)null);
                objective.enabled = false;
            }

            ScrollIconHUD scrollHud = UnityEngine.Object.FindAnyObjectByType<ScrollIconHUD>();
            if (scrollHud != null)
            {
                SetSerialized(scrollHud, "scrollLoadout", loadout);
                SetSerialized(scrollHud, "iconReady", FirstSprite("Assets/NinjaAssetPack/Ui/Skill Icon/Spell/BookFire.png"));
                SetSerialized(scrollHud, "iconCooldown", FirstSprite("Assets/NinjaAssetPack/Ui/Skill Icon/Spell/BookFireDisabled.png"));
                SetSerialized(scrollHud, "rockIconReady", FirstSprite("Assets/NinjaAssetPack/Ui/Skill Icon/Spell/BookRock.png"));
                SetSerialized(scrollHud, "rockIconCooldown", FirstSprite("Assets/NinjaAssetPack/Ui/Skill Icon/Spell/BookRockDisabled.png"));
            }

            Camera mainCamera = Camera.main != null ? Camera.main : UnityEngine.Object.FindAnyObjectByType<Camera>();
            if (mainCamera != null)
            {
                mainCamera.orthographic = true;
                mainCamera.orthographicSize = 5f;
                CameraFollow2D follow = GetOrAdd<CameraFollow2D>(mainCamera.gameObject);
                follow.SetTarget(player.transform);
                SetSerialized(follow, "target", player.transform);
            }

            GameObject previousRoot = GameObject.Find(AutoRootName);
            if (previousRoot != null) UnityEngine.Object.DestroyImmediate(previousRoot);
            GameObject autoRoot = new GameObject(AutoRootName);
            SceneManager.MoveGameObjectToScene(autoRoot, scene);

            Tilemap ground = FindTilemap("TilemapGround");
            Tilemap obstacles = FindTilemap("TileMap Obstacules");
            int obstacleLayer = LayerMask.NameToLayer("Obstacle");
            if (obstacleLayer >= 0) obstacles.gameObject.layer = obstacleLayer;
            Vector3 start = player.transform.position;

            TutorialPromptController prompts = CreatePromptCanvas(autoRoot.transform);
            CreatePromptTrigger(autoRoot.transform, prompts, FindWalkableNear(ground, obstacles, start + Vector3.right * 7f),
                "Prueba el ataque cuerpo a cuerpo con J o click izquierdo. El muñeco se repara solo.");

            PlacePrefab(DummyPrefabPath, autoRoot.transform, FindWalkableNear(ground, obstacles, start + Vector3.right * 11f), "MeleeTrainingDummy");
            CreateWorldLabel(autoRoot.transform, "ATAQUE MELEE: J / CLICK", FindWalkableNear(ground, obstacles, start + Vector3.right * 11f) + Vector3.up * 1.7f);

            PlacePrefab("Assets/Prefabs/Coleccionables/Coin.prefab", autoRoot.transform, FindWalkableNear(ground, obstacles, start + Vector3.right * 18f), "TutorialCoin");
            PlacePrefab("Assets/Prefabs/Coleccionables/Food.prefab", autoRoot.transform, FindWalkableNear(ground, obstacles, start + Vector3.right * 21f), "TutorialFood");
            PlacePrefab("Assets/Prefabs/Coleccionables/SpeedBuff.prefab", autoRoot.transform, FindWalkableNear(ground, obstacles, start + Vector3.right * 24f), "TutorialSpeedBuff");

            Vector3 scrollPosition = FindWalkableNear(ground, obstacles, start + Vector3.right * 30f);
            CreatePromptTrigger(autoRoot.transform, prompts, scrollPosition + Vector3.left * 2f,
                "Recoge el pergamino de Fuego. El pergamino elige qué habilidad está equipada.");
            PlacePrefab(FireScrollPath, autoRoot.transform, scrollPosition, "TutorialFireScroll");
            CreateWorldLabel(autoRoot.transform, "PERGAMINO DE FUEGO", scrollPosition + Vector3.up * 1.5f);

            Vector3 abilityDummyPosition = FindWalkableNear(ground, obstacles, start + Vector3.right * 39f);
            CreatePromptTrigger(autoRoot.transform, prompts, abilityDummyPosition + Vector3.left * 4f,
                "Con el pergamino equipado, presiona K para lanzar la bola de fuego.");
            PlacePrefab(DummyPrefabPath, autoRoot.transform, abilityDummyPosition, "ScrollTrainingDummy");
            CreateWorldLabel(autoRoot.transform, "HABILIDAD: K", abilityDummyPosition + Vector3.up * 1.7f);

            Vector3 exitPosition = FindWalkableNear(ground, obstacles, start + Vector3.right * 57f);
            CreateLevelExit(autoRoot.transform, levelFlow, exitPosition);
            CreateWorldLabel(autoRoot.transform, "SALIDA AL NIVEL PRINCIPAL", exitPosition + Vector3.up * 1.8f);

            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene, TutorialScenePath))
                throw new InvalidOperationException("Unity no pudo guardar Tutorial.unity");
        }

        private static void RemoveDuplicateAddedOverrides<T>(GameObject player) where T : Component
        {
            T[] components = player.GetComponents<T>();
            if (components.Length <= 1) return;

            foreach (T component in components)
            {
                if (PrefabUtility.GetCorrespondingObjectFromSource(component) == null)
                    UnityEngine.Object.DestroyImmediate(component);
            }
        }

        private static TutorialPromptController CreatePromptCanvas(Transform parent)
        {
            GameObject canvasObject = new GameObject("TutorialPromptCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(parent, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);

            GameObject panelObject = new GameObject("PromptPanel", typeof(RectTransform), typeof(Image));
            panelObject.transform.SetParent(canvasObject.transform, false);
            RectTransform panelRect = panelObject.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 1f);
            panelRect.anchorMax = new Vector2(0.5f, 1f);
            panelRect.pivot = new Vector2(0.5f, 1f);
            panelRect.anchoredPosition = new Vector2(0f, -30f);
            panelRect.sizeDelta = new Vector2(1100f, 100f);
            panelObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.72f);

            GameObject textObject = new GameObject("PromptText", typeof(RectTransform), typeof(TextMeshProUGUI));
            textObject.transform.SetParent(panelObject.transform, false);
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(25f, 12f);
            textRect.offsetMax = new Vector2(-25f, -12f);
            TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
            text.font = TMP_Settings.defaultFontAsset;
            text.fontSize = 29f;
            text.color = Color.white;
            text.alignment = TextAlignmentOptions.Center;
            text.textWrappingMode = TextWrappingModes.Normal;

            TutorialPromptController controller = canvasObject.AddComponent<TutorialPromptController>();
            SetSerialized(controller, "promptText", text);
            SetSerialized(controller, "initialMessage", "WASD / Flechas: moverse    J / Click: ataque melee");
            return controller;
        }

        private static void CreatePromptTrigger(Transform parent, TutorialPromptController prompts, Vector3 position, string message)
        {
            GameObject triggerObject = new GameObject("PromptTrigger");
            triggerObject.transform.SetParent(parent, false);
            triggerObject.transform.position = position;
            BoxCollider2D trigger = triggerObject.AddComponent<BoxCollider2D>();
            trigger.isTrigger = true;
            trigger.size = new Vector2(3f, 4f);
            TutorialPromptTrigger promptTrigger = triggerObject.AddComponent<TutorialPromptTrigger>();
            SetSerialized(promptTrigger, "promptController", prompts);
            SetSerialized(promptTrigger, "message", message);
            SetSerialized(promptTrigger, "duration", 7f);
            SetSerialized(promptTrigger, "playerLayer", LayerMask.GetMask("Player"));
        }

        private static void CreateWorldLabel(Transform parent, string message, Vector3 position)
        {
            GameObject canvasObject = new GameObject("Sign_" + message, typeof(RectTransform), typeof(Canvas));
            canvasObject.transform.SetParent(parent, false);
            canvasObject.transform.position = position;
            canvasObject.transform.localScale = Vector3.one * 0.01f;
            RectTransform canvasRect = canvasObject.GetComponent<RectTransform>();
            canvasRect.sizeDelta = new Vector2(500f, 90f);
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.overrideSorting = true;
            canvas.sortingLayerName = "UI";
            canvas.sortingOrder = 50;

            GameObject textObject = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            textObject.transform.SetParent(canvasObject.transform, false);
            RectTransform rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
            text.font = TMP_Settings.defaultFontAsset;
            text.fontSize = 40f;
            text.fontStyle = FontStyles.Bold;
            text.text = message;
            text.color = Color.white;
            text.alignment = TextAlignmentOptions.Center;
            text.outlineWidth = 0.25f;
            text.outlineColor = Color.black;
        }

        private static void CreateLevelExit(Transform parent, LevelFlowController levelFlow, Vector3 position)
        {
            GameObject exit = new GameObject("LevelExit");
            exit.transform.SetParent(parent, false);
            exit.transform.position = position;
            BoxCollider2D trigger = exit.AddComponent<BoxCollider2D>();
            trigger.isTrigger = true;
            trigger.size = new Vector2(2f, 4f);
            LevelExitTrigger levelExit = exit.AddComponent<LevelExitTrigger>();
            SetSerialized(levelExit, "levelFlow", levelFlow);
            SetSerialized(levelExit, "playerLayer", LayerMask.GetMask("Player"));
        }

        private static GameObject PlacePrefab(string prefabPath, Transform parent, Vector3 position, string instanceName)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null) throw new InvalidOperationException("No existe el prefab " + prefabPath);
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab, parent.gameObject.scene) as GameObject;
            instance.name = instanceName;
            instance.transform.SetParent(parent, true);
            instance.transform.position = position;
            instance.transform.rotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;
            return instance;
        }

        private static Tilemap FindTilemap(string name)
        {
            Tilemap tilemap = UnityEngine.Object.FindObjectsByType<Tilemap>(FindObjectsInactive.Exclude).FirstOrDefault(item => item.name == name);
            if (tilemap == null) throw new InvalidOperationException("No se encontró el Tilemap " + name);
            return tilemap;
        }

        private static Vector3 FindWalkableNear(Tilemap ground, Tilemap obstacles, Vector3 desired)
        {
            Vector3Int origin = ground.WorldToCell(desired);
            for (int radius = 0; radius <= 7; radius++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    for (int x = -radius; x <= radius; x++)
                    {
                        Vector3Int cell = origin + new Vector3Int(x, y, 0);
                        if (!ground.HasTile(cell)) continue;
                        if (obstacles != null && obstacles.HasTile(cell)) continue;
                        Vector3 point = ground.GetCellCenterWorld(cell);
                        point.z = 0f;
                        return point;
                    }
                }
            }
            return desired;
        }

        private static void EnsureBuildScenes()
        {
            List<EditorBuildSettingsScene> scenes = EditorBuildSettings.scenes.ToList();
            EnsureBuildScene(scenes, TutorialScenePath);
            EnsureBuildScene(scenes, LevelScenePath);
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        private static void EnsureBuildScene(List<EditorBuildSettingsScene> scenes, string path)
        {
            EditorBuildSettingsScene existing = scenes.FirstOrDefault(item => item.path == path);
            if (existing != null) existing.enabled = true;
            else scenes.Add(new EditorBuildSettingsScene(path, true));
        }

        private static void ConfigureTopDownBody(Rigidbody2D body, bool continuous)
        {
            body.bodyType = RigidbodyType2D.Dynamic;
            body.gravityScale = 0f;
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            body.collisionDetectionMode = continuous ? CollisionDetectionMode2D.Continuous : CollisionDetectionMode2D.Discrete;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        private static Transform FindOrCreateChild(Transform parent, string name)
        {
            Transform child = parent.Find(name);
            if (child != null) return child;
            GameObject childObject = new GameObject(name);
            childObject.transform.SetParent(parent, false);
            return childObject.transform;
        }

        private static SpriteRenderer EnsureVisualRenderer(GameObject root)
        {
            SpriteRenderer rootRenderer = root.GetComponent<SpriteRenderer>();
            if (rootRenderer == null)
            {
                SpriteRenderer childRenderer = root.GetComponentInChildren<SpriteRenderer>(true);
                if (childRenderer != null) return childRenderer;

                Transform visual = FindOrCreateChild(root.transform, "Visual");
                return GetOrAdd<SpriteRenderer>(visual.gameObject);
            }

            Sprite sprite = rootRenderer.sprite;
            Color color = rootRenderer.color;
            bool flipX = rootRenderer.flipX;
            bool flipY = rootRenderer.flipY;
            string sortingLayer = rootRenderer.sortingLayerName;
            int sortingOrder = rootRenderer.sortingOrder;
            Material material = rootRenderer.sharedMaterial;

            Transform visualTransform = FindOrCreateChild(root.transform, "Visual");
            SpriteRenderer renderer = GetOrAdd<SpriteRenderer>(visualTransform.gameObject);
            renderer.sprite = sprite;
            renderer.color = color;
            renderer.flipX = flipX;
            renderer.flipY = flipY;
            renderer.sortingLayerName = sortingLayer;
            renderer.sortingOrder = sortingOrder;
            renderer.sharedMaterial = material;
            UnityEngine.Object.DestroyImmediate(rootRenderer);
            return renderer;
        }

        private static T GetOrAdd<T>(GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            return component != null ? component : gameObject.AddComponent<T>();
        }

        private static void EditPrefab(string path, Action<GameObject> edit)
        {
            GameObject root = PrefabUtility.LoadPrefabContents(path);
            try
            {
                edit(root);
                PrefabUtility.SaveAsPrefabAsset(root, path);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(root);
            }
        }

        private static void SavePrefab(GameObject root, string path)
        {
            PrefabUtility.SaveAsPrefabAsset(root, path);
        }

        private static void SetSerialized(UnityEngine.Object target, string propertyName, UnityEngine.Object value)
        {
            SerializedObject serialized = new SerializedObject(target);
            SerializedProperty property = serialized.FindProperty(propertyName);
            if (property == null) throw new InvalidOperationException(target.GetType().Name + " no tiene la propiedad " + propertyName);
            property.objectReferenceValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetSerialized(UnityEngine.Object target, string propertyName, float value)
        {
            SerializedObject serialized = new SerializedObject(target);
            SerializedProperty property = serialized.FindProperty(propertyName);
            if (property == null) throw new InvalidOperationException(target.GetType().Name + " no tiene la propiedad " + propertyName);
            property.floatValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetSerialized(UnityEngine.Object target, string propertyName, int value)
        {
            SerializedObject serialized = new SerializedObject(target);
            SerializedProperty property = serialized.FindProperty(propertyName);
            if (property == null) throw new InvalidOperationException(target.GetType().Name + " no tiene la propiedad " + propertyName);
            property.intValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetSerialized(UnityEngine.Object target, string propertyName, string value)
        {
            SerializedObject serialized = new SerializedObject(target);
            SerializedProperty property = serialized.FindProperty(propertyName);
            if (property == null) throw new InvalidOperationException(target.GetType().Name + " no tiene la propiedad " + propertyName);
            property.stringValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetSerializedArray(UnityEngine.Object target, string propertyName, UnityEngine.Object[] values)
        {
            SerializedObject serialized = new SerializedObject(target);
            SerializedProperty property = serialized.FindProperty(propertyName);
            if (property == null || !property.isArray) throw new InvalidOperationException(target.GetType().Name + " no tiene el array " + propertyName);
            property.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++) property.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private sealed class DirectionalClips
        {
            public AnimationClip Down;
            public AnimationClip Right;
            public AnimationClip Up;
            public AnimationClip Left;
        }
    }
}
#endif

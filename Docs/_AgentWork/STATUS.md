# STATUS

## Estado general
Implementación completa del núcleo de gameplay. Todos los scripts creados, pendiente configuración en Unity.

## Fase actual
Etapa 3–5 completada: implementación de todos los hitos (1–13), excepto puntos extra.

## Decisiones BLOCKING resueltas (10/07/2026)
- **Power-up:** Buff simple de estadística. Tres tipos: Speed, Damage, AttackSpeed.
  Un único script BuffCollectible configurable por Inspector (un prefab por tipo).
- **Enemigo avanzado:** RangedEnemy — dispara proyectiles, mantiene distancia.
- **Jefe:** BossEnemy — persigue y ataca cuerpo a cuerpo. Embestida especial como segundo comportamiento.
- **Pergaminos:** 1 habilidad — FireAbility (proyectil de fuego). Arquitectura extensible para agregar más.

## Scripts creados (Assets/Scripts/)

### Interfaces/
- `IDamageable.cs` — Hito 1
- `ICollectible.cs` — Hito 1

### Health/
- `Health.cs` — Hito 2

### Player/
- `PlayerStats.cs` — Hito 3
- `PlayerMovement.cs` — Hito 3
- `PlayerAttack.cs` — Hito 3
- `PlayerCollector.cs` — Hito 8
- `TemporaryPowerUpController.cs` — Hito 9
- `ScrollLoadout.cs` — Hito 10

### Enemies/
- `EnemyBase.cs` — Hito 4
- `MeleeEnemy.cs` — Hito 5
- `Projectile.cs` — Hito 7
- `RangedEnemy.cs` — Hito 7
- `BossEnemy.cs` — Hito 11

### Score/
- `ScoreTracker.cs` — Hito 8

### Collectibles/
- `BuffType.cs` (enum) — Hito 8
- `CoinCollectible.cs` — Hito 8
- `FoodCollectible.cs` — Hito 8
- `BuffCollectible.cs` — Hito 9

### Abilities/
- `ScrollAbility.cs` — Hito 10
- `FireAbility.cs` — Hito 10

### Flow/
- `ObjectiveTracker.cs` — Hito 12
- `LevelFlowController.cs` — Hito 12

### UI/
- `HUDController.cs` — Hito 13
- `GameResultController.cs` — Hito 13

## Total: 24 scripts

## Pendiente (responsabilidad del estudiante en Unity)
- Crear carpeta Assets/Scripts/ ya existente con todos los scripts.
- Configurar Tags y Layers: Player, Enemy.
- Crear Sorting Layers: Background, Characters, FX, UI.
- Crear escena Tutorial y escena Level1, registrarlas en Build Settings.
- Crear Player GameObject con todos sus componentes y referencias.
- Crear prefabs de MeleeEnemy, RangedEnemy, BossEnemy.
- Crear prefab de Projectile (para enemigos ranged).
- Crear prefab de FireProjectile (para FireAbility).
- Crear prefabs de CoinCollectible, FoodCollectible, BuffCollectible (x3).
- Configurar PlayerInput con InputSystem_Actions y Behavior = Send Messages.
- Agregar acción "UseScroll" al InputSystem_Actions asset.
- Construir HUD: Canvas con TextMeshPro para HP, Puntos y Buff.
- Crear paneles VictoryPanel y DefeatPanel con botones conectados.
- Diseñar niveles con Tilemaps.

## Riesgos conocidos
- BossEnemy.OnDestroy llama a base.OnDestroy() — verificar que EnemyBase declare OnDestroy como virtual.
- La acción "UseScroll" debe existir en el InputSystem_Actions asset; si no existe, crearla.
- Time.timeScale = 0 pausa coroutines; si el buff está activo al morir, la coroutine se congela
  (aceptable para este scope).

## Próximo paso recomendado
Configurar el proyecto en Unity siguiendo la guía 06_UNITY_WIRING.md.
Comenzar por: Tags/Layers → Player GameObject → primer MeleeEnemy → probar colisión y daño.

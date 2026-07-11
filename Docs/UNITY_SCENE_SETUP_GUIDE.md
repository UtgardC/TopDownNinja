# Guía de montaje de Unity

Esta guía parte del estado actual del proyecto y de los scripts ubicados en `Assets/Scripts`.
El objetivo es obtener dos escenas funcionales: un tutorial breve y un nivel principal de menos de cinco minutos.

## 1. Preparación del proyecto

### Escenas en la build

Abrir `File > Build Profiles` (o `Build Settings`, según la ventana mostrada por Unity) y registrar, en este orden:

1. `Assets/Scenes/Tutorial.unity`
2. `Assets/Scenes/Level1.unity`

`Menu.unity` es opcional. Actualmente no es necesario para cumplir la consigna.

### Layers y Sorting Layers

En `Edit > Project Settings > Tags and Layers`, comprobar:

- Layer 8: `Player`
- Layer 9: `Enemy`
- Agregar `Obstacle`, `PlayerProjectile`, `EnemyProjectile` y `Collectible` si se desea separar la matriz de física.

Crear estas Sorting Layers, de fondo a frente:

1. `Ground`
2. `World`
3. `Characters`
4. `Foreground`
5. `FX`
6. `UI`

### Input System

En el componente `PlayerInput` del prefab Player:

- Actions: `Assets/InputSystem_Actions.inputactions`
- Default Map: `Player`
- Behavior: `Send Messages`

Controles configurados:

- Movimiento: WASD o flechas.
- Ataque: J o botón izquierdo del mouse.
- Pergamino: K.

Si `Default Map` queda vacío, es posible que las acciones no se habiliten al comenzar.

## 2. Escala de pixel art

El pack usa tiles de 16x16 píxeles y actores que normalmente ocupan 32x32. Para que un tile mida una unidad de Unity:

1. Seleccionar solamente las texturas que se usarán.
2. En el Texture Importer elegir:
   - Texture Type: `Sprite (2D and UI)`
   - Sprite Mode: `Multiple` cuando sea un spritesheet.
   - Pixels Per Unit: `16`
   - Filter Mode: `Point (no filter)`
   - Compression: `None`
3. Presionar `Apply`.

No hace falta cambiar todo el asset pack. Conviene hacerlo con los tilesets, personajes, enemigos, proyectiles y objetos realmente elegidos.

Tilesets recomendados para el bosque:

- `Assets/NinjaAssetPack/Backgrounds/Tilesets/TilesetField.png`
- `Assets/NinjaAssetPack/Backgrounds/Tilesets/TilesetNature.png`
- `Assets/NinjaAssetPack/Backgrounds/Tilesets/TilesetWater.png`
- `Assets/NinjaAssetPack/Backgrounds/Tilesets/TilesetRelief.png`

## 3. Crear una Tile Palette

1. Crear las carpetas `Assets/Tiles/Palette` y `Assets/Tiles/Generated`.
2. Abrir `Window > 2D > Tile Palette`.
3. Crear una paleta rectangular llamada `ForestPalette` dentro de `Assets/Tiles/Palette`.
4. Seleccionar un tileset en Project y abrir `Sprite Editor`.
5. Si no estuviera cortado, usar `Slice > Grid by Cell Size`, con celdas de 16x16.
6. Aplicar los cambios.
7. Expandir el spritesheet en Project, seleccionar los sprites necesarios y arrastrarlos a la Tile Palette.
8. Cuando Unity pregunte dónde guardar los Tile assets, elegir `Assets/Tiles/Generated`.

No es necesario llenar la paleta con todo el pack. Para comenzar bastan suelo de pasto, caminos, agua, árboles, rocas y bordes.

## 4. Crear el Grid y las colisiones

En cada escena:

1. Crear `GameObject > 2D Object > Tilemap > Rectangular`.
2. Renombrar el objeto padre como `WorldGrid`.
3. Renombrar el Tilemap creado como `Ground`.
4. Duplicarlo para obtener esta estructura:

```text
WorldGrid
├── Ground
├── GroundDetails
├── Obstacles
└── Foreground
```

Configuración:

- `Ground`: Sorting Layer `Ground`, sin Collider.
- `GroundDetails`: Sorting Layer `World`, sin Collider.
- `Obstacles`: Sorting Layer `World`, Layer física `Obstacle`.
- `Foreground`: Sorting Layer `Foreground`, sin Collider salvo casos realmente necesarios.

Para `Obstacles`:

1. Agregar `TilemapCollider2D`.
2. Agregar `Rigidbody2D` con Body Type `Static`.
3. Agregar `CompositeCollider2D`.
4. En `TilemapCollider2D`, activar la opción de composición (`Used By Composite` o `Composite Operation: Merge`, según el Inspector).

El Composite evita generar un collider independiente por cada tile y produce bordes más estables.

Pintar en `Obstacles` solamente lo que realmente bloquea movimiento: troncos, rocas grandes, paredes, agua profunda y límites del nivel. La copa visual de un árbol puede ir en `Foreground`, mientras su tronco o base bloqueante va en `Obstacles`.

## 5. Configurar el prefab Player

Usar `Assets/Prefabs/Player.prefab` como única fuente del jugador para ambas escenas.

### Componentes del root

- Layer: `Player`
- Tag: `Player`
- `Rigidbody2D`
  - Body Type: Dynamic
  - Gravity Scale: 0
  - Interpolate: Interpolate
  - Collision Detection: Continuous
  - Freeze Rotation Z: activado
- `Collider2D` de cuerpo, con `Is Trigger` desactivado.
- `PlayerInput`
- `Health`
- `PlayerStats`
- `PlayerMovement`
- `PlayerAttack`
- `PlayerCollector`
- `TemporaryPowerUpController`
- `ScoreTracker`
- `ScrollLoadout`
- `FireAbility`
- `RockAbility`
- `PlayerAnimator`

El collider corporal del Player no debe ser Trigger. Los coleccionables sí deben tener Collider2D Trigger; Unity igualmente llamará `OnTriggerEnter2D` del Player.

### Hijos recomendados

```text
Player
├── Visual
│   ├── SpriteRenderer
│   └── Animator
└── AttackOrigin
```

`Visual` permite animar o voltear el sprite sin deformar el collider y el Rigidbody del root.

### Referencias del Inspector

`PlayerMovement`:

- Stats: `PlayerStats` del mismo prefab.

`PlayerAttack`:

- Stats: `PlayerStats`.
- Movement: `PlayerMovement`.
- Attack Origin: hijo `AttackOrigin`.
- Attack Offset: aproximadamente 0.6.
- Attack Range: aproximadamente 0.7.
- Enemy Layer: `Enemy`.

El script reposiciona `AttackOrigin` automáticamente según la última dirección de movimiento.

`PlayerCollector`:

- Health: `Health`.
- Score Tracker: `ScoreTracker`.
- Power Up Controller: `TemporaryPowerUpController`.
- Scroll Loadout: `ScrollLoadout`.

`ScrollLoadout`:

- Movement: `PlayerMovement`.
- Available Abilities: tamaño 2.
  - Elemento 0: `FireAbility`.
  - Elemento 1: `RockAbility`.
- Equipped Ability: `FireAbility` para comenzar con fuego, o dejar vacío si se quiere entregar el primer pergamino durante el tutorial.

`FireAbility`:

- Fire Projectile Prefab: prefab de bola de fuego.
- Target Layers: `Enemy`.
- Damage sugerido: 15.
- Cooldown sugerido: 1.5.

`RockAbility`:

- Rock Effect Prefab: efecto visual de roca.
- Target Layers: `Enemy`.
- Damage sugerido: 30.
- Cooldown sugerido: 2.5.
- Effect Offset: 0.9.
- Effect Radius: 0.8.

`PlayerAnimator`:

- Movement: `PlayerMovement`.
- Player Attack: `PlayerAttack`.
- Health: `Health`.
- Animator: Animator del hijo `Visual`.

Después de configurar el prefab, presionar `Overrides > Apply All` si se trabajó sobre una instancia.

## 6. Proyectiles y efecto de roca

### Proyectil enemigo

En `Assets/Prefabs/Proyectiles/Projectile.prefab`:

- Layer: `EnemyProjectile` si fue creada.
- `Rigidbody2D`: Dynamic, Gravity Scale 0.
- Collider2D: `Is Trigger` activado.
- `Projectile`:
  - Target Layers: `Player`.
  - Speed sugerido: 5 o 6.
  - Lifetime: 4.

### Bola de fuego

El prefab actual `FireAbilty.prefab` necesita recibir el componente `Projectile`.

- Layer: `PlayerProjectile`.
- `Rigidbody2D`: Dynamic, Gravity Scale 0.
- Collider2D: Trigger.
- `Projectile`: Speed 7, Target Layers `Enemy`, Lifetime 4.

`FireAbility` vuelve a asignar la capa objetivo y el daño al instanciarla, pero el componente `Projectile` debe existir en el prefab.

### Efecto de roca

Crear `RockEffect.prefab` con:

- `SpriteRenderer` usando `Assets/NinjaAssetPack/Items/Resource/Rock.png` u otro sprite de roca.
- Sorting Layer `FX`.
- Sin Rigidbody y sin Collider: el daño lo calcula `RockAbility`.

Puede ser un sprite estático al principio. Después se le puede agregar Animator con una aparición corta.

## 7. Pergaminos intercambiables

Crear dos prefabs: `FireScroll.prefab` y `RockScroll.prefab`.

Cada uno necesita:

- SpriteRenderer.
- Collider2D con `Is Trigger`.
- Layer `Collectible` si fue creada.
- `ScrollCollectible`.

En ambos `ScrollCollectible`:

- Fire Sprite: `Assets/NinjaAssetPack/Items/Scroll/ScrollFire.png`.
- Rock Sprite: `Assets/NinjaAssetPack/Items/Scroll/ScrollRock.png`.
- Sprite Renderer: el componente del prefab.

En FireScroll elegir Type `Fire`; en RockScroll, Type `Rock`.

El intercambio funciona así:

1. El Player tiene Fire.
2. Recoge Rock.
3. El Player equipa Rock.
4. El objeto del suelo cambia a Fire.
5. Si vuelve a recogerlo, recupera Fire y deja Rock.

## 8. Enemigos

Todos los prefabs enemigos deben usar Layer `Enemy`.

Configuración común:

- `Rigidbody2D`: Dynamic, Gravity Scale 0, Freeze Rotation Z.
- Collider2D no Trigger.
- `Health`.
- Script derivado de `EnemyBase`.
- Animator en el root o en un hijo Visual.
- `EnemyAnimator`, apuntando al EnemyBase, Health y Animator correspondientes.
- Target: asignar la instancia del Player de la escena.
- Death Disable Delay: igual o un poco mayor que el clip de muerte, por ejemplo 0.75.

`MeleeEnemy`:

- Layer Mask Player: `Player`.
- Vida sugerida: 30.
- Daño: 10.
- Velocidad: 1.5-2.

`RangedEnemy`:

- Layer `Enemy`.
- Projectile Prefab: `Projectile.prefab` enemigo.
- Vida sugerida: 20-30.
- Shoot Range: 5.
- Stop Range: 2.5.
- Cooldown: 2.

`Boss`:

- Layer `Enemy`.
- Health: 250-300.
- Player Layer: `Player`.
- Attack Damage: 20.
- Charge Damage: 30.
- Charge Hit Radius: cercano al tamaño del collider.
- Death Disable Delay: igual al clip de muerte.

La embestida aplica daño una sola vez por objetivo durante cada carga.

## 9. Animator Controllers

Los scripts esperan exactamente estos parámetros.

Player:

- `Speed` Float
- `MoveX` Float
- `MoveY` Float
- `Attack` Trigger
- `Hit` Trigger
- `Dead` Bool

Enemigos:

- `Speed` Float
- `MoveX` Float
- `MoveY` Float
- `Attack` Trigger
- `Special` Trigger
- `Hit` Trigger
- `Dead` Bool

Para Player conviene crear Blend Trees direccionales para Idle y Walk. Los sprites de `CharacterAnimated/NinjaGreen/Separate` ya están cortados:

- Idle: 16 frames, normalmente 4 por dirección.
- Walk: 16 frames.
- Attack: 16 frames.
- Hit: 8 frames.
- Dead: 2 frames.

Transiciones recomendadas:

- Idle <-> Walk usando `Speed`.
- Any State -> Attack con Trigger `Attack`.
- Any State -> Hit con Trigger `Hit`.
- Any State -> Dead con `Dead = true`, sin transición de salida.

En los enemigos, `Attack` se dispara tanto para melee como para el disparo ranged. `Special` se usa para la embestida del Boss.

## 10. Cámara

El proyecto ya incluye Cinemachine. La configuración más simple es:

1. Conservar `Main Camera` con proyección Orthographic.
2. Crear `GameObject > Cinemachine > Cinemachine Camera`.
3. Asignar el Transform del Player como Tracking Target.
4. Ajustar Orthographic Size hasta ver unas 15-20 celdas de ancho.
5. Comprobar que Main Camera tenga `Cinemachine Brain`.

Si se desea limitar la cámara al mapa, agregar posteriormente un Confiner 2D con un PolygonCollider2D que rodee el nivel.

## 11. Montaje de Tutorial

Duración objetivo: 45-90 segundos.

Jerarquía sugerida:

```text
Tutorial
├── Systems
│   ├── LevelFlowController
│   └── GameResultController
├── WorldGrid
├── Player
├── Enemies
├── Collectibles
├── TutorialSigns
├── Exit
├── Camera
└── Canvas
```

Recorrido sugerido:

1. Cartel: mover con WASD.
2. Pasillo corto con paredes para verificar colisiones.
3. Cartel: atacar con J.
4. Un MeleeEnemy con poca vida.
5. Una moneda.
6. Comida colocada después del enemigo.
7. Un SpeedBuff.
8. Un RockScroll y cartel de habilidad K.
9. Otro enemigo para probar fuego/roca.
10. Portal de salida.

Para el portal:

- Crear un GameObject `Exit`.
- Agregar BoxCollider2D Trigger.
- Agregar `LevelExitTrigger`.
- Level Flow: referencia al `LevelFlowController` de la escena.
- Player Layer: `Player`.

En `GameResultController` del Tutorial:

- Player Health: Health del Player.
- Objective Tracker: puede quedar vacío.
- Level Flow: LevelFlowController.
- Defeat Panel: panel de derrota.
- Victory Panel: no es necesario en Tutorial.

## 12. Montaje de Level1

Duración objetivo: 3-5 minutos.

Estructura sugerida:

```text
Level1
├── Systems
│   ├── LevelFlowController
│   ├── ObjectiveTracker
│   └── GameResultController
├── WorldGrid
├── Player
├── Enemies
├── Collectibles
├── BossArena
├── Camera
└── Canvas
```

Distribución inicial razonable:

- 4-6 melee.
- 2-3 ranged.
- 8-12 monedas.
- 2 comidas.
- 2 buffs temporales.
- Al menos un pergamino para intercambiar.
- Un Boss.

Diseñar el mapa como un camino principal con uno o dos desvíos cortos. Evitar un mapa abierto muy grande: aumenta el trabajo visual y dificulta controlar que dure menos de cinco minutos.

Cableado final:

1. Arrastrar el Player a `target` de cada enemigo colocado.
2. Arrastrar la instancia del Boss a `ObjectiveTracker.boss`.
3. En `GameResultController` asignar:
   - Player Health.
   - Objective Tracker.
   - Level Flow Controller.
   - Victory Panel.
   - Defeat Panel.
4. Dejar VictoryPanel y DefeatPanel desactivados en la jerarquía.
5. Conectar botones a `GameResultController.OnClickRestart` y `OnClickMenu`.

## 13. Orden de comprobación manual

Probar en este orden para localizar fallos con facilidad:

1. Player se mueve y choca con Obstacles.
2. La cámara sigue al Player.
3. J ejecuta ataque en la dirección correcta.
4. Melee persigue, ataca y muere.
5. Ranged dispara al Player.
6. Fire daña Enemy y no daña Player.
7. Rock daña sólo dentro de su radio.
8. RockScroll y FireScroll se intercambian.
9. Moneda, comida y buffs actualizan HUD.
10. El Boss daña con melee y embestida.
11. La muerte del Boss activa VictoryPanel.
12. La muerte del Player activa DefeatPanel.
13. Reiniciar devuelve `Time.timeScale` a 1.
14. El portal del Tutorial carga Level1.

Los errores más probables durante el montaje son referencias vacías, LayerMasks en `Nothing`, `Default Map` vacío, colliders sin Trigger en coleccionables o escenas ausentes de Build Settings.

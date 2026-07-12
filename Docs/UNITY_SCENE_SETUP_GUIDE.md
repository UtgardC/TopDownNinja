# Guía de montaje de Unity

Esta guía parte del estado actual del proyecto y de los scripts ubicados en `Assets/Scripts`.
El objetivo es obtener dos escenas funcionales: un tutorial breve y un nivel principal de menos de cinco minutos.

## 0. Mapa mental: qué es cada cosa

Antes de arrastrar referencias conviene separar cuatro conceptos de Unity:

- **Script/componente:** contiene una conducta. Ejemplo: `FireAbility` sabe crear una bola de fuego.
- **Prefab:** es una plantilla de GameObject guardada en Project. Ejemplo: `FireProjectile.prefab` describe el objeto que volará por la escena.
- **Instancia:** es una copia de un prefab que existe dentro de una escena o fue creada durante el juego.
- **Referencia del Inspector:** conecta un componente con otro componente, prefab u objeto que necesita para trabajar.

### Las habilidades viven en el Player; el pergamino sólo elige una

El Player tiene agregados `FireAbility` y `RockAbility` desde el comienzo. Esto **no significa que pueda usar las dos al mismo tiempo**. Son las dos conductas disponibles, como dos herramientas guardadas en una caja.

`ScrollLoadout` conserva una sola referencia llamada `equippedAbility`. Esa referencia indica cuál herramienta está equipada ahora. Cuando se presiona K, solamente se ejecuta esa habilidad.

```text
Tecla K
  -> PlayerInput envía OnUseScroll
  -> ScrollLoadout mira equippedAbility
  -> ejecuta FireAbility O RockAbility
```

El pergamino del suelo no contiene la habilidad completa ni se agrega como componente al Player. `ScrollCollectible` solamente le dice al `ScrollLoadout`: "ahora equipa Rock" o "ahora equipa Fire".

```text
Player toca RockScroll
  -> PlayerCollector detecta un ICollectible
  -> ScrollCollectible pide equipar Rock
  -> ScrollLoadout cambia equippedAbility de FireAbility a RockAbility
  -> el pergamino del suelo pasa a representar Fire
```

Este diseño evita agregar y destruir componentes durante la partida, mantiene todas las referencias configurables en el prefab Player y permite que cada habilidad conserve su propio cooldown.

### Diferencia entre FireAbility, Projectile y FireScroll

| Elemento | Dónde existe | Responsabilidad |
|---|---|---|
| `FireAbility` | Componente del Player | Decide cuándo y desde dónde crear la bola; configura daño y objetivo. |
| `FireProjectile.prefab` | Asset dentro de Project | Plantilla de la bola visible que se mueve, detecta impacto y se destruye. |
| Instancia del proyectil | En la escena durante unos segundos | Es la copia real creada cada vez que se usa fuego. |
| `FireScroll.prefab` | Asset y luego objeto colocado en el nivel | Coleccionable que selecciona Fire en el `ScrollLoadout`. No dispara nada. |

### Diferencia entre RockAbility, RockEffect y RockScroll

Rock no usa un proyectil viajando. `RockAbility` calcula instantáneamente un círculo delante del Player y daña a los enemigos dentro de ese círculo.

- `RockAbility`: componente del Player; calcula posición, radio y daño.
- `RockEffect.prefab`: objeto visual opcional que aparece brevemente donde golpeó la roca. No necesita Collider ni script de daño.
- `RockScroll.prefab`: coleccionable que selecciona Rock en el loadout.

Por eso fuego necesita un prefab con `Rigidbody2D + Collider2D + Projectile`, mientras que el efecto de roca sólo necesita un `SpriteRenderer` y, opcionalmente, un Animator.

### Qué objeto debe conservar cada responsabilidad

```text
Player (root: física y gameplay)
├── Rigidbody2D / Collider2D / PlayerInput
├── Health / PlayerStats / PlayerMovement / PlayerAttack
├── PlayerCollector / ScoreTracker / TemporaryPowerUpController
├── ScrollLoadout / FireAbility / RockAbility
├── PlayerAnimator (puente entre gameplay y Animator)
├── Visual (hijo: solamente representación visual)
│   ├── SpriteRenderer
│   └── Animator
└── AttackOrigin (hijo: marcador invisible)
```

El root se mueve y colisiona. `Visual` cambia sprites. Separarlos evita que una animación mueva accidentalmente el collider o el Rigidbody.

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

### Abrir y editar el prefab sin modificar una escena

1. En la ventana Project, hacer doble clic en `Assets/Prefabs/Player.prefab`.
2. Unity abre Prefab Mode y muestra sólo el Player.
3. Seleccionar el objeto root `Player` para agregar scripts de gameplay.
4. Crear o seleccionar el hijo `Visual` para los componentes gráficos.
5. Guardar con Ctrl+S y volver a la escena con la flecha de la esquina superior izquierda.

No agregar componentes diferentes a las copias de Tutorial y Level1. Editar el prefab hace que ambas instancias reciban la misma configuración y evita que se desincronicen.

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

Para crearlo:

1. Clic derecho sobre Player en Hierarchy > `Create Empty`; nombrarlo `Visual`.
2. Con Visual seleccionado, `Add Component > Sprite Renderer`.
3. Arrastrar al campo Sprite un frame Idle del ninja. Es sólo la imagen que se verá antes de crear las animaciones.
4. `Add Component > Animator` sobre el mismo Visual.
5. No agregar Rigidbody2D ni Collider2D a Visual.
6. Clic derecho sobre Player > `Create Empty`; nombrarlo `AttackOrigin`.
7. Dejar AttackOrigin sin SpriteRenderer, Collider ni scripts. Es un punto invisible que `PlayerAttack` mueve automáticamente.

Si el Player actual ya tiene SpriteRenderer en el root, mover ese componente a Visual o crear Visual con uno nuevo y quitar el del root. Debe quedar un solo SpriteRenderer encargado del cuerpo del personaje.

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

Para asignarlos, arrastrar desde el encabezado de cada componente del mismo Inspector: arrastrar `Fire Ability (Script)` al elemento 0 y `Rock Ability (Script)` al elemento 1. No se arrastran los archivos `.cs` desde Project; se arrastran las instancias de los componentes que están agregadas al Player.

Ambos componentes pueden permanecer habilitados. `ScrollLoadout` no usa el estado enabled/disabled: simplemente llama a la referencia que está equipada.

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

En este caso sí se arrastra un componente de un hijo: desplegar Player en Hierarchy, seleccionar/ubicar Visual y arrastrar su componente Animator al campo Animator de `PlayerAnimator`.

Después de configurar el prefab, presionar `Overrides > Apply All` si se trabajó sobre una instancia.

## 6. Proyectiles y efecto de roca

Un proyectil no se coloca normalmente en la escena. Se construye una vez, se arrastra a Project para convertirlo en prefab y luego `FireAbility` o `RangedEnemy` crea instancias con `Instantiate`.

Jerarquía simple de cualquier proyectil:

```text
FireProjectile (un solo GameObject)
├── Transform
├── SpriteRenderer
├── Rigidbody2D
├── CircleCollider2D
└── Projectile
```

No necesita un objeto hijo salvo que el sprite tenga un efecto visual adicional.

### Proyectil enemigo

En `Assets/Prefabs/Proyectiles/Projectile.prefab`:

- Layer: `EnemyProjectile` si fue creada.
- `Rigidbody2D`: Dynamic, Gravity Scale 0.
- Collider2D: `Is Trigger` activado.
- `Projectile`:
  - Target Layers: `Player`.
  - Speed sugerido: 5 o 6.
  - Lifetime: 4.

El prefab no guarda el daño del enemigo: `RangedEnemy` llama `SetDamage` al crear cada instancia. Así dos enemigos pueden compartir el mismo prefab pero disparar con daños diferentes.

### Bola de fuego

El prefab actual `FireAbilty.prefab` necesita recibir el componente `Projectile`. El nombre tiene un typo histórico; puede renombrarse a `FireProjectile.prefab` desde Unity para que resulte más claro.

Para construirlo desde cero:

1. En una escena, crear `GameObject > Create Empty` y nombrarlo `FireProjectile`.
2. Resetear Transform a posición 0,0,0 y escala 1,1,1.
3. Agregar `SpriteRenderer` y colocar el sprite de bola de fuego en su campo Sprite.
4. Elegir Sorting Layer `FX`.
5. Agregar `Rigidbody2D`: Dynamic, Gravity Scale 0, Freeze Rotation Z.
6. Agregar `CircleCollider2D` y activar `Is Trigger`.
7. Ajustar el radio del collider para cubrir sólo la parte visible del fuego.
8. Agregar el script `Projectile`.
9. Configurar Target Layers = Enemy, Speed = 7, Lifetime = 4.
10. Arrastrar FireProjectile desde Hierarchy a `Assets/Prefabs/Proyectiles`.
11. Eliminar la copia de la escena: desde ahora la crea FireAbility.
12. Abrir `Player.prefab` y arrastrar el prefab de Project al campo `Fire Projectile Prefab` de FireAbility.

- Layer: `PlayerProjectile`.
- `Rigidbody2D`: Dynamic, Gravity Scale 0.
- Collider2D: Trigger.
- `Projectile`: Speed 7, Target Layers `Enemy`, Lifetime 4.

`FireAbility` vuelve a asignar la capa objetivo y el daño al instanciarla, pero el componente `Projectile` debe existir en el prefab.

Flujo al presionar K con fuego equipado:

```text
FireAbility recibe la dirección del Player
  -> instancia FireProjectile delante del Player
  -> configura Damage y Target Layers
  -> Projectile aplica velocidad al Rigidbody2D
  -> al tocar un IDamageable en Layer Enemy, causa daño
  -> destruye la instancia
```

### Efecto de roca

Crear `RockEffect.prefab` con:

- `SpriteRenderer` usando `Assets/NinjaAssetPack/Items/Resource/Rock.png` u otro sprite de roca.
- Sorting Layer `FX`.
- Sin Rigidbody y sin Collider: el daño lo calcula `RockAbility`.

Puede ser un sprite estático al principio. Después se le puede agregar Animator con una aparición corta.

Pasos exactos:

1. Crear Empty `RockEffect`.
2. Agregar `SpriteRenderer` y poner un sprite de roca.
3. Elegir Sorting Layer `FX`.
4. No agregar `Projectile`, Rigidbody2D, Health ni Collider2D.
5. Arrastrarlo a `Assets/Prefabs/FX/RockEffect.prefab` (crear la carpeta FX si falta).
6. Asignarlo al campo `Rock Effect Prefab` de RockAbility en el Player.

RockAbility crea esta imagen en el centro del círculo de ataque y la destruye luego de `Effect Lifetime`. El círculo blanco/gris puede verse seleccionando el Player en Scene porque el script lo dibuja como Gizmo. El daño funciona incluso si Rock Effect Prefab queda vacío; en ese caso simplemente no habrá feedback visual.

## 7. Pergaminos intercambiables

Crear dos prefabs: `FireScroll.prefab` y `RockScroll.prefab`.

Estos prefabs son objetos quietos del escenario. No deben tener `FireAbility` ni `RockAbility`: esas conductas pertenecen al Player. Tampoco deben tener `Projectile`.

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

### Crear FireScroll paso a paso

1. En Hierarchy, crear Empty `FireScroll`.
2. Agregar `SpriteRenderer`.
3. Arrastrar `ScrollFire.png` al campo Sprite.
4. Agregar `CircleCollider2D` o `BoxCollider2D` y activar `Is Trigger`.
5. Agregar `ScrollCollectible`.
6. En Scroll Type elegir `Fire`.
7. En Fire Sprite asignar `ScrollFire.png`.
8. En Rock Sprite asignar `ScrollRock.png`.
9. En Sprite Renderer arrastrar el SpriteRenderer del mismo objeto.
10. Arrastrar el GameObject a `Assets/Prefabs/Coleccionables` para crear el prefab.

### Crear RockScroll

Duplicar FireScroll en Project, renombrarlo `RockScroll`, abrirlo y cambiar `Scroll Type` a `Rock`. `ScrollCollectible.Awake` cambiará el SpriteRenderer al sprite de roca automáticamente al jugar.

No agregar Rigidbody2D si el pergamino permanecerá quieto. El Player ya tiene Rigidbody2D, por lo que la combinación Player + Collider Trigger del scroll genera el evento de recolección.

El intercambio funciona así:

1. El Player tiene Fire.
2. Recoge Rock.
3. El Player equipa Rock.
4. El objeto del suelo cambia a Fire.
5. Si vuelve a recogerlo, recupera Fire y deja Rock.

Si `PlayerCollector.Scroll Loadout` está vacío, tocar el pergamino no producirá ningún cambio. Si `ScrollLoadout.Available Abilities` no contiene FireAbility y RockAbility, aparecerá un warning indicando que la habilidad no está configurada.

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

### Qué hace cada pieza

- **Animation Clip (`.anim`):** secuencia de sprites y su velocidad. Ejemplo: `Player_Walk_Down`.
- **Animator Controller (`.controller`):** diagrama que decide qué clip reproducir.
- **Animator (componente):** reproduce el controller sobre el SpriteRenderer de un GameObject.
- **PlayerAnimator/EnemyAnimator (scripts):** traducen el gameplay a parámetros. No contienen sprites ni clips.

El componente Animator y el SpriteRenderer deben estar juntos en `Visual`. El script `PlayerAnimator` puede estar en el root Player porque recibe referencias explícitas.

```text
PlayerMovement dice "se mueve hacia arriba"
  -> PlayerAnimator escribe Speed, MoveX y MoveY
  -> Animator Controller elige WalkUp
  -> Animator cambia el campo Sprite del SpriteRenderer de Visual
```

### Carpetas recomendadas

Crear:

```text
Assets/Animations
├── Player
│   ├── Clips
│   └── Player.controller
├── MeleeEnemy
├── RangedEnemy
└── Boss
```

### Crear un Animation Clip de sprites

Para el Player, los sprites preparados se encuentran en `Assets/NinjaAssetPack/Actor/CharacterAnimated/NinjaGreen/Separate`. Cada archivo PNG aparece como una flecha desplegable en Project y contiene varios sub-sprites.

Ejemplo para `Player_Walk_Down`:

1. Abrir `Player.prefab` en Prefab Mode.
2. Seleccionar el hijo `Visual`, no el root Player.
3. Abrir `Window > Animation > Animation`.
4. Si Visual todavía no tiene controller, pulsar `Create` y guardar `Player.controller` en `Assets/Animations/Player`.
5. En el desplegable de clips elegir `Create New Clip`.
6. Guardar como `Assets/Animations/Player/Clips/Player_Walk_Down.anim`.
7. En Project, expandir `Walk.png` para ver sus sprites internos.
8. Seleccionar en orden solamente los frames que muestran caminar hacia abajo.
9. Arrastrarlos a la timeline de Animation.
10. Ajustar Samples a 8-12 para pixel art.
11. Seleccionar el clip en Project y activar `Loop Time` para Walk e Idle.

El orden exacto de los grupos de cuatro depende del spritesheet. Mirar la miniatura o reproducir el clip; no asumir que el primer grupo siempre es Down. Cada clip direccional debe contener sólo los frames de una dirección.

Crear como mínimo:

- `Player_Idle_Down`, `Idle_Up`, `Idle_Left`, `Idle_Right`.
- `Player_Walk_Down`, `Walk_Up`, `Walk_Left`, `Walk_Right`.
- `Player_Attack_Down`, `Attack_Up`, `Attack_Left`, `Attack_Right`.
- `Player_Hit` y `Player_Dead` pueden comenzar como clips no direccionales.

No activar Loop Time en Attack, Hit ni Dead.

### Parámetros exactos del Player Controller

En la ventana Animator, pestaña Parameters, crear respetando mayúsculas:

- `Speed` Float
- `MoveX` Float
- `MoveY` Float
- `Attack` Trigger
- `Hit` Trigger
- `Dead` Bool

Si se escribe `IsAttacking`, `IsDead` u otro nombre, el script no controlará ese parámetro.

### Idle direccional con Blend Tree

1. En Animator, clic derecho > `Create State > From New Blend Tree`.
2. Renombrar el estado `Idle`.
3. Doble clic en el Blend Tree.
4. Blend Type: `2D Simple Directional`.
5. Parameters: X = `MoveX`, Y = `MoveY`.
6. Agregar cuatro Motions:
   - IdleRight en `(1, 0)`.
   - IdleLeft en `(-1, 0)`.
   - IdleUp en `(0, 1)`.
   - IdleDown en `(0, -1)`.
7. Volver a Base Layer y marcar Idle como estado por defecto.

### Walk direccional

Crear otro Blend Tree `Walk` con la misma configuración y los cuatro clips Walk.

Transiciones:

- Idle -> Walk: desactivar Has Exit Time; condición `Speed > 0.01`.
- Walk -> Idle: desactivar Has Exit Time; condición `Speed < 0.01`.
- Transition Duration puede ser 0 para que el pixel art no mezcle poses.

### Ataque direccional

Crear un tercer Blend Tree 2D llamado `Attack` con los cuatro clips Attack y las mismas posiciones. Desactivar Loop en los clips.

Transiciones:

- Any State -> Attack: condición Trigger `Attack`, Has Exit Time desactivado, Duration 0.
- Attack -> Idle: Has Exit Time activado, Exit Time 1, sin condiciones, Duration 0.

Si Attack vuelve siempre a Idle aunque el jugador siga caminando, es aceptable para la primera versión. Luego puede agregarse una salida a Walk.

### Hit y Dead

- Crear estado `Hit` desde el clip Hit.
- Any State -> Hit con Trigger `Hit`.
- Hit -> Idle con Has Exit Time.
- Crear estado `Dead` desde el clip Dead.
- Any State -> Dead con condición `Dead = true`.
- Dead no debe tener transiciones de salida y su clip no debe hacer loop.

Conviene que la transición a Dead aparezca arriba o tenga prioridad sobre Hit, porque un golpe que deja la vida en cero dispara ambos eventos en el mismo frame.

### Enemigos

Jerarquía recomendada:

```text
MeleeEnemy (root)
├── Rigidbody2D / Collider2D / Health / MeleeEnemy
└── Visual
    ├── SpriteRenderer
    ├── Animator
    └── EnemyAnimator
```

Colocar `EnemyAnimator` en Visual hace que encuentre automáticamente el Animator del mismo objeto y `EnemyBase`/Health en el padre. También se puede colocar en el root, pero entonces hay que arrastrar manualmente el Animator de Visual.

Parámetros de controllers enemigos:

- `Speed` Float
- `MoveX` Float
- `MoveY` Float
- `Attack` Trigger
- `Special` Trigger
- `Hit` Trigger
- `Dead` Bool

Se puede empezar más simple que con el Player:

- Idle y Walk direccionales si el sprite los ofrece.
- Un solo clip Attack si no hay cuatro direcciones.
- Ranged usa el Trigger `Attack` cuando dispara.
- Boss usa `Attack` para melee y `Special` para embestida.
- Hit y Dead pueden ser no direccionales.

En `EnemyBase`, configurar `Death Disable Delay` ligeramente mayor o igual a la duración de Dead. Si el clip dura 0.6 segundos, usar 0.7. De otro modo el GameObject se desactivará antes de que termine la animación.

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

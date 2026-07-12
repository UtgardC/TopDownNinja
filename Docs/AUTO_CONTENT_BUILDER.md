# Generador automático de prefabs y Tutorial

El proyecto incluye una herramienta de Editor en `Assets/Editor/TopDownNinjaContentBuilder.cs`.

## Cómo ejecutarla

1. Abrir el proyecto con Unity 6000.4.6f1.
2. Esperar a que termine la importación y comprobar que la Console no muestre errores rojos.
3. Abrir el menú `Tools > TopDown Ninja > Build Gameplay Prefabs and Tutorial`.
4. Si Unity pregunta si debe guardar escenas modificadas, elegir Guardar.
5. Esperar el mensaje de Console: `[TopDown Ninja] Prefabs y Tutorial construidos correctamente.`

Antes de modificar nada, la herramienta crea copias de `Tutorial.unity` y `Player.prefab` dentro de `Library/TopDownNinjaSetupBackups`.

## Qué construye

- Animaciones direccionales y Animator Controller del Player.
- Animaciones/controladores básicos para Slime, Skull y Boss.
- Player prefab con Animator, PlayerAnimator, FireAbility, RockAbility y referencias completas.
- FireProjectile, EnemyProjectile y BossProjectile funcionales.
- RockEffect animado.
- FireScroll y RockScroll intercambiables.
- TrainingDummy reutilizable.
- Corrección de colliders de monedas, comida y buffs.
- Prefabs MeleeEnemy, RangedEnemy y Boss listos para arrastrar; buscan al Player por Tag.
- Cámara que sigue al Player.
- HUD y resultado del Tutorial conectados.
- Textos en pantalla y carteles de mundo.
- Secuencia: controles -> dummy melee -> moneda/comida/buff -> FireScroll -> dummy de habilidad -> salida a Level1.

## Seguridad e idempotencia

La herramienta no pinta, borra ni comprime ningún Tilemap. Dentro de `Tutorial.unity` sólo reemplaza el GameObject raíz `__AUTO_TUTORIAL_CONTENT__` y corrige referencias concretas del Player/HUD/cámara.

Puede ejecutarse de nuevo si se cambian los scripts. Al repetirla se reconstruye únicamente su contenido administrado, sin duplicarlo.

`RockScroll.prefab` queda preparado pero no se coloca en Tutorial. Está destinado a Level1.

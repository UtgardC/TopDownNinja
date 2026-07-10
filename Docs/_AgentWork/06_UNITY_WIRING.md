# 06 - Guía preliminar de wiring en Unity

> Confirmado por arquitectura: componentes y referencias generales. Provisional: nombres de layers/tags, parámetros de Animator, distribución de escena. Depende del diseño de escena: posiciones, prefabs, sprites y animaciones.

## Proyecto e inputs
- GameObjects: Player, UI, enemigos, coleccionables, objetivo, transición.
- Input provisional: WASD/flechas para movimiento, click/espacio para ataque, Q/E para habilidad/pergamino si se aprueba.
- Layers provisionales: Player, Enemy, Projectile, Collectible, Obstacle.
- Tags provisionales: Player solo si el estudiante lo prefiere; arquitectura intentará usar referencias/Interfaces.

## Player / movimiento / orientación
- Scripts: PlayerMovement, PlayerAttack, Health, PlayerCollector, opcional ScrollLoadout.
- Componentes: Rigidbody2D Dynamic o Kinematic según implementación; Collider2D no trigger para cuerpo; child AttackOrigin.
- Inspector: speed, damage, cooldown, attackOrigin, enemy layer mask.
- Animator: parámetros opcionales `MoveX`, `MoveY`, `Speed`, `IsAttacking`.
- Pruebas: mover en 4 direcciones, no atravesar obstáculos, orientar ataque.

## Salud, combate y daño
- Health en player/enemigos/destructibles.
- Collider2D de ataques/proyectiles como Trigger cuando corresponda.
- Rigidbody2D en proyectiles si se mueven con física.
- Pruebas: daño baja vida, curación no supera máximo, muerte dispara evento.

## Enemigos
- MeleeEnemy: Collider2D no trigger, rango de ataque configurable, referencia al Player.
- RangedEnemy: punto de disparo, prefab Projectile, cooldown, layer de colisión.
- BossEnemy: Health mayor, referencias a ataques especiales, evento de derrota.
- Animator provisional: `Speed`, `Attack`, `Hurt`, `Dead`, `Phase`.

## Proyectiles
- GameObject con Projectile, Collider2D Trigger, Rigidbody2D si aplica.
- Datos: speed, damage, lifetime, layers objetivo.
- Prueba: sale en dirección correcta, daña una vez, se destruye.

## Destructibles
- Opcional. GameObject con Health/IDamageable, Collider2D, posible sprite roto configurado manualmente.

## Coleccionables, puntuación, comida
- Scripts: CoinCollectible, FoodCollectible, PowerUpCollectible, ScrollCollectible.
- Collider2D: Trigger.
- Rigidbody2D: normalmente no necesario.
- Inspector: value/healAmount/duration/scroll reference.
- Pruebas: trigger solo con player, suma score, cura o activa efecto, desaparece.

## Power-up / pergaminos / habilidades
- Power-up confirmado como temporal; forma exacta pendiente.
- Si es buff: asignar referencias a PlayerMovement/PlayerAttack y multiplicadores.
- Si es pergamino temporal: ScrollLoadout con current/temporary/previous, HUD de duración.
- Pruebas: activa, muestra duración, finaliza, restaura/cancela según regla.

## HUD
- GameObject Canvas con textos/barras: salud, score, power-up, pergamino, victoria/derrota.
- HUDController se suscribe a eventos; referencias del Inspector obligatorias.
- Pruebas: cambios reflejados sin errores al desactivar/activar UI.

## Tutorial y nivel principal
- Tutorial: textos simples y zonas de práctica. Nivel principal: enemigos, coleccionables, jefe y objetivo.
- No se diseñan escenas aquí. El estudiante define layout y prefabs.

## Victoria, derrota y transiciones
- ObjectiveTracker escucha boss/objetivo; GameResultController muestra paneles.
- LevelFlowController carga escenas por nombres configurados en Inspector y Build Settings.
- Pruebas: derrota por vida 0, victoria por objetivo, botones de reinicio/siguiente.

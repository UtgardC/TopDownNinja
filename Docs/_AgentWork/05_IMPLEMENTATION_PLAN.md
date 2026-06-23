# 05 - Plan de implementación por hitos

> Plan futuro. No se implementó gameplay en esta tarea.

| # | Hito | Objetivo | Scripts a crear | Scripts a modificar | Dependencias | Mecánicas | Conceptos | Wiring | Riesgos/decisiones | Criterio de finalización | Revisión estática | Prueba Unity | Docs |
|---:|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | Contratos y bases | Definir interfaces y enums mínimos | IDamageable, ICollectible | Ninguno | Feedback power-up no bloquea | Contratos | Interfaces | Ninguno | Destructibles opcionales | Firmas claras | Compilación textual | No aplica | 01/04 |
| 2 | Salud, daño y eventos | Vida reutilizable | Health | Ninguno | Hito 1 | daño/curación/muerte | Encaps., Action | asignar Health | Ninguno | Eventos definidos | Suscripciones previstas | Daño manual | 06/08 |
| 3 | Movimiento y combate jugador | Núcleo jugador | PlayerMovement, PlayerAttack | Ninguno | 1-2 | mover/atacar | Métodos | Rigidbody2D, colliders | input | Jugador daña IDamageable | firmas/layers | movimiento/ataque | 06 |
| 4 | Base enemigos | Clase común | EnemyBase | Ninguno | 2-3 | target/ataque común | Herencia | target ref | evitar jerarquía | Base reusable | overrides | No aplica | 04 |
| 5 | Primer enemigo | Melee básico | MeleeEnemy | EnemyBase si hace falta | 4 | persecución/daño | Polimorfismo | collider | balance | Enemigo melee funcional a nivel código | referencias | escena tutorial | STATUS |
| 6 | Checkpoint núcleo | Revisar núcleo | Ninguno | docs | 1-5 | núcleo | trazabilidad | guía | feedback | Pausa de revisión | auditoría | pendiente Unity | todos |
| 7 | Segundo enemigo | Ranged/avanzado | RangedEnemy, Projectile | Ninguno | 4 | proyectiles | Polimorfismo | prefab proyectil | patrón elegido | Ataque distancia | colisiones | probar proyectil | 06/08 |
| 8 | Coleccionables/score | Puntos y comida | ScoreTracker, PlayerCollector, CoinCollectible, FoodCollectible | Ninguno | 1-2 | recolección | interfaces/Action | triggers | valores | Score/curación | eventos | recolectar | 06 |
| 9 | Power-up temporal | Buff limitado | TemporaryPowerUpController, PowerUpCollectible | Player stats/loadout si aplica | decisión BLOCKING | power temporal | Action | UI refs | solución elegida | activa/finaliza/restaura | timer | probar duración | 07 |
| 10 | Pergaminos/habilidades | Habilidades simples | ScrollLoadout, ScrollAbility, FireAbility, WindAbility | PlayerAttack | 9 si integrado | cambiar habilidad | abstracción/herencia | refs | alcance | 1-2 habilidades | cooldown | usar habilidad | 06 |
| 11 | Jefe final | Boss simple | BossEnemy | ObjectiveTracker parcial | 7/10 | boss/fase | override | refs | comportamiento | boss muere | eventos | combate | 08 |
| 12 | Tutorial/progresión/objetivo | Flujo niveles | LevelFlowController, ObjectiveTracker | Ninguno | 8/11 | victoria/transición | Action | scene names | editor | objetivo completo | nombres | cambio escena | 06 |
| 13 | Victoria/derrota/HUD | UI lógica | HUDController, GameResultController | Health/Score subs | 2/8/12 | HUD/end states | eventos | textos/paneles | UI editor | eventos conectados | OnEnable/Disable | paneles | 06/08 |
| 14 | Revisión académica | Trazabilidad final | Ninguno | docs | todos | defensa | todos | no | faltantes | matriz completa | checklist | pendiente | 01/02 |
| 15 | Puntos extra | Solo si conviene | opcionales | mínimos | juego funcional | LINQ/gen/lambda | extra | no | riesgo nota | aislado | removible | probar | 07 |
| 16 | Handoff final | Entrega | Ninguno | docs | todos | todo | todo | guía final | Unity no probado por agente | lista clara | git clean | estudiante | STATUS |

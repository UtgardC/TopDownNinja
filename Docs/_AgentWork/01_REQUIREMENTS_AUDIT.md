# 01 - Auditoría de requisitos

## Requisitos académicos obligatorios

| Requisito | Fuente | Prioridad | Estado | Sistema propuesto | Concepto | Riesgo | Dudas |
|---|---|---:|---|---|---|---|---|
| Clases y objetos | Consigna partes 1 y 2 | Alta | Pendiente de implementación | PlayerController, Health, EnemyBase, ScoreTracker | Clases/objetos | Bajo | Ninguna |
| Métodos con parámetros | Consigna, ejercicios de daño/salud | Alta | Pendiente | TakeDamage(int amount), Heal(int amount), TryUseAbility(Vector2 direction) | Métodos/parámetros | Bajo | Ninguna |
| Métodos con retorno | Consigna, ejercicios CalcularDano/ObtenerCalificacion | Alta | Pendiente | CalculateDamage(...), IsAlive(), CanUse() | Retornos | Bajo | Ninguna |
| Encapsulamiento | Consigna, GET/SET, AGENTS | Alta | Pendiente | campos `[SerializeField] private`, propiedades readonly | Encapsulamiento | Bajo | Ninguna |
| Abstracción | Consigna | Alta | Pendiente | Health abstrae vida; ScrollAbility abstrae habilidad | Abstracción | Medio | Alcance de pergaminos |
| Herencia | Consigna | Alta | Pendiente | EnemyBase -> MeleeEnemy, RangedEnemy, BossEnemy | Herencia | Medio | Evitar jerarquía profunda |
| Polimorfismo | Consigna | Alta | Pendiente | listas/referencias EnemyBase e IDamageable | Polimorfismo | Medio | Ninguna |
| Interfaces | Consigna | Alta | Pendiente | IDamageable, ICollectible | Interfaces | Bajo | Confirmar destructibles |
| Misma interfaz por distintos tipos | Consigna | Alta | Pendiente | IDamageable en player/enemies/destructibles; ICollectible en moneda/comida/pergamino | Interfaces | Bajo | Destructibles opcionales |
| Eventos/delegados Action | Parte 2, Unidad 4 | Alta | Pendiente | OnHealthChanged, OnDied, OnScoreChanged, OnPowerUpStarted | Action | Medio | Alcance UI |

## Requisitos jugables obligatorios

| Requisito | Fuente | Prioridad | Estado | Sistema propuesto | Concepto relacionado | Riesgo | Dudas |
|---|---|---:|---|---|---|---|---|
| Juego Top-Down 2D | Consigna permite plataforma o top-down; AGENTS fija top-down ninja | Alta | Decidido provisional | Movimiento 2D con Rigidbody2D | Clases/Unity | Bajo | Ninguna |
| Movimiento funcional | Consigna | Alta | Pendiente | PlayerMovement | Métodos | Medio | Input exacto editor |
| Salud/vidas | Consigna | Alta | Pendiente | Health | Encapsulamiento/Action | Bajo | Vida o vidas visuales |
| Ataque e interacción con enemigos | Consigna | Alta | Pendiente | PlayerAttack + IDamageable | Interfaces | Medio | Hitbox melee vs proyectil |
| Colisiones | Consigna | Alta | Pendiente Unity | Colliders/Triggers documentados | Unity | Medio | Layers editor |
| Puntuación o coleccionables | Consigna | Alta | Pendiente | ScoreTracker + collectibles | Action | Bajo | Monedas como score |
| Power-up temporal | Consigna | Alta | Pendiente decisión | PowerUpController / ScrollLoadout | Abstracción/Action | Alto | Ver sección power-up |
| Dos enemigos diferentes | Consigna | Alta | Pendiente | MeleeEnemy, RangedEnemy | Herencia | Medio | Visuales/editor |
| Patrones simples | Parte 2 | Alta | Pendiente | Patrol points / chase radius | Métodos | Medio | Complejidad |
| Enemigo avanzado | Parte 2 | Alta | Pendiente | RangedEnemy con disparo o persecución | Polimorfismo | Medio | Elegir persecución/disparo |
| Jefe final y especial | Consigna | Alta | Pendiente | BossEnemy con fases simples | Herencia | Alto | Alcance boss |
| Objetivo, victoria, derrota | Consigna | Alta | Pendiente | ObjectiveTracker, GameResultController | Action | Medio | Condición exacta |
| Tutorial y nivel principal <=5 min | Parte 2 | Alta | Pendiente editor | LevelFlowController + guía wiring | Organización | Medio | Escenas/nombres |
| Unity completo, GitHub, escena jugable | Entrega | Alta | Pendiente humano/editor | Documentación y scripts futuros | Organización | Alto | No validable ahora |

## Opcionales / puntos extra

LINQ, clase genérica `T` y múltiples lambdas son opcionales. Recomendación: no incluir en el núcleo; considerar solo después de juego funcional. Usos naturales: LINQ para enemigo más cercano, genérico `RuntimeCollection<T>` o pool simple, lambdas en suscripción breve; todos son recortables.

## TP2 original

Por ilegibilidad textual del PDF, se separa según elementos mencionados por la tarea y AGENTS:

- Coinciden: ninja top-down, enemigos, jefe, coleccionables, narrativa simple.
- Ayudan: monedas, comida/curación, pergaminos como habilidades, destructibles para `IDamageable`.
- Opcionales: mejoras permanentes, cuatro elementos completos, objetos destructibles extensos.
- Aumentan alcance: árbol completo de habilidades fuego/viento/roca/hielo, economía, upgrades permanentes.
- Deben ajustarse: power-up temporal no debe confundirse con cambio permanente de pergamino.
- Podrían eliminarse: mejoras permanentes y más de dos habilidades si falta tiempo.

## Matriz de trazabilidad

| Consigna | Material | Mecánica | Código propuesto |
|---|---|---|---|
| Interfaces | Interface.pdf y ejemplos | Daño/recolección común | IDamageable, ICollectible |
| Eventos Action | Unidad 4 | UI desacoplada | Health.OnHealthChanged, ScoreTracker.OnScoreChanged |
| Herencia/polimorfismo | POO/relaciones | Enemigos derivados | EnemyBase, MeleeEnemy, RangedEnemy, BossEnemy |
| Métodos con retorno | Ejercicios daño/puntuación | Cálculos | DamageCalculator.CalculateDamage, Ability.CanUse |
| Coleccionables | Consigna/ejemplo Collectible | Monedas/comida/pergaminos | CoinCollectible, FoodCollectible, ScrollCollectible |
| Power-up temporal | Consigna | Buff limitado | TemporaryPowerUpController |
| Victoria/derrota | Consigna | Objetivo final / muerte | GameResultController |

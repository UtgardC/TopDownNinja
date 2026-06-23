# 08 - Plan de pruebas futuro

> Plan para después de implementar. No afirma ejecución.

## Revisión estática

| Prueba | Preparación | Acción | Resultado esperado | Fallos posibles | Sistema | Requisito |
|---|---|---|---|---|---|---|
| Firmas de interfaces | Scripts creados | Revisar IDamageable/ICollectible | Múltiples implementaciones | Interfaz artificial | Daño/recolección | Interfaces |
| Encapsulamiento | Scripts creados | Buscar campos públicos indebidos | `[SerializeField] private` y propiedades | Estado expuesto | Todos | Encapsulamiento |
| Métodos con retorno | Scripts creados | Revisar IsAlive/CanUse/CalculateDamage | Retornos usados naturalmente | Métodos inútiles | Salud/habilidades | Métodos retorno |
| Eventos Action | Scripts creados | Revisar `event Action` y `?.Invoke` | Suscripción/desuscripción OnEnable/OnDisable | Memory leaks/NullReference | UI/sistemas | Eventos |
| Herencia | Enemigos creados | Revisar EnemyBase/overrides | Común en base, diferencias en derivados | Base vacía o gigante | Enemigos | Herencia |
| Dependencias | Scripts creados | Revisar Inspector refs | Sin Find global innecesario | Dependencias ocultas | Todos | Organización |
| Trazabilidad | Docs actualizados | Comparar 01 vs scripts | Cada requisito cubierto | Huecos | Proyecto | Consigna |

## Pruebas dentro de Unity

| Prueba | Preparación | Acción | Resultado esperado | Fallos posibles | Sistema | Requisito |
|---|---|---|---|---|---|---|
| Movimiento | Player con Rigidbody2D | Presionar WASD | Se mueve top-down | velocidad/collider mal | Movimiento | Movimiento funcional |
| Colisiones | Obstáculo con collider | Chocar | No atraviesa | Rigidbody mal | Física | Colisiones |
| Daño | Enemigo con Health | Atacar | Baja vida | Layer mask mal | Combate | Ataque |
| Curación | Player dañado + comida | Recolectar | Sube vida sin pasar max | trigger/ref mal | Salud | Salud |
| Muerte | Vida llega a 0 | Aplicar daño | OnDied, derrota/enemigo muere | evento no dispara | Salud | Derrota/muerte |
| Proyectil | RangedEnemy | Entrar en rango | Dispara y daña | dirección/lifetime | Enemigos | Avanzado |
| Persecución | MeleeEnemy | Acercarse | Persigue/ataca | target null | Enemigos | Patrones |
| Jefe | Boss en nivel | Combatir | Cambia comportamiento y muere | fase no cambia | Boss | Jefe especial |
| Recolección/score | Monedas | Recolectar | Score aumenta/HUD actualiza | suscripción mal | Score | Coleccionables |
| Power-up | PowerUp | Recolectar | Efecto temporal inicia/termina | no restaura | Power-up | Mejora temporal |
| Pergaminos | Scrolls | Cambiar/usar | Habilidad correcta/cooldown | estado confuso | Habilidades | Métodos/abstracción |
| Escenas | Build Settings | Transición | Carga tutorial/principal | nombre mal | Progresión | 2 niveles |
| Victoria | Derrotar boss/objetivo | Completar | Panel victoria | evento no conectado | Resultado | Victoria |
| Derrota | Player muere | Recibir daño | Panel derrota | evento duplicado | Resultado | Derrota |
| HUD | UI referenciada | Cambiar salud/score | Textos correctos | ref null | HUD | Eventos |

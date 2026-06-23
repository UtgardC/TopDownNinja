# 02 - Mapa de conceptos de cursada

> Las definiciones se basan en consigna legible, ejemplos `.cs`, nombres/metadatos de presentaciones y `AGENTS.md`. Cuando un PDF fue parcial, se registra como apoyo parcial.

## Conceptos obligatorios

| Concepto | Definición del material | Documentos | Uso en juego | Clases | Demostración | Explicación oral | Errores comunes | Defensa |
|---|---|---|---|---|---|---|---|---|
| Clases | Moldes para elementos principales. | Consigna, Clase Vs Objeto.pdf, ejercicios | Scripts de jugador/enemigos/coleccionables | PlayerMovement, Health, EnemyBase | Cada archivo define responsabilidades | “Una clase describe qué datos y comportamientos tiene algo.” | Clases gigantes | Qué representa cada clase |
| Objetos | Instancias concretas relacionadas entre sí. | Consigna, Clase Vs Objeto.pdf | GameObjects con scripts | Player, enemigos, monedas | Varios enemigos usan la misma clase | “El objeto es el enemigo real en escena.” | Confundir clase con prefab | Instancias en Unity |
| Métodos | Acciones que ejecuta una clase. | Métodos.pdf, ejercicios | TakeDamage, Heal, Collect | Todas | Separan acciones | “Un método es una acción con nombre.” | Todo en Update | Por qué cada método existe |
| Parámetros | Datos de entrada a un método. | Ejercicios salud/daño | cantidad de daño, dirección | Health, PlayerAttack | TakeDamage(int amount) | “Le digo cuánto daño aplicar.” | Hardcodear valores | Parámetro vs campo |
| Retornos | Resultado devuelto por método. | Ejercicios CalcularDano, ObtenerCalificacion | IsAlive, CalculateDamage | Health, DamageCalculator | Devuelven bool/int | “El método responde una pregunta.” | Usar void siempre | Qué se calcula |
| Encapsulamiento | Lógica interna privada y métodos públicos adecuados. | Consigna, GET and SET.pdf, AGENTS | `[SerializeField] private`, propiedades | Health, ScoreTracker | Estado no se modifica libremente | “Protejo la vida para que no pase límites.” | Campos públicos | Setters controlados |
| Abstracción | Mostrar solo lo necesario de un objeto. | Consigna | Health oculta clamp; interfaces ocultan implementación | Health, ScrollAbility | API simple | “Uso una idea general: dañable.” | Abstraer de más | Qué se oculta |
| Herencia | Base común extendida por derivados. | Consigna, Fundamento POO.pdf | EnemyBase común | Melee/Ranged/Boss | Código compartido | “Todos son enemigos, cada uno actúa distinto.” | Jerarquía profunda | Miembros comunes |
| Polimorfismo | Mismo contrato/base, comportamientos distintos. | Consigna | EnemyBase.Attack() virtual; IDamageable | Enemigos, destructibles | Llamada común ejecuta variante | “Le pido atacar y cada enemigo lo hace a su manera.” | If gigante por tipo | Virtual/override |
| Interfaces | Contrato común entre distintos objetos. | Consigna, Interface.pdf | IDamageable/ICollectible | Player, Enemy, Destructible, Coin | Desacopla ataque/recolección | “No importa qué es, importa que pueda recibir daño.” | Interfaz de un solo uso | Implementaciones distintas |
| Eventos | Notificaciones desacopladas. | Parte 2, Unidad 4 | salud, score, muerte | Health, ScoreTracker, UI | Publican cambios sin conocer UI | “Aviso que cambió la vida.” | Usar eventos para todo | Suscripción/desuscripción |
| Delegados | Referencias a métodos compatibles. | Delegados.pdf, EventComparison | Base de Action | Eventos Action | `Action<float>` transporta datos | “Action guarda métodos a llamar.” | Invocar sin `?.` | Firma del Action |
| Action | Delegado genérico de C#. | EventComparison, ejercicios | OnHealthChanged, OnCollected | Health, Collectible | Menos clases que UnityEvent | “Action permite eventos de código puro.” | No desuscribirse | Por qué no UnityEvent |

## Puntos extra

| Concepto | Uso posible | Recomendación |
|---|---|---|
| LINQ | Consultar enemigo más cercano o filtrar power-ups activos. | Considerar solo al final si no complica. |
| Genéricos `T` | Colección reusable `RuntimeList<T>`. | Riesgo medio; omitir salvo necesidad real. |
| Lambdas | Suscripciones simples o filtros LINQ. | No usar artificialmente; explicar sintaxis si se adopta. |

## Aclaraciones técnicas adicionales

Unity `MonoBehaviour` no elimina la POO: cada componente sigue siendo una clase. `ScriptableObject` no es obligatorio y no se propone para el núcleo porque agregaría una capa de assets a configurar.

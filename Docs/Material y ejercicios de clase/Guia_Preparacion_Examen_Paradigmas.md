# Guia de preparacion para examen - Paradigmas de programacion

Material armado a partir de los PDFs, ejercicios y resoluciones presentes en esta carpeta.
El enfoque esta pensado para un examen virtual con parte teorica y parte practica en C# / Unity.

## Como usar esta guia

Si tenes poco tiempo, estudiala en este orden:

1. Lee el resumen de cada unidad.
2. Practica escribir de memoria los esqueletos de codigo de la seccion "Plantillas clave".
3. Hace el simulacro de examen sin mirar respuestas.
4. Revisa la checklist final y los errores tipicos.

La prioridad practica es: metodos con parametros y retorno, POO, interfaces, eventos/delegados, LINQ/lambdas y refactorizacion.

---

## Mapa general de temas

| Unidad | Tema central | Que te pueden pedir |
|---|---|---|
| 1 | POO basica, clases, objetos, atributos, metodos, getters/setters | Explicar clase vs objeto, crear metodos con parametros y retorno, controlar estado con variables de clase |
| 2 | Pilares de POO y relaciones entre clases | Distinguir encapsulamiento, herencia, abstraccion, polimorfismo; elegir dependencia/asociacion/agregacion/composicion |
| 3 | Interfaces | Crear contratos (`IInteractuable`, `IReciboDano`) y usarlos para tratar objetos distintos de forma uniforme |
| 4 | Delegados, eventos, `UnityEvent`, `event Action` | Implementar patron observador, suscribir/desuscribir en `OnEnable`/`OnDisable`, invocar eventos sin acoplar UI y logica |
| 5 | Genericos, LINQ, lambdas, refactorizacion | Usar `List<T>`, clases genericas, `Where`, `Select`, `OrderBy`, `FirstOrDefault`, lambdas y criterios de codigo limpio |

---

## Unidad 1 - POO, clases, objetos y metodos

### Paradigmas de programacion

Un paradigma es un conjunto de reglas, ideas y herramientas para pensar y construir programas.

Los vistos en el material:

- Imperativo: describe paso a paso que hace el programa. Usa variables mutables, `if`, `for`, `while`.
- Funcional: favorece funciones puras, inmutabilidad y transformaciones como `map`, `filter`, `reduce`.
- Logico: declara hechos y reglas, y luego consulta relaciones.
- Dirigido por eventos: el flujo depende de eventos y listeners.
- Orientado a objetos: el sistema se construye con objetos que tienen estado, comportamiento e identidad.

En Unity se mezclan varios paradigmas: se escribe C# imperativo, se modelan scripts como objetos, se usan eventos para comunicar sistemas y se pueden usar funciones/lambdas con LINQ.

### Clase vs objeto

Una clase es el molde. Define que datos y comportamientos tendran sus objetos.

Un objeto es una instancia concreta creada a partir de una clase en tiempo de ejecucion.

Ejemplo:

```csharp
public class Heroe : MonoBehaviour
{
    public float speed;
    public int life;
    public float damage;

    public void Jump()
    {
        Debug.Log("El heroe salta");
    }

    public void Attack()
    {
        Debug.Log("El heroe ataca");
    }
}
```

La clase `Heroe` define atributos y metodos. Cada heroe concreto en escena es un objeto con su propio estado.

### Atributos, estado e identidad

Un objeto tiene:

- Estado: valores internos actuales, por ejemplo `vidaActual`, `pesoActual`, `puntos`.
- Comportamiento: metodos que sabe ejecutar, por ejemplo `RecibirDano`, `Moverse`, `Atacar`.
- Identidad: aunque dos objetos tengan el mismo estado, siguen siendo dos objetos distintos.

### Metodos

Un metodo es un bloque de codigo que se ejecuta cuando se llama.

```csharp
void Saludar()
{
    Debug.Log("Hola");
}
```

Con parametros:

```csharp
void Saludar(string nombre, int edad)
{
    Debug.Log($"Hola {nombre}, edad: {edad}");
}
```

Con retorno:

```csharp
int Sumar(int a, int b)
{
    return a + b;
}
```

Reglas importantes:

- Si el metodo es `void`, no devuelve un valor.
- Si el metodo dice `int`, `float`, `string`, `bool`, etc., debe tener `return` compatible.
- El orden y tipo de parametros importa.
- Las variables locales viven solo dentro del metodo.
- Las variables de clase conservan su valor entre llamadas.

### Getters, setters y propiedades

Sirven para controlar acceso a datos internos.

```csharp
private int vida;

public int Vida
{
    get { return vida; }
    set { vida = value; }
}
```

Version abreviada:

```csharp
private int vida;
public int Vida { get => vida; set => vida = value; }
```

Version de solo lectura desde afuera:

```csharp
[SerializeField] private int vidaMaxima = 100;
private int vidaActual;

public int VidaActual => vidaActual;
```

En Unity es muy comun usar:

```csharp
[SerializeField] private int vidaMaxima = 100;
```

Esto permite editar desde Inspector sin hacer publico el campo.

### Practica base de Unidad 1

Los ejercicios de la unidad trabajan estos patrones:

- Cofre: metodo `void` con parametro `string`.
- Salud: metodo con `int` y `bool`, estado global y `Mathf.Max`.
- Proyectiles: metodo que calcula y retorna, otro metodo que aplica.
- Inventario: metodo `bool` para decidir si se puede recoger.
- Puntuacion: separar calculo, clasificacion y visualizacion.

Patron recomendado:

```csharp
int CalcularDano(int danoBase, float multiplicador, int resistencia)
{
    int resultado = Mathf.RoundToInt(danoBase * multiplicador) - resistencia;
    return Mathf.Max(1, resultado);
}

void AplicarDano(string enemigo, int dano)
{
    Debug.Log($"{enemigo} recibe {dano} de dano");
}
```

---

## Unidad 2 - Pilares de POO y relaciones entre clases

### Encapsulamiento

Consiste en guardar datos y logica dentro de una clase, evitando que otros scripts modifiquen el estado de cualquier manera.

Ejemplo:

```csharp
public class Jugador : MonoBehaviour
{
    [SerializeField] private int vidaMaxima = 100;
    private int vidaActual;

    public int VidaActual => vidaActual;

    private void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDano(int cantidad)
    {
        vidaActual = Mathf.Max(0, vidaActual - cantidad);
        if (vidaActual == 0) Morir();
    }

    public void Curar(int cantidad)
    {
        vidaActual = Mathf.Min(vidaMaxima, vidaActual + cantidad);
    }

    private void Morir()
    {
        gameObject.SetActive(false);
    }
}
```

Idea clave: desde afuera no se asigna `vidaActual` directamente. Se pide una accion: `RecibirDano` o `Curar`.

### Herencia

Permite crear clases hijas que reutilizan atributos y comportamientos de una clase padre.

```csharp
public class Enemigo : MonoBehaviour
{
    protected float vida;
    protected float velocidad;

    public virtual void Moverse()
    {
        transform.Translate(Vector3.left * velocidad * Time.deltaTime);
    }
}

public class Goomba : Enemigo
{
    private void Start()
    {
        vida = 1f;
        velocidad = 2f;
    }
}
```

Usa herencia cuando realmente existe una relacion "es un": `Goomba` es un `Enemigo`.

### Abstraccion

Es quedarse con lo relevante para el problema y ocultar detalles innecesarios.

Una clase abstracta no se instancia directamente; sirve como base para clases concretas.

```csharp
public abstract class Arma : MonoBehaviour
{
    protected float dano;
    protected float cooldown;

    public abstract void Disparar();
}

public class Pistola : Arma
{
    public override void Disparar()
    {
        Debug.Log("Disparo simple");
    }
}
```

### Polimorfismo

Permite tratar distintos objetos a traves de un mismo tipo base y que cada uno ejecute su propia version del metodo.

```csharp
public class Enemigo : MonoBehaviour
{
    public virtual void Atacar()
    {
        Debug.Log("Ataque base");
    }
}

public class Planta : Enemigo
{
    public override void Atacar()
    {
        Debug.Log("Mordisco");
    }
}

public class Bill : Enemigo
{
    public override void Atacar()
    {
        Debug.Log("Lanza proyectil");
    }
}
```

Uso polimorfico:

```csharp
foreach (Enemigo enemigo in enemigos)
{
    enemigo.Atacar();
}
```

No hace falta un `if` o `switch` preguntando el tipo concreto.

### Relaciones entre clases

| Relacion | Idea | Ejemplo Unity | Fuerza |
|---|---|---|---|
| Dependencia | Usa a otra clase puntualmente, no la guarda | Buscar `ScoreManager` para sumar puntos | Debil |
| Asociacion | Guarda una referencia constante | Camara con `Transform target` | Media |
| Agregacion | Tiene partes que pueden existir sin el todo | Personaje con `List<Item>` | Media-fuerte |
| Composicion | La parte depende del ciclo de vida del todo | Boss con componentes internos/prefab | Fuerte |
| Implementacion | Una clase cumple una interfaz | `Puerta : IInteractuable` | Contrato |
| Herencia | Una clase hija extiende a una padre | `Goomba : Enemigo` | Jerarquica |

Regla rapida:

- Si lo usa una vez: dependencia.
- Si lo consulta seguido: asociacion.
- Si lo lleva pero puede soltarlo: agregacion.
- Si forma parte de su existencia: composicion.
- Si comparte contrato: interfaz.
- Si realmente "es un": herencia.

### UML y diagramas

UML es un lenguaje visual para modelar sistemas. Un diagrama de clases muestra clases, atributos, metodos y relaciones.

Sirve para:

- Planificar antes de programar.
- Evitar codigo demasiado acoplado.
- Comunicar decisiones de diseno.
- Documentar como se conectan scripts.

En Unity, si una clase tiene muchas flechas saliendo hacia otras, probablemente esta haciendo demasiadas cosas.

---

## Unidad 3 - Interfaces

Una interfaz define un contrato. Toda clase que la implementa debe proveer los metodos y propiedades que la interfaz declara.

Convencion: el nombre suele empezar con `I`.

```csharp
public interface IInteractuable
{
    void Accion();
}
```

Implementacion:

```csharp
public class Cofre : MonoBehaviour, IInteractuable
{
    public void Accion()
    {
        Debug.Log("Abrir cofre");
    }
}
```

Uso desde otro script:

```csharp
private void IntentarInteractuar(GameObject objetivo)
{
    IInteractuable interactuable = objetivo.GetComponent<IInteractuable>();

    if (interactuable != null)
    {
        interactuable.Accion();
    }
}
```

Con operador moderno:

```csharp
if (objetivo.TryGetComponent(out IInteractuable interactuable))
{
    interactuable.Accion();
}
```

### Cuando conviene una interfaz

Conviene cuando objetos distintos deben responder a la misma accion sin compartir una clase padre util.

Ejemplos:

- `Cofre`, `Puerta`, `NPC` pueden implementar `IInteractuable`.
- `Enemigo`, `Caja`, `Jugador` pueden implementar `IReciboDano`.

```csharp
public interface IReciboDano
{
    void RecibirDano(int cantidad);
}
```

Ventaja principal: el jugador no necesita saber si golpeo un enemigo, una caja o un boss. Solo pregunta si el objeto recibe dano.

---

## Unidad 4 - Delegados, eventos y UnityEvent

### Delegate

Un delegado representa una referencia a un metodo con una firma compatible.

```csharp
public delegate int Operacion(int a, int b);

int Sumar(int a, int b)
{
    return a + b;
}

void Start()
{
    Operacion operacion = Sumar;
    Debug.Log(operacion(2, 3));
}
```

El metodo asignado debe coincidir en:

- Cantidad de parametros.
- Tipo de parametros.
- Tipo de retorno.

### `Action`

`Action` es un delegado generico para metodos sin retorno.

```csharp
using System;

public event Action OnPlayerDead;
public event Action<float> OnHealthChanged;
```

Se invoca de forma segura con `?.Invoke()`:

```csharp
OnHealthChanged?.Invoke(vidaActual);
```

### Patron observador con eventos

El objeto que emite no necesita conocer todos los objetos que reaccionan.

Ejemplo salud:

```csharp
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    public event Action<float> OnHealthChanged;

    public float Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0f, 100f);
            OnHealthChanged?.Invoke(health);
        }
    }

    public void TakeDamage(float amount) => Health -= amount;
    public void Heal(float amount) => Health += amount;
}
```

Script que escucha:

```csharp
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI healthText;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateDisplay;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateDisplay;
    }

    private void UpdateDisplay(float health)
    {
        healthText.text = $"Salud: {health:F0}";
    }
}
```

Regla de examen: si te suscribis con `+=`, tambien tenes que desuscribirte con `-=`.

### `UnityEvent` vs `event Action`

| Criterio | UnityEvent | event Action |
|---|---|---|
| Inspector | Visible y conectable por disenadores | No visible |
| Codigo | Usa `AddListener` / `RemoveListener` | Usa `+=` / `-=` |
| Parametros | Para parametros custom suele requerir clase serializable | `Action<T>`, `Action<T1,T2>` directo |
| Rendimiento | Mas lento por serializacion/reflection | Mas directo y rapido |
| Mejor uso | UI, botones, animaciones, conexiones desde escena | Logica de sistemas, salud, inventario, estados |

Ejemplo con `UnityEvent`:

```csharp
using UnityEngine;
using UnityEngine.Events;

public class BotonDePuerta : MonoBehaviour
{
    public UnityEvent OnPressed;

    public void Press()
    {
        OnPressed.Invoke();
    }
}
```

### Eventos tipicos de la materia

- Salud: `Action<float> OnHealthChanged`.
- Recoleccion: `Action OnCollected`.
- Estado de juego: `Action OnGameStart`, `Action OnGamePause`.
- Cambio de nivel: `Action<int> OnLevelChanged`.
- NPC: `Action<string> OnInteract`.

---

## Unidad 5 - Genericos, LINQ, lambdas y refactorizacion

### Genericos

Los genericos permiten escribir clases o metodos reutilizables manteniendo seguridad de tipos.

```csharp
public class GenericManager<T>
{
    private List<T> items = new List<T>();

    public void AddItem(T item)
    {
        items.Add(item);
    }

    public void RemoveItem(T item)
    {
        items.Remove(item);
    }

    public List<T> GetItems()
    {
        return new List<T>(items);
    }
}
```

Uso:

```csharp
GenericManager<Enemy> enemyManager = new GenericManager<Enemy>();
GenericManager<Item> itemManager = new GenericManager<Item>();
```

Ventajas:

- Evita duplicar clases iguales para tipos distintos.
- Detecta errores de tipo en compilacion.
- Reduce conversiones innecesarias.
- Hace el codigo mas mantenible.

### Expresiones lambda

Una lambda es una funcion anonima.

Sintaxis:

```csharp
(parametros) => expresion
```

Ejemplos:

```csharp
Func<int, bool> mayorQueDiez = numero => numero > 10;
Debug.Log(mayorQueDiez(15)); // true
```

En Unity:

```csharp
button.onClick.AddListener(() => CambiarColor(Color.red));
```

Ojo: si necesitas desuscribirte luego, no conviene usar una lambda anonima inline, porque no tenes una referencia facil para removerla.

### LINQ

LINQ permite consultar colecciones de forma declarativa.

Necesita:

```csharp
using System.Linq;
```

Ejemplos clave:

```csharp
List<Enemy> debiles = enemies
    .Where(e => e.Health < 50)
    .ToList();
```

```csharp
List<Enemy> ordenados = enemies
    .OrderBy(e => e.EnemyName)
    .ToList();
```

```csharp
List<string> nombres = enemies
    .Select(e => e.EnemyName)
    .ToList();
```

```csharp
Enemy primeroFuerte = enemies
    .FirstOrDefault(e => e.Health > 100);

if (primeroFuerte != null)
{
    Debug.Log(primeroFuerte.EnemyName);
}
```

```csharp
int cantidadDebiles = enemies.Count(e => e.Health < 50);
```

```csharp
List<string> todosLosNombres = enemyNames
    .Concat(itemNames)
    .ToList();
```

Uso tipico en juegos:

- Filtrar enemigos por vida.
- Ordenar inventario.
- Obtener nombres de objetos.
- Encontrar el primer objetivo valido.
- Contar elementos que cumplen una condicion.

### Refactorizacion

Refactorizar es mejorar estructura interna sin cambiar comportamiento externo.

Objetivo: reducir deuda tecnica.

Codigo limpio:

- Es obvio de leer.
- No duplica logica.
- Tiene clases y metodos con responsabilidades claras.
- Pasa pruebas.
- No contiene codigo muerto.

Cuándo refactorizar:

- Al agregar una caracteristica.
- Al corregir un bug.
- Durante revision de codigo.
- Regla de tres: la tercera vez que repetis una logica, extraela.

Malos olores y soluciones:

| Problema | Sintoma | Refactor posible |
|---|---|---|
| Metodo largo | Muchas tareas mezcladas | Extraer metodo |
| Demasiados parametros | Firma dificil de leer | Usar objeto como parametro |
| Clase larga | Muchas responsabilidades | Extraer clase |
| Modificaciones fragmentadas | Un cambio toca muchas clases | Reorganizar responsabilidades |
| Condicional complejo | `if`/`switch` dificil de leer | Descomponer condicional |
| Switch por tipo | Cada nuevo tipo obliga a tocar switch | Polimorfismo |
| Codigo muerto | Codigo que no se usa | Eliminar |
| Mala jerarquia | Subclase no usa lo heredado | Cambiar herencia, extraer superclase/interfaz |
| Campos temporales | Campos solo usados por un algoritmo | Extraer clase o usar objeto de contexto |

---

## Plantillas clave para memorizar

### Metodo con retorno usado por otro metodo

```csharp
int CalcularPuntaje(int enemigos, int tiempoRestante, float bonus)
{
    return Mathf.RoundToInt((enemigos * 100 + tiempoRestante * 10) * bonus);
}

string ObtenerRango(int puntaje)
{
    if (puntaje >= 5000) return "S";
    if (puntaje >= 3500) return "A";
    if (puntaje >= 2000) return "B";
    if (puntaje >= 1000) return "C";
    return "D";
}

void MostrarResultado(string jugador, int puntaje, string rango)
{
    Debug.Log($"{jugador}: {puntaje} - Rango {rango}");
}
```

### Interface de interaccion

```csharp
public interface IInteractuable
{
    void Interactuar();
}

public class Puerta : MonoBehaviour, IInteractuable
{
    public void Interactuar()
    {
        Debug.Log("Abrir puerta");
    }
}
```

### Interface de dano

```csharp
public interface IReciboDano
{
    void RecibirDano(int cantidad);
}

public class Enemigo : MonoBehaviour, IReciboDano
{
    [SerializeField] private int vida = 100;

    public void RecibirDano(int cantidad)
    {
        vida = Mathf.Max(0, vida - cantidad);
    }
}
```

### Clase abstracta + polimorfismo

```csharp
public abstract class Personaje : MonoBehaviour
{
    [SerializeField] private int vida;
    public int Vida => vida;

    public abstract void Accion();
}

public class Guerrero : Personaje
{
    public override void Accion()
    {
        Debug.Log("Ataque fisico");
    }
}

public class Mago : Personaje
{
    public override void Accion()
    {
        Debug.Log("Lanzar hechizo");
    }
}
```

### Evento con `Action<T>`

```csharp
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public event Action<int> OnLevelChanged;
    private int currentLevel = 1;

    public void LoadNextLevel()
    {
        currentLevel++;
        OnLevelChanged?.Invoke(currentLevel);
    }
}
```

### Suscriptor de evento

```csharp
public class LevelDisplay : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    private void OnEnable()
    {
        levelManager.OnLevelChanged += UpdateDisplay;
    }

    private void OnDisable()
    {
        levelManager.OnLevelChanged -= UpdateDisplay;
    }

    private void UpdateDisplay(int level)
    {
        Debug.Log($"Nivel {level}");
    }
}
```

### LINQ sobre enemigos

```csharp
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyQuery : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies;

    private void Start()
    {
        List<Enemy> debiles = enemies.Where(e => e.Health < 50).ToList();
        Enemy boss = enemies.FirstOrDefault(e => e.Health > 100);
        List<string> nombres = enemies.Select(e => e.EnemyName).ToList();
        int cantidadDebiles = enemies.Count(e => e.Health < 50);
    }
}
```

---

## Preguntas teoricas probables

### 1. Diferencia entre clase y objeto

Una clase es la definicion o molde. Un objeto es una instancia concreta creada a partir de esa clase. La clase define atributos y metodos; cada objeto tiene valores propios para esos atributos.

### 2. Diferencia entre variable local y variable de clase

Una variable local existe solo dentro del metodo y se pierde al terminar la llamada. Una variable de clase pertenece al objeto y mantiene su valor entre llamadas.

### 3. Por que usar `private` + propiedad publica

Para encapsular el estado. Desde afuera se puede leer o pedir acciones, pero no modificar cualquier valor arbitrario sin validacion.

### 4. Por que un metodo `int` debe tener `return`

Porque la firma promete devolver un entero. Si algun camino del metodo no retorna valor, C# da error de compilacion.

### 5. Para que sirve una interfaz

Para definir un contrato comun entre clases distintas. Permite usar objetos diferentes de forma uniforme sin depender de su clase concreta.

### 6. Diferencia entre clase abstracta e interfaz

Una clase abstracta puede contener estado, logica compartida y metodos abstractos. Una interfaz define principalmente un contrato que varias clases pueden implementar. En C#, una clase hereda de una sola clase base, pero puede implementar varias interfaces.

### 7. Que es polimorfismo

Es tratar objetos de distintos tipos concretos como un tipo comun y ejecutar el comportamiento correcto segun el tipo real en tiempo de ejecucion.

### 8. Por que usar eventos

Para comunicar sistemas sin acoplarlos directamente. El emisor avisa que algo ocurrio; los interesados se suscriben y reaccionan.

### 9. Por que suscribirse en `OnEnable` y desuscribirse en `OnDisable`

Porque evita referencias colgadas, llamadas a objetos destruidos o duplicacion de suscripciones cuando el objeto se activa/desactiva.

### 10. Diferencia entre `UnityEvent` y `event Action`

`UnityEvent` se ve en Inspector y sirve para conectar elementos desde escena. `event Action` es codigo C# directo, mas limpio y eficiente para logica interna.

### 11. Para que sirven genericos

Para escribir codigo reutilizable para distintos tipos sin perder seguridad de tipos. Ejemplo: `GenericManager<T>` puede manejar `Enemy`, `Item` o cualquier otro tipo.

### 12. Que hace `Where` en LINQ

Filtra una coleccion segun una condicion y devuelve los elementos que la cumplen.

### 13. Que hace `FirstOrDefault`

Devuelve el primer elemento que cumple una condicion o `null` si no encuentra ninguno.

### 14. Que es una lambda

Una funcion anonima de sintaxis corta, usada por ejemplo en LINQ o eventos: `e => e.Health < 50`.

### 15. Que significa refactorizar

Modificar la estructura interna del codigo para hacerlo mas claro, mantenible y limpio, sin cambiar su comportamiento observable.

---

## Errores tipicos a evitar

- Declarar un metodo `int` y olvidarse del `return`.
- Invertir parametros: `ModificarSalud(true, 50)` cuando la firma espera `(int, bool)`.
- Hacer publica una variable que deberia estar encapsulada.
- Usar `GetComponent` dentro de `Update` sin necesidad.
- Suscribirse a un evento y nunca desuscribirse.
- Invocar un `event Action` sin `?.Invoke()` cuando podria no tener suscriptores.
- Usar herencia cuando una interfaz o composicion seria mas clara.
- Resolver polimorfismo con un `switch` por tipo en vez de `virtual`/`override`.
- Modificar directamente la lista interna de una clase generica en vez de devolver copia.
- No verificar `null` despues de `FirstOrDefault`.
- Escribir metodos que hacen demasiadas cosas.

---

## Practica guiada

### Ejercicio A - Inventario con peso

Crear un script `InventarioJugador`:

- `pesoActual`
- `pesoMaximo`
- metodo `bool HayEspacio(float peso)`
- metodo `void RecogerObjeto(string nombre, float peso)`
- si entra, suma peso y muestra mensaje
- si no entra, avisa inventario lleno

Punto evaluable: retorno `bool` usado para controlar flujo.

Esqueleto:

```csharp
public class InventarioJugador : MonoBehaviour
{
    private float pesoActual = 0f;
    [SerializeField] private float pesoMaximo = 20f;

    private bool HayEspacio(float peso)
    {
        return pesoActual + peso <= pesoMaximo;
    }

    private void RecogerObjeto(string nombre, float peso)
    {
        if (!HayEspacio(peso))
        {
            Debug.Log($"No puedes cargar {nombre}");
            return;
        }

        pesoActual += peso;
        Debug.Log($"Recogiste {nombre}. Peso: {pesoActual}/{pesoMaximo}");
    }
}
```

### Ejercicio B - Sistema de dano con interfaz

Crear `IReciboDano` y dos clases que lo implementen: `Enemigo` y `CajaRompible`.

Objetivo: un proyectil debe aplicar dano sin saber que tipo de objeto golpeo.

```csharp
public interface IReciboDano
{
    void RecibirDano(int cantidad);
}

public class Proyectil : MonoBehaviour
{
    [SerializeField] private int dano = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IReciboDano objetivo))
        {
            objetivo.RecibirDano(dano);
            Destroy(gameObject);
        }
    }
}
```

### Ejercicio C - Evento de salud

Crear `PlayerHealth` que emita `OnHealthChanged` cada vez que cambia la salud, y `HealthDisplay` que escuche.

Punto evaluable: `Action<float>`, `?.Invoke`, `+=`, `-=`.

### Ejercicio D - LINQ de enemigos

Dada una lista de enemigos:

- obtener los que tienen menos de 50 de vida
- ordenar por nombre
- contar los debiles
- buscar el primero con vida mayor a 100

```csharp
List<Enemy> debiles = enemies.Where(e => e.Health < 50).ToList();
List<Enemy> ordenados = enemies.OrderBy(e => e.EnemyName).ToList();
int cantidadDebiles = enemies.Count(e => e.Health < 50);
Enemy tanque = enemies.FirstOrDefault(e => e.Health > 100);
```

### Ejercicio E - Refactorizacion

Codigo problematico:

```csharp
void Update()
{
    if (tipo == "Goblin") Debug.Log("Ataca con cuchillo");
    else if (tipo == "Dragon") Debug.Log("Escupe fuego");
    else if (tipo == "Mago") Debug.Log("Lanza hechizo");
}
```

Refactor esperado: crear clase base `Enemigo` con metodo `Atacar()` virtual o abstracto y subclases `Goblin`, `Dragon`, `Mago` con `override`.

---

## Simulacro de examen

Tiempo sugerido: 90 minutos.

### Parte 1 - Teoria corta

Responder en 3 a 6 lineas cada una:

1. Explica clase vs objeto con un ejemplo de Unity.
2. Explica encapsulamiento y por que `[SerializeField] private` puede ser mejor que `public`.
3. Diferencia entre herencia, interfaz y composicion.
4. Explica que problema resuelven los eventos en Unity.
5. Diferencia entre `UnityEvent` y `event Action`.
6. Que hace `Where`, `Select` y `FirstOrDefault` en LINQ.
7. Que es una lambda y por que aparece tanto en LINQ.
8. Da un ejemplo de refactorizacion para reemplazar un `switch`.

### Parte 2 - Practica C# / Unity

Crear un mini sistema de combate con estos requisitos:

1. Interfaz `IReciboDano` con `void RecibirDano(int cantidad)`.
2. Clase abstracta `Personaje` con vida encapsulada, propiedad de lectura `VidaActual` y metodo abstracto `Accion()`.
3. Clases `Guerrero` y `Mago` que hereden de `Personaje` y sobrescriban `Accion()`.
4. Clase `Proyectil` que al colisionar busque `IReciboDano` y aplique dano.
5. Evento `Action<int> OnVidaCambiada` que avise la nueva vida.
6. Script `VidaUI` que se suscriba al evento en `OnEnable` y se desuscriba en `OnDisable`.
7. Una lista de personajes y una consulta LINQ que encuentre los que tienen menos de 30 de vida.

### Criterios de correccion

| Criterio | Puntos |
|---|---:|
| Interfaz correctamente declarada e implementada | 15 |
| Encapsulamiento de vida y validacion con limites | 15 |
| Uso correcto de herencia/abstract/override | 15 |
| Evento con `Action<int>` invocado de forma segura | 15 |
| Suscripcion/desuscripcion correcta | 15 |
| LINQ correcto con lambda | 10 |
| Codigo claro, responsabilidades separadas | 15 |

---

## Solucion orientativa del simulacro

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public interface IReciboDano
{
    void RecibirDano(int cantidad);
}

public abstract class Personaje : MonoBehaviour, IReciboDano
{
    [SerializeField] private int vidaMaxima = 100;
    private int vidaActual;

    public int VidaActual => vidaActual;
    public event Action<int> OnVidaCambiada;

    protected virtual void Start()
    {
        vidaActual = vidaMaxima;
        OnVidaCambiada?.Invoke(vidaActual);
    }

    public void RecibirDano(int cantidad)
    {
        vidaActual = Mathf.Max(0, vidaActual - cantidad);
        OnVidaCambiada?.Invoke(vidaActual);

        if (vidaActual == 0)
        {
            Morir();
        }
    }

    protected virtual void Morir()
    {
        gameObject.SetActive(false);
    }

    public abstract void Accion();
}

public class Guerrero : Personaje
{
    public override void Accion()
    {
        Debug.Log("Guerrero ataca con espada");
    }
}

public class Mago : Personaje
{
    public override void Accion()
    {
        Debug.Log("Mago lanza hechizo");
    }
}

public class Proyectil : MonoBehaviour
{
    [SerializeField] private int dano = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IReciboDano objetivo))
        {
            objetivo.RecibirDano(dano);
            Destroy(gameObject);
        }
    }
}

public class VidaUI : MonoBehaviour
{
    [SerializeField] private Personaje personaje;
    [SerializeField] private TextMeshProUGUI vidaText;

    private void OnEnable()
    {
        personaje.OnVidaCambiada += Actualizar;
    }

    private void OnDisable()
    {
        personaje.OnVidaCambiada -= Actualizar;
    }

    private void Actualizar(int vida)
    {
        vidaText.text = $"Vida: {vida}";
    }
}

public class ConsultaPersonajes : MonoBehaviour
{
    [SerializeField] private List<Personaje> personajes;

    private void Start()
    {
        List<Personaje> heridos = personajes
            .Where(p => p.VidaActual < 30)
            .ToList();

        foreach (Personaje personaje in heridos)
        {
            Debug.Log($"{personaje.name} esta herido");
        }
    }
}
```

---

## Checklist final antes del examen

Podes resolver el examen si podes hacer esto sin mirar:

- Crear una clase `MonoBehaviour` con atributos, `Start()` y metodos.
- Decidir si un metodo debe ser `void`, `int`, `bool`, `string`, etc.
- Usar `return` correctamente.
- Usar una variable de clase para conservar estado.
- Encapsular con `private`, `[SerializeField]` y propiedad publica de lectura.
- Crear una interfaz y usar `GetComponent<IInterfaz>()` o `TryGetComponent`.
- Crear clase base abstracta y subclases con `override`.
- Explicar polimorfismo con un `foreach` sobre un array/lista del tipo base.
- Declarar `event Action`, invocarlo con `?.Invoke()` y escuchar con `+=`.
- Desuscribirte con `-=`.
- Elegir entre `UnityEvent` y `event Action`.
- Usar `Where`, `Select`, `OrderBy`, `FirstOrDefault`, `Count`.
- Identificar un metodo largo, clase larga o `switch` reemplazable por polimorfismo.

---

## Mini chuleta de sintaxis

```csharp
// Metodo void
void HacerAlgo() { }

// Metodo con retorno
int Calcular() { return 10; }

// Propiedad solo lectura
public int Vida => vida;

// Interface
public interface IAlgo { void Ejecutar(); }

// Implementar interface
public class MiClase : MonoBehaviour, IAlgo
{
    public void Ejecutar() { }
}

// Abstract + override
public abstract class Base { public abstract void Accion(); }
public class Hija : Base { public override void Accion() { } }

// Evento
public event Action<int> OnCambio;
OnCambio?.Invoke(5);

// Suscripcion
emisor.OnCambio += Metodo;
emisor.OnCambio -= Metodo;

// Lambda
x => x > 10

// LINQ
lista.Where(x => x.Activo).ToList();
```

---

## Recomendacion de estudio por dias

### Dia 1

Unidad 1 y 2. Reescribi de memoria:

- metodo con retorno
- inventario con `bool HayEspacio`
- clase abstracta + dos hijas
- diferencia dependencia/asociacion/agregacion/composicion

### Dia 2

Unidad 3 y 4. Reescribi de memoria:

- `IInteractuable`
- `IReciboDano`
- evento de salud con `Action<float>`
- suscriptor UI con `OnEnable`/`OnDisable`

### Dia 3

Unidad 5 y simulacro. Reescribi:

- `GenericManager<T>`
- consultas LINQ
- refactor de `switch` a polimorfismo
- simulacro completo en 90 minutos


using System;
using UnityEngine;
using UnityEngine.Events;

// ============================================================
// COMPARACIÓN: UnityEvent vs event Action
// ============================================================

public class EventComparison : MonoBehaviour
{
    // ----------------------------------------------------------
    // 1. VISIBILIDAD EN INSPECTOR
    // ----------------------------------------------------------

    // UnityEvent aparece en el Inspector, podés conectar métodos sin escribir código.
    public UnityEvent OnDeathUnity;

    // event Action invisible en el Inspector, todo se conecta por código.
    public event Action OnDeathAction;


    // ----------------------------------------------------------
    // 2. PARÁMETROS
    // ----------------------------------------------------------

    // UnityEvent con parámetro necesitás declarar una clase custom.
    [Serializable] public class HealthEvent : UnityEvent<float> { }
    public HealthEvent OnHealthChangedUnity;

    // event Action con parámetro, sin clases extra.
    public event Action<float> OnHealthChangedAction;

    // event Action con MÚLTIPLES parámetros soporta hasta 16 sin esfuerzo.
    public event Action<float, int, bool> OnComplexEvent;


    // ----------------------------------------------------------
    // 3. INVOCACIÓN SEGURA
    // ----------------------------------------------------------

    private void DemoInvoke()
    {
        // UnityEvent nunca lanza NullReferenceException aunque nadie escuche.
        OnDeathUnity.Invoke();

        // event Action → EXPLOTA si nadie está suscripto y no usás el operador ?.
        // OnDeathAction.Invoke();      // NullReferenceException
        OnDeathAction?.Invoke();        // seguro con null-conditional
    }


    // ----------------------------------------------------------
    // 4. PROTECCIÓN DEL EVENTO (keyword "event")
    // ----------------------------------------------------------

    private void DemoProtection()
    {
        // La keyword "event" impide que un script externo REEMPLACE todos los suscriptores.
        // Desde afuera solo se puede usar += y -=, nunca =.

        // OnDeathAction = null;        // error de compilación
        // OnDeathAction = MyMethod;    // error de compilación
        OnDeathAction += MyMethod;      // único uso permitido desde afuera
        OnDeathAction -= MyMethod;
    }

    private void MyMethod() { }


    // ----------------------------------------------------------
    // 5. SUSCRIPCIÓN Y DESUSCRIPCIÓN
    // ----------------------------------------------------------

    // UnityEvent usa AddListener / RemoveListener
    // event Action usa += / -=

    private void OnEnable()
    {
        // UnityEvent (por código):
        OnDeathUnity.AddListener(OnPlayerDeadUnity);

        // event Action:
        OnDeathAction += OnPlayerDeadAction;
    }

    private void OnDisable()
    {
        OnDeathUnity.RemoveListener(OnPlayerDeadUnity);
        OnDeathAction -= OnPlayerDeadAction;
    }

    private void OnPlayerDeadUnity() => Debug.Log("Muerte (UnityEvent)");
    private void OnPlayerDeadAction() => Debug.Log("Muerte (event Action)");


    // ----------------------------------------------------------
    // 6. PERFORMANCE
    // ----------------------------------------------------------

    // UnityEvent (vía Inspector) usa reflection (cuando un programa lee y ejecuta su propio código mientras está corriendo) y serialización haciendolo más lento que un delegado directo.
    // event Action utiliza delegado C# directo donde lo hace MÁS RÁPIDO.
    //
    // Para eventos de UI o gameplay casual: la diferencia es insignificante.
    // Para cientos de eventos por frame (ej: partículas, física): event Action gana.


    // ----------------------------------------------------------
    // RESUMEN: ¿CUÁNDO USAR CADA UNO?
    // ----------------------------------------------------------
    //
    // Usá UnityEvent cuando:
    //   Un diseñador (no programador) necesita conectar cosas en el Inspector.
    //   Cuando manipulas Botones, animaciones, sonidos, trigger de escena.
    //
    // Usá event Action cuando:
    //   La comunicación es entre sistemas de código puro.
    //   Ejemplos: Salud, inventario, estados del juego, lógica de negocio.
    //   Se usa cuando querés más control, más velocidad y código más limpio.
}

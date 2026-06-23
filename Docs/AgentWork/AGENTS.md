# AGENTS.md

## 1. Propósito del proyecto

Este repositorio contiene un proyecto académico de Unity para la materia **Paradigmas de la Programación**.

El proyecto consiste en un videojuego **Top-Down 2D** protagonizado por un ninja. Su finalidad principal es demostrar de manera clara, correcta y defendible los conceptos de programación enseñados en clase.

La prioridad del proyecto es:

1. Cumplir la consigna académica.
2. Aplicar correctamente los conceptos vistos en clase.
3. Mantener una arquitectura sencilla de comprender y defender oralmente.
4. Producir scripts funcionales y fáciles de integrar manualmente en Unity.
5. Evitar complejidad innecesaria.

La sofisticación técnica no es un objetivo por sí misma. Ante dos soluciones válidas, preferir la que sea más simple, explícita y fácil de explicar.

---

## 2. División de responsabilidades

El agente es responsable de:

* Analizar las consignas y el material de clase.
* Diseñar la arquitectura del código.
* Crear y modificar scripts de C#.
* Documentar decisiones, dependencias y requisitos.
* Escribir instrucciones precisas de integración manual en Unity.
* Mantener trazabilidad entre los conceptos académicos y el código.
* Revisar estáticamente el código en busca de errores previsibles.

El estudiante es responsable de:

* Construir las escenas.
* Crear y configurar GameObjects.
* Crear prefabs.
* Asignar sprites y animaciones.
* Configurar Animator Controllers.
* Añadir colliders, rigidbodies y otros componentes.
* Asignar referencias en el Inspector.
* Configurar layers, tags, física e inputs.
* Diseñar los niveles.
* Probar el proyecto dentro del editor de Unity.
* Ajustar valores de gameplay.

No intentar sustituir el trabajo manual de Unity mediante herramientas automáticas.

---

## 3. Fuentes de verdad

Antes de diseñar o implementar cualquier sistema, leer recursivamente:

1. `Docs/Consignas/`
2. `Docs/MaterialDeClase/`
3. Las actividades, ejercicios y ejemplos de código incluidos dentro de `Docs/`
4. El TP2 original incluido dentro de `Docs/`
5. `Docs/_AgentWork/`
6. El código existente dentro de `Assets/Scripts/`
7. La estructura general del proyecto Unity

La prioridad entre fuentes es:

1. Consigna final y sus partes complementarias.
2. Material y ejemplos dados por el profesor.
3. Actividades prácticas de la materia.
4. TP2 original presentado por el estudiante.
5. Decisiones humanas registradas posteriormente.
6. Propuestas o suposiciones del agente.

El TP2 original describe una propuesta de juego, pero puede necesitar ajustes para cumplir la consigna final.

Cuando dos documentos se contradigan:

* No resolver la contradicción silenciosamente.
* Registrar ambos planteos.
* Indicar cuál tiene mayor prioridad.
* Explicar el impacto sobre el diseño.
* Consultar al estudiante si la decisión afecta significativamente la arquitectura o el alcance.

No inventar el contenido de archivos que no puedan leerse.

---

## 4. Política de idioma

El proyecto utilizará un enfoque bilingüe consistente.

### Código C#

Escribir en inglés:

* Clases.
* Interfaces.
* Structs y enums.
* Métodos.
* Propiedades.
* Campos.
* Parámetros.
* Eventos.
* Namespaces.
* Nombres de archivos de código.

Ejemplos correctos:

* `PlayerHealth`
* `EnemyBase`
* `TakeDamage`
* `CurrentHealth`
* `OnHealthChanged`
* `IDamageable`

### Español

Escribir en español:

* Comentarios explicativos.
* Instrucciones de configuración en Unity.
* Documentación académica.
* Documentación de arquitectura.
* Guías de wiring.
* Registro de decisiones.
* Material de estudio.
* Explicaciones para la defensa oral.
* Textos visibles para el jugador, salvo decisión contraria.

No mezclar idiomas dentro de un mismo identificador.

Evitar ejemplos como:

* `RecibirDamage`
* `EnemyEnemigo`
* `GetVidaActual`
* `OnJugadorDead`

Los nombres propios de Unity, C# y sus APIs deben conservarse en inglés.

---

## 5. Alcance permitido

El agente puede:

* Leer todo el repositorio.
* Crear y modificar scripts dentro de `Assets/Scripts/`.
* Crear y modificar documentación dentro de `Docs/_AgentWork/`.
* Inspeccionar nombres y carpetas de los assets importados.
* Usar los assets como referencia para conocer qué contenido visual podría estar disponible.
* Proponer ajustes al alcance del juego.
* Crear diagramas textuales o Mermaid dentro de archivos Markdown cuando ayuden a explicar relaciones.

El agente no debe:

* Modificar, mover, renombrar o eliminar assets gráficos de terceros.
* Modificar escenas.
* Crear escenas.
* Modificar prefabs.
* Crear prefabs.
* Editar manualmente archivos YAML de Unity.
* Diseñar niveles.
* Crear Animator Controllers.
* Configurar animaciones.
* Agregar componentes automáticamente a GameObjects.
* Crear Custom Inspectors.
* Crear ventanas de editor.
* Crear herramientas de validación.
* Crear generadores de assets o Scriptable Objects.
* Crear scripts dentro de carpetas `Editor`.
* Añadir paquetes o dependencias externas.
* Modificar `Packages/` salvo autorización explícita.
* Acoplar el código a rutas concretas de assets.
* Hardcodear nombres de sprites, prefabs, escenas o carpetas.
* Introducir sistemas ajenos a la consigna.
* Realizar cambios masivos fuera del alcance del hito actual.

Los assets importados son material de referencia de solo lectura. No dedicar tiempo a analizar individualmente cada sprite salvo que sea necesario para comprender una mecánica.

---

## 6. Configuración manual de Unity

No utilizar mecanismos de configuración automática como:

* `[RequireComponent]`
* `Reset()`
* `OnValidate()`
* Creación automática de componentes.
* Búsquedas automáticas globales para evitar asignaciones del Inspector.
* Generación automática de prefabs o assets.
* Herramientas de editor.

Solo se podrá usar alguna de estas opciones si:

1. Fue enseñada o requerida explícitamente en el material de clase.
2. Tiene una justificación académica clara.
3. Su uso fue registrado en la documentación.
4. No oculta al estudiante cómo se relacionan los objetos.

Cada `MonoBehaviour` que requiera configuración manual debe incluir, cerca del comienzo de la clase, un comentario con esta estructura:

```csharp
/*
CONFIGURACIÓN EN UNITY

GameObject:
- Indicar a qué GameObject debe añadirse el script.

Componentes necesarios:
- Indicar Rigidbody2D, Collider2D u otros componentes que deben agregarse.
- Indicar configuraciones relevantes de esos componentes.

Referencias del Inspector:
- Enumerar cada referencia que debe asignarse manualmente.

Layers y Tags:
- Indicar cualquier layer o tag necesario.

Animación e Input:
- Indicar eventos de animación, parámetros del Animator o acciones de input necesarias.

Notas:
- Incluir cualquier otro paso manual necesario para que el script funcione.
*/
```

No incluir secciones que no correspondan al script.

La misma información debe mantenerse de forma centralizada y ordenada en:

`Docs/_AgentWork/06_UNITY_WIRING.md`

El código no debe asumir que una escena, prefab, tag, layer o referencia ya existe sin documentarlo.

---

## 7. Requisitos académicos obligatorios

El núcleo del proyecto debe demostrar claramente:

* Clases y objetos.
* Métodos con parámetros.
* Métodos con valores de retorno.
* Encapsulamiento.
* Abstracción.
* Herencia.
* Polimorfismo.
* Interfaces.
* Uso de una misma interfaz por diferentes tipos de objetos.
* Eventos y delegados mediante `Action`.

Cada concepto obligatorio debe:

1. Resolver una necesidad real del juego.
2. Ser reconocible dentro del código.
3. Estar documentado.
4. Poder explicarse oralmente de manera sencilla.
5. Estar relacionado con scripts concretos.

No agregar conceptos únicamente para decir que fueron utilizados.

---

## 8. Requisitos jugables conocidos

La arquitectura debe contemplar como mínimo:

* Juego Top-Down 2D.
* Movimiento del jugador.
* Sistema de salud o vidas.
* Ataque principal.
* Colisiones.
* Sistema de puntuación o coleccionables.
* Al menos dos tipos diferentes de enemigos.
* Enemigo cuerpo a cuerpo.
* Enemigo a distancia o avanzado.
* Enemigos con patrones simples.
* Persecución y ataque básico.
* Jefe final con comportamiento especial.
* Power-up o mejora temporal.
* Objetivo final.
* Condición de victoria.
* Condición de derrota.
* Nivel tutorial.
* Nivel principal.
* Duración breve, respetando el máximo definido por la consigna.

El TP2 también propone:

* Objetos destructibles.
* Monedas.
* Comida para recuperar salud.
* Mejoras permanentes.
* Pergaminos intercambiables.
* Habilidades de fuego, viento, roca y hielo.

Estos elementos forman parte de la propuesta, pero deben evaluarse contra la consigna, el tiempo disponible y el alcance final.

---

## 9. Elementos opcionales

Los siguientes elementos son opcionales y corresponden a puntos extra:

* LINQ.
* Clase genérica `T`.
* Múltiples expresiones lambda.

No incluirlos en el núcleo obligatorio.

Solo proponer o implementar alguno cuando:

* El proyecto obligatorio ya esté correctamente diseñado.
* Su aplicación sea natural.
* Sea fácil de explicar.
* No aumente significativamente el riesgo.
* No obligue a deformar la arquitectura.
* Exista una utilidad concreta dentro del juego.

Mantener cualquier punto extra aislado y fácilmente removible.

Los Scriptable Objects tampoco deben asumirse como obligatorios. Utilizarlos solamente cuando:

* Aparezcan en el material de clase, o
* Simplifiquen claramente el manejo de datos sin agregar una capa difícil de explicar.

---

## 10. Principios de diseño del código

Priorizar:

* Clases pequeñas y con una responsabilidad clara.
* Encapsulamiento mediante campos privados.
* `[SerializeField] private` para valores configurables desde el Inspector.
* Propiedades de solo lectura cuando otros sistemas necesiten consultar estado.
* Métodos públicos controlados para modificar estado.
* Interfaces para comportamientos compartidos entre objetos distintos.
* Clases base cuando exista lógica común real.
* Métodos virtuales o abstractos solamente cuando existan comportamientos derivados.
* Eventos `Action` para comunicación desacoplada.
* Dependencias explícitas y asignables desde el Inspector.
* Valores de gameplay configurables.
* Código legible para un estudiante.

Evitar:

* God objects.
* Managers que controlen todo el juego.
* Singletons sin justificación.
* Estado global estático.
* Service locators.
* Jerarquías de herencia profundas.
* Patrones avanzados innecesarios.
* Abstracciones creadas antes de que exista una necesidad.
* Interfaces con un único uso artificial.
* Eventos utilizados donde una llamada directa sería más clara.
* Duplicación innecesaria.
* Comentarios que simplemente repitan lo que dice el código.
* Uso excesivo de regiones.
* Dependencias ocultas mediante búsquedas globales.
* Optimización prematura.

La arquitectura debe ser suficientemente modular para crecer, pero no debe diseñarse como si fuera un juego comercial de gran escala.

---

## 11. Métodos, eventos e interfaces

Los métodos con parámetros y retorno deben aparecer de manera natural.

Ejemplos posibles:

* Calcular daño final.
* Consultar si un personaje continúa vivo.
* Consultar si una habilidad está disponible.
* Obtener distancia a un objetivo.
* Aplicar curación.
* Aplicar una mejora.
* Intentar activar una habilidad y devolver si fue posible.

Los eventos `Action` deben usarse para notificaciones desacopladas, por ejemplo:

* Cambio de salud.
* Muerte de una entidad.
* Recolección de un objeto.
* Cambio de puntuación.
* Activación o finalización de un power-up.
* Victoria o derrota.

No usar eventos para toda comunicación entre clases. Elegirlos cuando exista una relación de notificación apropiada.

Las interfaces obligatorias deben ser implementadas por tipos diferentes. Por ejemplo, `IDamageable` podría ser implementada por enemigos, jugador y objetos destructibles.

---

## 12. Dudas y decisiones de diseño

El agente debe clasificar las dudas como:

### `BLOCKING`

Decisiones que:

* Afectan varios sistemas.
* Cambian la arquitectura.
* Podrían causar una refactorización importante.
* Impiden definir correctamente un hito.

No implementar sistemas dependientes de una pregunta `BLOCKING` sin recibir una respuesta.

### `NON-BLOCKING`

Decisiones que:

* Pueden resolverse temporalmente con una opción simple.
* Son fácilmente reversibles.
* Se relacionan con valores de gameplay.
* No cambian la estructura principal.

Adoptar una alternativa provisional razonable y registrarla.

### `EDITOR-ONLY`

Decisiones que:

* Corresponden al armado de escenas.
* Dependen del diseño visual.
* Dependen de animaciones, prefabs o distribución del nivel.
* Pueden ser resueltas posteriormente por el estudiante dentro de Unity.

Documentarlas en la guía de wiring sin bloquear el código.

No interrumpir al estudiante por cada detalle menor. Agrupar las preguntas importantes en los checkpoints establecidos.

---

## 13. Dirección de diseño pendiente: power-up temporal

El requisito de power-up temporal todavía necesita una decisión definitiva.

Existe una propuesta inicial:

* El jugador posee normalmente un pergamino equipado.
* Puede recoger un pergamino temporal más poderoso.
* El sistema guarda el pergamino normal que tenía equipado.
* Durante un tiempo limitado utiliza la habilidad temporal.
* Al terminar el efecto recupera automáticamente el pergamino anterior.

Esta propuesta no está aprobada definitivamente.

Durante la planificación se debe evaluar:

* Si cumple claramente el requisito de mejora temporal.
* Si es más compleja de lo necesario.
* Cómo interactúa con el cambio normal de pergaminos.
* Qué ocurre si se recoge otro pergamino durante el efecto.
* Qué ocurre si se recoge otro power-up temporal.
* Qué ocurre al morir o cambiar de escena.
* Si se reinicia, acumula o reemplaza su duración.
* Si conviene usar una habilidad nueva o una versión potenciada.
* Qué eventos y estados requiere.
* Qué alternativa más sencilla podría cumplir el mismo requisito.

Esta decisión debe formar parte del primer checkpoint de diseño.

---

## 14. Documentación interna

Toda documentación temporal del agente debe guardarse dentro de:

`Docs/_AgentWork/`

Usar como mínimo:

* `00_MATERIAL_INDEX.md`
* `01_REQUIREMENTS_AUDIT.md`
* `02_COURSE_CONCEPT_MAP.md`
* `03_GAME_SCOPE_AND_DESIGN.md`
* `04_ARCHITECTURE.md`
* `05_IMPLEMENTATION_PLAN.md`
* `06_UNITY_WIRING.md`
* `07_DECISIONS_AND_ASSUMPTIONS.md`
* `08_TEST_PLAN.md`
* `STATUS.md`

No dispersar documentación del agente por todo el repositorio.

Mantener estos documentos sincronizados con el código.

---

## 15. Proceso de desarrollo

### Etapa 1: auditoría y planificación

Antes de crear gameplay:

1. Leer todo el material académico disponible.
2. Crear un índice de documentos.
3. Clasificar los requisitos.
4. Identificar conceptos enseñados.
5. Comparar la consigna final con el TP2.
6. Definir el alcance completo.
7. Diseñar la arquitectura.
8. Dividir el desarrollo en hitos.
9. Identificar el wiring manual.
10. Consolidar preguntas de diseño.

No escribir gameplay durante esta etapa.

### Etapa 2: checkpoint de diseño

Al completar la planificación:

* Detenerse.
* Presentar las decisiones importantes.
* Presentar preguntas `BLOCKING`.
* Presentar preguntas `NON-BLOCKING`.
* Presentar cuestiones `EDITOR-ONLY`.
* Recomendar una alternativa para cada pregunta.
* Esperar feedback humano antes de implementar.

### Etapa 3: implementación progresiva

Después de recibir aprobación:

* Trabajar un hito por vez.
* No implementar sistemas de hitos futuros innecesariamente.
* Actualizar la documentación.
* Actualizar la guía de wiring.
* Registrar decisiones provisionales.
* Realizar una revisión estática al terminar cada hito.

### Etapa 4: checkpoint del núcleo

Detenerse nuevamente después de completar el núcleo arquitectónico, que previsiblemente incluirá:

* Interfaces principales.
* Salud y daño.
* Eventos centrales.
* Movimiento del jugador.
* Ataque principal.
* Clase base de enemigos.
* Primer enemigo funcional a nivel de código.

Presentar:

* Código creado.
* Decisiones tomadas.
* Riesgos detectados.
* Wiring necesario.
* Cambios respecto del plan.
* Próximo hito recomendado.

### Etapa 5: implementación del contenido restante

Después de la revisión del núcleo:

* Segundo tipo de enemigo.
* Jefe.
* Coleccionables.
* Puntuación.
* Power-up temporal.
* Pergaminos.
* Destructibles, si permanecen en el alcance.
* Tutorial y progresión.
* Victoria y derrota.
* Otros sistemas aprobados.

### Etapa 6: handoff final

Preparar:

* Código completo.
* Wiring actualizado.
* Lista de pruebas manuales.
* Limitaciones conocidas.
* Riesgos de compilación.
* Mapa entre conceptos académicos y scripts.
* Guía para estudiar el código.
* Posibles preguntas de defensa oral.

---

## 16. Estado y trazabilidad

Actualizar `Docs/_AgentWork/STATUS.md` al terminar cada tarea.

Debe incluir:

* Estado general.
* Hito actual.
* Material leído.
* Archivos creados.
* Archivos modificados.
* Trabajo completado.
* Decisiones tomadas.
* Supuestos vigentes.
* Preguntas pendientes.
* Bloqueos.
* Riesgos.
* Próxima tarea recomendada.

Actualizar `07_DECISIONS_AND_ASSUMPTIONS.md` cuando:

* Se elija una alternativa arquitectónica.
* Se descarte una mecánica.
* Se adopte un supuesto.
* Exista una contradicción documental.
* Se incluya o excluya un elemento opcional.
* Se cambie el plan original.

---

## 17. Validación y honestidad técnica

El agente puede realizar:

* Revisión estática.
* Búsqueda de referencias.
* Inspección de firmas y dependencias.
* Comprobaciones textuales.
* Análisis de posibles errores de compilación.
* Pruebas que realmente estén disponibles en su entorno.

El agente no debe afirmar que:

* El proyecto compila en Unity.
* Una escena funciona.
* Un prefab está configurado correctamente.
* Una animación se reproduce.
* Una colisión funciona.
* El gameplay fue probado.

Salvo que efectivamente haya podido ejecutar y verificar esas condiciones.

Distinguir siempre entre:

* Revisado estáticamente.
* Probado mediante herramientas disponibles.
* Pendiente de probar en Unity.

---

## 18. Condición de finalización de un hito

Un hito solo está terminado cuando:

1. Su código fue creado.
2. Sus responsabilidades están claras.
3. Cumple los requisitos académicos asignados.
4. Las dependencias están documentadas.
5. El wiring manual está documentado.
6. Los comentarios de configuración están actualizados.
7. Se realizó una revisión estática.
8. Las limitaciones están registradas.
9. `STATUS.md` fue actualizado.
10. No se expandió silenciosamente el alcance.

No comenzar el siguiente hito si existe una decisión `BLOCKING` pendiente que lo afecte.

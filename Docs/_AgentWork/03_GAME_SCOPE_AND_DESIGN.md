# 03 - Alcance y diseño del juego

## Núcleo obligatorio

Top-Down 2D breve de ninja: movimiento, orientación, salud, ataque, daño, dos enemigos, jefe, coleccionables/puntuación, power-up temporal, tutorial, nivel principal, victoria y derrota. Todo debe demostrar POO sin sofisticación innecesaria.

## Contenido necesario

- Tutorial: zona corta con movimiento, ataque, coleccionable, curación/power-up y enemigo básico.
- Nivel principal: enemigos melee y ranged, coleccionables, pergamino/power-up, jefe y objetivo final.
- Enemigos: melee perseguidor simple; ranged con proyectil o avanzado.
- Jefe: vida superior, dos comportamientos simples (perseguir + ataque especial temporizado).
- Coleccionables: monedas para score, comida para curación, un power-up temporal.

## TP2 a conservar

| Elemento | Decisión | Motivo |
|---|---|---|
| Monedas | Conservar | Cumple puntuación con bajo costo. |
| Comida | Conservar simple | Cumple curación y recolección. |
| Mejoras permanentes | Recortar inicialmente | No obligatorias, riesgo de balance. |
| Pergaminos | Conservar simplificado | Buen marco para habilidades y power-up. |
| Fuego/viento/roca/hielo | Conservar máximo 1-2 al inicio | Cuatro habilidades elevan alcance. |
| Destructibles | Opcional | Útiles para IDamageable, recortables. |
| Narrativa comida robada | Conservar mínima | Da objetivo claro sin costo técnico. |

## Contenido recortable

Economía avanzada, upgrades permanentes, cuatro habilidades completas, inventario complejo, múltiples jefes, diálogos extensos, puzzle de elementos y dependencias visuales específicas.

## Puntos extra

Ignorar en núcleo. Considerar LINQ al final si existe lista de enemigos y necesidad real. Genéricos/lambdas solo si el estudiante puede defenderlos.

## Flujo de juego

Inicio -> Tutorial con instrucciones textuales -> transición manual a nivel principal -> exploración breve -> combate y recolección -> power-up temporal -> jefe final -> victoria al derrotar jefe/recuperar comida -> derrota si Health llega a 0.

## Power-up temporal: alternativas

1. **Buff simple de estadística**: aumenta velocidad/daño por tiempo. Ventajas: claro y simple. Desventajas: menos integrado a pergaminos. Complejidad baja. Recomendado si se prioriza entrega.
2. **Pergamino temporal que reemplaza y restaura**: guarda `previousScroll`, equipa temporal, restaura al finalizar. Ventajas: integrado y reconocible. Desventajas: casos borde. Complejidad media-alta. Recomendado solo si pergaminos quedan en alcance.
3. **Versión potenciada del pergamino actual**: no cambia habilidad, multiplica daño/rango. Ventajas: evita guardar scroll. Desventajas: menos variedad. Complejidad media. Buena alternativa.
4. **Invulnerabilidad temporal independiente**: no toca habilidades. Ventajas: muy clara. Desventajas: no usa pergaminos. Complejidad baja.

Decisión provisional: solución 3 o 1 para reducir riesgos; solución 2 queda BLOCKING si se quiere conservar fantasía de pergamino temporal.

Casos a definir: recoger normal durante efecto (posponer o reemplazar base), recoger otro temporal (reiniciar duración), morir/cambiar escena (cancelar efecto), cooldown (no resetear cooldown salvo decisión), duración (reemplaza/reinicia, no acumula).

## Decisiones pendientes

- BLOCKING: alcance de pergaminos/power-up; tipo de enemigo avanzado; comportamiento mínimo del jefe.
- NON-BLOCKING: valores de salud/daño/velocidad, cantidad de monedas, textos de tutorial.
- EDITOR-ONLY: layouts de escenas, sprites, animaciones, tags/layers finales.

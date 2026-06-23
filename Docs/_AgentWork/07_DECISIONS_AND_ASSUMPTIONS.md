# 07 - Decisiones, supuestos y preguntas

## Decisiones arquitectónicas
- Priorizar arquitectura simple con `MonoBehaviour` pequeños y contratos claros.
- Usar `IDamageable` e `ICollectible` como interfaces reales con múltiples implementaciones.
- Usar `EnemyBase` solo para lógica compartida real; derivados melee/ranged/boss.
- Usar eventos `Action` en salud, score, recolección, power-up, victoria y derrota.
- No usar singletons ni managers globales todopoderosos.
- No usar ScriptableObjects en núcleo inicial.

## Alternativas descartadas
- Inventario/economía completa: demasiado alcance.
- Cuatro habilidades elementales completas desde el inicio: alto riesgo.
- Herramientas de editor o auto-wiring: prohibido/no conveniente.
- LINQ/genéricos/lambdas en núcleo: opcionales, pueden deformar diseño.

## Contradicciones documentales
- Parte 1 exige POO e interfaces pero no eventos ni dos niveles; Parte 2 agrega eventos con `Action`, tutorial, nivel principal y puntos extra. Se aplica Parte 2 como complemento de mayor detalle.
- Consigna permite plataforma o top-down; AGENTS y TP2 orientan a Top-Down Ninja. Se adopta Top-Down.
- TP2 original exacto no legible; sus elementos quedan por debajo de consigna final.

## Supuestos provisionales
- El juego será de una duración menor a 5 minutos, con tutorial y un nivel principal.
- Monedas = puntuación; comida = curación; pergaminos = habilidades simples.
- Boss se derrota para ganar o habilita recoger la comida robada.
- Se podrá crear `Assets/Scripts/` en hitos futuros, pero no en esta tarea.

## Preguntas BLOCKING
1. Power-up temporal: buff simple, pergamino temporal, potenciar pergamino actual o invulnerabilidad. Recomendación: potenciar pergamino actual o buff simple.
2. Enemigo avanzado: ranged con proyectiles o perseguidor con dash. Recomendación: ranged con proyectil por claridad académica.
3. Boss mínimo: fase por vida, ataque especial temporizado o spawn de minions. Recomendación: fase por vida + proyectil/área simple.
4. Alcance de pergaminos: 1 habilidad base + power-up, 2 pergaminos, o 4 elementos. Recomendación: 2 máximo.

## Preguntas NON-BLOCKING
- Valores de vida, daño, velocidad, duración de power-up y puntajes.
- Cantidad de coleccionables y enemigos por nivel.
- Textos exactos de tutorial y narrativa.
- Si destructibles entran como contenido opcional.

## Cuestiones EDITOR-ONLY
- Sprites, animaciones, prefabs, colliders concretos, layers/tags definitivos, nombres de escenas, layout de niveles y Canvas.

## Riesgos
- Ilegibilidad parcial de PDFs puede ocultar matices de definiciones.
- Demasiados sistemas opcionales pueden impedir una escena funcional.
- Power-up integrado a pergaminos agrega casos borde.
- No se puede afirmar compilación ni funcionamiento en Unity desde esta auditoría.

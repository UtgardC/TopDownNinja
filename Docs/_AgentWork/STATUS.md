# STATUS

## Estado general
Checkpoint de diseño completado. No se implementó gameplay.

## Fase actual
Etapa 1/2: auditoría, arquitectura, planificación y preparación de feedback humano.

## Material leído
- `Docs/AgentWork/AGENTS.md` completo. No existe `AGENTS.md` en la raíz del repositorio.
- `Docs/Consignas TP 2.txt` completo.
- Ejemplos `.cs` de Unidad 1 y Unidad 4 completos.
- PDFs de material de clase y TP2 con lectura parcial por metadatos/`strings`.
- Estructura general de `Assets/`: NinjaAssetPack, Scenes, Settings, TextMesh Pro.
- `Assets/Scripts/` no existe o no contiene scripts.

## Material no legible completo
Todos los PDFs quedaron parcialmente legibles por falta de extractor PDF disponible y bloqueo 403 al intentar instalar `pypdf`/`poppler-utils`. El más crítico es `Docs/TP2 DAMIAN CARBONE.pdf`.

## Documentos creados
- `Docs/_AgentWork/00_MATERIAL_INDEX.md`
- `Docs/_AgentWork/01_REQUIREMENTS_AUDIT.md`
- `Docs/_AgentWork/02_COURSE_CONCEPT_MAP.md`
- `Docs/_AgentWork/03_GAME_SCOPE_AND_DESIGN.md`
- `Docs/_AgentWork/04_ARCHITECTURE.md`
- `Docs/_AgentWork/05_IMPLEMENTATION_PLAN.md`
- `Docs/_AgentWork/06_UNITY_WIRING.md`
- `Docs/_AgentWork/07_DECISIONS_AND_ASSUMPTIONS.md`
- `Docs/_AgentWork/08_TEST_PLAN.md`
- `Docs/_AgentWork/STATUS.md`

## Trabajo completado
Auditoría de requisitos, mapa conceptual, alcance recomendado, arquitectura propuesta, plan de hitos, guía de wiring, decisiones y plan de pruebas.

## Contradicciones
Parte 2 amplía Parte 1 con eventos, niveles y puntos extra; se toma como complemento obligatorio salvo puntos extra. TP2 original está subordinado a consigna final.

## Riesgos
PDFs parcialmente ilegibles, alcance excesivo de pergaminos/habilidades, boss demasiado complejo, configuración manual de Unity pendiente.

## Decisiones provisionales
Top-Down 2D ninja; monedas/comida; dos enemigos; boss simple; eventos Action; no puntos extra en núcleo; no ScriptableObjects iniciales.

## Preguntas BLOCKING
Power-up temporal, alcance de pergaminos, enemigo avanzado, comportamiento mínimo del jefe.

## Preguntas NON-BLOCKING
Valores de gameplay, cantidades, textos, destructibles opcionales.

## Cuestiones EDITOR-ONLY
Escenas, prefabs, sprites, animaciones, colliders, tags/layers, Canvas y Build Settings.

## Próximo hito recomendado
Después del feedback: Hito 1, contratos y bases del dominio (`IDamageable`, `ICollectible`) y luego salud/daño/eventos.

## Confirmación
Todavía no se implementó gameplay, no se crearon scripts en `Assets/Scripts/`, no se modificaron escenas, prefabs, assets ni paquetes.

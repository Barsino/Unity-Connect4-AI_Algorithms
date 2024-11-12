# Connecta4 con Inteligencia Artificial
Este proyecto es una implementación del juego Connecta4 en Unity que utiliza diferentes algoritmos de inteligencia artificial para mejorar la jugabilidad y el desafío. Puedes seleccionar entre varios algoritmos para jugar contra la IA o enfrentar dos algoritmos entre sí. El objetivo principal de este proyecto es explorar y optimizar distintos enfoques de IA para resolver problemas de decisión en juegos de tablero.



## Descripción
El juego de Connecta4 es un juego de estrategia en el que dos jugadores alternan en turnos para soltar fichas en una cuadrícula vertical de 7 columnas y 6 filas. Gana el primer jugador que consiga alinear cuatro fichas consecutivas de su color en cualquier dirección (horizontal, vertical o diagonal).

Este proyecto incluye una implementación de IA con varios algoritmos para evaluar y mejorar las decisiones en cada turno, optimizando la jugada ganadora o evitando una derrota.



## Algoritmos Implementados
### 1. Negamax
Una variación del algoritmo Minimax optimizado para problemas de dos jugadores, aprovechando la simetría de resultados. Se evalúan todas las posibles jugadas y se elige la más favorable para el jugador activo.

### 2. Negamax con Poda Alpha-Beta (NegamaxAB)
Extiende Negamax con poda Alpha-Beta, lo que permite reducir el número de nodos evaluados al podar ramas que no mejorarán la evaluación actual.

### 3. Negascout (Poda Scout)
Una optimización adicional sobre Alpha-Beta que trata de mejorar la poda mediante ventanas de búsqueda reducidas. Negascout realiza comparaciones adicionales para mejorar la eficiencia.

### 4. Búsqueda Aspiracional
Un enfoque que utiliza un valor de evaluación esperado y realiza búsquedas alrededor de este valor para ajustar el resultado, buscando mejorar la rapidez de convergencia.

### 5. Monte Carlo
Este algoritmo realiza simulaciones aleatorias desde el estado actual del tablero y evalúa el éxito de cada jugada en función de las victorias o derrotas observadas en estas simulaciones.

### 6. MTD(f)
Una variación del Negamax que realiza una serie de búsquedas incrementales sobre un valor de referencia (f) para ajustar la evaluación de la mejor jugada. Este algoritmo permite una convergencia más eficiente hacia el valor óptimo, utilizando búsquedas estrechas y evitando exploraciones innecesarias.



## Requisitos
Unity (versión 2021 o superior recomendada)
Git para clonar el repositorio
C# como lenguaje de programación

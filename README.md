# Cosmos

Procedurally-generated game implemented in Monogame framework.

Split into two parts: Cosmos and Cosmos-Worldgen.

## Cosmos
Main game files. Features a n to n body simulation with an optimized Barnes-Hut algorithm for O(n*logn) complexity. 
The current implementation is a simulation of the universe with stars and planets placed in a circular galaxy.

## Cosmos-Worldgen
Algorithm for world generation. Used to generate the worlds that are generated in the Cosmos project. 
Implements a hexagon-based tilemap and world generation using a simplex noise and perlin noise algorithms. 

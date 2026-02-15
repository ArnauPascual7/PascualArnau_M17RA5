# Arnau Pascual - RPG 3D

## Build

La build es troba en format .zip en el drive ja que per el seu pes no s'ha pogut pujar al github.

[Build](https://drive.google.com/file/d/1PMpuZxiB_C9VEbDhuCLyM4sPmRUeb6u_/view?usp=sharing)

## Controls

WASD - Moviment

Space - Saltar

Shift - Correr

E - Interactuar

B - Ballar

Right Click - Apuntar (si l'arma està equipada)

Left Click - Disparar (si l'arma està equipada)

X - KABOOM!

## Entrega

### Personatge

El personatge té animacions de Idle, Caminar, Correr, Saltar, Apuntar, Atacar i Ballar.

### Càmera

La càmera està en 3a persona i aquesta s'apropa en apuntar.

### Inputs

S'utilitza el New Input System.

### Escenaris

L'escena principal conté un terreny amb un height map pintat per a crear un bioma de muntanya, el terreny també conté elements com arbres i herba.

Hi ha un skybox que fa notar com si l'escenari estigues ambientat en un altre planeta on es veu l'espai.

La segona escena és un interior completament fosc en el que es pot accedir a través d'un portal que està situat en les dues escenes per a poder moure's entre elles.

### Objectes

Hi ha diversos elements amb mapes de textures com, la lamparà de la segona escena i el portal que s'utilitza per a canviar d'escena.

Hi ha dos objectes equipalbles un barret d'aniversari que s'equipa al cap del personatge i una arma de foc que es troba en la segona escena.

### Shader/Partícules

L'NPC amb el que s'interactua té un material fet a través d'un shader i el portal per a canviar d'esnea també conté un shader.

L'arma de foc utiliza partícules a l'hora de disparar i unes altres en el lloc de impacte. També fent que el personatge exploti s'utilizen partícules.

### Llum

La ilumiació de l'escena es fa a través del skybox, i s'han tocat les configuracions de llums a més de realitzat un backeig de llums.

En la segona escena interior hi ha una llum i el barret equipable també conté una llum.

### User Interface

En la zona superior dreta es mostra un minimapa on es pot veure la posició del personatge i els diferents elements de l'escena.

### Gameplay

El jugador es pot moure pel mon, recollir el barret d'aniversari que s'euipa al seu cap, atravessar el portal i entrar en una altre escena interior on es pot ballar amb la tecla "B", i es pot tornar a la escena principal utilitzant el portal.

El joc es pot guardar interactuant amb l'NPC que té una textura amb un shader. En guardar partida es desa la posició i rotació del personatge i si té el barret d'aniversari equipat o no.

asciiadventure
===
Ascii adventure in C#
* Screen is one screen, with numRows and numCols
    * 
* GameObject is an object in the game.
    * Owned by Screen, and does not need to know about screen
    * Some GameObjects include Wall and Treasure
    * Is passable? If it's passable, you can walk over it.
* MovingGameObject extends GameObject, but adds movement as a possibility.
    * Knows about Screen
    * needs to ask Screen about legal moves
* Player IS-A MovingGameObject.
    * Adds a Move() method.
    * Needs to be able to interact with other GameObjects.
        * How?

## Challenges
* Not possible for moving gameobjs to move over objects with replacing them
* Really clunky menu options
* Not great MVC for display; the Screen is essentially everything
    * Need to draw moving gameobjs over the top of the underlying stuff
    * Two layers? Basic stuff, plus moving things?


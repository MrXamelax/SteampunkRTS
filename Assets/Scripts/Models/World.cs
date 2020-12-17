using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Models
{
    public class World
    {

        Tilemap wall;
        TileBase walls;
        Tilemap ground;
        public World(Tilemap ground, Tilemap wall, TileBase walls)
        {
            this.wall = wall;
            this.walls = walls;
            this.ground = ground;
        }

        public void SetTileAt(Vector3 destination)
        {
            if (wall)
                wall.SetTile(wall.WorldToCell(destination), walls);
        }
        public (Vector3, Vector3) getCorners()
        {
            return (ground.origin, ground.origin+ground.size);
        }
    }
}
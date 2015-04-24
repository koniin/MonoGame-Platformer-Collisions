using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CollisionDetectionPlatformer
{
    public class SimpleCollisionHandler
    {
        private int PointToTile(int pos)
        {
            return (int)(pos / 32);
        }

        private int TileToPoint(int point) {
            return point*32;
        }

        internal void Collide(SimplePlayer player, TileMap tileMap)
        {
            int tx = PointToTile(player.x);
            int ty = PointToTile(player.y);
            var nx = player.x % 32;
            var ny = player.y % 32;

            Tile cell = tileMap.GetTile(tx, ty);
            Tile cellRight = tileMap.GetTile(tx + 1, ty);
            Tile cellDown = tileMap.GetTile(tx, ty + 1);
            Tile cellDiag = tileMap.GetTile(tx + 1, ty + 1);
            
            if (player.dy > 0)
            {
                if ((cellDown.IsSolid && !cell.IsSolid) ||
                    (cellDiag.IsSolid && !cellRight.IsSolid && nx != 0))
                {
                    player.y = TileToPoint(ty);
                    player.dy = 0;
                    player.falling = false;
                    player.jumping = false;
                    ny = 0;
                }
            }
            //else if (entity.dy < 0)
            //{
            //    if ((cell && !celldown) ||
            //        (cellright && !celldiag && nx))
            //    {
            //        entity.y = t2p(ty + 1);
            //        entity.dy = 0;
            //        cell = celldown;
            //        cellright = celldiag;
            //        ny = 0;
            //    }
            //}

            //if (player.dx > 0)
            //{
            //    if ((cellright && !cell) ||
            //        (celldiag && !celldown && ny))
            //    {
            //        player.x = ToTilePoint(tx);
            //        player.dx = 0;
            //    }
            //}
            //else if (player.dx < 0)
            //{
            //    if ((cell && !cellright) ||
            //        (celldown && !celldiag && ny))
            //    {
            //        player.x = ToTilePoint(tx + 1);
            //        player.dx = 0;
            //    }
            //}
        }
    }
}

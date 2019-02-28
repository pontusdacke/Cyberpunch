using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Cyberpunch_game
{
    static class WallCollision
    {
        //http://go.colorize.net/xna/2d_collision_response_xna/

        static public Vector2 GetWallCollision(List<Rectangle> collidedObj, GameTime gameTime, Rectangle playerRectIn, Vector2 playerPositionIn, out bool onGround)   //kallas när man kolliderar med en vägg, tar in alla vägg tiles som den kolliderar med, fixa positionen svårt som fan
        {
          
            Vector2 playerPosition = playerPositionIn;  //karaktärens position
            Rectangle playerRect = playerRectIn;        //karaktärens rectangle
            List<CorrectionVector2> corrections = new List<CorrectionVector2>();
            onGround = false;

            foreach (Rectangle rect in collidedObj)
            {
                corrections.Add(WallCollision.GetCorrectionVector(rect, playerRect));
            }

            int horizontalSum = WallCollision.SumHorizontal(corrections);
            int verticalSum = WallCollision.SumVertical(corrections);
            
            //if (verticalSum < 0) onGround = true;

            DirectionX directionX = DirectionX.None;
            DirectionY directionY = DirectionY.None;


            if (horizontalSum <= (float)DirectionX.Left)
                directionX = DirectionX.Left;
            else if (horizontalSum >= (float)DirectionX.Right)
                directionX = DirectionX.Right;
            else
                directionX = DirectionX.None; // if they cancel each other out, i.e 2 Left and 2 Right


            if (verticalSum <= (float)DirectionY.Up)
                directionY = DirectionY.Up;
            else if (verticalSum >= (float)DirectionY.Down)
                directionY = DirectionY.Down;
            else
                directionY = DirectionY.None; // if they cancel each other out, i.e 1 Up and 1 Down



            CorrectionVector2 smallestCorrectionY = WallCollision.getSmallestCorrectionY(directionY, corrections);
            CorrectionVector2 smallestCorrectionX = WallCollision.getSmallestCorrectionX(directionX, corrections);
            bool correctY = true;
            bool correctX = true;


            if (Math.Abs(verticalSum) > Math.Abs(horizontalSum)) // start with Y, if collision = then try X
            {
                playerPosition = WallCollision.correctCollision(smallestCorrectionY, false, playerPosition);
                playerRect = UpdateRect(playerRect, playerPosition);
                correctY = false;
                onGround = true;
                if (WallCollision.IsCollidingWith(collidedObj, playerRect))
                {
                    correctX = false;
                    playerPosition = WallCollision.correctCollision(smallestCorrectionX, true, playerPosition);
                }
                else
                {
                    directionX = DirectionX.None;
                }
            }
            else if (Math.Abs(horizontalSum) > Math.Abs(verticalSum)) // start with X, if collision = then try Y
            {
                playerPosition = WallCollision.correctCollision(smallestCorrectionX, true, playerPosition);
                playerRect = UpdateRect(playerRect, playerPosition);
                correctX = false;
                if (WallCollision.IsCollidingWith(collidedObj, playerRect))
                {
                    correctY = false;
                    playerPosition = WallCollision.correctCollision(smallestCorrectionY, false, playerPosition);
                }
                else
                {
                    onGround = true;
                    directionY = DirectionY.None;
                }
            }

            //smallestYCorrection
            if (smallestCorrectionX.X > smallestCorrectionY.Y && correctY == true) // start with Y
            {
                playerPosition = WallCollision.correctCollision(smallestCorrectionY, false, playerPosition);
                playerRect = UpdateRect(playerRect, playerPosition);
                onGround = true;
                if (WallCollision.IsCollidingWith(collidedObj, playerRect))
                    playerPosition = WallCollision.correctCollision(smallestCorrectionX, true, playerPosition);
                else
                    directionX = DirectionX.None;
            }
            else if (correctX == true) // start with X
            {
                playerPosition = WallCollision.correctCollision(smallestCorrectionX, true, playerPosition);
                playerRect = UpdateRect(playerRect, playerPosition);
                if (WallCollision.IsCollidingWith(collidedObj, playerRect))
                    playerPosition = WallCollision.correctCollision(smallestCorrectionY, false, playerPosition);
                else
                {
                    onGround = true;
                    directionY = DirectionY.None;
                }
            }

                       
            return playerPosition;
        }

        static private int SumHorizontal(List<CorrectionVector2> corrections)
        {
            int counter = 0;
            foreach (CorrectionVector2 cv2 in corrections)
            {
                counter += (int)cv2.DirectionX;
            }
            return counter;
        }

        static private int SumVertical(List<CorrectionVector2> corrections)
        {
            int counter = 0;
            foreach (CorrectionVector2 cv2 in corrections)
            {
                counter += (int)cv2.DirectionY;
            }
            return counter;
        }

        static private CorrectionVector2 getSmallestCorrectionX(DirectionX directionX, List<CorrectionVector2> corrections)
        {
            CorrectionVector2 smallest = new CorrectionVector2();
            smallest.X = int.MaxValue;

            foreach (CorrectionVector2 correction in corrections)
            {
                if (correction.DirectionX == directionX && correction.X < smallest.X)
                    smallest = correction;
            }

            return smallest;
        }

        static private CorrectionVector2 getSmallestCorrectionY(DirectionY directionY, List<CorrectionVector2> corrections)
        {
            CorrectionVector2 smallest = new CorrectionVector2();
            smallest.Y = int.MaxValue;

            foreach (CorrectionVector2 correction in corrections)
            {
                if (correction.DirectionY == directionY && correction.Y < smallest.Y)
                    smallest = correction;
            }

            return smallest;
        }


        static private CorrectionVector2 GetCorrectionVector(Rectangle target, Rectangle playerRect)
        {
            CorrectionVector2 ret = new CorrectionVector2();

            float x1 = Math.Abs(playerRect.Right - target.Left);
            float x2 = Math.Abs(playerRect.Left - target.Right);
            float y1 = Math.Abs(playerRect.Bottom - target.Top);
            float y2 = Math.Abs(playerRect.Top - target.Bottom);

            // calculate displacement along X-axis
            if (x1 < x2)
            {
                ret.X = x1;
                ret.DirectionX = DirectionX.Left;
            }
            else if (x1 > x2)
            {
                ret.X = x2;
                ret.DirectionX = DirectionX.Right;
            }

            // calculate displacement along Y-axis
            if (y1 < y2)
            {
                ret.Y = y1;
                ret.DirectionY = DirectionY.Up;
            }
            else if (y1 > y2)
            {
                ret.Y = y2;
                ret.DirectionY = DirectionY.Down;
            }

            return ret;
        }

        static private Vector2 correctCollision(CorrectionVector2 correction, bool correctHorizontal, Vector2 playerPosition)
        {
            if (correctHorizontal) // horizontal
                playerPosition.X += correction.X * (float)correction.DirectionX;
            else // vertical
                playerPosition.Y += correction.Y * (float)correction.DirectionY;

            return playerPosition;
        }

        static private bool IsCollidingWith(List<Rectangle> spritesThatMightBeColliding, Rectangle playerRect)
        {
            foreach (Rectangle sprite in spritesThatMightBeColliding)
            {
                if (playerRect.Intersects(sprite))
                    return true;
            }

            return false;
        }

        static private Rectangle UpdateRect(Rectangle originRect, Vector2 position)
        {
            return new Rectangle((int)position.X, (int)position.Y, originRect.Width, originRect.Height);
        }

    }
}

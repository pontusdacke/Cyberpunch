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
using System.IO;

namespace Cyberpunch_game
{
    public class Map : DrawableGameComponent
    {
        public Camera camera;

        SpriteBatch spritebatch;

        #region variabler
        private List<TileData> tiles = new List<TileData>();
        private string mapLocation = "Map1";    //Kan bytas ut om man senare har flera olika banor och man vill kunna välja vilken man vill lira
        private Rectangle whiteSpaceRect = new Rectangle(0, 0, 50, 50);
        public List<Vector2> enemySpawnerPositionList = new List<Vector2>();
        #endregion

        #region Properties
        public List<TileData> GetTiles
        {
            get { return tiles; }
        }

        public Map(Game game)
            : base(game)
        {

        }
        #endregion

        protected override void LoadContent()
        {
            spritebatch = new SpriteBatch(GraphicsDevice);

            #region Läsa in tileData innan man läser textfilen för hur bannan ska byggas upp
            List<TileData> checkTiles = new List<TileData>();   //denna lista håller alla olika typer av tiles som finns, dessa kommer sedan att användas för att kopiera data till alla tiles som ska vara av denna typen
            StreamReader reader = new StreamReader("Content/tiledata.txt"); //Textfilen med tiledata
            while (reader.ReadLine() != "**")   //läser tills filen är färdigläst
            {
                string tempId = reader.ReadLine();  //Idet för texturen
                string textureName = reader.ReadLine(); //namnet på texturen

                if (tempId == " ")  //specialfall om det är whitespace dvs inget alls
                {
                    //skapar ändå en tiledata som enbart innehåller information om hur stort utrymme som en icke tile ska representera
                    //denna variabel går att ändra i denna class, heter "whiteSpaceRect"
                    checkTiles.Add(new TileData(new Rectangle(int.Parse(reader.ReadLine()), int.Parse(reader.ReadLine()), int.Parse(reader.ReadLine()), int.Parse(reader.ReadLine())), null, tempId));
                }
                else    //annars skapas en ny tiledata och man laddar in texturen den ska ha och gör des kolliionsrektangel lika stor som texturen
                {
                    Texture2D temp = Game.Content.Load<Texture2D>("Tile/" + textureName);
                    checkTiles.Add(new TileData(new Rectangle(int.Parse(reader.ReadLine()), int.Parse(reader.ReadLine()), int.Parse(reader.ReadLine()), int.Parse(reader.ReadLine())), temp, tempId));
                }

            }
            // List<TileData> checkTiles = new List<TileData>();   //denna lista håller alla olika typer av tiles som finns, dessa kommer sedan att användas för att kopiera data till alla tiles som ska vara av denna typen
            //StreamReader reader = new StreamReader("tiledata.txt"); //Textfilen med tiledata
            //while (reader.ReadLine() != "**")   //läser tills filen är färdigläst
            //{
            //    string tempId = reader.ReadLine();  //Idet för texturen
            //    string textureName = reader.ReadLine(); //namnet på texturen

            //    if (tempId == " ")  //specialfall om det är whitespace dvs inget alls
            //    {
            //        //skapar ändå en tiledata som enbart innehåller information om hur stort utrymme som en icke tile ska representera
            //        //denna variabel går att ändra i denna class, heter "whiteSpaceRect"
            //        checkTiles.Add(new TileData(new Rectangle(whiteSpaceRect.X, whiteSpaceRect.Y, whiteSpaceRect.Width, whiteSpaceRect.Height), null, tempId));
            //    }
            //    else    //annars skapas en ny tiledata och man laddar in texturen den ska ha och gör des kolliionsrektangel lika stor som texturen
            //    {
            //        Texture2D temp = Game.Content.Load<Texture2D>(textureName);
            //        checkTiles.Add(new TileData(temp.Bounds, temp, tempId));
            //    }

            //}
            #endregion

            #region Tolkar textfilen för hur bannan ska ritas ut och skapar en lista av TileData
            reader = new StreamReader("Content/" + mapLocation + ".txt");
            Point drawpos = new Point(0, 0);    //postionen där nästa tile ska ritas ut
            string banaText = reader.ReadToEnd();
            bool addToY = false;

            for (int i = 0; i < banaText.Length; i++)
            {
                string nextchar = char.ToString(banaText[i]);
                TileData temp = checkTiles.Find(delegate(TileData tD) { return tD.GetIdentifier == nextchar; });
                if (nextchar == "\n")   //om man byter rad, måste öka på y pos
                    addToY = true;

                if (temp != null)   //då var det någon giltig karaktär som lästes in
                {
                    if (addToY == true) //man ska byta rad, även nollställa X-led
                    {
                        drawpos.Y += 200;
                        //if (temp.GetTexture != null)
                        //    drawpos.Y += temp.GetTexture.Height;
                        //else
                        //    drawpos.Y += temp.GetBoundRect.Height;
                        drawpos.X = 0;
                        addToY = false;
                    }

                    if (temp.GetTexture == null)    //om det är ett whitespace, ingen textur läggs till men man ökar xled
                    {

                    }
                    else    //om det inte är whitespace ritar man ut vad det nu ska vara
                    {
                        if (temp.GetIdentifier != "s")
                        {
                            tiles.Add(new TileData(new Rectangle(temp.GetBoundRect.X + drawpos.X, temp.GetBoundRect.Y + drawpos.Y, temp.GetBoundRect.Width, temp.GetBoundRect.Height),
                                                   temp.GetTexture, nextchar, new Point(drawpos.X, drawpos.Y)));
                        }
                        else
                        { 
                            enemySpawnerPositionList.Add(new Vector2(drawpos.X, drawpos.Y));
                        }
                    }

                    drawpos.X += 200;
                    //if (temp.GetTexture != null)
                    //    drawpos.X += temp.GetTexture.Width;
                    //else
                    //    drawpos.X += temp.GetBoundRect.Width;
                }
            }
            #endregion

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            if (camera != null)
            {
                spritebatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.viewMatrix);

                foreach (TileData tD in tiles)
                    spritebatch.Draw(tD.GetTexture, new Vector2(tD.DrawPos.X, tD.DrawPos.Y), Color.White);

                spritebatch.End();
            }
            base.Draw(gameTime);
        }

    }
}

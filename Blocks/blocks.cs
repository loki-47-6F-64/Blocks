using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blocks
{
    public class group_block
    {
        private char[][] chShape;
        public int pos_x, pos_y;
        private int GroupSize_x, GroupSize_y;
        private int BackColor = 16, TextColor = 16;

        public group_block(int x, int y)
        {
            GroupSize_x = 1;
            GroupSize_y = 1;
            chShape = new char[GroupSize_y][];
            chShape[0] = new char[GroupSize_x];
            pos_x = x;
            pos_y = y;
        }

        public group_block(int x, int y, int size_x, int size_y)
        {
            chShape = new char[size_y][];
            for (int array_y = 0; array_y < size_y; array_y++)
            {
                chShape[array_y] = new char[size_x];
            }
            GroupSize_x = size_x;
            GroupSize_y = size_y;
            pos_x = x;
            pos_y = y;
        }

        public int GetSize_x()
        {
            return GroupSize_x;
        }
        public int GetSize_y()
        {
            return GroupSize_y;
        }

        public char GetShape(int x, int y)
        {
            if (x < GroupSize_x && x >= 0 && y < chShape.Length && y >= 0)
            {
                return chShape[y][x];
            }
            else return (char)0;
        }
        public char[][] GetShape()
        {
            return chShape;
        }

        public void SetColor(int iText, int iBack)
        {
            TextColor = iText;
            BackColor = iBack;
        }
        public int GetBackColor()
        {
            return BackColor;
        }
        public int GetTextColor()
        {
            return TextColor;
        }

        public void SetShape(char chPiece, int x, int y)// Set a piece of the shape
        {
            chShape[y][x] = chPiece;
        }
        public void SetShape(char[][] Shape) //Set the entire shape.
        {
            this.chShape = Shape;
        }
        public void PrintShape() //Make the object visible.
        {
            int StoreTextColor = 16;
            int StoreBackgroundColor = 16;
            //If the color has been defined it needs to be stored first.
            if ((TextColor <= 15 || BackColor <= 15) && (TextColor >= 0 || BackColor >= 0))
            {
                StoreTextColor = (int)Console.ForegroundColor; //Store the colors
                StoreBackgroundColor = (int)Console.BackgroundColor;
                //Set the colors if they have been defined.
                if (TextColor <= 15 && TextColor >= 0)
                {
                    Console.ForegroundColor = (ConsoleColor)TextColor;
                }
                if (BackColor <= 15 && BackColor >= 0)
                {
                    Console.BackgroundColor = (ConsoleColor)BackColor;
                }
            }
            for (int x = 0; x < GroupSize_x; x++)
            {
                for (int y = 0; y < chShape.Length; y++)
                {
                    Console.SetCursorPosition(pos_x + x, pos_y + y);
                    Console.Write(chShape[y][x]);
                }
            }
            if ((TextColor <= 15 || BackColor <= 15) && (TextColor >= 0 || BackColor >= 0))
            {
                Console.ForegroundColor = (ConsoleColor)StoreTextColor;
                Console.BackgroundColor = (ConsoleColor)StoreBackgroundColor;
            }
        }

        public void Clear() //Make the object visible.
        {
                for (int x = 0; x < GroupSize_x; x++)
                {
                    for (int y = 0; y < chShape.Length; y++)
                    {
                        Console.SetCursorPosition(pos_x + x, pos_y + y);
                        Console.Write(' ');
                    }
                }
        }

        //Collision detection.
        public static bool bCollision(group_block cObject1, group_block cObject2)
        {
            bool bResult = false;

            /* Er zijn zeven verschillende mogelijkheden
             * Als het object een ander object overschrijft zijn een van de hoeken van het object in het andere object
             * Of het doorkruist het object verticaal of horizontaal
             * Of het object overlapt een ander object volledig*/
            //Linker boven hoek
            if (cObject2.pos_x >= cObject1.pos_x &&
                cObject2.pos_x < cObject1.pos_x + cObject1.GetSize_x() &&
                cObject2.pos_y >= cObject1.pos_y &&
                cObject2.pos_y < cObject1.pos_y + cObject1.GetSize_y()
                || //Rechter boven hoek
                cObject2.pos_x + cObject2.GetSize_x() > cObject1.pos_x &&
                cObject2.pos_x + cObject2.GetSize_x() <= cObject1.pos_x + cObject1.GetSize_x() &&
                cObject2.pos_y >= cObject1.pos_y &&
                cObject2.pos_y < cObject1.pos_y + cObject1.GetSize_y()
                || //Rechter grondhoek
                cObject2.pos_x + cObject2.GetSize_x() > cObject1.pos_x &&
                cObject2.pos_x + cObject2.GetSize_x() <= cObject1.pos_x + cObject1.GetSize_x() &&
                cObject2.pos_y + cObject2.GetSize_y() > cObject1.pos_y &&
                cObject2.pos_y + cObject2.GetSize_y() <= cObject1.pos_y + cObject1.GetSize_y()
                || //Linker grondhoek
                cObject2.pos_x >= cObject1.pos_x &&
                cObject2.pos_x < cObject1.pos_x + cObject1.GetSize_x() &&
                cObject2.pos_y + cObject2.GetSize_y() > cObject1.pos_y &&
                cObject2.pos_y + cObject2.GetSize_y() <= cObject1.pos_y + cObject1.GetSize_y()
                || //Verticale kruising
                ((cObject2.pos_x >= cObject1.pos_x &&
                cObject2.pos_x < cObject1.pos_x + cObject1.GetSize_x()) ||
                (cObject2.pos_x + cObject2.GetSize_x() > cObject1.pos_x &&
                cObject2.pos_x + cObject2.GetSize_x() < cObject1.pos_x + cObject1.GetSize_x())) &&
                cObject2.pos_y <= cObject1.pos_y &&
                cObject2.pos_y + cObject2.GetSize_y() > cObject1.pos_y + cObject1.GetSize_y()
                || //horizontale kruising
                ((cObject2.pos_y >= cObject1.pos_y &&
                cObject2.pos_y < cObject1.pos_y + cObject1.GetSize_y()) ||
                (cObject2.pos_y + cObject1.GetSize_y() > cObject1.pos_y &&
                cObject2.pos_y + cObject1.GetSize_y() < cObject1.pos_y + cObject1.GetSize_y())) &&
                cObject2.pos_x <= cObject1.pos_x &&
                cObject2.pos_x + cObject2.GetSize_x() > cObject1.pos_x + cObject1.GetSize_x()
                ||//Volledige overlapping
                cObject2.pos_x <= cObject1.pos_x &&
                cObject2.pos_x + cObject2.GetSize_x() >= cObject1.pos_x + cObject1.GetSize_x() &&
                cObject2.pos_y <= cObject1.pos_y &&
                cObject2.pos_y + cObject2.GetSize_y() >= cObject1.pos_y + cObject1.GetSize_y())
            {
                bResult = true;
            }
            return bResult;
        }
    }
}

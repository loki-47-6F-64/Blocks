using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blocks
{
    /* Create a box wich has text in the middle
     * Give the user a choise or choises depending on the kind of box
     * Return the choise */
    public class pop_up
    {
        group_block[] cBox;
        Menu cChoise;

        public char chWall;
        private int pos_x;
        private int pos_y;

        //The colors
        private int TextColorWall = 16;
        private int BackColorWall = 16;

        private int ColorText = 16;
        private int BackColorText = 16;

        private int ColorSurrounding = 16;
        private int ColorSign = 16;


        public pop_up(int x, int y)
        {
            chWall = Constant.chBlock;
            pos_x = x;
            pos_y = y;
        }
        public pop_up(char wall, int x, int y)
        {
            chWall = wall;
            pos_x = x;
            pos_y = y;
        }
        public bool Confirm(string sText)//yes or no
        {
            //Create a group_block with the shape of the string sText
            //Its more easy to manipulate than a string
            group_block cText;
            cChoise = new Menu();

            int position;
            if (sText.Length > 9)
            {
                box(sText.Length + 8, 7);
                position = sText.Length / 3 + 2;
                cText = new group_block(pos_x + 4, pos_y + 2, sText.Length, 1);
            }
            else
            {
                box(17, 7);
                position = 5;
                Console.SetCursorPosition(pos_x + (17 - sText.Length) / 2, pos_y + 2);
                cText = new group_block(pos_x + (17 - sText.Length) / 2, pos_y + 2, sText.Length, 1);
            }

            for (int x = 0; x < sText.Length; x++)
            {
                cText.SetShape(sText[x], x, 0); //zet de vorm van de regel
            }
            cText.SetColor(ColorText, BackColorText);
            cText.PrintShape();

            cChoise.CreateItem("yes", pos_x + position, pos_y + 4);
            cChoise.CreateItem("no", pos_x + position * 2, pos_y + 4);

            cChoise.SetColor(0, ColorText, BackColorText);
            cChoise.SetColor(1, ColorText, BackColorText);

            //Set the cursor with color
            cChoise.SetCursor(Constant.chArrow.ToString(), ColorText, 16);

            //Maak de keuze
            if (Choose() == 0)
            {
                ErazeBox(sText.Length + 8, 7);
                return true;
            }
            else
            {
                ErazeBox(sText.Length + 8, 7);
                return false;
            }
        }
        public int Choise(string sText, string[] sChoises)//multiple choise
        {
            cChoise = new Menu();

            int iLength_x = sText.Length;

            for (int x = 0; x < sChoises.Length; x++)
            {
                if (sChoises[x].Length > iLength_x)
                {
                    iLength_x = sChoises[x].Length;
                }
            }

            int iLength_y = sChoises.Length + 6;
            iLength_x += 8;


            int position = (iLength_x - sText.Length) / 2;
            box(iLength_x, iLength_y);

            //Create a group_block with the shape of the string sText
            //Its more easy to manipulate than a string
            group_block cText = new group_block(pos_x + position, pos_y + 2, sText.Length, 1);
            for (int x = 0; x < sText.Length; x++)
            {
                cText.SetShape(sText[x], x, 0); //zet de vorm van de regel
            }
            cText.SetColor(ColorText, BackColorText);
            cText.PrintShape();

            for (int y = 0; y < sChoises.Length; y++)
            {
                position = (iLength_x - sChoises[y].Length) / 2;
                cChoise.CreateItem(sChoises[y], pos_x + position, pos_y + y + 4);
                cChoise.SetColor(y, ColorText, BackColorText);
            }

            //Set the cursor with color
            cChoise.SetCursor(Constant.chArrow.ToString(), ColorText, 16);

            int iChosen = Choose();
            ErazeBox(iLength_x, iLength_y);

            return iChosen;
        }
        public void Message(string sText)//Show a message
        {
            cChoise = new Menu();

            box(sText.Length + 8, 7);

            //Create a group_block with the shape of the string sText
            //Its more easy to manipulate than a string
            group_block cText = new group_block(pos_x + 4, pos_y + 2, sText.Length, 1);
            for (int x = 0; x < sText.Length; x++)
            {
                cText.SetShape(sText[x], x, 0); //zet de vorm van de regel
            }
            cText.SetColor(ColorText, BackColorText);
            cText.PrintShape();

            int position = pos_x + sText.Length / 2 + 3;
            cChoise.CreateItem("Ok", position, pos_y + 4);

            cChoise.SetColor(0, ColorText, BackColorText);

            //Set the cursor with color
            cChoise.SetCursor(Constant.chArrow.ToString(), ColorText, 16);

            Choose();

            ErazeBox(sText.Length + 8, 7);
        }
        private void box(int size_x, int size_y)
        {
            if (ColorSign <= 15)
            {
                Console.BackgroundColor = (ConsoleColor)ColorSign;
            }
            //First, clear the space for the box.
            for (int y = 0; y < size_y; y++)
            {
                for (int x = 0; x < size_x; x++)
                {
                    Console.SetCursorPosition(pos_x + x, pos_y + y);
                    Console.Write(" ");
                }
            }

            cBox = new group_block[4]; //4 walls
            cBox[0] = new group_block(pos_x, pos_y, 1, size_y); //Left wall
            cBox[1] = new group_block(pos_x + size_x - 1, pos_y, 1, size_y); //Right wall
            cBox[2] = new group_block(pos_x, pos_y, size_x, 1); //upper wall
            cBox[3] = new group_block(pos_x, pos_y + size_y - 1, size_x, 1); //lower wall
            for (int z = 0; z < 4; z++)//the shape
            {
                for (int y = 0; y < cBox[z].GetSize_y(); y++)
                {
                    for (int x = 0; x < cBox[z].GetSize_x(); x++)
                    {
                        cBox[z].SetShape(chWall, x, y);
                    }
                }
                cBox[z].SetColor(TextColorWall, BackColorWall);
                cBox[z].PrintShape();
            }
        }
        private int Choose()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo Input = Console.ReadKey(true);
                    switch (Input.Key)
                    {
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow:
                            cChoise.MoveCursor(-1);
                            break;
                        case ConsoleKey.D:
                        case ConsoleKey.S:
                        case ConsoleKey.RightArrow:
                        case ConsoleKey.DownArrow:
                            cChoise.MoveCursor(1);
                            break;
                        case ConsoleKey.Enter:
                            return cChoise.pos_cursor;
                    }
                }
            }
        }
        private void ErazeBox(int size_x, int size_y)//Clear the box
        {
            if (ColorSurrounding <= 15)
            {
                //First set the color to the original settings.
                Console.BackgroundColor = (ConsoleColor)ColorSurrounding;
            }
            //Then clear the box
            for (int y = 0; y < size_y; y++)
            {
                for (int x = 0; x < size_x; x++)
                {
                    Console.SetCursorPosition(pos_x + x, pos_y + y);
                    Console.Write(" ");
                }
            }
            cBox = null;
        }

        public void SetColorWall(int iText, int iBack)//the color of the wall
        {
            TextColorWall = iText;
            BackColorWall = iBack;
        }

        public void SetColorText(int iText, int iBack)//the color of the text inside the box
        {
            ColorText = iText;
            BackColorText = iBack;
        }

        public void SetColorSign(int iBack)
        {
            ColorSurrounding = (int)Console.BackgroundColor;
            ColorSign = iBack;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blocks
{
    public class TextBox
    {
        //The colors
        private int TextColorWall = 16;
        private int BackColorWall = 16;

        private int ColorText = 16;
        private int ColorOriginText = 16;

        private int ColorOriginField = 16;
        private int ColorField = 16;

        public int pos_x, pos_y;
        private int iLength_x, iLength_y;
        public char chWall; //The walls of the textBox

        private int MaxCharacter; //The maximum amount of chars displayed in the box.
        private char[] chSentence;

        public TextBox(int x, int y, int size_x)
        {
            chWall = Constant.chBlock;
            pos_x = x;
            pos_y = y;
            iLength_x = size_x;
            iLength_y = 3;
            MaxCharacter = iLength_x; //De grootte van de string.
        }
        public void NewBox()
        {
            if (ColorField <= 15)//De achtergrond kleur veranderen
            {
                Console.BackgroundColor = (ConsoleColor)ColorField;
            }
            for (int y = 0; y < iLength_y; y++)
            {
                Console.SetCursorPosition(pos_x, pos_y + y);
                for (int x = 0; x < iLength_x; x++)
                {
                    Console.Write(" ");
                }
            }

            group_block[] cWall = new group_block[4];
            cWall[0] = new group_block(pos_x, pos_y, iLength_x, 1); //bovenste muur
            cWall[1] = new group_block(pos_x, pos_y + iLength_y - 1, iLength_x, 1); //onderste muur
            cWall[2] = new group_block(pos_x, pos_y, 1, iLength_y); //linker muur
            cWall[3] = new group_block(pos_x + iLength_x - 1, pos_y, 1, iLength_y); //rechter muur

            for (int z = 0; z < 4; z++)//the shape
            {
                for (int y = 0; y < cWall[z].GetSize_y(); y++)
                {
                    for (int x = 0; x < cWall[z].GetSize_x(); x++)
                    {
                        cWall[z].SetShape(chWall, x, y);
                    }
                }
                cWall[z].SetColor(TextColorWall, BackColorWall);
                cWall[z].PrintShape();
            }
            if (ColorOriginField <= 15) //Reset the background color.
            {
                Console.BackgroundColor = (ConsoleColor)ColorOriginField;
            }
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public string sWrite()
        {
            if (ColorField <= 15 && ColorField >= 0)//Change the background color.
            {
                Console.BackgroundColor = (ConsoleColor)ColorField;
            }
            if (ColorField <= 15 && ColorText >= 0)//Change the color of the text.
            {
                Console.ForegroundColor = (ConsoleColor)ColorText;
            }

            chSentence = new char[MaxCharacter];

            Console.SetCursorPosition(pos_x + 1, pos_y + 1);

            bool bVisible = Console.CursorVisible; //Remember the setting for the visibillity of the cursor before altering it.
            Console.CursorVisible = true;

            string sResult = "";

            int iCharacter = 0;
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo Input = Console.ReadKey(true);
                    if (Input.Key == ConsoleKey.Escape)
                    {
                        iCharacter = 0; //Brake of the loop without an return value.
                        break;
                    }
                    if (Input.Key == ConsoleKey.Enter)
                    {
                        break; //Brake of the loop with an return value.
                    }
                    if (Input.Key == ConsoleKey.Backspace)
                    {
                        if (iCharacter > 0) //First check if there is a char in the array before attempting to remove it.
                        {
                            Console.SetCursorPosition(Console.CursorLeft - 1, pos_y + 1);
                            Console.Write(" ");

                            chSentence[iCharacter] = (char)0; //Remove the char at the end of the array.
                            iCharacter--;

                            //Dont use to much unneccesary memory.
                            if (MaxCharacter - iCharacter > 20)
                            {
                                MaxCharacter -= 10;
                                Array.Resize(ref chSentence, MaxCharacter);
                            }
                        }
                    }
                    //is the char from the keyboard?
                    else if ((int)Input.KeyChar >= 33 && (int)Input.KeyChar <= 126)
                    {
                        if (iCharacter >= MaxCharacter)
                        {
                            MaxCharacter += 10;
                            Array.Resize(ref chSentence, MaxCharacter);
                        }
                        chSentence[iCharacter] = Input.KeyChar;
                        iCharacter++;
                    }
                    Console.SetCursorPosition(pos_x + 1, pos_y + 1);
                    //Only show a part of the array.
                    for (int x = iCharacter - iLength_x + 2; x < iCharacter; x++)
                    {
                        if (x >= 0)
                        {
                            Console.Write(chSentence[x]);
                        }
                    }
                }
            } //End of loop.

            for (int x = 0; x < iCharacter; x++)
            {
                sResult += chSentence[x];
            }

            Console.SetCursorPosition(pos_x + 1, pos_y + 1);
            for (int x = 1; x < iLength_x - 1; x++)
            {
                Console.Write(" "); //Remove the output in the box.
            }

            Console.CursorVisible = bVisible; //Put the visibillity back to its previous setting.

            if (ColorOriginField <= 15 && ColorOriginField >= 0) //Reset the color of the background
            {
                Console.BackgroundColor = (ConsoleColor)ColorOriginField;
            }
            if (ColorOriginText <= 15 && ColorOriginText >= 0) //rest the color of the text.
            {
                Console.ForegroundColor = (ConsoleColor)ColorOriginText;
            }

            return sResult; //Finally retrun the input.
        }
        public void clear_box() //Clear the entire textfield.
        {
            for (int y = 0; y < 3; y++)
            {
                Console.SetCursorPosition(pos_y + y, pos_x);
                for (int x = 0; x < iLength_x; x++)
                {
                    Console.Write(" ");
                }
            }
        }

        public void SetColorWall(int iText, int iBack)
        {
            TextColorWall = iText;
            BackColorWall = iBack;
        }

        public void SetColorText(int iText)
        {
            ColorText = iText;
            ColorOriginText = (int)Console.ForegroundColor;
        }

        public void SetColorBackground(int iBack)
        {
            ColorOriginField = (int)Console.BackgroundColor;
            ColorField = iBack;
        }
    }
}

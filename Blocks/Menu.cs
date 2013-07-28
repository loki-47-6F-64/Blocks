using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blocks
{
    public class Menu
    {
        public group_block[] cMenu;
        public group_block cCursor;

        public int pos_cursor;
        public bool MoveCursorForMenu; //If enabeled the Menu wont make room for the cursor
        public Menu()
        {
            pos_cursor = 0;
            MoveCursorForMenu = true;
            cMenu = new group_block[0];
            //Allocate the cursor without shape or size.
            cCursor = new group_block(0, 0, 0, 0); //Then the Menu knows it doesnt have to print it.
        }
        //ID tells wich Menu needs to change color.
        public void SetColor(int ID, int iText, int iBack)
        {
            cMenu[ID].SetColor(iText, iBack);
            PrintMenu(ID, ID);
        }

        public void SetCursor(string sCursor) //Cursor with default color
        {
            cCursor = new group_block(cMenu[pos_cursor].pos_x, cMenu[pos_cursor].pos_y, sCursor.Length, 1);
            for (int x = 0; x < sCursor.Length; x++)
            {
                cCursor.SetShape(sCursor[x], x, 0);
            }
            PrintMenu(pos_cursor, pos_cursor);
        }

        public void SetCursor(string sCursor, int iText, int iBack) //Cursor with custom color.
        {
            cCursor = new group_block(cMenu[pos_cursor].pos_x, cMenu[pos_cursor].pos_y, sCursor.Length, 1);
            for (int x = 0; x < sCursor.Length; x++)
            {
                cCursor.SetShape(sCursor[x], x, 0);
            }
            cCursor.SetColor(iText, iBack);
            PrintMenu(pos_cursor, pos_cursor);
        }

        public void MoveCursor(int iMovement)
        {
            //Is the new position of the cursor within the boundaries?
            if (pos_cursor + iMovement >= 0 && pos_cursor + iMovement < cMenu.Length)
            {
                ClearCursor(); //Clear the cursor at the old spot.
                pos_cursor += iMovement; //Move the cursor

                cCursor.pos_x = cMenu[pos_cursor].pos_x;
                cCursor.pos_y = cMenu[pos_cursor].pos_y;

                PrintMenu(pos_cursor, pos_cursor);
                PrintMenu(pos_cursor - iMovement, pos_cursor - iMovement);
            }
        }

        public void ClearCursor()
        {
            /* If the cursor had been moved for the Menu then the position of the cursor doesnt match
             * the picture you see on the screen.
             * To remove the cursor the position must be changed before callin Clear()
             */
            if (MoveCursorForMenu)
            {

                cCursor.pos_x -= cCursor.GetSize_x();
                cCursor.Clear();
                cCursor.pos_x += cCursor.GetSize_x();
            }
            else
            {
                cCursor.Clear();
            }
        }

        public void CreateItem(string sLine, int pos_x, int pos_y)
        {
            Array.Resize(ref cMenu, cMenu.Length + 1);

            cMenu[cMenu.Length - 1] = new group_block(pos_x, pos_y, sLine.Length, 1);
            for (int x = 0; x < sLine.Length; x++)
            {
                cMenu[cMenu.Length - 1].SetShape(sLine[x], x, 0);
            }
            PrintMenu(cMenu.Length - 1, cMenu.Length - 1);
        }

        public void ChangeItem(string sLine, int iItem)
        {
            cMenu[iItem].Clear();

            int pos_x = cMenu[iItem].pos_x;
            int pos_y = cMenu[iItem].pos_y;
            cMenu[iItem] = new group_block(pos_x, pos_y, sLine.Length, 1);
            for (int x = 0; x < sLine.Length; x++)
            {
                cMenu[iItem].SetShape(sLine[x], x, 0); //zet de vorm van de regel
            }
            cMenu[iItem].PrintShape();
        }

        public void DeleteItem(int ID)
        {
            clear_menu(ID, cMenu.Length - 1);
            //Eerst vul het gat van de Menu item die gedelete is.
            for (int item = cMenu.Length - 1; item > ID; item--)
            {
                cMenu[item].pos_x = cMenu[item - 1].pos_x;
                cMenu[item].pos_y = cMenu[item - 1].pos_y;
            }

            for (int item = ID; item < cMenu.Length - 1; item++)
            {
                cMenu[item] = cMenu[item + 1]; //De allerlaatste menu_item wordt automatisch null.
            }
            Array.Resize(ref cMenu, cMenu.Length - 1);
            PrintMenu(ID, cMenu.Length - 1);
        }

        public void MoveItem(int iItem, int extra_x, int extra_y, int abs_x, int abs_y) //abs = absolute
        {
            cMenu[iItem].pos_x += extra_x;
            cMenu[iItem].pos_y += extra_y;

            if (abs_x >= 0 && abs_y >= 0)
            {
                cMenu[iItem].pos_x = abs_x;
                cMenu[iItem].pos_y = abs_y;
            }
        }

        public void PrintMenu()
        {
            /* If the cursor needs to be printed for the Menu than the position of the cursor needs to be
             * temporarely changed.
             * After it is printed it needs to be changed back.
             */
            for (int x = 0; x < cMenu.Length; x++)
            {
                if (x == pos_cursor)
                {
                    if (!MoveCursorForMenu)
                    {
                        cCursor.PrintShape();
                        cMenu[x].pos_x += cCursor.GetSize_x(); //Make room for the cursor
                        cMenu[x].PrintShape();
                        cMenu[x].pos_x -= cCursor.GetSize_x(); //It is already printed. It can now be put back.
                    }
                    else
                    {
                        cCursor.pos_x -= cCursor.GetSize_x(); //Make room for the Menu
                        cCursor.PrintShape(); //print the Menu
                        cCursor.pos_x += cCursor.GetSize_x(); //It is already printed. It can now be put back.
                        cMenu[x].PrintShape();
                    }
                }
                /* Because there could be a chanche that the Menu item previously had an cursor attached
                 * It needs to be cleaned up.
                 */
                else if (!MoveCursorForMenu)
                {
                    cMenu[x].PrintShape();
                    for (int y = 0; y < cCursor.GetSize_x(); y++)
                    {
                        Console.Write(" ");
                    }
                }
                else if (MoveCursorForMenu)
                {
                    Console.SetCursorPosition(cMenu[x].pos_x - cCursor.GetSize_x(), cMenu[x].pos_y);
                    for (int y = 0; y < cCursor.GetSize_x(); y++)
                    {
                        Console.Write(" ");
                    }
                    cMenu[x].PrintShape();
                }
            }
        }

        public void PrintMenu(int ID_start, int ID_end) //Print a part the Menu.
        {
            for (int x = ID_start; x <= ID_end && x < cMenu.Length; x++)
            {
                if (pos_cursor == x) //The Menu line wth the cursor.
                {
                    if (!MoveCursorForMenu)
                    {
                        cCursor.PrintShape(); //print the cursor
                        cMenu[x].pos_x += cCursor.GetSize_x();
                        cMenu[x].PrintShape();
                        cMenu[x].pos_x -= cCursor.GetSize_x();
                    }
                    else if (MoveCursorForMenu)
                    {
                        cCursor.pos_x -= cCursor.GetSize_x();
                        cCursor.PrintShape();
                        cCursor.pos_x += cCursor.GetSize_x();
                        cMenu[x].PrintShape();
                    }
                }
                else if (!MoveCursorForMenu)
                {
                    cMenu[x].PrintShape();
                    for (int y = 0; y < cCursor.GetSize_x(); y++)
                    {
                        Console.Write(" ");
                    }
                }
                else if (MoveCursorForMenu)
                {
                    Console.SetCursorPosition(cMenu[x].pos_x - cCursor.GetSize_x(), cMenu[x].pos_y);
                    for (int y = 0; y < cCursor.GetSize_x(); y++)
                    {
                        Console.Write(" ");
                    }
                    cMenu[x].PrintShape();
                }
            }
        }

        public void clear_menu()
        {
            ClearCursor();

            for (int x = 0; x < cMenu.Length; x++)
            {
                cMenu[x].Clear();
            }
        }

        public void clear_menu(int ID_start, int ID_end) //clear a part of the Menu.
        {
            ClearCursor();

            for (int x = ID_start; x <= ID_end; x++)
            {
                cMenu[x].Clear();
            }
        }
        public int GetLength()
        {
            return cMenu.Length;
        }
    }
}

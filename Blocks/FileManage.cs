using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Blocks
{
    public class FileManage
    {
        public static bool SaveField(string FileName, group_block[] cField, int iObject)
        {
            if (FileName != "")//The file must have a name.
            {
                StreamWriter cSave = new StreamWriter(FileName);

                cSave.Write("iObject: "); cSave.WriteLine(iObject);
                cSave.WriteLine("");

                for (int x = 0; x < cField.Length && cField[x] != null; x++)
                {
                    //The object
                    cSave.Write("Object "); cSave.Write(x); cSave.WriteLine(":");
                    //The size
                    cSave.Write("size_x =\""); cSave.Write(cField[x].GetSize_x()); cSave.WriteLine("\"");
                    cSave.Write("size_y =\""); cSave.Write(cField[x].GetSize_y()); cSave.WriteLine("\"");
                    //The position
                    cSave.Write("pos_x =\""); cSave.Write(cField[x].pos_x); cSave.WriteLine("\"");
                    cSave.Write("pos_y =\""); cSave.Write(cField[x].pos_y); cSave.WriteLine("\"");
                    //The color
                    cSave.Write("TextColor =\""); cSave.Write(cField[x].GetTextColor()); cSave.WriteLine("\"");
                    cSave.Write("BackgroundColor =\""); cSave.Write(cField[x].GetBackColor()); cSave.WriteLine("\"");

                    cSave.WriteLine("//"); //End of the properties of the object.

                    //The shape
                    for (int piece_y = 0; piece_y < cField[x].GetSize_y(); piece_y++)
                    {
                        for (int piece_x = 0; piece_x < cField[x].GetSize_x(); piece_x++)
                        {
                            cSave.Write(cField[x].GetShape(piece_x, piece_y));
                        }
                        cSave.WriteLine("");
                    }
                }
                cSave.Close();
                return true; //It succeeded
            }
            return false; //It failed
        }
        public bool load_field(string FileName, out group_block[] cField, int start_x, int start_y)
        {
            int iObject = 0;

            cField = new group_block[0];

            bool bSucces = File.Exists(FileName);
            if (bSucces && FileName != "")
            {
                StreamReader cLoad = new StreamReader(FileName);

                string sInput;

                while (!cLoad.EndOfStream)
                {
                    sInput = cLoad.ReadLine();
                    string[] sResult = sInput.Split(' ');
                    string[] sSeperator = { "\"" };

                    if (sResult[0] == "iObject:") //The amount of objects.
                    {
                        cField = new group_block[Function.StringToInt(sResult[1])];
                    }
                    else
                        if (sResult[0] == "Object")
                        {
                            //The default properties
                            int size_x = 0;
                            int size_y = 0;
                            int pos_x = 0;
                            int pos_y = 0;
                            int TextColor = 16;
                            int BackColor = 16;

                            for (int line = 0; sResult[0] != "//"; line++)
                            {
                                sInput = cLoad.ReadLine();

                                sResult = sInput.Split(sSeperator, StringSplitOptions.RemoveEmptyEntries);

                                if (sResult[0] == "pos_x =") //position
                                {
                                    pos_x = Function.StringToInt(sResult[1]);
                                }
                                else
                                    if (sResult[0] == "pos_y =")
                                    {
                                        pos_y = Function.StringToInt(sResult[1]);
                                    }
                                    else
                                        if (sResult[0] == "size_x =") //size
                                        {
                                            size_x = Function.StringToInt(sResult[1]);
                                        }
                                        else
                                            if (sResult[0] == "size_y =")
                                            {
                                                size_y = Function.StringToInt(sResult[1]);
                                            }
                                            else
                                                if (sResult[0] == "TextColor =") //the color
                                                {
                                                    TextColor = Function.StringToInt(sResult[1]);
                                                }
                                                else
                                                    if (sResult[0] == "BackgroundColor =")
                                                    {
                                                        BackColor = Function.StringToInt(sResult[1]);
                                                    }
                            }
                            //The object may not go outside the boundaries.
                            if (pos_x + start_x > Console.BufferWidth || pos_y + start_y > Console.BufferHeight)
                            {
                                Console.Clear();
                                Console.SetCursorPosition(Console.WindowTop, Console.WindowLeft);
                                Console.Write("Error: the object is outside of the bufferarea.");
                                while (!Console.KeyAvailable) { };
                                Console.ReadKey(true);
                                Console.Clear();

                                return false;
                            }
                            cField[iObject] = new group_block(pos_x + start_x, pos_y + start_y, size_x, size_y);
                            for (int y = 0; y < size_y; y++) //The shape
                            {
                                sInput = cLoad.ReadLine(); //A line of the shape.
                                for (int x = 0; x < size_x; x++)
                                {
                                    cField[iObject].SetShape(sInput[x], x, y);
                                }
                            }
                            cField[iObject].SetColor(TextColor, BackColor);
                            cField[iObject].PrintShape();
                            iObject++; //There is a new object.
                        }
                }
                cLoad.Close();
            }
            return bSucces;
        }
    }
}

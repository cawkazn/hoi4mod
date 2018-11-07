using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TagConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            //AUTOMATE THIS TO A LIST
            //Console.WriteLine("Give me an old tag");
            //string tag = Console.ReadLine();

            //Console.WriteLine("Give me a new tag");
            //string newTag = Console.ReadLine();
            ////AUTOMATE THIS TO A LIST
            //Console.WriteLine("Give me a directory");
            //string directory = Console.ReadLine();
            int level = 0;
            string tag = "X05";
            string newTag = "BAV";
            string directory = "C:\\myprojects\\hoi4mod\\test";

            SlamItOut(tag, directory, level, newTag);

            Console.WriteLine("hello");
            Console.ReadLine();
        }

        public static void SlamItOut(string tag, string directory, int level, string newTag)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);

            if (IgnoredDirectories.Contains(directoryInfo.Name))
            {
                return;
            }
            else
            {
                DirectoryInfo[] directories = directoryInfo.GetDirectories();
                List<DirectoryInfo> bestFuckinList = new List<DirectoryInfo>();

                if (directories != null && directories.Length > 0)
                {
                    for (int i = 0; i < directories.Length; i++)
                    {
                        bestFuckinList.Add(directories[i]);
                    }
                    //LEVEL ONE
                    if (bestFuckinList.Count > 0)
                    {
                        foreach (DirectoryInfo di in bestFuckinList)
                        {
                            SlamItOut(tag, di.FullName, level + 1, newTag);
                        }
                    }
                }
                FileInfo[] filesRaw = directoryInfo.GetFiles();
                List<FileInfo> files = new List<FileInfo>();

                for (int i = 0; i < filesRaw.Length; i++)
                {
                    files.Add(filesRaw[i]);
                }
                foreach (FileInfo fi in files)
                {
                    ProcessFile(fi, level, directoryInfo, tag, newTag);
                }       
            }
        }

        public static void ProcessFile(FileInfo fi, int level, DirectoryInfo directoryInfo, string tag, string newTag)
        {
            switch (level.ToString())
            {
                case "0":
                    break;
                case "1":
                    if (directoryInfo.Name == "history")
                    {
                        break;
                    }
                    if(directoryInfo.Name == "gfx")
                    {
                        break;
                    }
                    break;
                //case "2":
                //    break;
                case "2":
                    bool countryEdited = false;
                    if (directoryInfo.Name == "countries")
                    {
                        //open a text file for the tag to be replaced
                        //if one is found
                        if (File.Exists(fi.FullName))
                        {
                            //check to see if our own tag already exists to change
                            string fileName = fi.Name;
                            if (fileName.Substring(0, 3) == tag)
                            {
                                if (fileName.Length > 3)
                                {
                                    string newFileName = directoryInfo.FullName + "\\" + newTag + fileName.Substring(3);
                                    ProcessCountry(fi, tag, newTag);
                                    File.Delete(newFileName);
                                    File.Move(fi.FullName, newFileName);
                                    countryEdited = true;
                                }
                            }
                            if (!countryEdited)
                            {
                                ProcessCountry(fi, tag, newTag);
                            }
                        }
                        else
                        {
                            Console.WriteLine("CAN't FIND THA FUCKIN " + fi.FullName + "FILE");
                            //NEW COUNTRY;
                        }
                    }
                    if (directoryInfo.Name == "states")
                    {
                        if (File.Exists(fi.FullName))
                        {
                            ProcessCountry(fi, tag, newTag);
                        }
                        else
                        {
                            Console.WriteLine("CAN't FIND THA FUCKIN " + fi.FullName + "FILE");
                            //NEW COUNTRY;
                        }
                    }
                    if (directoryInfo.Name == "units")
                    {
                            if (File.Exists(fi.FullName))
                            {
                                string fileName = fi.Name;
                                if (fileName.Substring(0, 3) == tag)
                                {
                                    if (fileName.Length > 3)
                                    {
                                        string newFileName = directoryInfo.FullName + "\\" + newTag + "_OOB.txt";
                                        File.Delete(newFileName);
                                        File.Move(fi.FullName, newFileName);
                                        ProcessCountry(fi, tag, newTag);
                                    }
                                }
                            }
                        else
                        {
                            Console.WriteLine("CAN't FIND THA FUCKIN " + fi.FullName + "FILE");
                            //NEW COUNTRY;
                        }
                    }

                    if (directoryInfo.Name == "flags")
                    {
                        if(fi.Name.Substring(3) == "X05")
                        {
                            string whatthefuck = "yes";
                        }
                        if (File.Exists(fi.FullName))
                        {
                            string fileName = fi.Name;

                            if (fileName.Substring(0, 3) == tag)
                            {
                                if (fileName.Length > 3)
                                {
                                    string newFileName = directoryInfo.FullName + "\\" + newTag;
                                    File.Delete(newFileName);
                                    File.Move(fi.FullName, newFileName);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("CAN't FIND THA FUCKIN " + fi.FullName + "FILE");
                            //NEW COUNTRY;
                        }
                    }


                    
                    break;
            }
        }

        public static void ProcessCountry(FileInfo fi, string tag, string newTag)
        {
            string file = File.ReadAllText(fi.FullName, Encoding.Unicode);
            string newText = file.Replace(tag.ToUpperInvariant(), newTag.ToUpperInvariant());
            File.WriteAllText(fi.FullName, newText);
        }
        //first level folders
        public static List<string> LevelOne
        {

            get
            {
                return new List<string>() {
                    "history",
                    "gfx",
                    "events"
                };
            }
        }

        //second level folders
        public static List<string> LevelTwo
        {
            get
            {
                return new List<string>()
                {
                    "countries",
                    "states",
                    "units",
                    "flags"
                };
            }
        }

        public static List<string> IgnoredDirectories
        {
            get
            {
                return new List<string>()
                {
                    "common"
                };
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TagConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            ////COMMON/NATIONAL_FOCUS.  it's generating focus files for countries i'm making tags for.   why.


            int level;
            string baseDirectory = "C:\\myprojects\\hoi4mod\\test";
            string outputDirectory = "C:\\myprojects\\hoi4mod\\testoutput";

            ClearDirectory(outputDirectory);
            Directory.CreateDirectory(outputDirectory);
            Copy(baseDirectory, outputDirectory);

            Dictionary<string, TagHelper> tagsToChange = new Dictionary<string, TagHelper>();
            Dictionary<int, CountryMergeHelper> statesToChangeOwnership = new Dictionary<int, CountryMergeHelper>();
            statesToChangeOwnership.Add(659, new CountryMergeHelper("WUR", "BAD", 659));
            tagsToChange.Add("X02", new TagHelper("X02", "BAD"));
            tagsToChange.Add("X58", new TagHelper("X58", "WUR"));
            level = 0;
            SlamItOut(tagsToChange, statesToChangeOwnership, outputDirectory, level);
            //Console.WriteLine("finished tag " + th.oldTag + " to " + th.newTag);

            Console.WriteLine("Tag change done, continue with country merging?");
            Console.ReadLine();
        }

        public static void SlamItOut(Dictionary<string, TagHelper> tagsToChange, Dictionary<int, CountryMergeHelper> statesToChangeOwnership, string directory, int level)
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
                            SlamItOut(tagsToChange, statesToChangeOwnership, di.FullName, level + 1);
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
                    ProcessFile(fi, level, directoryInfo, tagsToChange, statesToChangeOwnership);
                }
            }
        }

        public static void ProcessFile(FileInfo fi, int level, DirectoryInfo directoryInfo, Dictionary<string, TagHelper> tagsToChange, Dictionary<int, CountryMergeHelper> statesToChangeOwner)
        {
            if (File.Exists(fi.FullName))
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
                        if (directoryInfo.Name == "common")
                        {
                            break;
                        }
                        break;
                    //case "2":
                    //    break;
                    case "2":
                        bool filenameEdited = false;
                        if (directoryInfo.Name == "names")
                        {
                            ProcessNamesFile(fi, tagsToChange);
                        }
                        if (directoryInfo.Name == "countries")
                        {
                            //open a text file for the tag to be replaced
                            //if one is found

                            //scrub this file for every tag we're changing
                            ProcessCountryTags(fi, tagsToChange);
                            string fileName = fi.Name;
                            string newFileName = "";

                            //direct lookup this file in our list of tags to change
                            TagHelper th;
                            if (tagsToChange.TryGetValue(fileName.Substring(0, 3), out th))
                            {
                                if (fileName.Length > 3)
                                {
                                    newFileName = directoryInfo.FullName + "\\" + th.newTag + fileName.Substring(3);
                                    File.Delete(newFileName);
                                    File.Move(fi.FullName, newFileName);
                                    filenameEdited = true;
                                }
                            }
                            if (filenameEdited)
                            {
                                fileName = newFileName;
                            }
                        }
                        if(directoryInfo.Name == "country_tags")
                        {
                            ProcessCountryTags(fi, tagsToChange);
                        }
                        if(directoryInfo.Name == "ideas")
                        {
                            ProcessCountryTags(fi, tagsToChange);

                            string fileName = fi.Name;
                            string newFileName = "";

                            //direct lookup this file in our list of tags to change
                            TagHelper th;
                            if (tagsToChange.TryGetValue(fileName.Substring(0, 3), out th))
                            {
                                if (fileName.Length > 3)
                                {
                                    newFileName = directoryInfo.FullName + "\\" + th.newTag + fileName.Substring(3);
                                    File.Delete(newFileName);
                                    File.Move(fi.FullName, newFileName);
                                    filenameEdited = true;
                                }
                            }
                        }
                        if (directoryInfo.Name == "states")
                        {
                            ProcessCountryTags(fi, tagsToChange);

                            int stateId = 0;
                            if (int.TryParse(fi.Name.Replace(fi.Extension, ""), out stateId))
                            {
                                CountryMergeHelper cmh;
                                if (statesToChangeOwner.TryGetValue(stateId, out cmh))
                                {
                                    ChangeStateOwner(fi, cmh.tagToGetRidOf, cmh.tagToMergeTo);
                                }
                            }

                        }
                        if (directoryInfo.Name == "units")
                        {

                            string fileName = fi.Name;
                            TagHelper th;
                            if (tagsToChange.TryGetValue(fileName.Substring(0, 3), out th))
                            {
                                if (fileName.Length > 3)
                                {
                                    string newFileName = directoryInfo.FullName + "\\" + th.newTag + fileName.Substring(3);
                                    ProcessCountry(fi, th.oldTag, th.newTag);
                                    File.Delete(newFileName);
                                    File.Move(fi.FullName, newFileName);
                                }
                            }
                        }

                        if (directoryInfo.Name == "flags")
                        {
                            string fileName = fi.Name;
                            TagHelper th;
                            if (tagsToChange.TryGetValue(fileName.Substring(0, 3), out th))
                            {
                                if (fileName.Length > 3)
                                {
                                    string newFileName = directoryInfo.FullName + "\\" + th.newTag + fileName.Substring(3);
                                    File.Delete(newFileName);
                                    File.Move(fi.FullName, newFileName);
                                }
                            }
                        }

                        break;

                    case "3":
                        if (directoryInfo.Name == "names")
                        {
                            ProcessNamesFile(fi, tagsToChange);
                        }
                        if(directoryInfo.Name == "colors")
                        {
                            ProcessNamesFile(fi, tagsToChange);
                        }
                        break;
                }
            }
            else
            {
                Console.WriteLine("CAN't FIND THA FUCKIN " + fi.FullName + "FILE");
                //NEW COUNTRY;
            }
        }
    

        public static void ProcessCountry(FileInfo fi, string tag, string newTag)
        {
            Console.WriteLine("changing country tags for " + fi.Name);
            string file = File.ReadAllText(fi.FullName, Encoding.UTF8);
            string newText = file.Replace(tag.ToUpperInvariant(), newTag.ToUpperInvariant());
            File.WriteAllText(fi.FullName, newText);
        }

        public static void ProcessCountryTags(FileInfo fi, Dictionary<string, TagHelper> tagsToChange)
        {
            Console.WriteLine("changing country tags for " + fi.Name);
            string file = File.ReadAllText(fi.FullName, Encoding.UTF8);
            string newText = "";
            bool firstRun = true;
            foreach(TagHelper th in tagsToChange.Values)
            {
                if (firstRun)
                {
                    newText = file.Replace(th.oldTag.ToUpperInvariant(), th.newTag.ToUpperInvariant());
                    firstRun = false;                    
                }
                else
                {
                    newText = newText.Replace(th.oldTag.ToUpperInvariant(), th.newTag.ToUpperInvariant());
                }
            }
            if(String.IsNullOrWhiteSpace(newText))
            {
                return;
            }
            File.WriteAllText(fi.FullName, newText);
        }

        public static void ChangeStateOwner(FileInfo fi, string tag, string newTag)
        {
            Console.WriteLine("changing state owner for stateId " + fi.Name);

            string keyphrase = "owner = ";
            string file = File.ReadAllText(fi.FullName, Encoding.UTF8);
            string newText = file.Replace(keyphrase + tag.ToUpperInvariant(), keyphrase + newTag.ToUpperInvariant());
            File.WriteAllText(fi.FullName, newText);
        }

        public static void ChangeStateOwners(FileInfo fi, Dictionary<string, CountryMergeHelper> tagsToMerge)
        {
            string keyphrase = "owner = ";
            string file = File.ReadAllText(fi.FullName, Encoding.UTF8);
            string newText = "";
            bool firstRun = true;
            Console.WriteLine("changing state owner for stateId " + fi.Name);
            foreach (CountryMergeHelper cmh in tagsToMerge.Values)
            {
                if (firstRun)
                {
                    newText = file.Replace(keyphrase + cmh.tagToGetRidOf.ToUpperInvariant(), keyphrase + cmh.tagToMergeTo.ToUpperInvariant());
                    firstRun = false;
                }
                else
                {
                    newText = newText.Replace(keyphrase + cmh.tagToGetRidOf.ToUpperInvariant(), keyphrase + cmh.tagToMergeTo.ToUpperInvariant());
                }
            }
            if(String.IsNullOrWhiteSpace(newText))
            {
                return;
            }
            File.WriteAllText(fi.FullName, newText);
        }

        //this needs to be the master text replacer method for sure
        public static void ProcessNamesFile(FileInfo fi, Dictionary<string, TagHelper> tagsToChange)
        {
            string prefix = "";
            string suffix = " = {";
            string file = File.ReadAllText(fi.FullName, Encoding.UTF8);
            string newText = "";
            bool firstRun = true;
            Console.WriteLine("changing generated names in " + fi.Name);
            foreach (TagHelper th in tagsToChange.Values)
            {
                if (firstRun)
                {
                    newText = file.Replace(prefix + th.oldTag.ToUpperInvariant() + suffix, prefix + th.newTag.ToUpperInvariant() + suffix);
                    firstRun = false;
                }
                else
                {
                    newText = newText.Replace(prefix + th.oldTag.ToUpperInvariant() + suffix, prefix + th.newTag.ToUpperInvariant() + suffix);
                }
            }
            if (String.IsNullOrWhiteSpace(newText))
            {
                return;
            }
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
                };
            }
        }

        public static void ClearDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                DirectoryInfo di = new DirectoryInfo(directory);
                foreach(DirectoryInfo subdir in di.GetDirectories())
                {
                    ClearDirectory(subdir.FullName);
                }
                foreach(FileInfo fi in di.GetFiles())
                {
                    fi.Delete();
                }
                Directory.Delete(directory);
            }
        }

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}

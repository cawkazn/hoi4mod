using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TagConverter.HelperModels;

namespace TagConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            ////COMMON/NATIONAL_FOCUS.  it's generating focus files for countries i'm making tags for.   why.


            int level;

            //home
            string baseDirectory = "C:\\myprojects\\hoi4mod\\test";
            string outputDirectory = @"C:\Users\Jobber 2k17\Documents\Paradox Interactive\Hearts of Iron IV\mod\test";
            string dataDirectory = "C:\\myprojects\\hoi4mod\\data\\";


            //work
            //string baseDirectory = "C:\\coreymods\\hoi4mod\\test";
            //string outputDirectory = "C:\\coreymods\\hoi4mod\\testoutput";
            //string dataDirectory = "C:\\coreymods\\hoi4mod\\data\\";

            string mergeDataFilename = "TerritoryMergingData.csv";
            string countriesFilename = "Countries.csv";
            string provincesFilename = "Provinces.csv";

            Dictionary<string, TagHelper> tagsToChange = new Dictionary<string, TagHelper>();

            TextReader reader = new StreamReader(dataDirectory + countriesFilename);
            var csvReader = new CsvReader(reader);
            csvReader.Read();
            csvReader.ReadHeader();
            var records = csvReader.GetRecords<TagHelper>();
            foreach (TagHelper th in records)
            {
                if (String.IsNullOrWhiteSpace(th.NewTag))
                {
                    continue;
                }
                else
                {
                    tagsToChange.Add(th.Tag, th);
                }
            }
            csvReader.Dispose();

            Dictionary<int, CountryMergeHelper> stateChanges = new Dictionary<int, CountryMergeHelper>();

            TextReader stateReader = new StreamReader(dataDirectory + mergeDataFilename);
            var stateCsvReader = new CsvReader(stateReader);
            stateCsvReader.Read();
            stateCsvReader.ReadHeader();
            var stateRecords = stateCsvReader.GetRecords<CountryMergeHelper>();
            foreach(CountryMergeHelper cmh in stateRecords)
            {
                if (cmh.stateId == null || String.IsNullOrWhiteSpace(cmh.stateId))
                {
                    continue;
                }
                stateChanges.Add(int.Parse(cmh.stateId), cmh);
            }
            stateCsvReader.Dispose();

            Dictionary<int, ProvinceHelper> provinceChanges = new Dictionary<int, ProvinceHelper>();

            TextReader provinceReader = new StreamReader(dataDirectory + provincesFilename);
            var provinceCsvReader = new CsvReader(provinceReader);
            provinceCsvReader.Read();
            provinceCsvReader.ReadHeader();
            var provinceRecords = provinceCsvReader.GetRecords<ProvinceHelper>();
            ProvincesToChange provincesToChange = new ProvincesToChange();
            provincesToChange.provincesFrom = new Dictionary<string, List<string>>();
            provincesToChange.provincesTo = new Dictionary<string, List<string>>();
            foreach (ProvinceHelper cmh in provinceRecords)
            {
                if (cmh.provinceId == null || String.IsNullOrWhiteSpace(cmh.provinceId))
                {
                    continue;
                }
                List<string> provincesFrom;
                if (provincesToChange.provincesFrom.TryGetValue(cmh.stateFrom, out provincesFrom))
                {
                    provincesFrom.Add(cmh.provinceId);
                    provincesToChange.provincesFrom[cmh.stateFrom] = provincesFrom;
                }
                else
                {
                    provincesFrom = new List<string>();
                    provincesFrom.Add(cmh.provinceId);
                    provincesToChange.provincesFrom.Add(cmh.stateFrom, provincesFrom);
                }

                List<string> provincesTo;
                if (provincesToChange.provincesTo.TryGetValue(cmh.stateTo, out provincesTo))
                {
                    provincesTo.Add(cmh.provinceId);
                    provincesToChange.provincesTo[cmh.stateTo] = provincesTo;
                }
                else
                {
                    provincesTo = new List<string>();
                    provincesTo.Add(cmh.provinceId);
                    provincesToChange.provincesTo.Add(cmh.stateTo, provincesTo);
                }
            }
            stateCsvReader.Dispose();



            ClearDirectory(outputDirectory);
            Directory.CreateDirectory(outputDirectory);
            Copy(baseDirectory, outputDirectory);
            
            level = 0;
            SlamItOut(tagsToChange, stateChanges, provincesToChange, outputDirectory, level);
            //Console.WriteLine("finished tag " + th.oldTag + " to " + th.newTag);

            Console.WriteLine("TAll done."); 
            Console.ReadLine();
        }

        public static void SlamItOut(Dictionary<string, TagHelper> tagsToChange, Dictionary<int, CountryMergeHelper> statesToChangeOwnership, 
            ProvincesToChange provincesToChange, string directory, int level)
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
                            SlamItOut(tagsToChange, statesToChangeOwnership, provincesToChange, di.FullName, level + 1);
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
                    ProcessFile(fi, level, directoryInfo, tagsToChange, statesToChangeOwnership, provincesToChange);
                }
            }
        }

        public static void ProcessFile(FileInfo fi, int level, DirectoryInfo directoryInfo, Dictionary<string, TagHelper> tagsToChange, 
            Dictionary<int, CountryMergeHelper> statesToChangeOwner, ProvincesToChange provincesToChange)
        {
            if (File.Exists(fi.FullName))
            {
                switch (level.ToString())
                {
                    case "0":
                        break;
                    case "1":
                        if(directoryInfo.Name == "localisation")
                        {
                            List<Tuple<string, string>> prefixSuffixes = new List<Tuple<string, string>>();
                            prefixSuffixes.Add(new Tuple<string, string>("", ""));
                            ProcessCountryTags(fi, tagsToChange, prefixSuffixes);
                        }
                        if(directoryInfo.Name == "interface")
                        {
                            if(fi.Name == "converter_ideas.gfx")
                            {
                                List<Tuple<string, string>> prefixSuffixes = new List<Tuple<string, string>>();
                                prefixSuffixes.Add(new Tuple<string, string>("", ""));
                                ProcessCountryTags(fi, tagsToChange, prefixSuffixes);
                            }
                        }
                        if (directoryInfo.Name == "history")
                        {
                            break;
                        }
                        if (directoryInfo.Name == "common")
                        {
                            break;
                        }
                        if(directoryInfo.Name == "events")
                        {
                            if(fi.Name == "ElectionEvents.txt")
                            {
                                List<Tuple<string, string>> prefixSuffixes = new List<Tuple<string, string>>();
                                prefixSuffixes.Add(new Tuple<string, string>("", ""));
                                ProcessCountryTags(fi, tagsToChange, prefixSuffixes);

                                //change ideology names in here as well
                            }
                            else if(fi.Name == "converterPoliticalEvents.txt")
                            {
                                //change ideology names in here
                            }
                            else if(fi.Name == "StabilityEvents.txt")
                            {
                                //change ideology names in here as well
                            }
                            else if(fi.Name == "WarJustification.txt")
                            {
                                //change ideology names in here.  way at the end.
                            }
                        }
                        break;
                    //case "2":
                    //    break;
                    case "2":
                        bool filenameEdited = false;
                        if (directoryInfo.Name == "names")
                        {
                            List<Tuple<string, string>> prefixSuffixes = new List<Tuple<string, string>>();
                            prefixSuffixes.Add(new Tuple<string, string>("", ""));
                            ProcessCountryTags(fi, tagsToChange, prefixSuffixes);
                        }
                        if (directoryInfo.Name == "countries")
                        {
                            //open a text file for the tag to be replaced
                            //if one is found

                            //scrub this file for every tag we're changing
                            List<Tuple<string, string>> prefixSuffixes = new List<Tuple<string, string>>();
                            prefixSuffixes.Add(new Tuple<string, string>("", ""));
                            ProcessCountryTags(fi, tagsToChange, prefixSuffixes);
                            string fileName = fi.Name;
                            string newFileName = "";

                            //direct lookup this file in our list of tags to change
                            TagHelper th;
                            if (tagsToChange.TryGetValue(fileName.Substring(0, 3), out th))
                            {
                                if (fileName.Length > 3)
                                {
                                    newFileName = directoryInfo.FullName + "\\" + th.NewTag + fileName.Substring(3);
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
                            List<Tuple<string, string>> prefixSuffixes = new List<Tuple<string, string>>();
                            prefixSuffixes.Add(new Tuple<string, string>("", ""));
                            ProcessCountryTags(fi, tagsToChange, prefixSuffixes);
                        }
                        if(directoryInfo.Name == "ideas")
                        {
                            List<Tuple<string, string>> prefixSuffixes = new List<Tuple<string, string>>();
                            prefixSuffixes.Add(new Tuple<string, string>("", ""));
                            ProcessCountryTags(fi, tagsToChange, prefixSuffixes);

                            string fileName = fi.Name;
                            string newFileName = "";

                            //direct lookup this file in our list of tags to change
                            TagHelper th;
                            if (tagsToChange.TryGetValue(fileName.Substring(0, 3), out th))
                            {
                                if (fileName.Length > 3)
                                {
                                    newFileName = directoryInfo.FullName + "\\" + th.NewTag + fileName.Substring(3);
                                    File.Delete(newFileName);
                                    File.Move(fi.FullName, newFileName);
                                    filenameEdited = true;
                                }
                            }
                        }
                        if (directoryInfo.Name == "states")
                        {
                            List<Tuple<string, string>> countryPrefixSuffixes = new List<Tuple<string, string>>();
                            countryPrefixSuffixes.Add(new Tuple<string, string>("", ""));
                            ProcessCountryTags(fi, tagsToChange, countryPrefixSuffixes);

                            int stateId = 0;
                            if (int.TryParse(fi.Name.Replace(fi.Extension, ""), out stateId))
                            {
                                CountryMergeHelper cmh;
                                List<string> provinces = new List<string>();
                                if (statesToChangeOwner.TryGetValue(stateId, out cmh))
                                {
                                    List<Tuple<string, string>> statesPrefixSuffixes = new List<Tuple<string, string>>();
                                    statesPrefixSuffixes.Add(new Tuple<string, string>("owner = ", ""));
                                    statesPrefixSuffixes.Add(new Tuple<string, string>("add core of = ", ""));
                                    ChangeStateOwners(fi, cmh, statesPrefixSuffixes);
                                }
                                if(provincesToChange.provincesFrom.TryGetValue(stateId.ToString(), out provinces))
                                {
                                    //remove province from state
                                    RemoveProvincesFromState(fi, provinces);
                                }

                                provinces = new List<string>();
                                if(provincesToChange.provincesTo.TryGetValue(stateId.ToString(), out provinces))
                                {
                                    //add province to state
                                    AddProvincesToState(fi, provinces);
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
                                    List<Tuple<string, string>> prefixSuffixes = new List<Tuple<string, string>>();
                                    prefixSuffixes.Add(new Tuple<string, string>("", ""));
                                    ProcessCountryTags(fi, tagsToChange, prefixSuffixes);
                                    string newFileName = directoryInfo.FullName + "\\" + th.NewTag + fileName.Substring(3);
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
                                    string newFileName = directoryInfo.FullName + "\\" + th.NewTag + fileName.Substring(3);
                                    File.Delete(newFileName);
                                    File.Move(fi.FullName, newFileName);
                                }
                            }
                        }

                        break;

                    case "3":
                        if (directoryInfo.Name == "names")
                        {
                            List<Tuple<string, string>> prefixSuffixes = new List<Tuple<string, string>>();
                            prefixSuffixes.Add(new Tuple<string, string>("", " = {"));
                            ProcessCountryTags(fi, tagsToChange, prefixSuffixes);
                        }
                        if(directoryInfo.Name == "colors")
                        {
                            List<Tuple<string, string>> prefixSuffixes = new List<Tuple<string, string>>();
                            prefixSuffixes.Add(new Tuple<string, string>("", " = {"));
                            ProcessCountryTags(fi, tagsToChange, prefixSuffixes);
                        }
                        if(directoryInfo.Name == "small")
                        {
                            string fileName = fi.Name;
                            TagHelper th;
                            if (tagsToChange.TryGetValue(fileName.Substring(0, 3), out th))
                            {
                                if (fileName.Length > 3)
                                {
                                    string newFileName = directoryInfo.FullName + "\\" + th.NewTag + fileName.Substring(3);
                                    File.Delete(newFileName);
                                    File.Move(fi.FullName, newFileName);
                                }
                            }
                        }
                        if (directoryInfo.Name == "medium")
                        {
                            string fileName = fi.Name;
                            TagHelper th;
                            if (tagsToChange.TryGetValue(fileName.Substring(0, 3), out th))
                            {
                                if (fileName.Length > 3)
                                {
                                    string newFileName = directoryInfo.FullName + "\\" + th.NewTag + fileName.Substring(3);
                                    File.Delete(newFileName);
                                    File.Move(fi.FullName, newFileName);
                                }
                            }

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
    

        public static string SwapTag(string text, string tag, string newTag, string prefix, string suffix)
        {
            return text.Replace(prefix + tag.ToUpperInvariant() + suffix, prefix + newTag.ToUpperInvariant() + suffix);
        }

        public static void RemoveProvincesFromState(FileInfo fi, List<string> provinces)
        {
            string text = File.ReadAllText(fi.FullName, Encoding.UTF8);
            int locationOfProvince = 0;
            string frontline = "provinces={";
            Regex provinceFinder = new Regex(frontline);
            try
            {
                if(provinceFinder.IsMatch(text))
                {
                    Match locationOfProvinces = provinceFinder.Match(text);

                    locationOfProvince = locationOfProvinces.Index;
                    int lengthofSection = locationOfProvinces.Length;
                    string provincesList = text.Substring(locationOfProvince);
                    
                    foreach(string province in provinces)
                    {
                        provincesList = provincesList.Replace(province, "");
                    }

                    text = text.Substring(0, locationOfProvince) + provincesList;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("regex matching in history->state file fucked up while removing provinces.\n\tLeaving it unchanged" + "\n\t" + e.Message);
            }

            File.WriteAllText(fi.FullName, text);
        }

        public static void AddProvincesToState(FileInfo fi, List<string> provinces)
        {
            string text = File.ReadAllText(fi.FullName, Encoding.UTF8);
            int locationOfProvince = 0;
            string frontline = "provinces={";
            Regex provinceFinder = new Regex(frontline);
            try
            {
                if (provinceFinder.IsMatch(text))
                {
                    Match locationOfProvinces = provinceFinder.Match(text);

                    locationOfProvince = locationOfProvinces.Index;
                    int lengthofSection = locationOfProvinces.Length;
                    string provincesList = text.Substring(locationOfProvince);

                    foreach (string province in provinces)
                    {
                        text = text.Insert(locationOfProvince + frontline.Length, province + " ");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("regex matching in history->state file fucked up while removing provinces.\n\tLeaving it unchanged" + "\n\t" + e.Message);
            }

            File.WriteAllText(fi.FullName, text);
        }

        public static string AddCoreToState(string text, CountryMergeHelper cmh, string prefix, string suffix)
        {
            int locationOfSection = 0;
            string frontline = "history={";
            string endline = "}\r\n\r\n\tprovinces={";
            string newlinePattern = "\r\n\t";
            string historyPattern = endline;
            Regex historyFinder = new Regex(historyPattern);
            try
            {
                if (historyFinder.IsMatch(text))
                {
                    Match locationOfHistory = historyFinder.Match(text);

                    locationOfSection = locationOfHistory.Index;
                    int lengthofSection = locationOfHistory.Length;

                    if (cmh.retainCore != "y" && cmh.addCore != "y")
                    {
                        return text.Remove(locationOfSection, (prefix + cmh.tagFrom + suffix + newlinePattern).Length);
                    }
                    else if (cmh.retainCore == "y" && cmh.addCore == "y")
                    {
                        return text.Insert(locationOfSection, prefix + cmh.tagTo + suffix + newlinePattern);
                    }
                    else if (cmh.retainCore != "y" && cmh.addCore == "y")
                    {
                        return SwapTag(text, cmh.tagFrom, cmh.tagTo, prefix, suffix);
                    }
                    else
                    {
                        return text;
                    }
                }
                else
                {
                    return text;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("regex matching in history->state file fucked up.\n\tLeaving it unchanged" + "\n\t" + e.Message);
                return text;
            }
            
        }

        public static void ProcessCountryTags(FileInfo fi, Dictionary<string, TagHelper> tagsToChange, List<Tuple<string, string>> prefixSuffixes)
        {
            Console.WriteLine("changing country tags for file " + fi.Name);
            string file = File.ReadAllText(fi.FullName, Encoding.UTF8);
            foreach(TagHelper th in tagsToChange.Values)
            {
                foreach (Tuple<string, string> prefixSuffix in prefixSuffixes)
                {
                    file = SwapTag(file, th.Tag.ToUpperInvariant(), th.NewTag.ToUpperInvariant(), prefixSuffix.Item1, prefixSuffix.Item2);
                }
            }
            if(String.IsNullOrWhiteSpace(file))
            {
                return;
            }
            File.WriteAllText(fi.FullName, file);
        }

        public static void ChangeStateOwners(FileInfo fi, CountryMergeHelper cmh, List<Tuple<string, string>> prefixSuffixes)
        {
            Console.WriteLine("changing state owner for stateId " + fi.Name);
            string file = File.ReadAllText(fi.FullName, Encoding.UTF8);
                int counter = 0;
                foreach (Tuple<string, string> prefixSuffix in prefixSuffixes)
                {
                        if (counter == 1)
                        {
                            file = AddCoreToState(file, cmh, prefixSuffix.Item1, prefixSuffix.Item2);
                        }
                        if(counter == 0)
                        {
                            file = SwapTag(file, cmh.tagFrom.ToUpperInvariant(), cmh.tagTo.ToUpperInvariant(), prefixSuffix.Item1, prefixSuffix.Item2);
                        }                    
                    counter++;
                }
            File.WriteAllText(fi.FullName, file);
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

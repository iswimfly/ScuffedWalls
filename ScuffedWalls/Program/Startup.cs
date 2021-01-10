﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ScuffedWalls
{
    class Startup
    {
        private string[] args;
        private string ConfigFileName;
        public Config ScuffedConfig;
        
        public Startup(string[] args) 
        {
            Console.Title = $"ScuffedWalls {ScuffedWalls.ver}";
            this.args = args;
            ConfigFileName = $"{AppDomain.CurrentDomain.BaseDirectory}ScuffedWalls.json";
            Console.WriteLine($"{AppDomain.CurrentDomain.BaseDirectory} ScuffedWalls.json");
            ScuffedConfig = GetConfig();
        }

        public Config GetConfig()
        {
            VerifyConfig();
            return JsonSerializer.Deserialize<Config>(File.ReadAllText(ConfigFileName));
        }
        public void VerifySW()
        {
            if (!File.Exists(ScuffedConfig.SWFilePath))
            {
                using (StreamWriter file = new StreamWriter(ScuffedConfig.SWFilePath))
                {
                    file.WriteLine($"#ScuffedWalls {ScuffedWalls.ver}");
                    file.WriteLine("#New SWFile Created");
                    file.WriteLine("#DM @thelightdesigner#1337 for help?");
                    file.WriteLine("");
                    file.WriteLine("Workspace");
                    if (ScuffedConfig.AutoImport)
                    {
                        file.WriteLine("");
                        file.WriteLine("0: Import");
                        file.WriteLine($" Path:{new FileInfo(ScuffedConfig.OldMapPath).Name}");
                    }
                }
                Console.Write("[ConsoleLoggerDefault] Main: "); Console.ForegroundColor = ConsoleColor.Red; Console.Write("N"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("e"); Console.ForegroundColor = ConsoleColor.Green; Console.Write("w"); Console.Write(" "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("S"); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("W"); Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("F"); Console.ForegroundColor = ConsoleColor.Red; Console.Write("i"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("l"); Console.ForegroundColor = ConsoleColor.Green; Console.Write("e"); Console.Write(" "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("C"); Console.ForegroundColor = ConsoleColor.Blue; Console.Write("r"); Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("e"); Console.ForegroundColor = ConsoleColor.Red; Console.Write("a"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("t"); Console.ForegroundColor = ConsoleColor.Green; Console.Write("e"); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("d"); Console.ForegroundColor = ConsoleColor.Blue; Console.WriteLine("!"); Console.ResetColor();
            }
        }
        public void VerifyOld()
        {
            if ((!File.Exists(ScuffedConfig.OldMapPath)) && ScuffedConfig.AutoImport)
            {
                using (StreamWriter file = new StreamWriter(ScuffedConfig.OldMapPath))
                {
                    file.Write(File.ReadAllText(ScuffedConfig.MapFilePath));
                }
                ScuffedLogger.Log("Created New Old Map File");
            }
        }
        public void VerifyConfig()
        {
            if (args.Length != 0 || !File.Exists(ConfigFileName))
            {
                Config reConfig = ConfigureSW();
                if (File.Exists(ConfigFileName)) File.Delete(ConfigFileName);
                using StreamWriter file = new StreamWriter(ConfigFileName);
                file.Write(JsonSerializer.Serialize(reConfig));
                File.SetAttributes(ConfigFileName, FileAttributes.Hidden);
            }
        }

        Config ConfigureSW()
        {
            if ((args.Length == 0 || args == null))
            {
                Console.WriteLine("No file was dragged into the exe!");
                Console.ReadLine();
                Environment.Exit(1);
            }

            // get beatmap option
            DirectoryInfo mapFolder = new DirectoryInfo(args[0]);
            FileInfo[] mapDataFiles = mapFolder.GetFiles("*.dat");

            int j = 0;
            foreach (var filename in mapDataFiles)
            {
                if (filename.Name != "info.dat" && filename.Name != "Info.dat")
                {
                    Console.WriteLine(j + ": " + filename.Name.Split('.')[0]);
                }
                j++;
            }

            Console.Write("Input Difficulty Number:");
            int option = Convert.ToInt32(Console.ReadLine());

            Console.Write("AutoImport Map? (y/n):");
            char answer = Convert.ToChar(Console.ReadLine());
            bool AutoImportMap = false;
            if (answer == 'y') AutoImportMap = true;

            //path of the sw file by difficulty name
            string SWFilePath = mapFolder.FullName + @"\" + mapDataFiles[option].Name.Split('.')[0] + "_ScuffedWalls.sw";

            string OldMapPath = mapFolder.FullName + @"\" + mapDataFiles[option].Name.Split('.')[0] + "_OldMap.dat";

            return new Config() { SWFilePath = SWFilePath, MapFilePath = mapDataFiles[option].FullName, MapFolderPath = args[0], OldMapPath = OldMapPath, AutoImport = AutoImportMap };
        }
    }
}
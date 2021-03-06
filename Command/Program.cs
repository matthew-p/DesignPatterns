﻿using Command.Commands;
using Command.Models;
using CommonClientLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Command
{
    class Program
    {
        private static TextPrinter TxtPrinter = new TextPrinter();
        private static TypeParser TypeParser = new TypeParser(new TextParser());
        private static QuestionAsker Asker = new QuestionAsker();
        private static ContinuationDeterminer ContinuationDeterminer = new ContinuationDeterminer();
        private const string ConfigurableItemsPath = "Items.json";

        static void Main(string[] args)
        {
            TxtPrinter.PrintInformation("WELCOME TO THE COMMAND PROGRAM - WHICH IS MILDLY THOUGHT-PROVOKING");
            List<Item> items;
            try
            {
                using (var reader = new StreamReader(ConfigurableItemsPath))
                {
                    var json = reader.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<Item>>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to retrieve item from '{ConfigurableItemsPath}'.\n" +
                                  $"Exception message: {ex.Message}\n" +
                                  $"Using default questions and answers instead.\n");

                items = DefaultItems.GetDefaultItems();
            }

            var (commandDict, _) = TypeParser.GetInstantiatedTypeDictionaryAndNameList<ICommand>();
            var commandList = commandDict
                .OrderBy(c => c.Key)
                .Select(c => c.Value.Description)
                .ToList();

            while (true)
            {
                var order = new Order();
                while (true)
                {
                    var commandChoice = Asker.GetChoiceFromList("What do you want to do with your order?", commandList) + 1;

                    order = commandDict[commandChoice].Execute(order, items);
                    
                    if (!ContinuationDeterminer.GoAgain("Do you want to perform another action on your order?"))
                    {
                        break;
                    }
                }

                if (!ContinuationDeterminer.GoAgain())
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}

﻿using CommonClientLib;
using Newtonsoft.Json.Linq;
using Observer.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Observer
{
    class Program
    {
        private static TextPrinter TxtPrinter = new TextPrinter();
        private static string NewsItemsPath = "NewsItems.json";
        private static string ResponsesPath = "Responses.json";
        private static QuestionAsker Asker = new QuestionAsker();
        private static List<Person> NewsSubscribers = new List<Person>();
        private static readonly List<string> Choices = new List<string>
        {
            "Add news subscriber",
            "Remove news subscriber",
            "Publish news item",
            "End this"
        };

        static void Main(string[] args)
        {
            TxtPrinter.PrintInformation("WELCOME TO THE OBSERVER PROGRAM -- WHICH IS SOMEWHAT FUNNY");

            var news = new List<News>();
            try
            {
                using (var reader = new StreamReader(NewsItemsPath))
                {
                    var json = reader.ReadToEnd();
                    var jObject = JObject.Parse(json);
                    foreach (var obj in jObject)
                    {
                        switch (obj.Key)
                        {
                            case "good":
                                news.AddRange(obj.Value.Select(n => n.ToNews(NewsType.Good)));
                                break;
                            case "bad":
                                news.AddRange(obj.Value.Select(n => n.ToNews(NewsType.Bad)));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to load news items from {NewsItemsPath}. Exception: {ex.Message}\n " +
                    $"Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            var responses = new List<Response>();
            try
            {
                using (var reader = new StreamReader(ResponsesPath))
                {
                    var json = reader.ReadToEnd();
                    var jObject = JObject.Parse(json);
                    foreach (var obj in jObject)
                    {
                        switch (obj.Key)
                        {
                            case "good":
                                responses.AddRange(obj.Value.Select(r => r.ToResponse(NewsType.Good)));
                                break;
                            case "bad":
                                responses.AddRange(obj.Value.Select(r => r.ToResponse(NewsType.Bad)));
                                break;
                            default:
                                break;
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to load responses from {ResponsesPath}. Exception: {ex.Message}\n " +
                    $"Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            var newsPublisher = new NewsPublisher();
            while (true)
            {
                var choice = Asker.GetChoiceFromList("What do you want to do?", Choices);
                switch (choice)
                {
                    case 0:
                        AddSubscriber(newsPublisher, responses);
                        break;
                    case 1:
                        RemoveSubscriber();
                        break;
                    case 2:
                        PublishNewsItem(news, newsPublisher);
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void AddSubscriber(NewsPublisher newsPublisher, List<Response> responses)
        {
            var name = Asker.GetValue<string>("\nWhat is the name of the new subscriber?");
            var subscriber = new Person(name, responses);
            NewsSubscribers.Add(subscriber);
            subscriber.Subscribe(newsPublisher);
        }

        private static void RemoveSubscriber()
        {
            if (!NewsSubscribers.Any())
            {
                Console.WriteLine("\nThere are no news subscribers to remove.\n");

                return;
            }

            var subscriber = NewsSubscribers[Asker.GetChoiceFromList("\nWhich subscriber do you want to remove?", 
                NewsSubscribers.Select(n => n.Name).ToList())];

            NewsSubscribers.Remove(subscriber);
            subscriber.Unsubscribe();
        }

        private static void PublishNewsItem(List<News> news, NewsPublisher newsPublisher)
        {
            var newsType = (NewsType)Asker.GetChoiceFromList("What kind of news do you want to publish?", 
                Enum.GetValues(typeof(NewsType)).Cast<NewsType>().Select(n => n.ToString()).ToList());

            var newsChoices = news.Where(n => n.NewsType == newsType).ToList();

            var chosenNews = newsChoices[Asker.GetChoiceFromList($"What {newsType.ToString().ToLower()} news do you want to publish?",
                newsChoices.Select(n => n.NewsItem).ToList())];

            Console.WriteLine();
            newsPublisher.PublishNews(chosenNews);
        }
    }
}

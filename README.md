# Design Patterns
This repo contains examples of common OOP design patterns.

## Creational Patterns

### Simple Factory

The program will produce an array of random numbers after the user chooses the size of the array. Then, the program gives the user a choice of various sort methods. After the user chooses a method, the program uses the *simple factory* pattern to create an instance of a class that sorts the array according to the method that the user chose. Output shows how long it took to sort the array.

### Builder

Choose a character from among several interesting options, and the program will build and describe it. You can create your own character too--simply make a class that implements `AbstractCharacterBuilder`.

### Prototype

This is a simple implementation of the prototype pattern. Choose a website from among a list of several classic sites, and the `WebPageExplorer` class, which implements .NET `HttpClient`, will return interesting information about that site! The client operates by creating a list of instances of `WebPageExplorer` by cloning an initial instance rather than constructing new instances.

### Singleton

Did you come here for an argument? Good. Choose from a variety of topics on which to argue. If you get tired of arguing a certain topic, you can switch to a different topic. Every topic is a class registered with an IoC container which provides a only a single instance of each.

## Structural Patterns

### Adapter

After making your way through some questions about yourself, the program will render a report regarding your answers. The report can render either as a simple list of the questions and the answers you provided, or it can show you your incorrect answers along with a total of your score. The adapter pattern is used to allow render methods of various signatures to use the functionality that another render method provides.
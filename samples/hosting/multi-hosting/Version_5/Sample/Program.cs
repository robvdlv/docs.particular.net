﻿using System;
using NServiceBus;

class Program
{

    static void Main()
    {
        #region multi-hosting

        using (IBus bus1 = StartInstance1())
        using (IBus bus2 = StartInstance2())
        {
            Console.WriteLine("Press '1' to send a message from Instance1 to Instance2");
            Console.WriteLine("Press '2' to send a message from Instance2 to Instance1");
            Console.WriteLine("To exit, press Ctrl + C");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key == ConsoleKey.D1)
                {
                    bus1.Send("Samples.MultiHosting.Instance2", new MyMessage());
                } 
                else if (key.Key == ConsoleKey.D2)
                {
                    bus2.Send("Samples.MultiHosting.Instance1", new MyMessage());
                }
            }
        }

        #endregion
    }

    static IBus StartInstance1()
    {
        #region multi-hosting-assembly-scan

        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.MultiHosting.Instance1");
        busConfiguration.AssembliesToScan(AllAssemblies.Matching("Instance1.").And("Shared"));
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        return Bus.Create(busConfiguration).Start();

        #endregion
    }

    static IBus StartInstance2()
    {
        BusConfiguration busConfiguration = new BusConfiguration();

        busConfiguration.EndpointName("Samples.MultiHosting.Instance2");
        busConfiguration.AssembliesToScan(AllAssemblies.Matching("Instance2.").And("Shared"));
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        return Bus.Create(busConfiguration).Start();
    }
}
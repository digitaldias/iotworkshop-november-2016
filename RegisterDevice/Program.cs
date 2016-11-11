using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using System;

namespace RegisterDevice
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Forferdelig!");
                return;
            }


            // Utfør registrering i Azure
            string connectionString = "HostName=demo-iothub-pedro.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=MLwdCRkEtgcdwV+Cyj3zV//OUG+4zVdsVrwvWRnhnjk=";
            var registryManager = RegistryManager.CreateFromConnectionString(connectionString);

            Device device = new Device(args[0]);
            Device registered = null;

            try
            {
                registered = registryManager.AddDeviceAsync(device).Result;
            }
            catch(AggregateException ex)
            {
                if(ex.InnerException.GetType() == typeof(DeviceAlreadyExistsException))
                {
                    Console.WriteLine($"Device {args[0]} allerede registrert");
                    registered = registryManager.GetDeviceAsync(args[0]).Result;
                }
            }

            Console.WriteLine($"Enhet registert: {registered.Authentication.SymmetricKey.PrimaryKey}");
            Console.WriteLine("Ferdig, trykk ANY tasten for å fortsette");
            Console.ReadKey();
        }
    }
}

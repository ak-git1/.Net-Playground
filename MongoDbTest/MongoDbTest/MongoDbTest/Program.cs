using System;
using MongoDbTest.Entities;
using MongoDbTest.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace MongoDbTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string dbName = "test";
                string collectionName = "users";

                // Adding convention
                ConventionPack conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
                ConventionRegistry.Register("camelCase", conventionPack, t => true);

                TestRepository repository = new TestRepository();

                // Get databases names
                Console.WriteLine("Databases:");
                foreach (string s in repository.GetDatabaseNames().Result)
                    Console.WriteLine(s);
                Console.WriteLine();


                // Get collections names
                Console.WriteLine("Collections in database {0}:", dbName);
                foreach (string s in repository.GetCollectionsNames(dbName).Result)
                    Console.WriteLine(s);
                Console.WriteLine();

                // Objects mapping
                BsonClassMap.RegisterClassMap<Person>(cm =>
                {
                    cm.AutoMap();
                });

                // Adding documents
                Person p = new Person
                {
                    Name = "Andrew",
                    Surname = "Li",
                    Age = 52,
                    Company = new Company {Name = "Google"}
                };
                repository.AddDocument(dbName, collectionName, p);

                // Find docs
                Console.WriteLine("Find docs in {0}:", dbName);
                foreach (string s in repository.FindDocs<BsonDocument>(dbName, collectionName).Result)
                    Console.WriteLine(s);
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }
    }
}

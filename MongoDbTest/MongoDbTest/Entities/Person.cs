using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbTest.Entities
{
    class Person
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonIgnore]
        public string Surname { get; set; }

        public int Age { get; set; }

        public Company Company { get; set; }

        public List<string> Languages { get; set; }
    }
}

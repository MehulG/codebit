using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using MongoDB.Driver;

namespace api.Services
{
    public class DummyService
    {
        private readonly IMongoCollection<Dummy> _value;

        public DummyService(IDummyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _value = database.GetCollection<Dummy>(settings.DummyCollectionName);
        }

        public List<Dummy> Get() =>
            _value.Find(c => true).ToList();

        public Dummy Get(string id) =>
            _value.Find<Dummy>(c => c.id == id).FirstOrDefault();

        public Dummy Create(Dummy c)
        {
            _value.InsertOne(c);
            return c;
        }

        public void Update(string id, Dummy _dataIn) =>
            _value.ReplaceOne(c => c.id == id,_dataIn);

        public void Remove(Dummy _dataIn) =>
            _value.DeleteOne(c => c.id == _dataIn.id);

        public void Remove(string id) =>
            _value.DeleteOne(c => c.id == id);

    }
}

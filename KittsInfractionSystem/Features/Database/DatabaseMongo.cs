#if MONGODB
using KittsInfractionSystem.Features.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace KittsInfractionSystem.Features.Database;

internal static class DatabaseMongo
{
    private static MongoClient _client;
    private static IMongoDatabase _db;
    private static IMongoCollection<InfractionData> _infractionDataCollection;

    public static void Init()
    {
        _client = new MongoClient(KittsInfractionSystem.Config.MongoDBURI);
        _db = _client.GetDatabase(KittsInfractionSystem.Config.MongoDBName);
        _infractionDataCollection = _db.GetCollection<InfractionData>(KittsInfractionSystem.Config.MongoDBCollectionName);

        CreateIndexes();
    }

    private static void CreateIndexes()
    {
        _infractionDataCollection.Indexes.CreateOne(
            new CreateIndexModel<InfractionData>(
                Builders<InfractionData>.IndexKeys.Ascending(p => p.Issued),
                new CreateIndexOptions { Unique = true }
            )
        );
    }

    public static void Stop()
    {
        _infractionDataCollection = null;
        _client = null;
        _db = null;
    }

    public static void AddInfraction(InfractionData infraction) =>
        _infractionDataCollection.InsertOne(infraction);

    public static IReadOnlyList<InfractionData> GetInfractions(string offenderId)
    {
        return _infractionDataCollection
            .Find(i => i.OffenderId == offenderId)
            .ToList()
            .AsReadOnly();
    }
}
#endif
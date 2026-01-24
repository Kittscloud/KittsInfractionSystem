using KittsInfractionSystem.Features.Enums;
#if MONGODB
using MongoDB.Bson.Serialization.Attributes;
#endif
using System;

namespace KittsInfractionSystem.Features.Models;

public sealed class InfractionData
{
#if MONGODB
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string ObjectId = null;

    [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
#endif
    public DateTime Issued = DateTime.Now;

    public string Type = Enum.GetName(typeof(InfractionType), InfractionType.Other);

    public string OffenderName = "";
    public string OffenderId = "";

    public string ModeratorName = "";
    public string ModeratorId = "";

    public TimeSpan? Duration = null;
    public string ReasonAndEvidence = null;
}

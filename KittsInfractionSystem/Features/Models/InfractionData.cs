using KittsInfractionSystem.Features.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace KittsInfractionSystem.Features.Models;

public sealed class InfractionData
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string ObjectId = null;

    [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
    public DateTime Issued = DateTime.Now;

    public string Type = Enum.GetName(typeof(InfractionType), InfractionType.Other);

    public string OffenderName = "";
    public string OffenderId = "";

    public string ModeratorName = "";
    public string ModeratorId = "";

    public TimeSpan? Duration = null;
    public string ReasonAndEvidence = null;
}

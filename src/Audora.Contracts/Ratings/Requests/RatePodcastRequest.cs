using System.ComponentModel.DataAnnotations;
using Audora.Contracts.Episodes.Requests;

namespace Audora.Contracts.Podcasts.Requests;

public class RatePodcastRequest
{
    [Range(0, 5)]
    public byte Rating { get; init; }
}
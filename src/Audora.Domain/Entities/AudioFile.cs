using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class AudioFile : Entity
{
    public string AudioUrl { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? StorageProviderName { get; init; }
    public string Extension { get; init; } = null!;
    public string ContentType { get; init; } = null!;
    public long ByteSize { get; init; }
    public int Duration { get; init; }
    public int? BitrateKbps { get; init; }
    public bool IsTranscoded { get; init; }
    public string? Checksum { get; init; }

    public AudioFile(
        string audioUrl,
        string name,
        string extension,
        string contentType,
        long byteSize,
        int duration,
        bool isTranscoded,
        string? storageProviderName = null,
        int? bitrateKbps = null,
        string? checksum = null)
    {
        AudioUrl = audioUrl;
        Name = name;
        StorageProviderName = storageProviderName;
        Extension = extension;
        ContentType = contentType;
        ByteSize = byteSize;
        Duration = duration;
        BitrateKbps = bitrateKbps;
        IsTranscoded = isTranscoded;
        Checksum = checksum;
    }


    private AudioFile()
    {
    }
}
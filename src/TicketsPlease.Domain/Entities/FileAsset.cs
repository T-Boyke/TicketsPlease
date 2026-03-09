using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class FileAsset : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string BlobPath { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public Guid UploadedByUserId { get; set; }
    public User? UploadedByUser { get; set; }
}

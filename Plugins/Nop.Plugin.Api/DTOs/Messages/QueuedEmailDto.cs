using Newtonsoft.Json;
using Nop.Plugin.Api.DTO.Base;
using Nop.Plugin.Api.Helpers;
using System;

namespace Nop.Plugin.Api.DTO.Messages;

#nullable enable

/// <summary>
/// Represents an email item
/// </summary>
public partial class QueuedEmailDto : BaseSyncDto
{
    public QueuedEmailDto(string? subject, string? body, string? attachmentFilePath, string? attachmentFileName, DateTime createdOnUtc, int sentTries, DateTime? sentOnUtc)
    {
        Subject = subject;
        Body = body;
        AttachmentFilePath = attachmentFilePath;
        AttachmentFileName = attachmentFileName;
        CreatedOnUtc = createdOnUtc;
        SentTries = sentTries;
        SentOnUtc = sentOnUtc;
    }

    [JsonProperty("subject")]
    public string? Subject { get; set; }

    /// <summary>
    /// Gets or sets the body
    /// </summary>
    [JsonProperty("body")]
    public string? Body { get; set; }

    /// <summary>
    /// Gets or sets the attachment file path (full file path)
    /// </summary>
    [JsonProperty("attachment_file_path")]
    public string? AttachmentFilePath { get; set; }

    /// <summary>
    /// Gets or sets the attachment file name. If specified, then this file name will be sent to a recipient. Otherwise, "AttachmentFilePath" name will be used.
    /// </summary>
    [JsonProperty("attachment_file_name")]
    public string? AttachmentFileName { get; set; }

    /// <summary>
    /// Gets or sets the send tries
    /// </summary>
    [JsonProperty("sent_tries")]
    public int SentTries { get; set; }

    /// <summary>
    /// Gets or sets the sent date and time
    /// </summary>
    [JsonProperty("sent_on_utc")]
    public DateTime? SentOnUtc { get; set; }

    [JsonProperty("sent_on_ts")]
    public long? SentOnTs { get => SentOnUtc is null ? null : DTOHelper.DateTimeToTimestamp((DateTime)SentOnUtc); }
}

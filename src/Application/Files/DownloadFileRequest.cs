namespace KindleEmailSenderBot.Domain.Options;

public record DownloadFileRequest(string? FileName, string? ChatId, string? FileId);
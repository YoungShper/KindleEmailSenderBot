namespace KindleEmailSenderBot.Application.Files;

public sealed record SaveFileRequest(
        string? FileName,
        long? ChatId,
        string? FileId);
namespace KindleEmailSenderBot.Domain.Options;

public record DeliverFileRequest(string? FileName, long? ChatId, string? FileId);
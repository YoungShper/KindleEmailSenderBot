namespace KindleEmailSenderBot.Application.Files;

public record DeliverFileRequest(
    string? FileName, 
    long? ChatId, 
    string? FileId);
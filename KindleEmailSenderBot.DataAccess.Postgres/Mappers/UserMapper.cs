using KindleEmailSenderBot.Core.Models;
using KindleEmailSenderBot.DataAccess.Postgres.Models;

namespace KindleEmailSenderBot.DataAccess.Postgres.Mappers;

public static class UserMapper
{
    public static UserModel? ToUserModel(this UserEntity? userEntity)
    {
        if (userEntity == null) return null;
        return new UserModel
        {
            ChatId = userEntity.ChatId,
            Email = userEntity.Email,
            IsActive = userEntity.IsActive,
        };
    }
    
    public static UserEntity ToUserEntity(this UserModel userModel)
    {
        return new UserEntity
        {
            ChatId = userModel.ChatId,
            Email = userModel.Email,
            IsActive = userModel.IsActive,
        };
    }
}
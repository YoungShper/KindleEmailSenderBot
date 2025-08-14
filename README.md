# Kindle Email Sender Bot

A Telegram bot that allows users to send documents to their Kindle devices via email. The bot handles file uploads and automatically forwards them to your Kindle email address.

## Features

- Send documents to your Kindle via Telegram
- Simple setup and configuration
- Secure storage of user preferences
- Support for various document formats

## Prerequisites

- .NET 8.0 SDK or later
- PostgreSQL database
- Telegram Bot Token from [@BotFather](https://t.me/botfather)
- SMTP server credentials for sending emails

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/KindleEmailSenderBot.git
   cd KindleEmailSenderBot
   ```

2. Configure the application:
   - Copy `appsettings_example.json` to `appsettings.json`
   - Update the configuration with your settings:
     ```json
     {
         "ConnectionStrings": {
             "KindleDbContext": "Server=your_server;Port=5432;Database=your_db;User Id=your_user;Password=your_password;"
         },
         "WorkDir": {
             "Path": "path_to_store_temporary_files"
         },
         "Telegram": {
             "Token": "your_telegram_bot_token"
         },
         "Smtp": {
             "Server": "smtp.your-email-provider.com",
             "Port": 587,
             "Username": "your_email@example.com",
             "Password": "your_email_password"
         }
     }
     ```

3. Run the application:
   ```bash
   dotnet run --project KindleEmailSenderBot.TelegramBot
   ```

## Docker Support

You can also run the application using Docker:
- Create `.env` file for database variables

```bash
docker-compose up -d
```

## Usage

1. Start a chat with your bot on Telegram
2. Use the `/start` command to begin
3. Follow the instructions to set up your Kindle email
4. Send documents directly to the bot to forward them to your Kindle

## Available Commands

- `/start` - Start the bot
- `/set_email` - Set your Kindle email
- `/help` - Show this help
- `/stop` - Stop the bot
- `/my_email` - Check your current Email


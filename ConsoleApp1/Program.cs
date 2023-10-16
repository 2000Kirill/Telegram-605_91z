using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


var botClient = new TelegramBotClient("6046195838:AAH2Qq37HtAMHupT4nKWL7QEkymYkWlnioM");

using var cts = new CancellationTokenSource();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>() 
};
botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{

    int[] mas = {1 ,2 ,3 ,4 ,5 ,6 ,7 ,8 ,9 ,10 };
    var maks = string.Join(" ", mas);
    if (update.Message is not { } message)
        return;

    if (message.Text is not { } messageText)
        return;

    if (int.TryParse(messageText, out int userNumber) && mas.Contains(userNumber))
    {
        string imageUrl = $"https://cataas.com/cat/says/{userNumber}";
        var inputFile = new InputFileUrl(imageUrl);

        await botClient.SendPhotoAsync(
            chatId: message.Chat.Id,
            photo: inputFile,
            caption: "Фото",
            cancellationToken: cancellationToken);
    }
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}


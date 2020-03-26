# SMS Counter (C#)

Character counter for SMS messages.

Original inspiration : danxexe/sms-counter

## Usage

```csharp
var oInfos = new SmsCounter('content of the SMS');
```

You will have access to the following object:

```csharp
oInfos.Encoding = EncodingEnum.GSM_7BIT;
oInfos.Length = 18:
oInfos.Messages = 1:
oInfos.PerMessage = 160:
oInfos.Remaining = 142:
```

## ToDo

## Known Issue

(none)


## Other Languages

- Javascript: [https://github.com/danxexe/sms-counter](https://github.com/danxexe/sms-counter)
- PHP: [https://github.com/acpmasquerade/sms-counter-php](https://github.com/acpmasquerade/sms-counter-php)


## License

SMS Counter is released under the [MIT License](LICENSE.txt).
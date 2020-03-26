# SMS Counter (C#)

Character counter for SMS messages.

Original inspiration : [danxexe/sms-counter](https://github.com/danxexe/sms-counter)

## Usage

```csharp
var oInfos = new SmsCounter("content of the SMS");
```

You will have access to the following informations :

```csharp
oInfos.Messages 	// Number of messages (Int32) = 1
oInfos.Length   	// Total length of messages (Int32) = 18
oInfos.Remaining 	// Remaining chars in the message (Int32) = 142
oInfos.PerMessage 	// Max chars in 1 message (Int32) = 160
oInfos.Encoding 	// Encoding used by messages (EncodingEnum) = GSM_7BIT
```

## ToDo

## Known Issue

(none)


## Other Languages

- Javascript: [https://github.com/danxexe/sms-counter](https://github.com/danxexe/sms-counter)
- PHP: [https://github.com/acpmasquerade/sms-counter-php](https://github.com/acpmasquerade/sms-counter-php)


## License

SMS Counter is released under the [MIT License](LICENSE.txt).
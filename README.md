# SMS Counter (C#)

Character counter for SMS messages.

Original inspiration : [danxexe/sms-counter](https://github.com/danxexe/sms-counter)

## Usage

```csharp
var smsCounterInfo = new SmsCounter("content of the SMS");
```

You will have access to the following informations:

```csharp
smsCounterInfo.Messages 	// Number of messages (int) = 1
smsCounterInfo.Length   	// Total length of messages (int) = 18
smsCounterInfo.Remaining 	// Remaining chars in the message (int) = 142
smsCounterInfo.PerMessage 	// Max chars in 1 message (int) = 160
smsCounterInfo.Encoding 	// Encoding used by messages (EncodingEnum) = GSM_7BIT
```

You can also check test results: 
![test results](/test-case-results.png "Test results").

Just need XUnit testing library for adding to tests file your project


## ToDo

## Known Issue

(none)


## Other Languages

- Javascript: [https://github.com/danxexe/sms-counter](https://github.com/danxexe/sms-counter)
- PHP: [https://github.com/acpmasquerade/sms-counter-php](https://github.com/acpmasquerade/sms-counter-php)


## License

SMS Counter is released under the [MIT License](LICENSE.txt).
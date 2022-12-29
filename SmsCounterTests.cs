public class SmsCounterTests
{
    [Theory]
    [InlineData("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.")]
    public void Text_Length_Must_Be_1856(string text)
    {
        var smsCounter = new SmsCounter(text);
        Assert.True(text.Length == 574);
        Assert.Equal(SmsCounter.EncodingEnum.GSM_7BIT, smsCounter.Encoding);
    }

    [Theory]
    [InlineData("Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı 1500'lerden beri endüstri standardı sahte metinler olarak kullanılmıştır. Beşyüz yıl boyunca varlığını sürdürmekle kalmamış, aynı zamanda pek değişmeden elektronik dizgiye de sıçramıştır. 1960'larda Lorem Ipsum pasajları da içeren Letraset yapraklarının yayınlanması ile ve yakın zamanda Aldus PageMaker gibi Lorem Ipsum sürümleri içeren masaüstü yayıncılık yazılımları ile popüler olmuştur.")]
    public void Text_Length_Must_Not_Be_1856(string text)
    {
        var smsCounter = new SmsCounter(text);
        Assert.True(text.Length == 590);
        Assert.Equal(SmsCounter.EncodingEnum.UNICODE, smsCounter.Encoding);
    }
}

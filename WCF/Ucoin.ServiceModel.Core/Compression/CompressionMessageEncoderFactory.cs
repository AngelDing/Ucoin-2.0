using System.ServiceModel.Channels;

namespace  Ucoin.ServiceModel.Core
{
public class CompressionMessageEncoderFactory : MessageEncoderFactory
{
    public MessageEncoderFactory InnerMessageEncoderFactory { get; private set; }
    public MessageCompressor MessageCompressor { get; private set; }
    public CompressionMessageEncoderFactory(MessageEncoderFactory innerMessageEncoderFactory, MessageCompressor messageCompressor)
    {
        this.InnerMessageEncoderFactory = innerMessageEncoderFactory;
        this.MessageCompressor = messageCompressor;
    }
    public override MessageEncoder Encoder
    {
        get { return new CompressionMessageEncoder(this.InnerMessageEncoderFactory.Encoder, this.MessageCompressor); }
    }
    public override MessageVersion MessageVersion
    {
        get { return this.InnerMessageEncoderFactory.MessageVersion; }
    }
}
}

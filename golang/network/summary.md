# summary

## 粘包
多个数据包被连续存储于连续的缓存中，在对数据包进行读取时由于无法确定发生方的发送边界，
而采用某一估测值大小来进行数据读出，若双方的size不一致时就会使指发送方发送的若干包数据到接收方接收时粘成一包，
q从接收缓冲区看，后一包数据的头紧接着前一包数据的尾。

### **fixed length**

**解释：**
客户端和服务端发送到缓冲区的数据长度是固定的。
1. 息长度小于固定长度，那消息与消息之间有一定的空白区域，会浪费一定资源。
2. 总长度大于固定长度，则消息需要拆包发送，会出现半包现象。

**应用：**
Netty io.netty.handler.codec.FixedLengthFrameDecoder
***

### **delimiter based**

**解释：**
每则消息通过分割符断开，`'\n'` 来扫描字节流，
如果遇到了分割符则将消息两次分割符之间的字节流合并解包，作为一次请求处理。
但性能消耗严重，所以适合短字节流的请求。

**应用：**
Netty io.netty.handler.codec.DelimiterBasedFrameDecoder
***

### **length field based**

**解释：**
协议头里添加一个字符流长度的字段，这样每次接收到字节流请求，
先解析字节流头部的协议，确定整个字节流的长度，根据具体的长度来读取完整的字节流来确定一次完整的消息请求。

**应用：**
Netty io.netty.handler.codec.LengthFieldBasedFrameDecoder
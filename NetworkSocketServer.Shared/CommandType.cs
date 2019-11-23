
// ReSharper disable once CheckNamespace
namespace NetworkSocketServer.Messages
{
    public enum CommandType
    {
        UploadFileRequest,
        UploadFileResponse,
        DownloadFileRequest,
        DownloadFileResponse,
        EchoRequest,
        TimeRequest,
        TextResponse
    }
}

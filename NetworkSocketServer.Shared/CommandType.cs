namespace NetworkSocketServer.Commands
{
    public enum CommandType
    {
        UploadFileRequest,
        UploadFileResponse,
        DownloadFileRequest,
        DownloadFileResponse,
        EchoRequest,
        TimeRequest,
        TextResponse,
        Disconnect
    }
}

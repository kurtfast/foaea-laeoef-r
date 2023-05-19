﻿namespace FileBroker.Model.Interfaces
{
    public interface IMailServiceRepository
    {
        string SendEmail(string subject, string recipients, string body, string attachmentPath = null);
    }
}


using System.Net.Mail;
using System.Net;

public interface ISmtpClient : IDisposable
{
    int Port { get; set; }
    string Host { get; set; }
    bool EnableSsl { get; set; }
    ICredentialsByHost Credentials { get; set; }
    void Send(MailMessage message);
}

// Implementación concreta
public class SmtpClientWrapper : ISmtpClient
{
    private readonly SmtpClient _smtpClient;

    public SmtpClientWrapper()
    {
        _smtpClient = new SmtpClient();
    }

    public int Port { get => _smtpClient.Port; set => _smtpClient.Port = value; }
    public string Host { get => _smtpClient.Host; set => _smtpClient.Host = value; }
    public bool EnableSsl { get => _smtpClient.EnableSsl; set => _smtpClient.EnableSsl = value; }
    public ICredentialsByHost Credentials { get => _smtpClient.Credentials; set => _smtpClient.Credentials = value; }

    public void Send(MailMessage message) => _smtpClient.Send(message);
    public void Dispose() => _smtpClient.Dispose();
}
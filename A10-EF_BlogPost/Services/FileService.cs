using System;
using Microsoft.Extensions.Logging;

namespace ApplicationTemplate.Services;

/// <summary>
///     This concrete service and method only exists an example.
///     It can either be copied and modified, or deleted.
/// </summary>
public class FileService : IFileService
{
    private readonly ILogger<IFileService> _logger;

    public FileService(ILogger<IFileService> logger)
    {
        _logger = logger;
    }
    public void Read()
    {
        _logger.Log(LogLevel.Information, "Reading");
        Console.WriteLine("*** I am reading");
    }

    public void Write()
    {
        _logger.Log(LogLevel.Information, "Writing");
        Console.WriteLine("*** I am writing");
    }
}

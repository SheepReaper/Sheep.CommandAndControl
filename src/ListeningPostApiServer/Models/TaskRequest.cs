using System.Collections.Generic;
using ListeningPostApiServer.Models;

///
public class TaskRequest
{
    ///
    public IEnumerable<int>? Ids { get; set; }

    ///
    public TaskBase? Task { get; set; }
}
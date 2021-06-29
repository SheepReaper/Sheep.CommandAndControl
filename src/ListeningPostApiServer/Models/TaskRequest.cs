using System.Collections.Generic;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ListeningPostApiServer.Models
{
    ///
    public class TaskRequest
    {
        ///
        public IEnumerable<int>? Ids { get; set; }

        ///
        public TaskBase? Task { get; set; }
    }
}
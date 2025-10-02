using System;

namespace Tecmave.Api.Models
{
    public class RoleChangeAudit
    {
        public long Id { get; set; }

        public int TargetUserId { get; set; }
        public string? TargetUserName { get; set; }

        public string? PreviousRole { get; set; }
        public string? NewRole { get; set; }

        public int? ChangedByUserId { get; set; }
        public string? ChangedByUserName { get; set; }

        public DateTime ChangedAtUtc { get; set; } = DateTime.UtcNow;
        public string Action { get; set; } = "assign";
        public string? Detail { get; set; }
        public string? SourceIp { get; set; }
    }
}
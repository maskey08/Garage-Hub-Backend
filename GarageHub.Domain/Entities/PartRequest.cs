using System;
using System.Collections.Generic;
using System.Text;

namespace GarageHub.Domain.Entities;

public class PartRequest
{
    public int RequestId { get; set; }
    public int CustomerId { get; set; }
    public string PartName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "pending"; // pending | sourced | rejected
    public DateTime RequestedAt { get; set; }

    public User Customer { get; set; } = null!;
}
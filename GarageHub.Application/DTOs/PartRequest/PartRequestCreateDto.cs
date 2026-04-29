using System;
using System.Collections.Generic;
using System.Text;

namespace GarageHub.Application.DTOs.PartRequest;

public class PartRequestCreateDto
{
    public string PartName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
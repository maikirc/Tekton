using System;
using System.Collections.Generic;

namespace Tekton.Api.Entities;

public partial class Product
{
    public long ProductId { get; set; }

    public string Name { get; set; } = null!;

    public bool Status { get; set; }

    public int Stock { get; set; }

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime CreationDate { get; set; }

    public string CreationUser { get; set; } = null!;

    public DateTime LastModificationDate { get; set; }

    public string LastModificationUser { get; set; } = null!;
}

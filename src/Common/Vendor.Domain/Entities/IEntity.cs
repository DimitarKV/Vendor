﻿using System.ComponentModel.DataAnnotations;

namespace Vendor.Domain.Entities;

public interface IEntity<T>
{
    public T Id { get; set; }
    public DateTime CreatedOn { get; }
    public DateTime UpdatedOn { get; }
}
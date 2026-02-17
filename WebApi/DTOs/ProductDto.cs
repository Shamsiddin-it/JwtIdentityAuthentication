using System;

namespace WebApi.DTOs;

public record ProductDto(Guid Id, string Name, decimal Price);
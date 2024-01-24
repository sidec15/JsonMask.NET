
# JsonMask.NET 

[![Build Status](https://github.com/sidec15/JsonMask.NET/actions/workflows/test.yaml/badge.svg)](https://github.com/sidec15/JsonMask.NET/actions/workflows/test.yaml)
[![codecov](https://codecov.io/gh/sidec15/JsonMask.NET/graph/badge.svg)](https://codecov.io/gh/sidec15/JsonMask.NET)

JsonMask.NET is a simple .NET port of the popular JavaScript library [json-mask](https://github.com/nemtsov/json-mask), designed to allow selective filtering of JSON data using a concise syntax. This library is perfect for reducing payloads in APIs, extracting specific parts from JSON, and more, with compatibility across all .NET applications.

## Features

- Lightweight and easy to integrate
- Supports the same syntax as the original json-mask library
- Available as a static utility or as a service for dependency injection

## Getting Started

### Prerequisites

- .NET 6 or higher

### Installation

Install JsonMask.NET via NuGet:

```bash
dotnet add package JsonMask.NET
```

## Usage

### Static Usage

```csharp
var original = "{ \"a\": 1, \"b\": 1 }";
var mask = "a";
var result = Masker.Mask(original, mask);
Console.WriteLine(result); // Output: "{ \"a\": 1 }"
```

### Using MaskerService

```csharp
IMaskerService maskerService = new MaskerService();
var original = "{ \"a\": 1, \"b\": 1 }";
var mask = "a";
var result = maskerService.Mask(original, mask);
Console.WriteLine(result); // Output: "{ \"a\": 1 }"
```

## Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**. Please refer to the [CONTRIBUTING.md](https://github.com/sidec15/JsonMask.NET/blob/master/CONTRIBUTING.md) file for detailed contribution guidelines.

## License

Distributed under the MIT License. See [LICENSE](https://github.com/sidec15/JsonMask.NET/blob/master/LICENSE) for more information.

## Acknowledgments

- Inspired by [json-mask](https://github.com/nemtsov/json-mask) by nemtsov

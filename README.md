# System.Text.Json.Extensions

## What is this repo about?

[System.Text.Json](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-overview) is the library which is written by .Net team to enable applications to perform serialization and deserialization of Json in .Net applications. This library is [more performant](https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-apis/#user-content-performance) than popular Newtonsoft.Json library. However, in my efforts to migrate from Newtonsoft.Json to System.Text.Json I found a few places where the library has significant gaps in functionality from Newtonsoft.Json. This repo intends to cover a few of those. Hopefully, over time this repo would not be needed and System.Text.Json will achieve near-parity with Newtonsoft.Json.

Current this repo has following extensions

- Converters
  - Allow booleans as string
  - Allow polymorphic serialization of properties (SystemTextJson already supports for top level objects, [see](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-polymorphism))
  - Enable specifying converter to use for each item in collection
  - Camel case serialization
- Encoder
  - Newtonsoft.Json compatible encoder supporting following beyond the provided Relaxed encoder
    - Support for Emojis to not be escaped

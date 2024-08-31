// Copyright (c) Bili Copilot. All rights reserved.

using System.Text.Json.Serialization;
using Richasy.BiliKernel.Models.Media;
namespace BiliCopilot.UI;

[JsonSerializable(typeof(MediaIdentifier))]
internal partial class JsonContext : JsonSerializerContext
{
}

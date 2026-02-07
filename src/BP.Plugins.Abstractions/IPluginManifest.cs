namespace BP.Plugins.Abstractions;

public interface IPluginManifest
{
    string Id { get; }
    string Name { get; }
    Version Version { get; }
    IReadOnlyCollection<string> Capabilities { get; }
}

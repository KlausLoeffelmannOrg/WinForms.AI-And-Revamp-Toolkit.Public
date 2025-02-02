namespace Chatty.ViewModels;

[Flags]
internal enum PersonalityFileExtractionSetting
{
    None = 0,
    ExtractCodeBlocksAutomatically = 1,
    SaveExtractedFilesAutomatically = 2,
    StoreInDedicatedFolders = 4,
    SelectFolderAtRuntime = 8
}

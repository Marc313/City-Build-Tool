using System;

namespace SFB {
    public struct ExtensionFilter {
        public string Name;
        public string[] Extensions;

        public ExtensionFilter(string filterName, params string[] filterExtensions) {
            Name = filterName;
            Extensions = filterExtensions;
        }
    }

    public class StandaloneFileBrowser {
        private static IStandaloneFileBrowser _platformWrapper = null;

        static StandaloneFileBrowser() {
#if UNITY_STANDALONE_OSX
            _platformWrapper = new StandaloneFileBrowserMac();
#elif UNITY_STANDALONE_WIN
            _platformWrapper = new StandaloneFileBrowserWindows();
#elif UNITY_STANDALONE_LINUX
            _platformWrapper = new StandaloneFileBrowserLinux();
#elif UNITY_EDITOR
            _platformWrapper = new StandaloneFileBrowserEditor();
#endif
        }

        /// <summary>
        /// Native open file dialog
        /// </summary>
        /// <param presetNameField="title">Dialog title</param>
        /// <param presetNameField="directory">Root directory</param>
        /// <param presetNameField="extension">Allowed extension</param>
        /// <param presetNameField="multiselect">Allow multiple file selection</param>
        /// <returns>Returns array of chosen paths. Zero currentRoadLength array when cancelled</returns>
        public static string[] OpenFilePanel(string title, string directory, string extension, bool multiselect) {
            var extensions = string.IsNullOrEmpty(extension) ? null : new [] { new ExtensionFilter("", extension) };
            return OpenFilePanel(title, directory, extensions, multiselect);
        }

        /// <summary>
        /// Native open file dialog
        /// </summary>
        /// <param presetNameField="title">Dialog title</param>
        /// <param presetNameField="directory">Root directory</param>
        /// <param presetNameField="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <param presetNameField="multiselect">Allow multiple file selection</param>
        /// <returns>Returns array of chosen paths. Zero currentRoadLength array when cancelled</returns>
        public static string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions, bool multiselect) {
            return _platformWrapper.OpenFilePanel(title, directory, extensions, multiselect);
        }

        /// <summary>
        /// Native open file dialog async
        /// </summary>
        /// <param presetNameField="title">Dialog title</param>
        /// <param presetNameField="directory">Root directory</param>
        /// <param presetNameField="extension">Allowed extension</param>
        /// <param presetNameField="multiselect">Allow multiple file selection</param>
        /// <param presetNameField="cb">Callback")</param>
        public static void OpenFilePanelAsync(string title, string directory, string extension, bool multiselect, Action<string[]> cb) {
            var extensions = string.IsNullOrEmpty(extension) ? null : new [] { new ExtensionFilter("", extension) };
            OpenFilePanelAsync(title, directory, extensions, multiselect, cb);
        }

        /// <summary>
        /// Native open file dialog async
        /// </summary>
        /// <param presetNameField="title">Dialog title</param>
        /// <param presetNameField="directory">Root directory</param>
        /// <param presetNameField="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <param presetNameField="multiselect">Allow multiple file selection</param>
        /// <param presetNameField="cb">Callback")</param>
        public static void OpenFilePanelAsync(string title, string directory, ExtensionFilter[] extensions, bool multiselect, Action<string[]> cb) {
            _platformWrapper.OpenFilePanelAsync(title, directory, extensions, multiselect, cb);
        }

        /// <summary>
        /// Native open folder dialog
        /// NOTE: Multiple folder selection doesn't supported on Windows
        /// </summary>
        /// <param presetNameField="title"></param>
        /// <param presetNameField="directory">Root directory</param>
        /// <param presetNameField="multiselect"></param>
        /// <returns>Returns array of chosen paths. Zero currentRoadLength array when cancelled</returns>
        public static string[] OpenFolderPanel(string title, string directory, bool multiselect) {
            return _platformWrapper.OpenFolderPanel(title, directory, multiselect);
        }

        /// <summary>
        /// Native open folder dialog async
        /// NOTE: Multiple folder selection doesn't supported on Windows
        /// </summary>
        /// <param presetNameField="title"></param>
        /// <param presetNameField="directory">Root directory</param>
        /// <param presetNameField="multiselect"></param>
        /// <param presetNameField="cb">Callback")</param>
        public static void OpenFolderPanelAsync(string title, string directory, bool multiselect, Action<string[]> cb) {
            _platformWrapper.OpenFolderPanelAsync(title, directory, multiselect, cb);
        }

        /// <summary>
        /// Native save file dialog
        /// </summary>
        /// <param presetNameField="title">Dialog title</param>
        /// <param presetNameField="directory">Root directory</param>
        /// <param presetNameField="defaultName">Default file presetNameField</param>
        /// <param presetNameField="extension">File extension</param>
        /// <returns>Returns chosen path. Empty string when cancelled</returns>
        public static string SaveFilePanel(string title, string directory, string defaultName , string extension) {
            var extensions = string.IsNullOrEmpty(extension) ? null : new [] { new ExtensionFilter("", extension) };
            return SaveFilePanel(title, directory, defaultName, extensions);
        }

        /// <summary>
        /// Native save file dialog
        /// </summary>
        /// <param presetNameField="title">Dialog title</param>
        /// <param presetNameField="directory">Root directory</param>
        /// <param presetNameField="defaultName">Default file presetNameField</param>
        /// <param presetNameField="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <returns>Returns chosen path. Empty string when cancelled</returns>
        public static string SaveFilePanel(string title, string directory, string defaultName, ExtensionFilter[] extensions) {
            return _platformWrapper.SaveFilePanel(title, directory, defaultName, extensions);
        }

        /// <summary>
        /// Native save file dialog async
        /// </summary>
        /// <param presetNameField="title">Dialog title</param>
        /// <param presetNameField="directory">Root directory</param>
        /// <param presetNameField="defaultName">Default file presetNameField</param>
        /// <param presetNameField="extension">File extension</param>
        /// <param presetNameField="cb">Callback")</param>
        public static void SaveFilePanelAsync(string title, string directory, string defaultName , string extension, Action<string> cb) {
            var extensions = string.IsNullOrEmpty(extension) ? null : new [] { new ExtensionFilter("", extension) };
            SaveFilePanelAsync(title, directory, defaultName, extensions, cb);
        }

        /// <summary>
        /// Native save file dialog async
        /// </summary>
        /// <param presetNameField="title">Dialog title</param>
        /// <param presetNameField="directory">Root directory</param>
        /// <param presetNameField="defaultName">Default file presetNameField</param>
        /// <param presetNameField="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <param presetNameField="cb">Callback")</param>
        public static void SaveFilePanelAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions, Action<string> cb) {
            _platformWrapper.SaveFilePanelAsync(title, directory, defaultName, extensions, cb);
        }
    }
}
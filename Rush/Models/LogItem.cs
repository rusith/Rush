namespace Rush.Models
{
    public class LogItem
    {
        public enum LogItemType
        {
            CannotCreateDirectory,
            CannotCopyFile,
            PathTooLong,
            Duplicate,
            NoTag,
            TitleNotFound,
            ArtistNotFound,
            AlbumNotFound,
            TrackNotFound,
            GenreNotFound,
            YearNotFound,
            CannotDeleteExistingFile
        }

        public LogItem(LogItemType type, string message,params object[] messageparams)
        {
            MessageType = type;
            Message = string.Format(message, messageparams);
        }

        public string Message { get; set; }
        public LogItemType MessageType { get; set; }

        public string TypeString
        {
            get
            {
                switch (MessageType)
                {
                    case LogItemType.CannotCreateDirectory:
                        return "Cannot Create Directory";
                    case LogItemType.CannotCopyFile:
                        return "Cannot Copy File";
                    case LogItemType.PathTooLong:
                        return "Path Too Long";
                    case LogItemType.Duplicate:
                        return "Duplicate File";
                    case LogItemType.NoTag:
                        return "No Tag Found";
                    case LogItemType.TitleNotFound:
                        return "Title Not Found";
                    case LogItemType.ArtistNotFound:
                        return "Artist Not Found";
                    case LogItemType.AlbumNotFound:
                        return "Album Not Found";
                    case LogItemType.TrackNotFound:
                        return "Track Not Found";
                    case LogItemType.GenreNotFound:
                        return "Genre Not Found";
                    case LogItemType.YearNotFound:
                        return "Year Not Found";
                    case LogItemType.CannotDeleteExistingFile:
                        return "Cannot Delete Existing File";
                    default:
                        return "";
                }
            }
        }

        public static string TypeToString(LogItemType type)
        {
            switch (type)
            {
                case LogItemType.CannotCreateDirectory:
                    return "Cannot Create Directory";
                case LogItemType.CannotCopyFile:
                    return "Cannot Copy File";
                case LogItemType.PathTooLong:
                    return "Path Too Long";
                case LogItemType.Duplicate:
                    return "Duplicate File";
                case LogItemType.NoTag:
                    return "No Tag Found";
                case LogItemType.TitleNotFound:
                    return "Title Not Found";
                case LogItemType.ArtistNotFound:
                    return "Artist Not Found";
                case LogItemType.AlbumNotFound:
                    return "Album Not Found";
                case LogItemType.TrackNotFound:
                    return "Track Not Found";
                case LogItemType.GenreNotFound:
                    return "Genre Not Found";
                case LogItemType.YearNotFound:
                    return "Year Not Found";
                case LogItemType.CannotDeleteExistingFile:
                    return "Cannot Delete Existing File";
                default:
                    return "";
            }
        }

        public static LogItemType StringToType(string str)
        {
            switch (str)
            {
                case "Cannot Create Directory":
                    return LogItemType.CannotCreateDirectory;
                case "Cannot Copy File":
                    return LogItemType.CannotCopyFile;
                case "Path Too Long":
                    return LogItemType.PathTooLong;
                case "Duplicate File":
                    return LogItemType.Duplicate;
                case "No Tag Found":
                    return LogItemType.NoTag;
                case "Title Not Found":
                    return LogItemType.TitleNotFound;
                case "Artist Not Found":
                    return LogItemType.ArtistNotFound;
                case "Album Not Found":
                    return LogItemType.AlbumNotFound;
                case "Track Not Found":
                    return LogItemType.TrackNotFound;
                case "Genre Not Found":
                    return LogItemType.GenreNotFound;
                case "Year Not Found":
                    return LogItemType.YearNotFound;
                case "Cannot Delete Existing File":
                    return LogItemType.CannotDeleteExistingFile;
                default:
                    return LogItemType.Duplicate;
            }
        }
    }
}

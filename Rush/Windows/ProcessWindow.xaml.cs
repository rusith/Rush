using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Rush.Extensions;
using Rush.Models;

namespace Rush.Windows
{
    public partial class ProcessWindow 
    {
        private readonly OrganizeOrder _order;
        private readonly HashSet<string> _sources;
        private readonly DirectoryInfo _destination;
        private readonly List<FileInformation> _files;
        private readonly bool _overwrite;

        private CancellationTokenSource _cancellation;
          
        public ProcessWindow(OrganizeOrder order,HashSet<string> sources,DirectoryInfo destination,List<FileInformation> files,bool overwrite  )
        {
            _order = order;
            _sources = sources;
            _destination = destination;
            _files = files;
            _overwrite = overwrite;
            _cancellation=new CancellationTokenSource();
            InitializeComponent();
        }

        private void Organize()
        {
           Task.Factory.StartNew(() =>
            {
                try
                {
                    ProgressBar.Dispatcher.Invoke(() =>
                    {
                        ProgressBar.Minimum = 0;
                        ProgressBar.Maximum = _files.Count;
                        ProgressBar.Value = 0;
                    });

                    TitleLabel.Dispatcher.Invoke(() =>
                    {
                        TitleLabel.Content = "Processing Files";
                    });

                    var fileindex = new[] {0};
                    var log = new List<LogItem>();
                    foreach (var file in _files)
                    {
                        if (_cancellation.IsCancellationRequested)
                        {
                            if (log.Count > 0)
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    var logwindow = new LogWindow(_files, log,_destination,_sources);
                                    logwindow.ShowDialog();
                                });
                            }
                            Dispatcher.Invoke(() =>
                            {
                                Close();
                            });
                            return;
                        }
                        var file1 = file.SourceFile;
                        var file2 = file;
                        foreach (
                            var f in
                                _files.Where(f => !f.Equals(file2) && !f.IsDuplicate)
                                    .Where(f => f.SourceFile.Name == file2.SourceFile.Name &&
                                                f.SourceFile.Length == file2.SourceFile.Length))
                        {
                            f.IsDuplicate = true;
                            f.Duplicate = file;
                            log.Add(new LogItem(LogItem.LogItemType.Duplicate, "'{0}' is with file {1}", f.SourceFile.Name,
                                f.Duplicate.SourceFile.Name));
                        }

                        MessageLabel.Dispatcher.Invoke(() =>
                        {
                            MessageLabel.Content = file1.Name;
                        });

                        ProgressBar.Dispatcher.Invoke(() =>
                        {
                            ProgressBar.Value = fileindex[0];
                        });

                        var taginfo = TagLib.File.Create(file1.FullName);
                        if (taginfo == null)
                            log.Add(new LogItem(LogItem.LogItemType.NoTag,
                                "The file '{0}' has no tag info it will be copied to unknown[] folder", file1.Name));
                        
                        var newfile = _destination.FullName;
                        foreach (var tag in _order.Order)
                        {
                            switch (tag)
                            {
                                case OrderElement.Album:
                                    var alb = taginfo.Tag.Album;
                                    if (string.IsNullOrWhiteSpace(alb))
                                    {
                                        alb = "UnknownAlbum";
                                        log.Add(new LogItem(LogItem.LogItemType.AlbumNotFound, "Album tag not found in file '{0}'. File Will Copy to 'UnknownAlbum' folder", file1.Name));
                                    }
                                    newfile = Path.Combine(newfile, alb.ToSafeFileName());
                                    break;
                                case OrderElement.Artist:
                                    var art = "UnknownArtist";
                                    if (taginfo.Tag.Performers != null && taginfo.Tag.Performers.Length > 0 &&
                                        taginfo.Tag.Performers.All(c => c.Length > 0))
                                        art = taginfo.Tag.Performers.Aggregate((c, n) => c + " & " + n).TrimEnd('&');
                                    if(art== "UnknownArtist")
                                        log.Add(new LogItem(LogItem.LogItemType.ArtistNotFound, "Artist tag not found in file '{0}'. File Will Copy to 'UnknownArtist' folder", file1.Name));
                                    newfile = Path.Combine(newfile, art.ToSafeFileName());
                                    break;
                                case OrderElement.Genre:
                                    var genre = "UnknownGenre";
                                    if (taginfo.Tag.Genres != null && taginfo.Tag.Genres.Length > 0 &&
                                        taginfo.Tag.Genres.All(c => c.Length > 0))
                                        genre = taginfo.Tag.Genres.Aggregate((c, n) => c + " & " + n).TrimEnd('&');
                                    if(genre== "UnknownGenre")
                                        log.Add(new LogItem(LogItem.LogItemType.GenreNotFound, "Genre tag not found in file '{0}'. File Will Copy to 'UnknownGenre' folder", file1.Name));
                                    newfile = Path.Combine(newfile, genre.ToSafeFileName());
                                    break;
                                case OrderElement.Year:
                                    var year = "UnknownYear";
                                    if (taginfo.Tag.Year > 0)
                                        year = taginfo.Tag.Year.ToString();
                                    if(year== "UnknownYear")
                                        log.Add(new LogItem(LogItem.LogItemType.YearNotFound, "Year tag not found in file '{0}'. File Will Copy to 'UnknownYear' folder", file1.Name));
                                    newfile = Path.Combine(newfile, year);
                                    break;
                                case OrderElement.File:
                                    if (_order.FileNameTemplate.Template.Count > 0)
                                    {
                                        var fn = "";
                                        var litindex = 0;
                                        foreach (var f in _order.FileNameTemplate.Template)
                                        {
                                            switch (f)
                                            {
                                                case FileNameItem.Album:
                                                    var album = taginfo.Tag.Album;
                                                    if (string.IsNullOrWhiteSpace(album))
                                                    {
                                                        log.Add(new LogItem(LogItem.LogItemType.AlbumNotFound, "Album tag not found in file '{0}'. Album part of the file name will replace with empty string", file1.Name));
                                                        album = "";
                                                    }
                                                        
                                                    fn += album;
                                                    break;
                                                case FileNameItem.Artist:
                                                    var artist = "";
                                                    if (taginfo.Tag.Performers != null &&
                                                        taginfo.Tag.Performers.Length > 0 &&
                                                        taginfo.Tag.Performers.All(c => c.Length > 0))
                                                        artist =
                                                            taginfo.Tag.Performers.Aggregate((c, n) => c + " & " + n)
                                                                .TrimEnd('&');
                                                    if (string.IsNullOrWhiteSpace(artist))
                                                    {
                                                        log.Add(new LogItem(LogItem.LogItemType.ArtistNotFound, "Artist tag not found in file '{0}'. Artist part of the file name will replace with empty string", file1.Name));
                                                        artist = "";
                                                    }
                                                    fn += artist;
                                                    break;
                                                case FileNameItem.Count:
                                                    var count =
                                                        _files.Where(v => v.DestinationFile != null)
                                                            .Count(v => v.DestinationFile.DirectoryName == newfile) + 1;
                                                    fn += count.ToString();
                                                    break;
                                                case FileNameItem.Literel:
                                                    var lit =
                                                        _order.FileNameTemplate.Literel[litindex];
                                                    litindex++;
                                                    if (string.IsNullOrWhiteSpace(lit))
                                                        fn += "";
                                                    else
                                                        fn += lit;
                                                    break;
                                                case FileNameItem.Title:
                                                    var title = taginfo.Tag.Title;
                                                    if (string.IsNullOrWhiteSpace(title))
                                                    {
                                                        log.Add(new LogItem(LogItem.LogItemType.TitleNotFound, "Title tag not found in file '{0}'. Title part of the file name will replace with empty string", file1.Name));
                                                        title = "";
                                                    }
                                                        
                                                    fn += title;
                                                    break;
                                                case FileNameItem.Trak:
                                                    var track = taginfo.Tag.Track;
                                                    if (track < 1)
                                                    {
                                                        log.Add(new LogItem(LogItem.LogItemType.TrackNotFound, "Track tag not found in file '{0}'. Track part of the file name will replace with empty string", file1.Name));
                                                        fn += "";
                                                    }
                                                    else
                                                        fn += track.ToString();
                                                    break;
                                            }

                                        }
                                        fn += file1.Extension;
                                        newfile = Path.Combine(newfile, fn.ToSafeFileName());
                                    }
                                    else
                                    {
                                        var fileName = file1.Name;
                                        newfile = Path.Combine(newfile, fileName.ToSafeFileName());
                                    }
                                    break;
                            }
                        }
                        file.DestinationFile = new FileInfo(newfile);
                        file.Processed = true;
                        fileindex[0] = fileindex[0] + 1;
                    }
                    TitleLabel.Dispatcher.Invoke(() =>
                    {
                        TitleLabel.Content = "Copying  Files";
                    });
                    fileindex[0] = 0;
                    _cancellation = new CancellationTokenSource();
                    var pathTooLong = new DirectoryInfo(Path.Combine(_destination.FullName, "PathTooLong"));
                    foreach (var file in _files)
                    {
                        if (_cancellation.IsCancellationRequested)
                        {
                            if (log.Count > 0)
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    var logwindow = new LogWindow(_files, log,_destination,_sources);
                                    logwindow.ShowDialog();
                                });
                            }
                            Dispatcher.Invoke(() =>
                            {
                                Close();
                            });
                            return;
                        }
                        var sf = file.SourceFile;
                        MessageLabel.Dispatcher.Invoke(() =>
                        {
                            MessageLabel.Content = sf.Name;
                        });

                        ProgressBar.Dispatcher.Invoke(() =>
                        {
                            ProgressBar.Value = fileindex[0];
                        });
                        fileindex[0]++;

                        if (file.DestinationFile.Directory != null && !file.DestinationFile.Directory.Exists)
                        {
                            try
                            {
                                Directory.CreateDirectory(file.DestinationFile.Directory.FullName);
                            }
                            catch (Exception)
                            {
                                log.Add(new LogItem(LogItem.LogItemType.CannotCreateDirectory, "Cannot Create Directory '{0}' . File '{1}' Will Not be Coped", file.DestinationFile.Directory.FullName,file.DestinationFile.Name));
                                continue;
                            }
                        }

                        if (file.DestinationFile.Exists && _overwrite)
                        {
                            try
                            {
                                File.SetAttributes(file.DestinationFile.FullName, FileAttributes.Normal);
                                file.DestinationFile.Delete();
                            }
                            catch (Exception)
                            {
                                log.Add(new LogItem(LogItem.LogItemType.CannotDeleteExistingFile, "Cannot delete File '{0}'. The New File Will Not be Coped (Overwrite Enabled)", file.DestinationFile.Directory.FullName, file.DestinationFile.Name));
                                continue;
                            }
                        }
                        try
                        {
                            File.Copy(file.SourceFile.FullName, file.DestinationFile.FullName, true);
                            file.Copied = true;
                        }
                        catch (PathTooLongException)
                        {

                            if (pathTooLong.Exists == false)
                            {
                                try
                                {
                                    Directory.CreateDirectory(pathTooLong.FullName);
                                }
                                catch (Exception)
                                {
                                    log.Add(new LogItem(LogItem.LogItemType.CannotCreateDirectory, "Cannot Create PathTooLong Directory . File '{1}' Will Not be Coped", file.DestinationFile.Directory.FullName, file.DestinationFile.Name));
                                    continue;
                                }
                                File.Copy(file.SourceFile.FullName, Path.Combine(pathTooLong.FullName,file.DestinationFile.Name), true);
                                file.Copied = true;
                            }
                            log.Add(new LogItem(LogItem.LogItemType.PathTooLong, "Cannot Copy File '{0}' because the path is too long. the file copied to 'PathTooLong' folder", file.DestinationFile.Name));
                        }
                    }
                    if (log.Count > 0)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            var logwindow = new LogWindow(_files, log,_destination,_sources);
                            logwindow.ShowDialog();
                        });
                    }
                    Dispatcher.Invoke(() =>
                    {
                        Close();
                    });
                }
                catch (Exception)
                {
                    // ignored
                }



            },_cancellation.Token);
           
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            _cancellation.Cancel();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            Organize();
        }
    }
}

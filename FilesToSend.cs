using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Timers;
using Windows.UI.Xaml.Shapes;

namespace FilesToSend
{
    public class FilesToSend
    {
        private FileSystemWatcher watcher;
        private Timer timer;
        private int interval; // em milissegundos
        private List<string> changes = new List<string>();
        Notificacao notify = new Notificacao();
        dynamic prop;

        public void Initialize()
        {
            Directories.Check();

            notify.SendToastNotification("Monitoramento", "Monitoramento de pasta iniciado!", null);

            string configJson = System.IO.Path.Combine(Directories.ProgramPath, "props.json");
            if (File.Exists(configJson))
            {
                string configContent = File.ReadAllText(configJson);
                prop = JsonConvert.DeserializeObject<dynamic>(configContent);
                string originPath = prop.directorys.origin.path;
                string originHost = prop.directorys.origin.credential.server;
                string originUser = prop.directorys.origin.credential.user;
                string originPassword = prop.directorys.origin.credential.password;

                if (originPath.StartsWith("\\\\"))
                {
                    string command = $"/C cmdkey /add:{originHost} /user:{originUser} /pass:{originPassword}";
                    System.Diagnostics.Process.Start("CMD.exe", command);
                }

                if (!string.IsNullOrEmpty(originPath))
                {
                    watcher = new FileSystemWatcher();
                    watcher.Path = originPath;
                    watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                    watcher.Filter = "*.*";

                    watcher.Changed += OnChanged;
                    watcher.Created += OnChanged;
                    watcher.Deleted += OnChanged;
                    watcher.Renamed += OnRenamed;

                    watcher.EnableRaisingEvents = true;

                    timer = new Timer();
                    interval = 10000; // por exemplo, 10 segundos
                    timer.Interval = interval;
                    timer.Elapsed += OnElapsed;
                    timer.Start();
                }

            }
            else
            {
                notify.SendToastNotification("Monitoramento", "Arquivo de propriedades não encontrado. Verifique a documentação e reinicie o serviço", null);
            }
        }

        private async void OnChanged(object source, FileSystemEventArgs e)
        {
            string change = $"";
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:

                    //change = $"Arquivo: {e.Name} {e.ChangeType}";

                    change = "Arquivo recebido!";

                    WebSocketFileSender sender = new WebSocketFileSender("ws://192.168.1.133:3030/uploadFiles?hostname=Junior");
                    await sender.SendFileAsync(e.FullPath);

                    break;
                case WatcherChangeTypes.Deleted:

                    //change = $"Arquivo: {e.Name} {e.ChangeType}";

                    change = "Arquivo enviado!";

                    break;
            }

            changes.Add(change);
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            string change = $"Arquivo: {e.OldFullPath} renomeado para {e.FullPath}";
            changes.Add(change);
        }

        private void OnElapsed(object source, ElapsedEventArgs e)
        {
            if (changes.Count > 0)
            {
                StringBuilder msg = new StringBuilder();
                foreach (string change in changes)
                {
                    msg.Append(change);
                }
                notify.SendToastNotification(prop.name.ToString() , msg.ToString(), null);
                changes.Clear();
            }
        }
    }

    //public class FilesToSend
    //{
    //    private FileSystemWatcher watcher;
    //    Notificacao notify = new Notificacao();

    //    public void Initialize()
    //    {
    //        Directories.Check();

    //        notify.SendToastNotification("Monitoramento", "Monitoramento de pasta iniciado!", null);


    //        // Atualizando dados na tabela
    //        string configJson = System.IO.Path.Combine(Directories.ProgramPath, "props.json");
    //        if (File.Exists(configJson))
    //        {
    //            string configContent = File.ReadAllText(configJson);
    //            dynamic prop = JsonConvert.DeserializeObject<dynamic>(configContent);
    //            string originPath = prop.directorys.origin.path;
    //            string originHost = prop.directorys.origin.credential.server;
    //            string originUser = prop.directorys.origin.credential.user;
    //            string originPassword = prop.directorys.origin.credential.password;

    //            // Validando diretorios
    //            if (originPath.StartsWith("\\\\"))
    //            {
    //                string command = $"/C cmdkey /add:{originHost} /user:{originUser} /pass:{originPassword}";
    //                System.Diagnostics.Process.Start("CMD.exe", command);
    //            }

    //            if (!string.IsNullOrEmpty(originPath))
    //            {
    //                watcher = new FileSystemWatcher();
    //                watcher.Path = originPath;
    //                watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
    //                watcher.Filter = "*.*";

    //                // Adiciona os manipuladores de eventos.
    //                watcher.Changed += OnChanged;
    //                watcher.Created += OnChanged;
    //                watcher.Deleted += OnChanged;
    //                watcher.Renamed += OnRenamed;

    //                // Inicia o monitoramento.
    //                watcher.EnableRaisingEvents = true;
    //            }

    //        }
    //        else
    //        {
    //            notify.SendToastNotification("Monitoramento", "Arquivo de propriedades não encontrado. Verifique a documentação e reinicie o serviço", null);
    //        }
    //    }

    //    // Define os manipuladores de eventos.
    //    private void OnChanged(object source, FileSystemEventArgs e)
    //    {
    //        // Escreve no log quando um arquivo é alterado, criado ou excluído.
    //        WriteLog($"Arquivo: {e.FullPath} {e.ChangeType}");

    //        notify.SendToastNotification("Evento de Arquivo", $"Arquivo: {e.FullPath} {e.ChangeType}", null);
    //    }

    //    private void OnRenamed(object source, RenamedEventArgs e)
    //    {
    //        // Escreve no log quando um arquivo é renomeado.
    //        WriteLog($"Arquivo: {e.OldFullPath} renomeado para {e.FullPath}");

    //        notify.SendToastNotification("Evento de Arquivo", $"Arquivo: {e.OldFullPath} renomeado para {e.FullPath}", null);
    //    }

    //    private void WriteLog(string log)
    //    {
    //        Console.WriteLine(log);
    //    }
    //}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesToSend
{
    internal class Notificacao
    {
        public void SendToastNotification(string title, string message, string icone)
        {
            try
            {
                string iconPath = icone != null ? icone : "icon.ico";
                // Definir o caminho para o ícone do aplicativo
                string appLogoPath = $"C:\\Drogaleste\\Client\\Modules\\Notificacoes\\{iconPath}";
                string appLogoUrl = "https://raw.githubusercontent.com/juniioroliveira/DrogalesteSocketExtensions/main/icon.ico";


                // Criar a notificação do Toast
                var toastContentBuilder = new Microsoft.Toolkit.Uwp.Notifications.ToastContentBuilder()
                    .AddArgument("action", "viewConversation")
                    .AddArgument("conversationId", 9813)
                    .AddText(title)
                    .AddText(message);

                // Verificar se o arquivo de ícone existe antes de adicioná-lo
                if (File.Exists(appLogoPath))
                {
                    // Adicionar o ícone do aplicativo
                    toastContentBuilder.AddAppLogoOverride(new Uri($"file:///{appLogoPath}"));
                }

                // Adicionar o ícone do aplicativo a partir da URL
                //toastContentBuilder.AddAppLogoOverride(new Uri(appLogoUrl));


                // Exibir a notificação do Toast
                toastContentBuilder.Show();
            }
            catch (Exception ex)
            {
                //Log($"Erro ao exibir notificação: {ex.Message}");
            }
        }
    }
}

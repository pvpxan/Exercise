using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTTPHandler;
using Newtonsoft.Json;

namespace oloCateringExercise
{
    public class MenuReader
    {
        public Action<WebMenu> ReadComplete { get; set; }

        public void ReadMenu(string menuAddress)
        {
            if (ReadComplete == null)
            {
                return;
            }
            
            WebMenu webMenu = new WebMenu();
            Task t = Task.Run(async () =>
            {
                webMenu = await readMenuTask(menuAddress);
                ReadComplete(webMenu);
            });
        }

        private async Task<WebMenu> readMenuTask(string menuAddress)
        {
            return await Task.Run(() => blockingReadMenuWork(menuAddress));
        }

        private WebMenu blockingReadMenuWork(string menuAddress)
        {
            HTTP http = new HTTP();
            string[] menuFileLines = http.ReadWebTextFile(menuAddress);
            if (menuFileLines == null)
            {
                return new WebMenu();
            }

            string menuJSON = string.Join(Environment.NewLine, menuFileLines);
            WebMenu webMenu = JsonConvert.DeserializeObject<WebMenu>(menuJSON);
            webMenu.MenuItems.Sort(new MenuSort());
            return webMenu;
        }
    }
}

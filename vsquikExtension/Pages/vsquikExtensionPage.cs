using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace vsquikExtension
{
    internal sealed partial class vsquikExtensionPage : ListPage
    {
        public vsquikExtensionPage()
        {
            Icon = IconHelpers.FromRelativePath(path: "Assets\\vscode.png");
            Title = "VSQuik";
            Name = "Open VS Code Project";
        }

        public override IListItem[] GetItems()
        {
            var items = new List<IListItem>();

            // Path to VS Code storage.json
            string jsonPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"Code\User\globalStorage\storage.json"
            );

            if (!File.Exists(jsonPath))
                return items.ToArray();

            string jsonText = File.ReadAllText(jsonPath);
            JObject root = JObject.Parse(jsonText);

            // Get the "workspaces" object under profileAssociations
            var workspaces = root["profileAssociations"]?["workspaces"] as JObject;
            if (workspaces == null)
                return items.ToArray();

            foreach (var ws in workspaces.Properties())
            {
                string uri = ws.Name;
                string path = null;

                if (uri.StartsWith("file://"))
                {
                    try
                    {
                        path = new Uri(uri).LocalPath;
                    }
                    catch
                    {
                        continue;
                    }
                }
                else if (uri.StartsWith("vscode-remote://"))
                {
                    path = uri;
                }

                if (string.IsNullOrEmpty(path))
                    continue;

                // Create a ListItem with an InvokableCommand to open VS Code
                items.Add(new ListItem(new OpenVsCodeCommand(path))
                {
                    Title = System.IO.Path.GetFileName(path),
                    Subtitle = path
                });
            }

            return items.ToArray();
        }


    }
}

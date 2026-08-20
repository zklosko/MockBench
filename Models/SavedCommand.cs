using System;
using System.Collections.Generic;
using System.Text;

namespace MockBench.Models
{
    internal class SavedCommand
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;

        SavedCommand(string name, string description, string command)
        {
            Name = name;
            Description = description;
            Command = command;
        }
    }
}

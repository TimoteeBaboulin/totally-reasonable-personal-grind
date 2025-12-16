using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using TotallyPersonalReasonableGrind.Bot.Interfaces;

namespace TotallyPersonalReasonableGrind.Bot.CommandDispatcher;

public class CommandDispatcher
{
    private static Dictionary<ulong, Type> m_commandMaps = new();
    private static Dictionary<ulong, ICommand> m_commandInstances = new();

    private List<Type> GetCommandTypes()
    {
        // Get all types that implement ICommand interface in the current assembly
        var commandInterfaceType = typeof(ICommand);
        var commandTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => commandInterfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .ToList();
        return commandTypes;
    }
    
    public void RegisterCommands()
    {
        List<Type> commandTypes = GetCommandTypes();
        foreach (var commandType in commandTypes)
        {
            var buildPropertiesMethod = commandType.GetMethod("BuildProperties",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            if (buildPropertiesMethod == null)
                continue;

            var commandProperties = buildPropertiesMethod.Invoke(null, null) as SocketApplicationCommand;
            if (commandProperties == null)
                continue;

            m_commandMaps[commandProperties.Id] = commandType;
        }
    }

    bool TryGetCommandType(ulong id, out Type? outCommandType)
    {
        return m_commandMaps.TryGetValue(id, out outCommandType);
    }
    
    public async Task OnSlashCommandExecuted(SocketSlashCommand command)
    {
        command.DeferAsync();
        
        Type? commandType;
        if (!TryGetCommandType(command.Data.Id, out commandType))
        {
            await command.RespondAsync("Command not found.");
            return;
        }
        
        ICommand commandInstance = Activator.CreateInstance(commandType) as ICommand;
        if (commandInstance == null)
        {
            await command.RespondAsync("Failed to create command instance.");
            return;
        }
        
        bool result = await commandInstance.OnSlashCommand(command);
        if (result)
        {
            m_commandInstances.Add(commandInstance.Id, commandInstance);
        }
    }
    
    public async Task OnComponentExecuted(SocketMessageComponent component)
    {
        string customId = component.Data.CustomId;
        await component.DeferAsync();
        
        string[] tokens = customId.Split('|');
        if (tokens.Length == 0)
        {
            await component.RespondAsync("Invalid component ID.");
            return;
        }
        
        ulong id = ulong.Parse(tokens[0]);
        if (!m_commandInstances.TryGetValue(id, out ICommand? commandInstance))
        {
            await component.RespondAsync("Command instance not found for component.");
            return;
        }

        if (!await commandInstance.OnComponent(component))
        {
            m_commandInstances.Remove(id);
        }
    }
}
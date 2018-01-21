using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

// ReSharper disable once CheckNamespace
public class ModuleWeaver: BaseModuleWeaver
{

    public override void Execute()
    {
        var methodDefinition = FindType("ReactiveTracker.TrackEventInitializer")
            .Methods
            .Single(x=>x.Name=="Init");
        var initMethod = ModuleDefinition.ImportReference(methodDefinition);
        foreach (var typeDefinition in
            ModuleDefinition.Types.Where(
                x => x.CustomAttributes.Any(
                    attribute => attribute.AttributeType.FullName == "ReactiveTracker.TrackEventAttribute")))
        {
            WeaveTracker(typeDefinition, initMethod);
        }
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "ReactiveTracker";
    }

    private static void WeaveTracker(TypeDefinition typeDefinition, MethodReference initMethodReference)
    {
        foreach (var constructor in typeDefinition.GetConstructors())
        {
            var body = constructor.Body;
            body.Instructions.Remove(body.Instructions.Last());
            body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            body.Instructions.Add(Instruction.Create(OpCodes.Call, initMethodReference));
            body.Instructions.Add(Instruction.Create(OpCodes.Ret));
        }
    }
}

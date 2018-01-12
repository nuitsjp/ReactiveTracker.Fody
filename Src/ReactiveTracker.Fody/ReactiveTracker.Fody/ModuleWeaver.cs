using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using ReactiveTracker;

// ReSharper disable once CheckNamespace
public class ModuleWeaver
{
    public ModuleDefinition ModuleDefinition { get; set; }

    public void Execute()
    {
        var initMethod =
            ModuleDefinition.ImportReference(typeof(TrackEventInitializer).GetRuntimeMethods().Single(x => x.Name == "Init"));
        foreach (var typeDefinition in
            ModuleDefinition.Types.Where(
                x => x.CustomAttributes.Any(
                    attribute => attribute.AttributeType.FullName == typeof(TrackEventAttribute).FullName)))
        {
            WeaveTracker(typeDefinition, initMethod);
        }
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

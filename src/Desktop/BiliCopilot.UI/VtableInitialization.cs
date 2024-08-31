// Copyright (c) Bili Copilot. All rights reserved.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal static class VtableInitialization
{
    // This registers with CsWinRT a way to provide vtables for a couple types that CsWinRT
    // doesn't detect today but we use so that they can be made AOT and trimming safe.
    [ModuleInitializer]
    internal static void Initialize()
    {
        WinRT.ComWrappersSupport.RegisterTypeComInterfaceEntriesLookup(LookupVtableEntries);
        WinRT.ComWrappersSupport.RegisterTypeRuntimeClassNameLookup(new Func<Type, string>(LookupRuntimeClassName));
    }

    private static ComWrappers.ComInterfaceEntry[] LookupVtableEntries(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            var elemType = type.GetGenericArguments()[0];
            if (elemType.IsEnum || (elemType.IsClass && elemType != typeof(string)))
            {
                // CsWinRT already generates these for other scenarios we use.
                _ = WinRT.BiliCopilot_UIGenericHelpers.IReadOnlyList_object.Initialized;
                _ = WinRT.BiliCopilot_UIGenericHelpers.IEnumerable_object.Initialized;

                return new ComWrappers.ComInterfaceEntry[]
                {
                    new ComWrappers.ComInterfaceEntry
                    {
                        IID = ABI.System.Collections.Generic.IReadOnlyListMethods<object>.IID,
                        Vtable = ABI.System.Collections.Generic.IReadOnlyListMethods<object>.AbiToProjectionVftablePtr,
                    },
                    new ComWrappers.ComInterfaceEntry
                    {
                        IID = ABI.System.Collections.Generic.IEnumerableMethods<object>.IID,
                        Vtable = ABI.System.Collections.Generic.IEnumerableMethods<object>.AbiToProjectionVftablePtr,
                    },
                    new ComWrappers.ComInterfaceEntry
                    {
                        IID = ABI.System.Collections.IListMethods.IID,
                        Vtable = ABI.System.Collections.IListMethods.AbiToProjectionVftablePtr,
                    },
                    new ComWrappers.ComInterfaceEntry
                    {
                        IID = ABI.System.Collections.IEnumerableMethods.IID,
                        Vtable = ABI.System.Collections.IEnumerableMethods.AbiToProjectionVftablePtr,
                    },
                };
            }
        }
        else if (type.IsArray)
        {
            var elemType = type.GetElementType();
            if (elemType.IsEnum || (elemType.IsClass && elemType != typeof(string)))
            {
                // CsWinRT already generates these for other scenarios we use.
                _ = WinRT.BiliCopilot_UIGenericHelpers.IReadOnlyList_object.Initialized;
                _ = WinRT.BiliCopilot_UIGenericHelpers.IEnumerable_object.Initialized;

                return new ComWrappers.ComInterfaceEntry[]
                {
                    new ComWrappers.ComInterfaceEntry
                    {
                        IID = ABI.System.Collections.Generic.IReadOnlyListMethods<object>.IID,
                        Vtable = ABI.System.Collections.Generic.IReadOnlyListMethods<object>.AbiToProjectionVftablePtr,
                    },
                    new ComWrappers.ComInterfaceEntry
                    {
                        IID = ABI.System.Collections.Generic.IEnumerableMethods<object>.IID,
                        Vtable = ABI.System.Collections.Generic.IEnumerableMethods<object>.AbiToProjectionVftablePtr,
                    },
                    new ComWrappers.ComInterfaceEntry
                    {
                        IID = ABI.System.Collections.IListMethods.IID,
                        Vtable = ABI.System.Collections.IListMethods.AbiToProjectionVftablePtr,
                    },
                    new ComWrappers.ComInterfaceEntry
                    {
                        IID = ABI.System.Collections.IEnumerableMethods.IID,
                        Vtable = ABI.System.Collections.IEnumerableMethods.AbiToProjectionVftablePtr,
                    },
                };
            }
        }

        return default;
    }

    private static string LookupRuntimeClassName(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            var elemType = type.GetGenericArguments()[0];
            if (elemType.IsEnum || (elemType.IsClass && elemType != typeof(string)))
            {
                return "Windows.Foundation.Collections.IVectorView`1<Object>";
            }
        }
        else if (type.IsArray)
        {
            var elemType = type.GetElementType();
            if (elemType.IsEnum || (elemType.IsClass && elemType != typeof(string)))
            {
                return "Windows.Foundation.Collections.IVectorView`1<Object>";
            }
        }

        return default;
    }
}

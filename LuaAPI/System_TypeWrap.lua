---@class System.Type : System.Reflection.MemberInfo
---@field public Assembly System.Reflection.Assembly
---@field public AssemblyQualifiedName string
---@field public Attributes System.Reflection.TypeAttributes
---@field public BaseType System.Type
---@field public DeclaringType System.Type
---@field public DefaultBinder System.Reflection.Binder
---@field public FullName string
---@field public GUID System.Guid
---@field public HasElementType bool
---@field public IsAbstract bool
---@field public IsAnsiClass bool
---@field public IsArray bool
---@field public IsAutoClass bool
---@field public IsAutoLayout bool
---@field public IsByRef bool
---@field public IsClass bool
---@field public IsCOMObject bool
---@field public IsContextful bool
---@field public IsEnum bool
---@field public IsExplicitLayout bool
---@field public IsImport bool
---@field public IsInterface bool
---@field public IsLayoutSequential bool
---@field public IsMarshalByRef bool
---@field public IsNestedAssembly bool
---@field public IsNestedFamANDAssem bool
---@field public IsNestedFamily bool
---@field public IsNestedFamORAssem bool
---@field public IsNestedPrivate bool
---@field public IsNestedPublic bool
---@field public IsNotPublic bool
---@field public IsPointer bool
---@field public IsPrimitive bool
---@field public IsPublic bool
---@field public IsSealed bool
---@field public IsSerializable bool
---@field public IsSpecialName bool
---@field public IsUnicodeClass bool
---@field public IsValueType bool
---@field public MemberType System.Reflection.MemberTypes
---@field public Module System.Reflection.Module
---@field public Namespace string
---@field public ReflectedType System.Type
---@field public TypeHandle System.RuntimeTypeHandle
---@field public TypeInitializer System.Reflection.ConstructorInfo
---@field public UnderlyingSystemType System.Type
---@field public ContainsGenericParameters bool
---@field public IsGenericTypeDefinition bool
---@field public IsGenericType bool
---@field public IsGenericParameter bool
---@field public IsNested bool
---@field public IsVisible bool
---@field public GenericParameterPosition int
---@field public GenericParameterAttributes System.Reflection.GenericParameterAttributes
---@field public DeclaringMethod System.Reflection.MethodBase
---@field public StructLayoutAttribute System.Runtime.InteropServices.StructLayoutAttribute
---@field public Delimiter char
---@field public EmptyTypes table
---@field public FilterAttribute System.Reflection.MemberFilter
---@field public FilterName System.Reflection.MemberFilter
---@field public FilterNameIgnoreCase System.Reflection.MemberFilter
---@field public Missing object
local m = {}
---@param o object
---@return bool
function m:Equals(o) end
---@param typeName string
---@return System.Type
function m.GetType(typeName) end
---@param args table
---@return table
function m.GetTypeArray(args) end
---@param type System.Type
---@return System.TypeCode
function m.GetTypeCode(type) end
---@param handle System.RuntimeTypeHandle
---@return System.Type
function m.GetTypeFromHandle(handle) end
---@param o object
---@return System.RuntimeTypeHandle
function m.GetTypeHandle(o) end
---@param c System.Type
---@return bool
function m:IsSubclassOf(c) end
---@param filter System.Reflection.TypeFilter
---@param filterCriteria object
---@return table
function m:FindInterfaces(filter, filterCriteria) end
---@param name string
---@return System.Type
function m:GetInterface(name) end
---@param interfaceType System.Type
---@return System.Reflection.InterfaceMapping
function m:GetInterfaceMap(interfaceType) end
---@return table
function m:GetInterfaces() end
---@param c System.Type
---@return bool
function m:IsAssignableFrom(c) end
---@param o object
---@return bool
function m:IsInstanceOfType(o) end
---@return int
function m:GetArrayRank() end
---@return System.Type
function m:GetElementType() end
---@param name string
---@return System.Reflection.EventInfo
function m:GetEvent(name) end
---@return table
function m:GetEvents() end
---@param name string
---@return System.Reflection.FieldInfo
function m:GetField(name) end
---@return table
function m:GetFields() end
---@return int
function m:GetHashCode() end
---@param name string
---@return table
function m:GetMember(name) end
---@return table
function m:GetMembers() end
---@param name string
---@return System.Reflection.MethodInfo
function m:GetMethod(name) end
---@return table
function m:GetMethods() end
---@param name string
---@return System.Type
function m:GetNestedType(name) end
---@return table
function m:GetNestedTypes() end
---@return table
function m:GetProperties() end
---@param name string
---@return System.Reflection.PropertyInfo
function m:GetProperty(name) end
---@param types table
---@return System.Reflection.ConstructorInfo
function m:GetConstructor(types) end
---@return table
function m:GetConstructors() end
---@return table
function m:GetDefaultMembers() end
---@param memberType System.Reflection.MemberTypes
---@param bindingAttr System.Reflection.BindingFlags
---@param filter System.Reflection.MemberFilter
---@param filterCriteria object
---@return table
function m:FindMembers(memberType, bindingAttr, filter, filterCriteria) end
---@param name string
---@param invokeAttr System.Reflection.BindingFlags
---@param binder System.Reflection.Binder
---@param target object
---@param args table
---@return object
function m:InvokeMember(name, invokeAttr, binder, target, args) end
---@return string
function m:ToString() end
---@return table
function m:GetGenericArguments() end
---@return System.Type
function m:GetGenericTypeDefinition() end
---@param typeArguments table
---@return System.Type
function m:MakeGenericType(typeArguments) end
---@return table
function m:GetGenericParameterConstraints() end
---@return System.Type
function m:MakeArrayType() end
---@return System.Type
function m:MakeByRefType() end
---@return System.Type
function m:MakePointerType() end
---@param typeName string
---@param throwIfNotFound bool
---@param ignoreCase bool
---@return System.Type
function m.ReflectionOnlyGetType(typeName, throwIfNotFound, ignoreCase) end
return m